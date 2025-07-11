using Microsoft.EntityFrameworkCore;
using Write_from_csv_to_database.Entities;

namespace Write_from_csv_to_database.Repositories;

public class PetRepository : BaseRepository<Pet>
{
    public PetRepository(DbContext context) : base(context) { }
}
