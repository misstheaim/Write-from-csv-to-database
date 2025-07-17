using System.Runtime.Serialization;

namespace Write_from_csv_to_database.Models;

[DataContract]
public record class Person : BaseModel
{
    [DataMember]
    public int Id { get; set; }
    [DataMember]
    public string? Name { get; set; }
    [DataMember]
    public string? Surname { get; set; }
    [DataMember]
    public string? Email { get; set; }
    [DataMember]
    public int Age { get; set; }
}
