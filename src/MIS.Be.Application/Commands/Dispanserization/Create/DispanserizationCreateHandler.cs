using System;
using System.Linq;
using MIS.Be.Application.ViewModels;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Commands
{
	public class DispanserizationCreateHandler : IRequestHandler<DispanserizationCreateCommand, DispanserizationViewModel>
	{
		private readonly IDispanserizationsRepository _dispanserizations;

		public DispanserizationCreateHandler(
			IDispanserizationsRepository dispanserizations
		)
		{
			_dispanserizations = dispanserizations;
		}

		public DispanserizationViewModel Handle(DispanserizationCreateCommand request)
		{
			var dispanserization = new Dispanserization
			{
				BeginDate = request.BeginDate,
				EndDate = new DateTime(request.BeginDate.Year, 12, 31),
				PatientID = request.PatientID
			};

			var dispanserizationID = _dispanserizations.Create(dispanserization);

			dispanserization = _dispanserizations.Get(dispanserizationID);

			var result = new DispanserizationViewModel
			{
				BeginDate = dispanserization.BeginDate,
				PatientCode = request.PatientCode,
				PatientName = request.PatientName,
				IsClosed = dispanserization.IsClosed,
				IsEnabled = true,
				Researches = dispanserization.Researches.Select(a => a.Description).ToArray()
			};

			return result;
		}
	}
}
