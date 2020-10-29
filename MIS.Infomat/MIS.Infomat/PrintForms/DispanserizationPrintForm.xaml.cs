using MIS.Application.ViewModels;
using System;
using System.Windows.Controls;

namespace MIS.Infomat.PrintForms
{
    /// <summary>
    /// Логика взаимодействия для DispanserizationPrintForm.xaml
    /// </summary>
    public partial class DispanserizationPrintForm : UserControl
    {
        internal DispanserizationPrintForm()
        {
            throw new ArgumentNullException($"Print model can't be empty!");
        }

        internal DispanserizationPrintForm(DispanserizationViewModel dispanserization)
        {
            InitializeComponent();

            DataContext = dispanserization;
        }
    }
}
