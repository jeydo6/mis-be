using MediatR;
using MIS.Application.ViewModels;
using MIS.Domain.Providers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Queries
{
    public class DispanserizationLastHandler : IRequestHandler<DispanserizationLastQuery, DispanserizationViewModel>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public DispanserizationLastHandler(
            IDateTimeProvider dateTimeProvider
        )
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<DispanserizationViewModel> Handle(DispanserizationLastQuery request, CancellationToken cancellationToken)
        {
            DispanserizationViewModel viewModel = request.Patient.Dispanserizations
                .OrderBy(d => d.BeginDate)
                .LastOrDefault(d => !d.IsClosed && d.BeginDate.Year == _dateTimeProvider.Now.Year);

            return await Task.FromResult(viewModel);
        }
    }
}
