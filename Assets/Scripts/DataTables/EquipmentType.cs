using System.Collections.Generic;
using UnityEngine;

public class EquipmentType : DataTable
{
    public class Data
    {
        public int equType_ID { get; set; }
        public string name { get; set; }
        public int Size_ID { get; set; }
        public bool iscaptrue { get; set; }

        public AnimalSize captureAbleSize;
    }

    private Dictionary<int , Data> equipmentType = new Dictionary<int , Data>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAssets = Resources.Load<TextAsset>(path);
        var datas = LoadCsv<Data>(textAssets.text);

        foreach(var data in datas)
        {
            if(!equipmentType.ContainsKey(data.equType_ID))
            {
                equipmentType.Add(data.equType_ID, data);
                data.captureAbleSize |= (AnimalSize)(1 << data.Size_ID);
            }else
            {
                equipmentType[data.equType_ID].captureAbleSize |= (AnimalSize)(1 << data.Size_ID);
            }
        }
    }

    public Data Get(int equipmentId)
    {
        return equipmentType[equipmentId];
    }
}