#region Copyright © 2018-2022 Vladimir Deryagin. All rights reserved
/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

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
			var result = request.Patient.Dispanserizations
				.OrderBy(d => d.BeginDate)
				.LastOrDefault(d => !d.IsClosed && d.BeginDate.Year == _dateTimeProvider.Now.Year);

			return await Task.FromResult(result);
		}
	}
}
