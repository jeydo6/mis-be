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

using MIS.Application.Pagination;
using System;

namespace MIS.Application.ViewModels
{
	public class SpecialtyViewModel : IPaginable<SpecialtyViewModel>
	{
		public String SpecialtyName { get; set; }

		public Int32 Count { get; set; }

		public Boolean IsEnabled { get; set; }

		public Int32 SpecialtyID { get; set; }

		public ResourceViewModel[] Resources { get; set; }

		public (SpecialtyViewModel current, SpecialtyViewModel next) Paginate(ref Int32 length)
		{
			if (Resources.Length > length)
			{
				SpecialtyViewModel current = new SpecialtyViewModel
				{
					SpecialtyID = SpecialtyID,
					SpecialtyName = SpecialtyName,
					IsEnabled = IsEnabled,
					Count = Count,
					Resources = Resources[..length]
				};

				SpecialtyViewModel next = new SpecialtyViewModel
				{
					SpecialtyID = SpecialtyID,
					SpecialtyName = SpecialtyName,
					IsEnabled = IsEnabled,
					Count = Count,
					Resources = Resources[length..]
				};

				length = current.Resources.Length;
				return (current, next);
			}

			length = Resources.Length;
			return (this, null);
		}
	}
}
