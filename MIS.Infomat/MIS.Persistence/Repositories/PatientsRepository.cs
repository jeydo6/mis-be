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

        public PatientsRepository(String connectionString)
        {
            _db = new SqlConnection(connectionString);
        }

        public Patient First(String code, DateTime birthDate)
        {
            Patient patient = _db.QueryFirstOrDefaultAsync<Patient>(
                sql: "[dbo].[sp_Patients_First]",
                param: new { code, birthDate },
                commandType: CommandType.StoredProcedure
            ).Result;

            return patient;
        }

        public Patient Get(Int32 patientID)
        {
            Patient patient = _db.QueryFirstOrDefaultAsync<Patient>(
                sql: "[dbo].[sp_Patients_Get]",
                param: new { patientID },
                commandType: CommandType.StoredProcedure
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