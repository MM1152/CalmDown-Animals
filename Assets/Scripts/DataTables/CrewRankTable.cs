using System.Collections.Generic;
using UnityEngine;

public class CrewRankTable : DataTable
{
    public class Data
    {
        public int rank_ID { get; set; }
        public string name { get; set; }
        public string Color { get; set; }
        public int Buyround { get; set; }
    }

    private Dictionary<int , Data> rankTable = new Dictionary<int , Data>();

    public override void Load(string filename)
    {
        //var path = string.Format(FormatPath, filename);
        //var textAssets = Resources.Load<TextAsset>(path);
        //var datas = LoadCsv<Data>(textAssets.text);

        //foreach(var data in datas)
        //{
        //    rankTable.Add(data.rank_ID, data);
        //}
    }

    public Data Get(int rank_Id)
    {
        return rankTable[rank_Id];
    }
}