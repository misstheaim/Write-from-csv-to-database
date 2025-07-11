using Logging_Proxy;
using Microsoft.EntityFrameworkCore;
using Stream_Processing;
using Write_from_csv_to_database.Entities;
using Write_from_csv_to_database.Repositories;

namespace Write_from_csv_to_database.Services;

internal class PersonService
{
    private IRepository<Person> repository;
    private Logger _logger;

    public PersonService(DbContext context, Logger logger)
    {
        LoggingProxy<IRepository<Person>> personLoggingRepository = new(_logger);
        repository = personLoggingRepository.CreateInstance(new PersonRepository(context));
        _logger = logger;
    }
}
