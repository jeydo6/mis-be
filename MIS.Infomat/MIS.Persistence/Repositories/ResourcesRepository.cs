using Dapper;
using Microsoft.Data.SqlClient;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;

namespace MIS.Persistence.Repositories
{
    public class ResourcesRepository : IResourcesRepository, IDisposable
    {
        private readonly IDbConnection _db;

        public ResourcesRepository(String connectionString)
        {
            _db = new SqlConnection(connectionString);
        }

        public IEnumerable<Resource> ToList()
        {
            IEnumerable<Resource> resources = _db.QueryAsync<Resource, Doctor, Specialty, Room, Resource>(
                sql: "[dbo].[sp_Resources_List]",
                commandType: CommandType.StoredProcedure,
                map: (resource, doctor, specialty, room) =>
                {
                    resource.Doctor = doctor;
                    resource.Doctor.Specialty = specialty;
                    resource.Room = room;

                    return resource;
                }
            ).Result;

            return resources;
        }

        public IEnumerable<Resource> GetDispanserizations()
        {
            IEnumerable<Resource> resources = _db.QueryAsync<Resource, Doctor, Specialty, Room, Resource>(
                sql: "[dbo].[sp_Resources_GetDispanserizations]",
                commandType: CommandType.StoredProcedure,
                map: (resource, doctor, specialty, room) =>
                {
                    resource.Doctor = doctor;
                    resource.Doctor.Specialty = specialty;
                    resource.Room = room;

                    return resource;
                }
            ).Result;

            return resources;
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