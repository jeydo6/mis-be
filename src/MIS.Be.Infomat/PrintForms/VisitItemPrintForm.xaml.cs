using System;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MIS.Be.Application.Configs;
using MIS.Be.Application.ViewModels;
using MIS.Be.Domain.Services;

namespace MIS.Be.Infomat.PrintForms
{
	/// <summary>
	/// Логика взаимодействия для VisitItemPrintForm.xaml
	/// </summary>
	public partial class VisitItemPrintForm : UserControl, IPrintForm
	{
		internal VisitItemPrintForm()
		{
			throw new ArgumentNullException($"Print model can't be empty!");
		}

		internal VisitItemPrintForm(VisitItemViewModel visitItem)
		{
			var app = System.Windows.Application.Current as App;
			var settingsConfigOptions = app.ServiceProvider.GetService<IOptionsSnapshot<SettingsConfig>>();
			var settingsConfig = settingsConfigOptions.Value;

			InitializeComponent();

			DataContext = new VisitItemPrintFormViewModel
			{
				OrganizationName = settingsConfig.OrganizationName,
				VisitItem = visitItem
			};
		}
	}
}
