using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationCanvas : MonoBehaviour
{
    public GameObject overlay;
    public Text nameText;
    public Text conversationText;

    private Canvas canvas;
    private NPC Root { get; set; }
    private DynamicText dynamicText;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        Root = transform.root.GetComponent<NPC>();
        dynamicText = conversationText.GetComponent<DynamicText>();
    }

    public void OpenOverlay(string[] allText, string nameText)
    {
        if(canvas.enabled) { return ; }

        canvas.enabled = true;

        this.nameText.text = nameText;
        dynamicText.ActiveDynamicText(allText);

        StartCoroutine(CheckEndText());
    }

    private IEnumerator CheckEndText()
    {
        yield return new WaitUntil( () => conversationText.GetComponent<DynamicText>().IsShow == false);

        Root.InConversation = false;
    }

    public void CloseOverlay()
    {
        canvas.enabled = false;
    }
}
