#region Copyright © 2020-2021 Vladimir Deryagin. All rights reserved
/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

using System;
using System.Collections.Generic;

namespace MIS.Application.ViewModels
{
	public class PatientViewModel
	{
		public PatientViewModel()
		{
			VisitItems = new List<VisitItemViewModel>();
			Dispanserizations = new List<DispanserizationViewModel>();
		}

		public Int32 ID { get; set; }

		public String Code { get; set; }

		public String Name { get; set; }

		public DateTime BirthDate { get; set; }

		public Boolean DispanserizationIsEnabled { get; set; }

		public List<VisitItemViewModel> VisitItems { get; set; }

		public List<DispanserizationViewModel> Dispanserizations { get; set; }
	}
}
