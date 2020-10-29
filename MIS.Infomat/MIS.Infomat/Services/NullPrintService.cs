using MIS.Domain.Services;
using System;
using System.Threading.Tasks;

namespace MIS.Infomat.Services
{
    internal class NullPrintService : IPrintService
    {
        public async Task Print(Object obj)
        {
            await Task.CompletedTask;
        }
    }
}
