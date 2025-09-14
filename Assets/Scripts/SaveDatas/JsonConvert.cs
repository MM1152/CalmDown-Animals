using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using static Map;

public class Vector3Convertor : JsonConverter<Vector3>
{
    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Vector3 vec = Vector3.zero;
        JObject jObj = JObject.Load(reader);
        vec.x = (float)jObj["x"];
        vec.y = (float)jObj["y"];
        vec.z = (float)jObj["z"];
        return vec;
    }

    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("z");
        writer.WriteValue(value.z);
        writer.WriteEndObject();
    }
}


public class DrawDataConvertor : JsonConverter<Map.DrawData>
{
    public override Map.DrawData ReadJson(JsonReader reader, Type objectType, Map.DrawData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        DrawData data = new DrawData();
        JObject jObj = JObject.Load(reader);
        data.Position = new Vector3(jObj["Position"].Value<float>("x") , jObj["Position"].Value<float>("y") , jObj["Position"].Value<float>("z"));
        return data;
    }

    public override void WriteJson(JsonWriter writer, Map.DrawData value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WriteRaw("Position");
        writer.WritePropertyName("x");
        writer.WriteValue(value.Position.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.Position.y);
        writer.WritePropertyName("z");
        writer.WriteValue(value.Position.z);

        //writer.WritePropertyName("Rotation");
        //writer.WritePropertyName("x");
        //writer.WriteValue(value.Rotation.x);
        //writer.WritePropertyName("y");
        //writer.WriteValue(value.Rotation.y);
        //writer.WritePropertyName("z");
        //writer.WriteValue(value.Rotation.z);

        //writer.WritePropertyName("Rotation");
        //writer.WritePropertyName("x");
        //writer.WriteValue(value.DrawType);

        writer.WriteEndObject();
    }
}