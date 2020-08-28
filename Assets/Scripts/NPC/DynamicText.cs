using System.Collections;
using System.Collections.Generic;
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

    private bool IsShow { get; set; }
    private bool Skip { get; set; }

    private string showText;
    private string[] allText;

    private void Update()
    {
        if(IsShow == false && Skip) { return; }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            Skip = true;
        }
    }

    public void ActiveDynamicText(string[] allText)
    {
        showText = "";
        Text.text = showText;

        this.allText = allText;

        IsShow = true;
        Skip = false;

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
            else if(Skip)
            {
                showText = allText[textArrayCount];
                textLengthCount = allText[textArrayCount].Length;
            }
            else
            {
                showText += allText[textArrayCount][textLengthCount];
                textLengthCount++;
            }

            Text.text = showText;
            Skip = false;
            yield return new WaitForSeconds(textSpeed);
        }

        IsShow = false;
        CanvasManager.Instance.ConversationCanvas.CloseOverlay();
    }
}
