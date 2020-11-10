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
using System.Data;

namespace MIS.Persistence.Repositories
{
    public class PatientsRepository : IPatientsRepository, IDisposable
    {
        private readonly IDbConnection _db;
        private readonly IDbTransaction _transaction;

        public PatientsRepository(String connectionString)
        {
            _db = new SqlConnection(connectionString);
            _transaction = null;
        }

        public PatientsRepository(IDbTransaction transaction)
        {
            _db = transaction.Connection;
            _transaction = transaction;
        }

        public Patient First(String code, DateTime birthDate)
        {
            Patient patient = _db.QueryFirstOrDefaultAsync<Patient>(
                sql: "[dbo].[sp_Patients_First]",
                param: new { code, birthDate },
                commandType: CommandType.StoredProcedure,
                transaction: _transaction
            ).Result;

            return patient;
        }

        public Patient Get(Int32 patientID)
        {
            Patient patient = _db.QueryFirstOrDefaultAsync<Patient>(
                sql: "[dbo].[sp_Patients_Get]",
                param: new { patientID },
                commandType: CommandType.StoredProcedure,
                transaction: _transaction
            ).Result;

            return patient;
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