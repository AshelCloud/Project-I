using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [System.Serializable]
    private class NPCDataTable
    {
        public string NpcName;
        public string PrefabName;
        public string Text;
        public string RandomText;
    }

    public string ID = "1";

    private string nameText;
    public string[] Texts;

    private void Awake()
    {
        var text = AssetBundleContainer.LoadAsset<TextAsset>("jsons", "NPCTable");
        var data = JsonManager.LoadJson<Serialization<string, NPCDataTable>>(text).ToDictionary();

        Texts = data[ID].Text.Split('\n');
        nameText = data[ID].NpcName;

        CanvasManager.Instance.ConversationCanvas.CloseOverlay();
    }

    public void Conversation()
    {
        CanvasManager.Instance.ConversationCanvas.OpenOverlay(Texts, nameText);
    }
}
