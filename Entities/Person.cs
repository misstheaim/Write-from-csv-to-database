namespace Write_from_csv_to_database.Entities;

public record class Person : BaseEntity
{
    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Email { get; set; }

    public int Age { get; set; }
}
