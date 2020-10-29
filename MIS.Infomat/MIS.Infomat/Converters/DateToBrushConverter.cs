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
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MIS.Infomat.Converters
{
    internal class DateToBrushConverter : IMultiValueConverter
    {
        public Object Convert(Object[] values, Type targetType, Object parameter, CultureInfo culture)
        {
            DateTime date;
            try
            {
                date = (DateTime)values[0];
            }
            catch
            {
                date = new DateTime();
            }

            Boolean isEnabled;
            try
            {
                isEnabled = (Boolean)values[1];
            }
            catch
            {
                isEnabled = true;
            }

            if (date != new DateTime() && isEnabled)
            {
                return date.Month % 2 == 0 ? Brushes.Blue : Brushes.Green;
            }

            return Brushes.DarkGray;
        }

        public Object[] ConvertBack(Object value, Type[] targetTypes, Object parameter, CultureInfo culture)
        {
            return new DependencyProperty[0];
        }
    }
}
