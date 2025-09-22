using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class EquipmentInfo : DataTable
{
    public class Data
    {
        public int Equ_ID { get; set; }
        public int rank_ID { get; set; }
        public int equType_ID { get; set; }
        public string name { get; set; }
        public int Equ_capture { get; set; }
        public float Equ_atkspd { get; set; }
        public int atk_range { get; set; }

        public AnimalSize CaptureSize => DataTableManager.equipmentTypeTable.Get(equType_ID).captureAbleSize; 
    }

    private Dictionary<int , Data> equipInfoTable = new Dictionary<int , Data>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var datas = LoadCsv<Data>(textAsset.text);

        foreach(var data in datas)
        {
            if (!equipInfoTable.ContainsKey(data.Equ_ID))
                equipInfoTable.Add(data.Equ_ID, data);
        }
    }

    public Data Get(int equipmentId)
    {
        return equipInfoTable[equipmentId];
    }

    public List<Data> GetRankIdToEquipment(int rank_Id)
    {
        return equipInfoTable.Where(x => rank_Id == x.Value.rank_ID).Select(x => x.Value).ToList();
    }
}