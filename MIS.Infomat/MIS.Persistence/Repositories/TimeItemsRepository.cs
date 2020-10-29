using Dapper;
using Microsoft.Data.SqlClient;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;

namespace MIS.Persistence.Repositories
{
    public class TimeItemsRepository : ITimeItemsRepository, IDisposable
    {
        private readonly IDbConnection _db;

        public TimeItemsRepository(String connectionString)
        {
            _db = new SqlConnection(connectionString);
        }

        public IEnumerable<TimeItem> ToList(DateTime beginDate, DateTime endDate, Int32 resourceID = 0)
        {
            IEnumerable<TimeItem> timeItems = _db.QueryAsync<TimeItem, Resource, Doctor, Specialty, Room, VisitItem, TimeItem>(
                sql: "[dbo].[sp_TimeItems_List]",
                param: new { beginDate, endDate, resourceID },
                commandType: CommandType.StoredProcedure,
                map: (timeItem, resource, doctor, specialty, room, visitItem) =>
                {
                    timeItem.Resource = resource;
                    timeItem.Resource.Doctor = doctor;
                    timeItem.Resource.Doctor.Specialty = specialty;
                    timeItem.Resource.Room = room;
                    if (visitItem != null)
                    {
                        timeItem.VisitItem = visitItem;
                        timeItem.VisitItem.TimeItem = timeItem;
                    }

                    return timeItem;
                }
            ).Result;

            return timeItems;
        }

        public IEnumerable<TimeItemTotal> GetResourceTotals(DateTime beginDate, DateTime endDate, Int32 specialtyID = 0)
        {
            IEnumerable<TimeItemTotal> totals = _db.QueryAsync<TimeItemTotal>(
                sql: "[dbo].[sp_TimeItems_GetResourceTotals]",
                param: new { beginDate, endDate, specialtyID },
                commandType: CommandType.StoredProcedure
            ).Result;

            return totals;
        }

        public IEnumerable<TimeItemTotal> GetDispanserizationTotals(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<TimeItemTotal> totals = _db.QueryAsync<TimeItemTotal>(
                sql: "[dbo].[sp_TimeItems_GetDispanserizationTotals]",
                param: new { beginDate, endDate },
                commandType: CommandType.StoredProcedure
            ).Result;

            return totals;
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }
    }
}
