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
using MIS.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Queries
{
	public class DispanserizationListItemsHandler : IRequestHandler<DispanserizationListItemsQuery, DispanserizationViewModel[]>
	{

		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly IResourcesRepository _resources;
		private readonly ITimeItemsRepository _timeItems;

		public DispanserizationListItemsHandler(
			IDateTimeProvider dateTimeProvider,
			IResourcesRepository resources,
			ITimeItemsRepository timeItems
		)
		{
			_dateTimeProvider = dateTimeProvider;
			_resources = resources;
			_timeItems = timeItems;
		}

		public async Task<DispanserizationViewModel[]> Handle(DispanserizationListItemsQuery request, CancellationToken cancellationToken)
		{
			var beginDate = _dateTimeProvider.Now.Date;
			var endDate = _dateTimeProvider.Now.Date.AddDays(28);
			var beginDayOfWeek = beginDate.DayOfWeek == 0 ? 7 : (Int32)beginDate.DayOfWeek;

			var result = Enumerable
				.Range(1 - beginDayOfWeek, 35)
				.Select(i => new DispanserizationViewModel
				{
					BeginDate = beginDate.AddDays(i)
				})
				.ToArray();

			var resources = await _resources.GetDispanserizations();
			var totals = await _timeItems.GetDispanserizationTotals(beginDate, endDate);

			var dispanserizationItems = totals
				.GroupBy(t => t.Date)
				.Select(g => new DispanserizationViewModel
				{
					BeginDate = g.Key,
					IsEnabled = g.Count(t => (t.TimesCount - t.VisitsCount) > 0) == resources.Count
				})
				.ToArray();

			if (dispanserizationItems != null && dispanserizationItems.Any())
			{
				var joined = result
					.Join(dispanserizationItems, di => di.BeginDate, d => d.BeginDate, (di, d) =>
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
