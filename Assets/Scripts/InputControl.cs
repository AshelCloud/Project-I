using System;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    private Dictionary<KeyCode, Action> keyDictionary;

    [SerializeField]
    private Player player;

    void Start()
    {
        keyDictionary = new Dictionary<KeyCode, Action>
        {
            { KeyCode.LeftArrow, Left},
            { KeyCode.RightArrow, Right },
            { KeyCode.A, Key_A }
        };
    }

    void Update()
    {
        
    }

    private void Left()
    {
    }

    private void Right()
    {

    }

    private void Key_A()
    {

    }
}
