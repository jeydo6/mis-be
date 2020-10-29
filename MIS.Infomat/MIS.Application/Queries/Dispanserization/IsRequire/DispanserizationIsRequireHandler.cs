using MediatR;
using MIS.Domain.Providers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Queries
{
    public class DispanserizationIsRequireHandler : IRequestHandler<DispanserizationIsRequireQuery, Boolean>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public DispanserizationIsRequireHandler(
            IDateTimeProvider dateTimeProvider
        )
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Boolean> Handle(DispanserizationIsRequireQuery request, CancellationToken cancellationToken)
        {
            Boolean isRequire = request.Patient.Dispanserizations
                .Count(d => d.BeginDate.Year == _dateTimeProvider.Now.Year) == 0;

            return await Task.FromResult(isRequire);
        }
    }
}
