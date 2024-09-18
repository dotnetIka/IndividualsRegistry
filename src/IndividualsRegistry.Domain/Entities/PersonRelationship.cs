using IndividualsRegistry.Shared.Library;
using System.ComponentModel.DataAnnotations;

namespace IndividualsRegistry.Domain.Entities;

public sealed class PersonRelationship : Entity
{
    public int PersonId { get; private set; }
    public Person Person { get; private set; }

    public int RelatedPersonId { get; private set; }
    public Person RelatedPerson { get; private set; }

    [Required]
    [RegularExpression(@"^(კოლეგა|ნაცნობი|ნათესავი|სხვა)$", ErrorMessage = "Relation type must be either 'კოლეგა', 'ნაცნობი', 'ნათესავი', or 'სხვა'.")]
    public string RelationType { get; private set; }

    private PersonRelationship() { } // For ORM

    public PersonRelationship(int personId, int relatedPersonId, string relationType)
    {
        if (personId == relatedPersonId)
            throw new InvalidOperationException("A person cannot have a relationship with themselves.");

        if (string.IsNullOrWhiteSpace(relationType) || !new[] { "კოლეგა", "ნაცნობი", "ნათესავი", "სხვა" }.Contains(relationType))
            throw new ArgumentException("Invalid relation type.");

        PersonId = personId;
        RelatedPersonId = relatedPersonId;
        RelationType = relationType;
    }
}