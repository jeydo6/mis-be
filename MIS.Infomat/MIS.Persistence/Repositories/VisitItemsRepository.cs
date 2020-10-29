using Dapper;
using Microsoft.Data.SqlClient;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MIS.Persistence.Repositories
{
    public class VisitItemsRepository : IVisitItemsRepository, IDisposable
    {
        private readonly IDbConnection _db;

        public VisitItemsRepository(String connectionString)
        {
            _db = new SqlConnection(connectionString);
        }

        public Int32 Create(VisitItem visitItem)
        {
            Int32 visitItemID = _db.QuerySingleAsync<Int32>(
                sql: "[dbo].[sp_VisitItems_Create]",
                param: new
                {
                    patientID = visitItem.PatientID,
                    timeItemID = visitItem.TimeItemID
                },
                commandType: CommandType.StoredProcedure
            ).Result;

            return visitItemID;
        }

        public VisitItem Get(Int32 visitItemID)
        {
            VisitItem result = _db.QueryAsync<VisitItem, TimeItem, Resource, Doctor, Specialty, Room, VisitItem>(
                sql: "[dbo].[sp_VisitItems_Get]",
                param: new { visitItemID },
                commandType: CommandType.StoredProcedure,
                map: (visitItem, timeItem, resource, doctor, specialty, room) =>
                {
                    visitItem.TimeItem = timeItem;
                    visitItem.TimeItem.Resource = resource;
                    visitItem.TimeItem.Resource.Doctor = doctor;
                    visitItem.TimeItem.Resource.Doctor.Specialty = specialty;
                    visitItem.TimeItem.Resource.Room = room;
                    visitItem.TimeItem.VisitItem = visitItem;

                    return visitItem;
                }
            ).Result.FirstOrDefault();

            return result;
        }

        public IEnumerable<VisitItem> ToList(DateTime beginDate, DateTime endDate, Int32 patientID = 0)
        {
            IEnumerable<VisitItem> visitItems = _db.QueryAsync<VisitItem, TimeItem, Resource, Doctor, Specialty, Room, VisitItem> (
                sql: "[dbo].[sp_VisitItems_List]",
                param: new { beginDate, endDate, patientID },
                commandType: CommandType.StoredProcedure,
                map: (visitItem, timeItem, resource, doctor, specialty, room) =>
                {
                    visitItem.TimeItem = timeItem;
                    visitItem.TimeItem.Resource = resource;
                    visitItem.TimeItem.Resource.Doctor = doctor;
                    visitItem.TimeItem.Resource.Doctor.Specialty = specialty;
                    visitItem.TimeItem.Resource.Room = room;
                    visitItem.TimeItem.VisitItem = visitItem;

                    return visitItem;
                }
            ).Result;

            return visitItems;
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
