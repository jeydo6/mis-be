using System;
using System.Collections.Generic;

namespace MIS.Application.ViewModels
{
    public class SpecialtyViewModel
    {
        public String SpecialtyName { get; set; }

        public Boolean IsEnabled { get; set; }

        public IEnumerable<ResourceViewModel> Resources { get; set; }
    }
}
