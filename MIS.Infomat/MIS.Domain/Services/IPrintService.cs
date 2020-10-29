using System;
using System.Threading.Tasks;

namespace MIS.Domain.Services
{
    public interface IPrintService
    {
        public Task Print(Object obj);
    }
}
