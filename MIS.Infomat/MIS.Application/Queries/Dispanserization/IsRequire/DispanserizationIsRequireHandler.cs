#region Copyright © 2020 Vladimir Deryagin. All rights reserved
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
