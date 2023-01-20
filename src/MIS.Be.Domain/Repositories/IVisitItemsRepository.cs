using System;
using System.Collections.Generic;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories
{
	public interface IVisitItemsRepository
	{
		int Create(VisitItem item);

		VisitItem Get(int visitItemID);

		List<VisitItem> ToList(DateTime beginDate, DateTime endDate, int patientID = 0);
	}
}
