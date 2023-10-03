using System.Text;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Extensions;

public static class PersonExtension
{
    public static string GetName(this Person person)
    {
        var sbLength = person.LastName.Length + 7;
        var sb = new StringBuilder(sbLength);

        var lastName = person.LastName.Trim();
        if (!string.IsNullOrEmpty(lastName))
        {
            sb.Append(lastName + " ");
        }

        var firstName = person.FirstName.Trim();
        if (!string.IsNullOrEmpty(firstName))
        {
            sb.Append(firstName[0] + ". ");
        }

        var middleName = person.MiddleName.Trim();
        if (!string.IsNullOrEmpty(middleName))
        {
            sb.Append(middleName[0] + ". ");
        }

        return sb.Length > 0 ?
            sb.ToString().Trim() :
            string.Empty;
    }
}
