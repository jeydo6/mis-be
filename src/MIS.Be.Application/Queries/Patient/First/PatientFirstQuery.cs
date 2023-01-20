using System;
using MIS.Be.Application.ViewModels;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
{
	public class PatientFirstQuery : IRequest<PatientViewModel>
	{
		public PatientFirstQuery(string code, DateTime birthDate)
		{
			Code = code;
			BirthDate = birthDate;
		}

		public string Code { get; }

		public DateTime BirthDate { get; }
	}
}
