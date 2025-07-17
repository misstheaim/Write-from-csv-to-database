using CsvHelper;
using System.Collections;
using System.Globalization;
using Write_from_csv_to_database.Dependencies.Attributes;

namespace CSV_Enumerable.Services;

public class CsvEnumerator<T> : IEnumerator<T> where T : class
{
    private readonly StreamReader _stream;
    private readonly CsvReader _reader;
    private T? _current;

    public CsvEnumerator(string filePath)
    {
        _stream = new StreamReader(filePath);
        _reader = new CsvReader(_stream, CultureInfo.InvariantCulture);

        CsvMapAttribute mapType = (CsvMapAttribute)typeof(T).GetCustomAttributes(typeof(CsvMapAttribute), false).First();
        _reader.Context.RegisterClassMap(mapType.CsvMapType);

        _reader.Read();
        _reader.ReadHeader();
    }

    public T Current 
    {
        get
        {
            if (_current == null || _stream == null || _reader == null)
            {
                throw new InvalidOperationException();
            }
            return _current;
        }
    }

    object? IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (!_reader.Read())
        {
            return false;
        }
        _current = _reader.GetRecord<T>();
        return true;
    }

    public void Reset()
    {
        _stream.DiscardBufferedData();
        _stream.BaseStream.Seek(0, SeekOrigin.Begin);
        _reader.ReadHeader();
        _reader.Read();
        _current = null;
    }

    public void Dispose()
    {
        _current = null;
        if (_reader != null )
        {
            _reader.Dispose();
        }
        if (_stream != null)
        {
            _stream.Close();
            _stream.Dispose();
        }
    }
}
