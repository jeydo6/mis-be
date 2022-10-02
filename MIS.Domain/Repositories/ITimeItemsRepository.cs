﻿#region Copyright © 2018-2022 Vladimir Deryagin. All rights reserved
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
using MIS.Domain.Entities;

namespace MIS.Domain.Repositories
{
	public interface ITimeItemsRepository
	{
		List<TimeItem> ToList(DateTime beginDate, DateTime endDate, int resourceID = 0);

		List<TimeItemTotal> GetResourceTotals(DateTime beginDate, DateTime endDate, int specialtyID = 0);

		List<TimeItemTotal> GetDispanserizationTotals(DateTime beginDate, DateTime endDate);
	}
}
