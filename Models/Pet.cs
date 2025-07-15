using Write_from_csv_to_database.Dependencies.Attributes;
using Write_from_csv_to_database.Dependencies.Mappings;

namespace Write_from_csv_to_database.Models;

[CsvMap(typeof(PetMap))]
public record class Pet
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public Species Species { get; set; }

    public int OwnerId { get; set; }

    public Person? Owner { get; set; }
}
