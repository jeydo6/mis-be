using MIS.Application.ViewModels;
using System;
using System.Windows.Controls;

namespace MIS.Infomat.PrintForms
{
    /// <summary>
    /// Логика взаимодействия для VisitPrintForm.xaml
    /// </summary>
    public partial class VisitPrintForm : UserControl
    {
        internal VisitPrintForm()
        {
            throw new ArgumentNullException($"Print model can't be empty!");
        }

        internal VisitPrintForm(VisitItemViewModel visitItem)
        {
            InitializeComponent();

            DataContext = visitItem;
        }
    }
}
