using System.IO.Compression;

namespace ImdbDataLoader;

public class GzipFileReader : IDisposable
{
    private readonly FileStream _stream;
    private readonly BufferedStream _buffer;
    private readonly GZipStream _gzip;
    private readonly StreamReader _reader;

    public GzipFileReader(string fileName)
    {
        _stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        _buffer = new BufferedStream(_stream, 1048576);
        _gzip   = new GZipStream(_stream, CompressionMode.Decompress);
        _reader = new StreamReader(_gzip);
    }

    public void Dispose()
    {
        _reader.Dispose();
        _gzip.Dispose();
        _buffer.Dispose();
        _stream.Dispose();
    }

    public IEnumerable<string[]> Read(bool discardHeader)
    {
        if (discardHeader)
            _reader.ReadLine();

        for (;;)
        {
            var line = _reader.ReadLine()?.Trim();
            if (line == null)
                yield break;
            if (string.IsNullOrEmpty(line))
                continue;
            
            var fields = line.Split('\t', StringSplitOptions.TrimEntries);
            yield return fields;
        }
    }

    public double PercentRead => (double)_stream.Position / _stream.Length;
}