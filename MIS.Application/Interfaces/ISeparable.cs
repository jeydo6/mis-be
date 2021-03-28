using System;

namespace MIS.Application.Interfaces
{
	public interface ISeparable<T>
	{
		(T current, T next) Separate(ref Int32 length);
	}
}
