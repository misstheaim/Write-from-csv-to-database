using Stream_Processing;
using Write_from_csv_to_database.Models;
using Write_from_csv_to_database.Services;

namespace Write_from_csv_to_database
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Logger logger = new Logger();
            using ApplicationContext context = new ApplicationContext();
            PetService petService = new PetService(context, logger);
            PersonService personService = new PersonService(context, logger);

            string path = "Pets_and_Owners.csv";

            try
            {
                Console.WriteLine("\nDatabase before filling with data.\n");
                await petService.AddDataAsync(path);

                List<Pet> pets = await petService.GetDataAsync();

                Console.WriteLine("\nDatabase after filling with data.\n");
                foreach (Pet pet in pets)
                {
                    Console.WriteLine(pet.ToString());
                }

                await petService.DeleteDataAsync(pets.First().Id);

                Entities.Pet newPet = await petService.GetDataById(pets[1].Id);
                newPet.Name = "Lazanya";
                await petService.UpdateDataAsync(newPet);

                Console.WriteLine("\nDatabase after updating the data.\n");
                pets = await petService.GetDataAsync();
                foreach (Pet pet in pets)
                {
                    Console.WriteLine(pet.ToString());
                }
            }
            catch (ArgumentException)
            {
                logger.ErrorLog("Was given a wrong path to the csv.");
            }
            catch (FileNotFoundException ex)
            {
                logger.ErrorLog(ex.Message);
            }
            catch (Exception ex)
            {
                logger.ErrorLog(ex.Message);
                if (ex.InnerException != null)
                {
                    logger.ErrorLog($"{ex.InnerException.Message}");
                }
            }

            logger.Dispose();
            Console.WriteLine("\n\n\nEnter any key to exit.");
            Console.ReadKey();
        }
    }
}
