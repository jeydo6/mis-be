﻿#region Copyright © 2020-2021 Vladimir Deryagin. All rights reserved
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
	public class DispanserizationIsRequiredHandler : IRequestHandler<DispanserizationIsRequiredQuery, Boolean>
	{
		private readonly IDateTimeProvider _dateTimeProvider;

		public DispanserizationIsRequiredHandler(
			IDateTimeProvider dateTimeProvider
		)
		{
			_dateTimeProvider = dateTimeProvider;
		}

		public async Task<Boolean> Handle(DispanserizationIsRequiredQuery request, CancellationToken cancellationToken)
		{
			var result = !request.Patient.Dispanserizations
				.Any(d => d.BeginDate.Year == _dateTimeProvider.Now.Year);

			return await Task.FromResult(result);
		}
	}
}
