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

using System.Linq;
using MIS.Application.ViewModels;
using MIS.Domain.Providers;
using MIS.Mediator;

namespace MIS.Application.Queries
{
	public class DateListItemsHandler : IRequestHandler<DateListItemsQuery, DateItemViewModel[]>
	{
		private readonly IDateTimeProvider _dateTimeProvider;

		public DateListItemsHandler(
			IDateTimeProvider dateTimeProvider
		)
		{
			_dateTimeProvider = dateTimeProvider;
		}

		public DateItemViewModel[] Handle(DateListItemsQuery request)
		{
			var beginDate = _dateTimeProvider.Now.Date;
			var beginDayOfWeek = beginDate.DayOfWeek == 0 ? 7 : (int)beginDate.DayOfWeek;

			var result = Enumerable
				.Range(1 - beginDayOfWeek, 35)
				.Select(i => new DateItemViewModel
				{
					Date = beginDate.AddDays(i),
					ResourceID = request.Resource.ResourceID
				})
				.ToArray();

			if (request.Resource.Dates != null && request.Resource.Dates.Any())
			{
				_ = result
					.Join(request.Resource.Dates, di => di.Date, d => d.Date, (di, d) =>
					{
						di.IsEnabled = d.IsEnabled;

						return di;
					})
					.ToArray();
			}

			return result;
		}
	}
}
