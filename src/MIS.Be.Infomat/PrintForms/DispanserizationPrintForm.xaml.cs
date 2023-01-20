using System;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using MIS.Be.Application.ViewModels;
using MIS.Be.Domain.Providers;
using MIS.Be.Domain.Services;

namespace MIS.Be.Infomat.PrintForms
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
