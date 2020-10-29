using MIS.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MIS.Domain.Repositories
{
    public interface IDispanserizationsRepository
    {
        Int32 Create(Dispanserization dispanserization);

        Dispanserization Get(Int32 dispanserizationID);

        IEnumerable<Dispanserization> ToList(Int32 patientID);
    }
}