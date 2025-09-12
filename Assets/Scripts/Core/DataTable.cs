using System.IO;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public abstract class DataTable
{
    public static readonly string FormatPath = "DataTables/{0}";

    public abstract void Load(string filename);
    public static List<T> LoadCsv<T>(string csvText)
    {
        using (var reader = new StringReader(csvText))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var recodes = csvReader.GetRecords<T>();
            return recodes.ToList();
        }
    }
} 

