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

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Queries;
using MIS.Application.ViewModels;
using MIS.Infomat.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MIS.Infomat.Controls
{
    /// <summary>
    /// Логика взаимодействия для ActionsControl.xaml
    /// </summary>
    public partial class ActionsControl : UserControl
    {
        private readonly PatientViewModel _patient;

        private readonly MainWindow _mainWindow;

        private readonly IMediator _mediator;

        internal ActionsControl()
        {
            throw new ArgumentNullException($"Field '{nameof(_patient)}' can't be empty!");
        }

        internal ActionsControl(PatientViewModel patient)
        {
            _patient = patient;

            var app = System.Windows.Application.Current as App;

            _mainWindow = app.MainWindow as MainWindow;

            _mediator = app.ServiceProvider.GetService<IMediator>();

            InitializeComponent();
        }

        private void UserControl_Loaded(Object sender, RoutedEventArgs e)
        {
            Boolean dispanserizationIsRequire = _mediator.Send(
                new DispanserizationIsRequireQuery(_patient)
            ).Result;

            dispanserizationButton.Visibility = dispanserizationIsRequire ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TimesButton_Click(Object sender, RoutedEventArgs e)
        {
            if (!_mainWindow.IsServiceTime)
            {
                _mainWindow.NextWorkflow(new SpecialtiesControl(_patient));
            }
        }

        private void VisitsButton_Click(Object sender, RoutedEventArgs e)
        {
            if (!_mainWindow.IsServiceTime)
            {
                _mainWindow.NextWorkflow(new VisitItemsControl(_patient));
            }
        }

        private void DispanserizationButton_Click(Object sender, RoutedEventArgs e)
        {
            if (!_mainWindow.IsServiceTime)
            {
                _mainWindow.NextWorkflow(new DispanserizationControl(_patient));
            }
        }

        private void PrevButton_Click(Object sender, RoutedEventArgs e)
        {
            _mainWindow.MainWorkflow();
        }
    }
}