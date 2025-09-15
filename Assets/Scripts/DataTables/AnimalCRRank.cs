using System.Collections.Generic;
using UnityEngine;

public class AnimalCRRank : DataTable
{
    public class Data
    {
        public int CR_ID { get; set; }
        public string name { get; set; }
        public int Base_capture { get; set; }
    }

    private readonly Dictionary<int, Data> CRrank = new Dictionary<int, Data>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var datas = LoadCsv<Data>(textAsset.text);

        for (int i = 0; i < datas.Count; i++)
        {
            CRrank.Add(datas[i].CR_ID, datas[i]);
        }
    }

    public Data Get(int id)
    {
        return CRrank[id];
    }
}