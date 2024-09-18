using IndividualsRegistry.Shared.Library;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IndividualsRegistry.Domain.Entities;
public sealed class Person : Entity
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    [RegularExpression(@"^[ა-ჰa-zA-Z]+$", ErrorMessage = "First name must contain only Georgian or Latin alphabet letters.")]
    public string FirstName { get; private set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    [RegularExpression(@"^[ა-ჰa-zA-Z]+$", ErrorMessage = "Last name must contain only Georgian or Latin alphabet letters.")]
    public string LastName { get; private set; }

    [Required]
    [RegularExpression(@"^(ქალი|კაცი)$", ErrorMessage = "Gender must be either 'ქალი' or 'კაცი'.")]
    public string Gender { get; private set; }

    [Required]
    [StringLength(11, MinimumLength = 11)]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "Personal number must be exactly 11 digits.")]
    public string PersonalNumber { get; private set; }

    [Required]
    public DateTime DateOfBirth { get; private set; }

    private readonly List<PhoneNumber> _phoneNumbers = new List<PhoneNumber>();
    public IReadOnlyCollection<PhoneNumber> PhoneNumbers => _phoneNumbers.AsReadOnly();

    private readonly List<PersonRelationship> _relationships = new List<PersonRelationship>();
    public IReadOnlyCollection<PersonRelationship> Relationships => _relationships.AsReadOnly();

    public Person(string firstName, string lastName, string gender, string personalNumber, DateTime dateOfBirth)
    {
        UpdateDetails(firstName, lastName, gender, personalNumber, dateOfBirth);
    }

    public void UpdateDetails(string firstName, string lastName, string gender, string personalNumber, DateTime dateOfBirth)
    {
        if (!IsValidName(firstName) || !IsValidName(lastName))
            throw new ArgumentException("First name and last name must contain only Georgian or Latin alphabet letters and cannot mix alphabets.");

        if (gender != "ქალი" && gender != "კაცი")
            throw new ArgumentException("Gender must be either 'ქალი' or 'კაცი'.");

        if (!Regex.IsMatch(personalNumber, @"^\d{11}$"))
            throw new ArgumentException("Personal number must be exactly 11 digits.");

        if (DateTime.Now.AddYears(-18) < dateOfBirth)
            throw new ArgumentException("Person must be at least 18 years old.");

        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        PersonalNumber = personalNumber;
        DateOfBirth = dateOfBirth;
    }

    private bool IsValidName(string name)
    {
        return !string.IsNullOrWhiteSpace(name) &&
               (Regex.IsMatch(name, @"^[ა-ჰ]+$") || Regex.IsMatch(name, @"^[a-zA-Z]+$"));
    }
}