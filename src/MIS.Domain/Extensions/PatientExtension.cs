using System.Text;
using MIS.Domain.Entities;

namespace MIS.Domain.Extensions;

public static class PatientExtension
{
	public static string GetName(this Patient patient)
	{
		var sb = new StringBuilder();

		if (!string.IsNullOrEmpty(patient.LastName))
		{
			sb.Append(patient.LastName.Trim() + " ");
		}

		if (!string.IsNullOrEmpty(patient.FirstName))
		{
			sb.Append(patient.FirstName.Trim()[0] + ". ");
		}

		if (!string.IsNullOrEmpty(patient.MiddleName))
		{
			sb.Append(patient.MiddleName.Trim()[0] + ". ");
		}

		return sb.Length > 0 ?
			sb.ToString().Trim() :
			string.Empty;
	}
}
