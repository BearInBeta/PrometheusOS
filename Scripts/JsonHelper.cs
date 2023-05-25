using System;
using UnityEngine;


public static class JsonHelper
{
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

    public static T[] FromJson<T>(string json)
    {
        return JsonUtility.FromJson<JsonHelper.Wrapper<T>>(json).Items;
    }

    public static string ToJson<T>(T[] array)
    {
        return JsonUtility.ToJson(new JsonHelper.Wrapper<T>
        {
            Items = array
        });
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        return JsonUtility.ToJson(new JsonHelper.Wrapper<T>
        {
            Items = array
        }, prettyPrint);
    }
}
