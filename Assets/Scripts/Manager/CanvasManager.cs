using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    private ConversationCanvas _conversationCanvas;
    public ConversationCanvas ConversationCanvas
    {
        get
        {
            if(_conversationCanvas == null)
            {
                _conversationCanvas = FindObjectOfType(typeof(ConversationCanvas)) as ConversationCanvas;

                if(_conversationCanvas == null)
                {
                    var loadCanvas = ResourcesContainer.Load<ConversationCanvas>("Prefabs/Canvases/ConversationCanvas");
                    _conversationCanvas = Instantiate(loadCanvas);
                }
            }

            return _conversationCanvas;
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
