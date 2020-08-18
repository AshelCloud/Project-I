using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    void Start()
    {
        var text = AssetBundleContainer.LoadAsset<TextAsset>("jsons", "NPCTable");

        Log.Print(text.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
