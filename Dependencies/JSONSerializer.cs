using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Write_from_csv_to_database.Models;

namespace Write_from_csv_to_database.Dependencies;

internal class JSONSerializer
{
    JsonSerializerOptions options;

    DataContractJsonSerializerSettings settings;

    public JSONSerializer() 
    {
        options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        settings = new DataContractJsonSerializerSettings()
        {
            UseSimpleDictionaryFormat = true,
        };
    }

    public JSONSerializer(JsonSerializerOptions options, DataContractJsonSerializerSettings settings)
    {
        this.options = options;
        this.settings = settings;
    }

    public async Task<BaseModel?> ReadJsonAsync(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File on path {path} doesn't exists");
        }

        using FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);

        stream.Seek(0, SeekOrigin.Begin);

        BaseModel? model = await JsonSerializer.DeserializeAsync<BaseModel>(stream, options);

        return model;
    }

    public async Task WriteJsonAsync(string path, BaseModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (File.Exists(path))
        {
            File.WriteAllText(path, String.Empty);
        }

        using FileStream stream = File.Open(path, FileMode.OpenOrCreate);

        stream.Seek(0, SeekOrigin.Begin);

        await JsonSerializer.SerializeAsync(stream, model, options);
    }

    public void WriteJson(string path, BaseModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (File.Exists(path))
        {
            File.WriteAllText(path, String.Empty);
        }

        using FileStream stream = File.Open(path, FileMode.OpenOrCreate);

        stream.Seek(0, SeekOrigin.Begin);

        using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, System.Text.Encoding.UTF8, true, true, "   "))
        {

            var serializator = new DataContractJsonSerializer(model.GetType(), settings);

            serializator.WriteObject(writer, model);
        }
    }

    public BaseModel? ReadJson<T>(string path) where T : BaseModel
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File on path {path} doesn't exists");
        }

        using FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);

        stream.Seek(0, SeekOrigin.Begin);

        var serializator = new DataContractJsonSerializer(typeof(T), settings);

        T? model = (T?)serializator.ReadObject(stream);

        return model;
    }
}
