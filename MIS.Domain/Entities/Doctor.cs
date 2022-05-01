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
				var sb = new StringBuilder();

				if (!String.IsNullOrEmpty(FirstName))
				{
					sb.Append($" {FirstName[0]}.");
				}

				if (!String.IsNullOrEmpty(MiddleName))
				{
					sb.Append($" {MiddleName[0]}.");
				}

				if (!String.IsNullOrEmpty(LastName))
				{
					var spaceIndex = LastName.IndexOf(' ');
					if (spaceIndex > 0)
					{
						sb.Insert(0, LastName[0..spaceIndex]);
						sb.Append(LastName[spaceIndex..]);
					}
					else
					{
						sb.Insert(0, LastName);
					}
				}

				return sb.ToString().Trim();
			}
		}
	}
}
