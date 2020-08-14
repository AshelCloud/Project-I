using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleContainer
{
    private static Dictionary<string, AssetBundle> _bundles;
    private static Dictionary<string, AssetBundle> Bundles 
    {
        get
        {
            if(_bundles == null)
            {
                _bundles = new Dictionary<string, AssetBundle>();
            }
            return _bundles;
        }

        set
        {
            _bundles = value;
        }
    }
    private static string GetFileName(string _path)
    {
        string[] splitsPath = _path.Split('\\');
        string name = splitsPath[splitsPath.Length - 1];

        return name;
    }

    public static AssetBundle LoadFromFile(string path)
    {
        var name = GetFileName(path);

        if(Bundles.ContainsKey(name))
        {
            return Bundles[name];
        }
        AssetBundle localAssetBundle =  AssetBundle.LoadFromFile(path);

        Bundles.Add(name, localAssetBundle);

        return localAssetBundle;
    }

    public static T LoadAsset<T>(string bundleName, string assetName) where T : Object
    {
        T asset = Bundles[bundleName].LoadAsset<T>(assetName);

        return asset;
    }
}
