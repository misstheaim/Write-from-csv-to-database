using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Write_from_csv_to_database.Models;

[JsonDerivedType(typeof(BaseModel), typeDiscriminator: "base")]
[JsonDerivedType(typeof(Person), typeDiscriminator: "person")]
[JsonDerivedType(typeof(Pet), typeDiscriminator: "pet")]
[DataContract]
public record class BaseModel
{
}
