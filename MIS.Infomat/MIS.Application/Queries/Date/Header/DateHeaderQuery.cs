using MediatR;
using System;

namespace MIS.Application.Queries
{
    public class DateHeaderQuery : IRequest<String>
    {
        public DateHeaderQuery()
        {
            //
        }
    }
}
