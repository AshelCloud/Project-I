using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsMenu : MonoBehaviour
{
    private Player player = null;

    private GameObject menu = null;

    private Text value_HP;
    private Text value_Power;
    private Text value_Defence;

    private void Start()
    {
        StartCoroutine(SearchObject());
    }

    void LateUpdate()
    {
        StartCoroutine(UpdateStatistics());
    }

    private IEnumerator SearchObject()
    {
        yield return new WaitForSeconds(1f);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        menu = GameObject.Find("Menu_CharacterStats");

        value_HP = menu.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
        value_Power = menu.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();
        value_Defence = menu.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>();
    }

    private IEnumerator UpdateStatistics()
    {
        yield return new WaitForSeconds(1f);

        value_HP.text = player.hp.ToString();
        value_Power.text = player.OffensePower.ToString();
        value_Defence.text = player.defense.ToString();
    }

}
