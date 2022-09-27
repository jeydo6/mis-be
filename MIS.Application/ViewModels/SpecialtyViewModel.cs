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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MIS.Application.ViewModels
{
	public class SpecialtyViewModel : IGrouping<SpecialtyViewModel, ResourceViewModel>
	{
		public string SpecialtyName { get; set; }

		public int Count { get; set; }

		public bool IsEnabled { get; set; }

		public int SpecialtyID { get; set; }

		public ResourceViewModel[] Resources { get; set; }

		public SpecialtyViewModel Key => this;

		public IEnumerator<ResourceViewModel> GetEnumerator()
		{
			foreach (var resource in Resources)
			{
				yield return resource;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
