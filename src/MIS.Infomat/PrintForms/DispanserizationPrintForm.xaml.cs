using System;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.ViewModels;
using MIS.Domain.Providers;
using MIS.Domain.Services;

namespace MIS.Infomat.PrintForms
{
	/// <summary>
	/// Логика взаимодействия для DispanserizationPrintForm.xaml
	/// </summary>
	public partial class DispanserizationPrintForm : UserControl, IPrintForm
	{
		internal DispanserizationPrintForm()
		{
			throw new ArgumentNullException($"Print model can't be empty!");
		}

		internal DispanserizationPrintForm(DispanserizationViewModel dispanserization)
		{
			var app = System.Windows.Application.Current as App;

			var dateTimeProvider = app.ServiceProvider.GetService<IDateTimeProvider>();

			InitializeComponent();

			DataContext = new DispanserizationPrintFormViewModel
			{
				Now = dateTimeProvider.Now,
				Dispanserization = dispanserization
			};
		}
	}
}
