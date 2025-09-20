using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
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
        public int Range_min { get; set; }
        public int Range_max { get; set; }
        public string Model {
            set {
                model = value;

                Skin = Resources.Load<GameObject>(model);
                Animator = Resources.Load<RuntimeAnimatorController>(model + "_Animator");
                Avatar = Resources.Load<Avatar>(model + "_Avatar");
            }
        }
        public float Spawn { get; set; }

        private string model;

        public GameObject Skin;
        public RuntimeAnimatorController Animator;
        public Avatar Avatar;
            
        public float Time => DataTableManager.animalSpeedTable.Get(Spd).Time; 
        public int MaxHp => DataTableManager.animalCRRankTable.Get(CR_ID).Base_capture + DataTableManager.animalSizeTable.Get(Size_ID).Add_capture;
    }

    private readonly Dictionary<int, Data> animalInfos = new Dictionary<int, Data>();

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

    public Data Get(AnimalTypes animal)
    {
        return animalInfos[(int)animal];
    }

    public Data RandomGet(int CR_ID)
    {
        var list = animalInfos.Select(x => x.Value).ToList();
        var withCR_ID = list.Where(x => x.CR_ID == CR_ID);

        int rand = UnityEngine.Random.Range(0, withCR_ID.Count() - 1) ;
        return withCR_ID.ElementAt(rand);
    }

}
