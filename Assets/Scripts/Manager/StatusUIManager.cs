using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIManager : Singleton<StatusUIManager>
{
    [Header("TargetAmountSprite")]
    [SerializeField] private Image hpAmountImage = null;
    [SerializeField] private Image staminaAmountImage = null;

    [Header("AmountSetting")]
    [SerializeField] private float hpAmount = 0f;
    [SerializeField] private float staminaAmount = 0f;


    public float HPAmount
    {
        get
        {
            return hpAmount;
        }
        set
        {
            hpAmount = value;
            hpAmountImage.fillAmount = hpAmount;
        }
    }

    public float StaminaAmount
    {
        get
        {
            return staminaAmount;
        }
        set
        {
            staminaAmount = value;
            staminaAmountImage.fillAmount = staminaAmount;
        }
    }
}
