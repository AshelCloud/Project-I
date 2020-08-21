using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationCanvas : MonoBehaviour
{
    public GameObject overlay;
    public Text nameText;
    public Text conversationText;

    public void OpenOverlay(string[] allText, string nameText)
    {
        GetComponent<Canvas>().enabled = true;

        this.nameText.text = nameText;
        conversationText.GetComponent<DynamicText>().ActiveDynamicText(allText);
    }

    public void CloseOverlay()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
