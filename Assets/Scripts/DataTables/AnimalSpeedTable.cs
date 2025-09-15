using System.Collections.Generic;
using UnityEngine;

public class AnimalSpeedTable : DataTable
{
    public class Data
    {
        public int Spd_ID { get; set; }
        public string name { get; set; }
        public float Time { get; set; }
    }

    private readonly Dictionary<int , Data> speedTable = new Dictionary<int , Data>(); 

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var datas = LoadCsv<Data>(textAsset.text);

        for(int i = 0; i < datas.Count; i++)
        {
            speedTable.Add(datas[i].Spd_ID, datas[i]);
        }
    }

    public Data Get(int id)
    {
        return speedTable[id];
    }
}