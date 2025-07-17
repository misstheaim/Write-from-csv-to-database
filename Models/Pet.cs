using System.Runtime.Serialization;
using Write_from_csv_to_database.Dependencies.Attributes;
using Write_from_csv_to_database.Dependencies.Mappings;

namespace Write_from_csv_to_database.Models;

[CsvMap(typeof(PetMap))]
[DataContract]
public record class Pet : BaseModel
{
    [DataMember]
    public int Id { get; set; }
    [DataMember]
    public string? Name { get; set; }
    [DataMember]
    public Species Species { get; set; }
    [DataMember]
    public int OwnerId { get; set; }
    [DataMember]
    public Person? Owner { get; set; }
}
