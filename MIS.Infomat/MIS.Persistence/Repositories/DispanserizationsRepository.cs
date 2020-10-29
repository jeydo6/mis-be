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
    public class DispanserizationsRepository : IDispanserizationsRepository, IDisposable
    {
        private readonly IDbConnection _db;

        public DispanserizationsRepository(String connectionString)
        {
            _db = new SqlConnection(connectionString);
        }

        public Int32 Create(Dispanserization item)
        {
            Int32 dispanserizationID = _db.QuerySingleAsync<Int32>(
                sql: "[dbo].[sp_Dispanserizations_Create]",
                param: new
                {
                    patientID = item.PatientID,
                    beginDate = item.BeginDate,
                    endDate = item.EndDate
                },
                commandType: CommandType.StoredProcedure
            ).Result;

            return dispanserizationID;
        }

        public Dispanserization Get(Int32 dispanserizationID)
        {
            IDictionary<Int32, Dispanserization> keyValues = new Dictionary<Int32, Dispanserization>();

            Dispanserization result = _db.QueryAsync<Dispanserization, Analysis, Dispanserization>(
                sql: "[dbo].[sp_Dispanserizations_Get]",
                param: new { dispanserizationID },
                commandType: CommandType.StoredProcedure,
                map: (dispanserization, analysis) =>
                {
                    if (!keyValues.TryGetValue(dispanserization.ID, out Dispanserization value))
                    {
                        value = dispanserization;
                        value.Analyses = new List<Analysis>();
                        keyValues.Add(dispanserization.ID, dispanserization);
                    }

                    value.Analyses.Add(analysis);

                    return dispanserization;
                }
            ).Result
            .Distinct()
            .FirstOrDefault();

            return result;
        }

        public IEnumerable<Dispanserization> ToList(Int32 patientID)
        {
            IDictionary<Int32, Dispanserization> keyValues = new Dictionary<Int32, Dispanserization>();

            IEnumerable<Dispanserization> dispanserizations = _db.QueryAsync<Dispanserization, Analysis, Dispanserization>(
                sql: "[dbo].[sp_Dispanserizations_List]",
                param: new { patientID },
                commandType: CommandType.StoredProcedure,
                map: (dispanserization, analysis) =>
                {
                    if (!keyValues.TryGetValue(dispanserization.ID, out Dispanserization value))
                    {
                        value = dispanserization;
                        value.Analyses = new List<Analysis>();
                        keyValues.Add(dispanserization.ID, dispanserization);
                    }

                    value.Analyses.Add(analysis);
                    return value;
                }
            ).Result
            .Distinct()
            .ToList();

            return dispanserizations;
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