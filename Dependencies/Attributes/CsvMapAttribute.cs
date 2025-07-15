namespace Write_from_csv_to_database.Dependencies.Attributes;

internal class CsvMapAttribute : Attribute
{
    public Type CsvMapType { get; }

    public CsvMapAttribute(Type type)
    {
        CsvMapType = type;
    }
}
