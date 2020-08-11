﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    private void Update()
    {
    }

    public void SetField(Sprite icon, string slotName)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = icon;
        GetComponentInChildren<Text>().text = slotName;
    }

}
