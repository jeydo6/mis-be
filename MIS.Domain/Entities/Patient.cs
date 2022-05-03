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

namespace MIS.Domain.Entities
{
	public class Patient
	{
		public Patient()
		{
			VisitItems = new List<VisitItem>();
			Dispanserizations = new List<Dispanserization>();
		}

		public int ID { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }

		public DateTime BirthDate { get; set; }

		public int GenderID { get; set; }

		public ICollection<VisitItem> VisitItems { get; set; }

		public ICollection<Dispanserization> Dispanserizations { get; set; }
	}
}
