using CSV_Enumerable.Models;
using CSV_Enumerable.Services;
using Generic_object_mapper;
using Logging_Proxy;
using Microsoft.EntityFrameworkCore;
using Stream_Processing;
using Write_from_csv_to_database.Repositories;

namespace Write_from_csv_to_database.Services;

internal class PetService
{
    private IRepository<Entities.Pet> repository;

    private Logger _logger;

    public PetService(DbContext context, Logger logger)
    {
        LoggingProxy<IRepository<Entities.Pet>> petLoggingRepository = new(logger);
        repository = petLoggingRepository.CreateInstance(new PetRepository(context));
        _logger = logger;
    }

    public async Task AddDataAsync(string path)
    {
        var csv = new CsvEnumerable<Pet>(path);
        foreach (var item in csv)
        {
            Entities.Pet? existingPet = await repository.GetByIdAsync(item.Id);
            if (existingPet is not null)
            {
                _logger.Log($"Item with id: {item.Id} is already existing in the database.");
                continue;
            }
            else
            {
                Entities.Pet entity = Mapper.Map<Pet, Entities.Pet>(item, item => {
                    var result = Mapper.Map<Pet, Entities.Pet>(item);
                    result.Owner = Mapper.Map<Person, Entities.Person>(item.Owner);
                    return result;});
                entity.OwnerId = 0;
                entity.Id = 0;
                entity.Owner.Id = 0;
                await repository.CreateAsync(entity);
            }
        }
    }

    public async Task<Entities.Pet> GetDataById(int id)
    {
        Entities.Pet? entity = await repository.GetByIdAsync(id);

        if (entity is null)
        {
            _logger.Log($"Item with id: {id} is already existing in the database.");
            throw new KeyNotFoundException($"Item with id: {id} is already existing in the database.");
        }

        return entity;
    }

    public async Task<List<Pet>> GetDataAsync()
    {
        List<Pet> pets = new List<Pet>();

        List<Entities.Pet> entities = await repository.GetListAsync();

        foreach (Entities.Pet entity in entities)
        {
            pets.Add(Mapper.Map<Entities.Pet, Pet>(entity, entity => {
                var result = Mapper.Map<Entities.Pet, Pet>(entity);
                result.Owner = Mapper.Map<Entities.Person, Person>(entity.Owner);
                return result;
            }));
        }

        return pets;
    }

    public Task DeleteDataAsync(int id)
    {
        return repository.DeleteAsync(id);
    }

    public Task UpdateDataAsync(Entities.Pet pet)
    {
        return repository.UpdateAsync(pet);
    }
}
