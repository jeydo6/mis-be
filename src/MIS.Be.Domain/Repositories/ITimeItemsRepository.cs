using System;
using System.Collections.Generic;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories
{
	public interface ITimeItemsRepository
	{
		int Create(TimeItem item);

		TimeItem Get(int id);

		List<TimeItem> ToList(DateTime beginDate, DateTime endDate, int resourceID = 0);

		List<TimeItemTotal> GetResourceTotals(DateTime beginDate, DateTime endDate, int specialtyID = 0);

		List<TimeItemTotal> GetDispanserizationTotals(DateTime beginDate, DateTime endDate);
	}
}
