using Newtonsoft.Json;
using System.IO;
using UnityEditor.Overlays;
using UnityEngine;
using SaveDataVC = SaveDataV1;

public static class SaveLoadManager
{
    public static int SaveDataVersion { get; } = 1;
    public static SaveDataVC Data { get; set; } = new SaveDataVC(); //세이브 데이터 객체 유지
    private static readonly string SaveFileName = "SaveAuto.json";
    public static string SaveDirectory => $"{Application.persistentDataPath}/Save"; //세이브파일 폴더 경로

    //Format, TypeNameHandling, Converter를 미리 세팅할 수 있다.
    public static JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.All,
        Converters =
        {
            new Vector3Convertor(),
        }
    };

    static SaveLoadManager()
    {
        Load();
    }

    public static bool Save(int slot = 0)
    {
        if (Data == null || slot < 0 || slot >= SaveFileName.Length)
        {
            return false;
        }

        //간단하게 예외 처리
        try
        {
            //Directory.Exists : 해당 경로가 있는지 반환하는 함수
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }

            var path = Path.Combine(SaveDirectory, SaveFileName); //세이브파일 저장 경로
            Debug.Log(path);
            var json = JsonConvert.SerializeObject(Data, settings);
            File.WriteAllText(path, json); //원래는 예외처리가 필요하다.

            return true;
        }
        catch
        {
            Debug.LogError("Save 예외 발생");
            return false;
        }
    }

    public static bool Load(int slot = 0)
    {
        //로드는 데이터가 null이어도 상관 없다.
        if (slot < 0 || slot >= SaveFileName.Length)
        {
            return false;
        }

        var path = Path.Combine(SaveDirectory, SaveFileName);
        if (!File.Exists(path))
        {
            return false;
        }

        try
        {
            var json = File.ReadAllText(path);
            var dataSave = JsonConvert.DeserializeObject<SaveData>(json, settings);

            while (dataSave.Version < SaveDataVersion) //현재버전까지 업데이트 반복
            {
                dataSave = dataSave.VersionUp();
            }
            Data = dataSave as SaveDataVC;

            return true;
        }
        catch
        {
            Debug.LogError("Load 예외 발생");
            return false;
        }
    }
}