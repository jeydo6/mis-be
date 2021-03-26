using System;

namespace MIS.Application.Pagination
{
	public interface IPaginable<T>
	{
		(T current, T next) Paginate(ref Int32 length);
	}
}
