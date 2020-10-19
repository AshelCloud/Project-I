using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIManager : Singleton<StatusUIManager>
{
    [Header("TargetAmountSprite")]
    [SerializeField] private Image hpAmountImage;
    [SerializeField] private Image staminaAmountImage;

    [Header("AmountSetting")]
    [SerializeField] private float hpAmount;
    [SerializeField] private float staminaAmount;


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
