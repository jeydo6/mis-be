using System;
using System.Collections.Generic;
using MIS.Domain.Entities;

namespace MIS.Domain.Repositories
{
	public interface IVisitItemsRepository
	{
		int Create(VisitItem item);

		VisitItem Get(int visitItemID);

		List<VisitItem> ToList(DateTime beginDate, DateTime endDate, int patientID = 0);
	}
}
