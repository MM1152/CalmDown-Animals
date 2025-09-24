using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AnimalInfoTable : DataTable
{
    public class Data
    {
        public int Animal_ID { get; set; }
        public string Animal_name { get; set; }
        public int CR_ID { get; set; }
        public int Size_ID { get; set; }
        public int Spd { get; set; }
        public int CaptureHP { get; set; }
        public int Range_min { get; set; }
        public int Range_max { get; set; }
        public string Model {
            set {
                model = value;

                string pathname = model.Split('/')[2];

                Skin = Resources.Load<GameObject>(@$"{model}/{pathname}_LOD0");
                Animator = Resources.Load<RuntimeAnimatorController>(@$"{model}/AC_{pathname}");
                Avatar = Resources.Load<Avatar>(@$"{model}/{pathname}_AnimationsAvatar");

                if (Skin == null)
                {
                    Debug.Log("Fail To Load :" + Animal_name);
                }
            }
        }
        public float Spawn { get; set; }
        public string Kor_Name { get; set; }
        private string model;

        public GameObject Skin;
        public RuntimeAnimatorController Animator;
        public Avatar Avatar;
            
        public float Time => DataTableManager.animalSpeedTable.Get(Spd).Time; 
        public int MaxHp => CaptureHP;
    }

    private readonly Dictionary<int, Data> animalInfos = new Dictionary<int, Data>();
    private int index;

    public override void Load(string filename)
    {
        string path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);

        var datas = LoadCsv<Data>(textAsset.text);

        for(int i = 0; i < datas.Count; i++)
        {
            animalInfos.Add(datas[i].Animal_ID, datas[i]);
        }
    }

    public Data Get(int animal)
    {
        return animalInfos[(int)animal];
    }

    public Data GetSquentialGet()
    {
        return animalInfos.ElementAt(index++).Value;
    }
    public Data RandomGet(int CR_ID)
    {
        var list = animalInfos.Select(x => x.Value).ToList();
        var withCR_ID = list.Where(x => x.CR_ID == CR_ID).ToList();

        int rand = UnityEngine.Random.Range(0, withCR_ID.Count - 1) ;
        return withCR_ID[rand];
    }
}
