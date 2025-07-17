using Write_from_csv_to_database.Models;

namespace Write_from_csv_to_database.Entities;

public record class Pet : BaseEntity
{
    public string? Name { get; set; }

    public Species Species { get; set; }

    public int OwnerId { get; set; }

    public Person? Owner { get; set; }
}
