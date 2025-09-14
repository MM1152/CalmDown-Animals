using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Map;

public class Map
{
    private readonly static string mapDataPath = "MapDatas/{0}";

    public static List<MapData> mapDatas = new List<MapData>(); 

    static Map()
    {
        Load();
    }

    public class DrawData
    {
        public DrawType DrawType { get; set; }
        public Vector3 Rotation { get; set; } = Vector3.zero;
        public Vector3 Position { get; set; } = Vector3.zero;
        public Vector3 ConnectTile { get; set; } = Vector3.zero;
    }

    public class MapData
    {
        public string Id { get; set; }
        public Dictionary<int, List<DrawData>> tiles = new Dictionary<int, List<DrawData>>();
    }


    private static JsonSerializerSettings settings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        Converters = new List<JsonConverter>
        {
            new Vector3Convertor(),
        }
    };

    public static MapData CreateMapData(string id, List<List<DrawTile>> tiles)
    {
        MapData data = new MapData();

        data.Id = id;
        for(int i = 0; i < tiles.Count; i++)
        {
            data.tiles.Add(i, new List<DrawData>());
            for(int j = 0; j < tiles[i].Count; j++)
            {
                DrawData drawData = new DrawData()
                {
                    DrawType = tiles[i][j].DrawType,
                    Rotation = tiles[i][j].transform.eulerAngles,
                    Position = tiles[i][j].transform.position,
                    ConnectTile = tiles[i][j].ConnectTile != null ? tiles[i][j].ConnectTile.transform.position : Vector3.up,
                };

                data.tiles[i].Add(drawData);
            }
        }

        return data;
    }
 
    public static void Save(int mapIndex , MapData data)
    {
        string path = string.Format(mapDataPath, DataTableIds.MapDataIds[mapIndex]);
        string dir = Path.Combine(Application.persistentDataPath, "MapDatas");
        Debug.Log(dir);
        string filepath = Path.Combine(Application.persistentDataPath, path);
        string json = JsonConvert.SerializeObject(data , settings);
        
        if(!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        File.WriteAllText(filepath, json);
        Debug.Log($"Save {filepath}");
    }

    public static void Load()
    {
        for(int i = 0; i < DataTableIds.MapDataIds.Length; i++)
        {
            string path = string.Format(mapDataPath, DataTableIds.MapDataIds[i]);
            string fullpath = Path.Combine(Application.persistentDataPath , path);

            if (File.Exists(fullpath))
            {
                string json = File.ReadAllText(fullpath);
                var data = JsonConvert.DeserializeObject<MapData>(json, settings);
                mapDatas.Add(data);
            }
        }
    }

    public static void Get()
    {

    }
}