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

using MIS.Demo.DataContexts;
using MIS.Domain.Entities;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIS.Demo.Repositories
{
	public class TimeItemsRepository : ITimeItemsRepository
	{
		private readonly DemoDataContext _dataContext;
		private readonly IDateTimeProvider _dateTimeProvider;

		public TimeItemsRepository(
			IDateTimeProvider dateTimeProvider,
			DemoDataContext dataContext
		)
		{
			_dateTimeProvider = dateTimeProvider;
			_dataContext = dataContext;
		}

		public async Task<List<TimeItem>> ToList(DateTime beginDate, DateTime endDate, Int32 resourceID = 0)
		{
			var result = _dataContext.TimeItems
				.Where(ti => ti.Resource.Doctor.Specialty.ID > 0)
				.Where(ti => ti.Date >= beginDate && ti.Date <= endDate && (resourceID == 0 || ti.ResourceID == resourceID))
				.ToList();

			return await Task.FromResult(result);
		}

		public async Task<List<TimeItemTotal>> GetResourceTotals(DateTime beginDate, DateTime endDate, Int32 specialtyID = 0)
		{
			var result = _dataContext.TimeItems
				.Where(ti => ti.Date >= beginDate && ti.Date <= endDate && (specialtyID == 0 || ti.Resource.Doctor.SpecialtyID == specialtyID))
				.Where(ti => ti.Resource.Doctor.Specialty.ID > 0)
				.GroupBy(ti => new { ti.ResourceID, ti.Date })
				.Select(g => new TimeItemTotal
				{
					ResourceID = g.Key.ResourceID,
					Date = g.Key.Date,
					TimesCount = g.Count(),
					VisitsCount = g.Count(ti => ti.VisitItem != null)
				})
				.ToList();

			return await Task.FromResult(result);
		}

		public async Task<List<TimeItemTotal>> GetDispanserizationTotals(DateTime beginDate, DateTime endDate)
		{
			var result = _dataContext.TimeItems
				.Where(ti => ti.Date >= beginDate && ti.Date <= endDate)
				.Where(ti => ti.Resource.Doctor.Specialty.ID == 0)
				.GroupBy(ti => new { ti.ResourceID, ti.Date })
				.Select(g => new TimeItemTotal
				{
					ResourceID = g.Key.ResourceID,
					Date = g.Key.Date,
					TimesCount = g.Count(),
					VisitsCount = g.Count(ti => ti.VisitItem != null)
				})
				.ToList();

			return await Task.FromResult(result);
		}
	}
}
