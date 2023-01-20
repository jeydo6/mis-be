using System;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories
{
	public interface IPatientsRepository
	{
		int Create(Patient item);

		Patient Find(string code, DateTime birthDate);

		Patient Get(int id);
	}
}
