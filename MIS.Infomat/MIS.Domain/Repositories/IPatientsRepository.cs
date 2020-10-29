using MIS.Domain.Entities;
using System;

namespace MIS.Domain.Repositories
{
    public interface IPatientsRepository
    {
        Patient First(String code, DateTime birthDate);

        Patient Get(Int32 patientID);
    }
}
