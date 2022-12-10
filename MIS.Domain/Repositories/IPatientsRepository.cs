using System;
using MIS.Domain.Entities;

namespace MIS.Domain.Repositories
{
	public interface IPatientsRepository
	{
		int Create(Patient item);

		Patient Find(string code, DateTime birthDate);

		Patient Get(int id);
	}
}
