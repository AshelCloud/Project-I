using System;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * 
 * 
 * 
 * 연구하면서 개발중인 사항입니다!
 * 
 * 
 * 
 * 
 */

public class InputControl : MonoBehaviour
{
    public static InputControl instance = null;

    private Dictionary<KeyCode, Action> keyDictionary;
    public Dictionary<KeyCode, Action>  KeyDictionary { get { return keyDictionary; } }

    private Player _player;
    public Player player { set { _player = value; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        keyDictionary = new Dictionary<KeyCode, Action>
        {
            { KeyCode.LeftArrow, MoveLeft},
            { KeyCode.RightArrow, MoveRight },
            { KeyCode.A, Key_A }
        };
    }


    private Vector2 direction;

    public void MoveLeft()
    {
        _player.transform.Translate(Vector2.right * (-_player.MoveSpeed) * Time.deltaTime, Space.World);    //플레이어 좌측 이동
        direction.x = -Mathf.Abs(direction.x);                                                              //플레이어 방향전환
        _player.transform.localScale = direction;

    }

    public void MoveRight()
    {
        _player.transform.Translate(Vector2.right * _player.MoveSpeed * Time.deltaTime, Space.World);       //플레이어 우측 이동
        direction.x = Mathf.Abs(direction.x);                                                               //플레이어 방향전환
        _player.transform.localScale = direction;
    }

    public void Key_A()
    {

    }
}
