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
using MIS.Application.ViewModels;
using MIS.Domain.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Queries
{
	public class DateListItemsHandler : IRequestHandler<DateListItemsQuery, IEnumerable<DateItemViewModel>>
	{
		private readonly IDateTimeProvider _dateTimeProvider;

		public DateListItemsHandler(
			IDateTimeProvider dateTimeProvider
		)
		{
			_dateTimeProvider = dateTimeProvider;
		}

		public async Task<IEnumerable<DateItemViewModel>> Handle(DateListItemsQuery request, CancellationToken cancellationToken)
		{
			IEnumerable<DateItemViewModel> viewModels = null;

			if (request.Resource.Dates != null && request.Resource.Dates.Any())
			{
				DateTime beginDate = _dateTimeProvider.Now.Date;
				Int32 beginDayOfWeek = beginDate.DayOfWeek == 0 ? 7 : (Int32)beginDate.DayOfWeek;

				viewModels = Enumerable
					.Range(1 - beginDayOfWeek, 35)
					.Select(i => new DateItemViewModel
					{
						Date = beginDate.AddDays(i),
						ResourceID = request.Resource.ResourceID
					})
					.ToList();

				viewModels
					.Join(request.Resource.Dates, di => di.Date, d => d.Date, (di, d) =>
					{
						di.IsEnabled = d.IsEnabled;

						return di;
					})
					.ToList();
			}

			return await Task.FromResult(viewModels);
		}
	}
}
