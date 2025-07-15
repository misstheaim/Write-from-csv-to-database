using CsvHelper.Configuration;
using Write_from_csv_to_database.Models;

namespace Write_from_csv_to_database.Dependencies.Mappings;

internal class PetMap : ClassMap<Pet>
{
    public PetMap()
    {
        Map(p => p.Id).Name("PetId");
        Map(p => p.Name).Name("PetName");
        Map(p => p.Species).Convert(args => (Species)Enum.Parse(typeof(Species), args.Row.GetField("Species")));
        Map(p => p.OwnerId).Name("OwnerId");

        Map(p => p.Owner).Convert(args => new Person()
        {
            Id = int.Parse(args.Row.GetField("PersonId")),
            Name = args.Row.GetField("OwnerName"),
            Surname = args.Row.GetField("Surname"),
            Email = args.Row.GetField("Email"),
            Age = int.Parse(args.Row.GetField("Age")),
        });
    }
}
