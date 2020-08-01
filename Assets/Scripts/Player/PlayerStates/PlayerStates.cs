using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

/*
 **********************************************
    
    <차후 해결 사항들>
    코루틴, 델리게이트 등을 사용하여 수정할 예정
    중복 되는 코드 함수화
    if문으로 입력처리가 아닌 다른 과정으로 처리

 **********************************************
 */


//플레이어 상태 인터페이스
public interface IPlayerState
{
    void OnEnter(Player player);    //상태 진입시 최초 할 행동
    void Update();                  //상태에 맞는 행동실행
    void OnExit();                  //상태가 끝날 때 할 행동
}

//****************************차후 구현해야할 기능들*************************

//플레이어 상호작용
//if(Input.GetKeyDown(KeyCode.UpArrow))

//일시정지
//if(Input.GetKeyDown(KeyCode.Escape))

//지도
//if(Input.GetKeyDown(KeyCode.Tab))