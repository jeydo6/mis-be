using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MIS.Demo.DataContexts;
using MIS.Domain.Entities;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;

namespace MIS.Demo.Repositories
{
	public class VisitItemsRepository : IVisitItemsRepository
	{
		private readonly DemoDataContext _dataContext;

		public VisitItemsRepository(
			IDateTimeProvider _,
			DemoDataContext dataContext
		)
		{
			_dataContext = dataContext;
		}

		public int Create(VisitItem item)
		{
			if (_dataContext.VisitItems.FirstOrDefault(vi => vi.TimeItemID == item.TimeItemID) != null)
			{
				throw new Exception("Visit item already exists!");
			}

			item.TimeItem = _dataContext.TimeItems.FirstOrDefault(ti => ti.ID == item.TimeItemID);
			item.TimeItem.VisitItem = item;

			item.Patient = _dataContext.Patients.FirstOrDefault(p => p.ID == item.PatientID);

			item.ID = _dataContext.VisitItems.Count > 0 ? _dataContext.VisitItems.Max(vi => vi.ID) + 1 : 1;

			_dataContext.VisitItems.Add(item);

			var result = item.ID;

			return result;
		}

		public VisitItem Get(int visitItemID)
		{
			var result = _dataContext.VisitItems
				.FirstOrDefault(vi => vi.ID == visitItemID);

			return result;
		}

		public List<VisitItem> ToList(DateTime beginDate, DateTime endDate, int patientID = 0)
		{
			var result = _dataContext.VisitItems
				.Where(vi => vi.TimeItem.Resource.Employee.Specialty.ID > 0)
				.Where(vi => vi.TimeItem.Date >= beginDate && vi.TimeItem.Date <= endDate && (patientID == 0 || vi.PatientID == patientID))
				.ToList();

			return result;
		}
	}
}
