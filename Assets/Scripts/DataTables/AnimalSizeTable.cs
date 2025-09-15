using System.Collections.Generic;
using UnityEngine;

public class AnimalSizeTable : DataTable
{
    public class Data
    {
        public int Size_ID { get; set; }
        public string name { get; set; }
        public int Add_capture { get; set; }
    }

    private readonly Dictionary<int, Data> sizeTable = new Dictionary<int, Data>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var datas = LoadCsv<Data>(textAsset.text);

        for (int i = 0; i < datas.Count; i++)
        {
            sizeTable.Add(datas[i].Size_ID, datas[i]);
        }
    }

    public Data Get(int id)
    {
        return sizeTable[id];
    }
}