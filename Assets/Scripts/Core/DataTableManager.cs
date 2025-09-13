using System.Collections.Generic;
using UnityEngine;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables
        = new Dictionary<string, DataTable>();

    public static StringTable stringTable => Get<StringTable>(DataTableIds.StringTableIds);


    static DataTableManager()
    {
        Init();
    }

    private static void Init()
    {
        StringTable stringTable = new StringTable();
        stringTable.Load(DataTableIds.StringTableIds);

        tables.Add(DataTableIds.StringTableIds , stringTable);
    }

    public static T Get<T>(string id) where T : DataTable
    {
        if(!tables.ContainsKey(id))
        {
#if DEV_MODE
            Debug.Log("Fail to Load");
#endif
            return null;
        }

        return tables[id] as T;
    }
}
