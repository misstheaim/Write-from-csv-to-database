namespace Write_from_csv_to_database.Models;

public record class Person
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Email { get; set; }

    public int Age { get; set; }
}
