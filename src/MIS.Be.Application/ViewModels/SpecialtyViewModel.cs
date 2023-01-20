namespace MIS.Be.Application.ViewModels
{
	public class SpecialtyViewModel
	{
		public string SpecialtyName { get; set; }

		public int Count { get; set; }

		public bool IsEnabled { get; set; }

		public int SpecialtyID { get; set; }

		public ResourceViewModel[] Resources { get; set; }
	}
}
