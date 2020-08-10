using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;

public class ResourcesContainer
{
    private static Dictionary<string, Object> _cache;
    private static Dictionary<string, Object> Cache
    {
        get
        {
            if(_cache == null)
            {
                _cache = new Dictionary<string, Object>();
            }
            return _cache;
        }
        set
        {
            _cache = value;
        }
    }

    private static string GetFileName(string _path)
    {
        string[] splitsPath = _path.Split('/');
        string name = splitsPath[splitsPath.Length - 1];

        return name;
    }

    public static T Load<T>(string _path) where T : Object
    {
        string name = GetFileName(_path);
        if(Cache.ContainsKey(name))
        {
            return (T)Cache[name];
        }

        T _t = Resources.Load<T>(_path);
        if(_t == null)
        {
            return default;
        }
        Cache[name] = _t;

        return _t;
    }

    public static Object Load(string _path)
    {
        string name = GetFileName(_path);
        if(Cache.ContainsKey(name))
        {
            return Cache[name];
        }

        Object _t =  Resources.Load(_path);
        if(_t == null)
        {
            return default;
        }
        Cache[name] = _t;

        return _t;
    }

    public static T LoadInCache<T>(string _name) where T : Object
    {
        if(Cache.ContainsKey(_name))
        {
            return (T)Cache[_name];
        }

        return default;
    }

    public static void LoadAll(string subFloderPath)
    {
        Object[] _t = Resources.LoadAll(subFloderPath);
        foreach(var t in _t)
        {
            Cache[t.name] = t;
        }
    }

    //public static GameObject Instance(string key)
    //{
    //    GameObject go = Instantiate(Cache[key]);

    //    return go;
    //}

    //public static GameObject Instance(string key, string _name)
    //{
    //    GameObject go = Instantiate(Cache[key]);
    //    go.name = _name;

    //    return go;
    //}

    //public static GameObject Instance(string key, Transform parent)
    //{
    //    GameObject go = Instantiate(Cache[key], parent);

    //    return go;
    //}

    //public static GameObject Instance(string key, string _name, Vector3 pos)
    //{
    //    GameObject go = Instantiate(Cache[key], pos, Quaternion.identity);
    //    go.name = _name;

    //    return go;
    //}

    //public static GameObject Instance(string key, string _name, Vector3 pos, Quaternion rot)
    //{
    //    GameObject go = Instantiate(Cache[key], pos, rot);
    //    go.name = _name;

    //    return go;
    //}
}
