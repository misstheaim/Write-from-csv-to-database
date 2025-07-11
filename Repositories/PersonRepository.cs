using Microsoft.EntityFrameworkCore;
using Write_from_csv_to_database.Entities;

namespace Write_from_csv_to_database.Repositories;

public class PersonRepository : BaseRepository<Person>
{
    public PersonRepository(DbContext context) : base(context) { }
}
