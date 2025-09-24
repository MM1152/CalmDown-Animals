using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CrewTable : DataTable
{
    public class Data
    {
        public int Crew_ID { get; set; }
        public string Crew_name { get; set; }
        public int rank_ID { get; set; }
        public bool isDupli { get; set; }
        public int crewCost { get; set; }
        public int crewPaycheck { get; set; }
        public int equType_ID { get; set; }
        public bool isEquchange { get; set; }
        public int Crew_capture { get; set; }
        public float Crew_atkspd { get; set; }
        public string Crew_info { get; set; }

        public int GetSpawnAbleRound => DataTableManager.crewRankTable.Get(rank_ID).Buyround;
        public string GetColorInfomation => DataTableManager.crewRankTable.Get(rank_ID).Color;
        public Color Color => DataTableManager.crewRankTable.Get(rank_ID).GetColor;
    }

    private readonly Dictionary<int, Data> crewTable = new Dictionary<int, Data>();

    public override void Load(string filename)
    {
        var fullpath = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(fullpath);

        var datas = LoadCsv<Data>(textAsset.text);

        foreach (var crew in datas)
        {
            crewTable.Add(crew.Crew_ID, crew);
        }
    }

    public Data Get(CrewRank crew)
    {
        if(crewTable.ContainsKey((int)crew)) {
            return crewTable[(int)crew];
        }
        return null;
    }
}