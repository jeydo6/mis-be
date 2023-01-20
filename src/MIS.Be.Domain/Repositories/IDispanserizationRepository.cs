using System.Collections.Generic;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories
{
	public interface IDispanserizationsRepository
	{
		int Create(Dispanserization dispanserization);

		Dispanserization Get(int dispanserizationID);

		List<Dispanserization> ToList(int patientID);
	}
}
