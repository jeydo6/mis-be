#region Copyright © 2020 Vladimir Deryagin. All rights reserved
/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

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

        public TimeItemsRepository(IDbConnection db)
        {
            _db = db;
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
