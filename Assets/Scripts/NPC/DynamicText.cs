using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class DynamicText : MonoBehaviour
{
    public float textSpeed = 0.3f;
    public float waitTime = 1f;

    private Text _text;
    private Text Text
    {
        get
        {
            if(_text == null)
            {
                _text = GetComponent<Text>();
            }

            return _text;
        }
    }

    private bool showing;
    private bool skip;

    private string showText;
    private string[] allText;

    private void Update()
    {
        if(showing == false && skip) { return; }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            skip = true;
        }
    }

    public void ActiveDynamicText(string[] allText)
    {
        showText = "";
        Text.text = showText;

        this.allText = allText;

        showing = true;
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        int textCount = allText.Length;

        int textArrayCount = 0;
        int textLengthCount = 0;

        while(textCount > textArrayCount)
        {
            if(textLengthCount >= allText[textArrayCount].Length)
            {
                textArrayCount ++;
                textLengthCount = 0;

                yield return new WaitForSeconds(waitTime);
              
                showText = "";
            }
            else if(skip)
            {
                showText = allText[textArrayCount];
                textLengthCount = allText[textArrayCount].Length;

                skip = false;
            }
            else
            {
                showText += allText[textArrayCount][textLengthCount];
                textLengthCount++;
            }

            Text.text = showText;
            yield return new WaitForSeconds(textSpeed);
        }

        showing = false;
        CanvasManager.Instance.ConversationCanvas.CloseOverlay();
    }
}
