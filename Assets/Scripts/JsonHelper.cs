using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonHelper
{
    public static T[] getJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        JSONArrayWrapper<T> wrapper = JsonUtility.FromJson<JSONArrayWrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class JSONArrayWrapper<T>
    {
        public T[] array;
    }
}
