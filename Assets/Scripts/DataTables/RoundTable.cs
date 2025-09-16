using UnityEngine;
using System.Collections.Generic;

public class RoundTable : DataTable
{
    public class Data
    {
        public int Round { get; set; }
        public int Map_Size { get; set; }
        public int CR_ID1 { get; set; }
        public int CR_ID2 { get; set; }
        public int CR_ID3 { get; set; }
        public int CR_ID4 { get; set; }
        public int CR_ID5 { get; set; }
        public int CR_ID6 { get; set; }
        public int RewardGold { get; set; }
        public bool IsUnavail { get; set; }
    }

    private readonly List<Data> RoundData = new List<Data>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var datas = LoadCsv<Data>(textAsset.text);

        for(int i = 0; i < datas.Count; i++)
        {
            RoundData.Add(datas[i]);
        }
    }

    public Data Get(int round)
    {
        return RoundData[round];
    }
}
