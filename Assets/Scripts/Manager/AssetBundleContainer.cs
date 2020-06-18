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

    public static AssetBundle LoadFromFile(string path)
    {
        if(Bundles.ContainsKey(path))
        {
            return Bundles[path];
        }
        AssetBundle localAssetBundle =  AssetBundle.LoadFromFile(path);

        Bundles.Add(path, localAssetBundle);

        return localAssetBundle;
    }
}
