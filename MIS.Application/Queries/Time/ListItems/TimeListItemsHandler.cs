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
using MIS.Domain.Repositories;
using MIS.Mediator;

namespace MIS.Application.Queries
{
	public class TimeListItemsHandler : IRequestHandler<TimeListItemsQuery, TimeItemViewModel[]>
	{
		private readonly ITimeItemsRepository _timeItems;
		private readonly IDateTimeProvider _dateTimeProvider;

		public TimeListItemsHandler(
			IDateTimeProvider dateTimeProvider,
			ITimeItemsRepository timeItems
		)
		{
			_dateTimeProvider = dateTimeProvider;
			_timeItems = timeItems;
		}

		public TimeItemViewModel[] Handle(TimeListItemsQuery request)
		{
			var timeItems = _timeItems
				.ToList(request.Date, request.Date, request.ResourceID);

			var result = timeItems
				.Where(t => t.BeginDateTime > _dateTimeProvider.Now)
				.Select(t => new TimeItemViewModel
				{
					TimeItemID = t.ID,
					DateTime = t.BeginDateTime,
					IsEnabled = t.VisitItem == null
				})
				.OrderBy(ti => ti.DateTime)
				.Take(28)
				.ToArray();

			return result;
		}
	}
}
