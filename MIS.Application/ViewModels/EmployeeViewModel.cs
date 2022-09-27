#region Copyright © 2018-2022 Vladimir Deryagin. All rights reserved
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

namespace MIS.Application.ViewModels
{
	public class EmployeeViewModel
	{
		public string EmployeeName { get; set; }

		public string PostName { get; set; }

		public DateTime BeginTime { get; set; }

		public DateTime EndTime { get; set; }

		public string PhoneNumber { get; set; }

		public string RoomCode { get; set; }
	}
}
