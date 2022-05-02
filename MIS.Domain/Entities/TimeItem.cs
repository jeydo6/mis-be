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

namespace MIS.Domain.Entities
{
	public class TimeItem
	{
		public int ID { get; set; }

		public DateTime Date { get; set; }

		public DateTime BeginDateTime { get; set; }

		public DateTime EndDateTime { get; set; }

		public int ResourceID { get; set; }

		public Resource Resource { get; set; }

		public VisitItem VisitItem { get; set; }
	}
}
