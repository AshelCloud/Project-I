using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [System.Serializable]
    private class NPCDataTable
    {
        public string NpcName = null;
        public string PrefabName = null;
        public string Text = null;
        public string RandomText = null;
    }

    public string ID = "1";

    private string nameText;
    public string[] Texts;

    public ConversationCanvas conversationCanvas;

    public bool  InConversation { get; set; }

    private void Awake()
    {
        var text = AssetBundleContainer.LoadAsset<TextAsset>("jsons", "NPCTable");
        if(text == null)
        {
            AssetBundle result = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));

            text = result.LoadAsset<TextAsset>("NPCTable");
        }
        var data = JsonManager.LoadJson<Serialization<string, NPCDataTable>>(text).ToDictionary();

        Texts = data[ID].Text.Split('\n');
        nameText = data[ID].NpcName;

        conversationCanvas.Initialize();

    }
    private void Start()
    {
        conversationCanvas.CloseOverlay();
    }

    public void Conversation()
    {
        if(InConversation) { return; }

        StartCoroutine(ConversationCoroutine());
    }

    private IEnumerator ConversationCoroutine()
    {
        conversationCanvas.OpenOverlay(Texts, nameText);
        InConversation = true;

        yield return new WaitUntil( ()=> InConversation  == false);

        conversationCanvas.CloseOverlay();
    }
}
