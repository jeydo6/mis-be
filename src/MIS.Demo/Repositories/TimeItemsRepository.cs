using System;
using System.Collections.Generic;
using System.Linq;
using MIS.Demo.DataContexts;
using MIS.Domain.Entities;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;

namespace MIS.Demo.Repositories
{
	public class TimeItemsRepository : ITimeItemsRepository
	{
		private readonly DemoDataContext _dataContext;

		public TimeItemsRepository(
			IDateTimeProvider _,
			DemoDataContext dataContext
		)
		{
			_dataContext = dataContext;
		}

		public int Create(TimeItem item)
		{
			item.ID = _dataContext.TimeItems.LastOrDefault()?.ID ?? 1;
			_dataContext.TimeItems.Add(item);

			return item.ID;
		}

		public TimeItem Get(int id)
		{
			var result = _dataContext.TimeItems
				.Where(ti => ti.Resource.Employee.Specialty.ID > 0)
				.FirstOrDefault(ti => ti.ID == id);

			return result;
		}

		public List<TimeItem> ToList(DateTime beginDate, DateTime endDate, int resourceID = 0)
		{
			var result = _dataContext.TimeItems
				.Where(ti => ti.Resource.Employee.Specialty.ID > 0)
				.Where(ti => ti.Date >= beginDate && ti.Date <= endDate && (resourceID == 0 || ti.ResourceID == resourceID))
				.ToList();

			return result;
		}

		public List<TimeItemTotal> GetResourceTotals(DateTime beginDate, DateTime endDate, int specialtyID = 0)
		{
			var result = _dataContext.TimeItems
				.Where(ti => ti.Date >= beginDate && ti.Date <= endDate && (specialtyID == 0 || ti.Resource.Employee.SpecialtyID == specialtyID))
				.Where(ti => ti.Resource.Employee.Specialty.ID > 0)
				.GroupBy(ti => new { ti.ResourceID, ti.Date })
				.Select(g => new TimeItemTotal
				{
					ResourceID = g.Key.ResourceID,
					Date = g.Key.Date,
					BeginDateTime = g.Min(ti => ti.BeginDateTime),
					EndDateTime = g.Max(ti => ti.EndDateTime),
					TimesCount = g.Count(),
					VisitsCount = g.Count(ti => ti.VisitItem != null)
				})
				.ToList();

			return result;
		}

		public List<TimeItemTotal> GetDispanserizationTotals(DateTime beginDate, DateTime endDate)
		{
			var result = _dataContext.TimeItems
				.Where(ti => ti.Date >= beginDate && ti.Date <= endDate)
				.Where(ti => ti.Resource.Employee.Specialty.ID == 0)
				.GroupBy(ti => new { ti.ResourceID, ti.Date })
				.Select(g => new TimeItemTotal
				{
					ResourceID = g.Key.ResourceID,
					Date = g.Key.Date,
					BeginDateTime = g.Min(ti => ti.BeginDateTime),
					EndDateTime = g.Max(ti => ti.EndDateTime),
					TimesCount = g.Count(),
					VisitsCount = g.Count(ti => ti.VisitItem != null)
				})
				.ToList();

			return result;
		}
	}
}
