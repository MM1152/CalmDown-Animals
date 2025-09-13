using System.Collections.Generic;
using UnityEngine;

public class StringTable : DataTable
{
    public class Data
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    private readonly Dictionary<int , string> stringTable = new Dictionary<int , string>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var datas = LoadCsv<Data>(textAsset.text);

        foreach(var data in datas)
        {
            if(!stringTable.ContainsKey(data.Id))
            {
                stringTable.Add(data.Id, data.Name);
            }
            else
            {
#if DEV_MODE
                Debug.Log("Fail To Load");
#endif
            }
        }
    }

    public string Get(int key)
    {
        if(stringTable.ContainsKey(key))
        {
            return stringTable[key];
        }

        return "Fail To Get";
    }
}