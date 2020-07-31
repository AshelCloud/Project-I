using System.Collections;
using System.IO;
using UnityEngine;

public class Loader : MonoBehaviour
{
    private void Start()
    {
        AssetBundle result = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));
        if(result == null)
        {
            Log.PrintError("Failed to Load Jsons");
        }
    }
}
