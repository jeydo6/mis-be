using System.Collections.Generic;
using MIS.Domain.Entities;

namespace MIS.Domain.Repositories
{
	public interface IDispanserizationsRepository
	{
		int Create(Dispanserization dispanserization);

		Dispanserization Get(int dispanserizationID);

		List<Dispanserization> ToList(int patientID);
	}
}
