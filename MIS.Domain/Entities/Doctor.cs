#region Copyright © 2020 Vladimir Deryagin. All rights reserved
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
using System.Text;

namespace MIS.Domain.Entities
{
	public class Doctor
	{
		public Int32 ID { get; set; }

		public String Code { get; set; }

		public String FirstName { get; set; }

		public String MiddleName { get; set; }

		public String LastName { get; set; }

		public String DisplayName
		{
			get
			{
				StringBuilder sb = new StringBuilder();

				if (!String.IsNullOrEmpty(LastName))
				{
					if (LastName.Contains(' '))
					{
						sb.Append(LastName[0..LastName.IndexOf(' ')]);
					}
					else
					{
						sb.Append(LastName);
					}
				}

				if (!String.IsNullOrEmpty(FirstName))
				{
					sb.Append($" {FirstName[0]}.");
				}

				if (!String.IsNullOrEmpty(MiddleName))
				{
					sb.Append($" {MiddleName[0]}.");
				}

				return sb.ToString().Trim();
			}
		}

		public Int32 SpecialtyID { get; set; }

		public Specialty Specialty { get; set; }
	}
}
