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
using System.Collections.Generic;

namespace MIS.Domain.Entities
{
    public record Patient
    {
        public Patient()
        {
            VisitItems = new List<VisitItem>();
            Dispanserizations = new List<Dispanserization>();
        }

        public Int32 ID { get; set; }

        public String Code { get; set; }

        public String FirstName { get; set; }

        public String MiddleName { get; set; }

        public String DisplayName
        {
            get
            {
                return $"{FirstName} {MiddleName}".Trim();
            }
        }

        public DateTime BirthDate { get; set; }

        public Int32 Gender { get; set; }

        public ICollection<VisitItem> VisitItems { get; set; }

        public ICollection<Dispanserization> Dispanserizations { get; set; }
    }
}
