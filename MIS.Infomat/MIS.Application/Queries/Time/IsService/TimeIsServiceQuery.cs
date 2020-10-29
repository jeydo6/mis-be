using MediatR;
using System;

namespace MIS.Application.Queries
{
    public class TimeIsServiceQuery : IRequest<Boolean>
    {
        public TimeIsServiceQuery()
        {
            //
        }
    }
}
