using System;
using MIS.Application.ViewModels;
using MIS.Mediator;

namespace MIS.Application.Queries
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
