using MediatR;
using System;

namespace MIS.Application.Queries
{
	public class OrganizationNameQuery : IRequest<String>
	{
		public OrganizationNameQuery()
		{
			//
		}
	}
}
