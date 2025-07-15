using System.Collections;

namespace CSV_Enumerable.Services;

public class CsvEnumerable<T> : IEnumerable<T> where T : class
{
    private readonly string _filePath;

    public CsvEnumerable(string filePath)
    {
        _filePath = filePath;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new CsvEnumerator<T>(_filePath);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
