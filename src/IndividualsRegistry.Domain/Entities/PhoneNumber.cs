using IndividualsRegistry.Shared.Library;
using System.ComponentModel.DataAnnotations;

namespace IndividualsRegistry.Domain.Entities;
public sealed class PhoneNumber : Entity
{
    [Required]
    [RegularExpression(@"^(მობილური|ოფისის|სახლის)$", ErrorMessage = "Phone type must be either 'მობილური', 'ოფისის', or 'სახლის'.")]
    public string Type { get; private set; }

    [Required]
    [StringLength(50, MinimumLength = 4)]
    public string Number { get; private set; }

    public int PersonId { get; private set; }
    public Person Person { get; private set; }

    private PhoneNumber() { } // For ORM

    public PhoneNumber(string type, string number, int personId)
    {
        SetType(type);
        SetNumber(number);
        SetPerson(personId);
    }

    public void SetType(string type)
    {
        if (string.IsNullOrWhiteSpace(type) || !new[] { "მობილური", "ოფისის", "სახლის" }.Contains(type))
            throw new ArgumentException("Invalid phone type.");
        Type = type;
    }

    public void SetNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number) || number.Length < 4 || number.Length > 50)
            throw new ArgumentException("Phone number must be between 4 and 50 characters.");
        Number = number;
    }

    private void SetPerson(int personId)
    {
        if (personId == 0)
            throw new ArgumentException("personId must be more than 0.");
        PersonId = personId;
    }
}