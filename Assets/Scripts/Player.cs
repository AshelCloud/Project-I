using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    private void Update()
    {
        //좌측이동
        if(Input.GetKey(KeyCode.LeftArrow))
            gameObject.transform.Translate(Vector3.right * -_moveSpeed * Time.deltaTime, Space.World);

        //우측이동
        if (Input.GetKey(KeyCode.RightArrow))
           gameObject .transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime, Space.World);

        //플레이어 상호작용
        //if(Input.GetKeyDown(KeyCode.UpArrow))

        //하향 점프
        //if(Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.D))

        //점프
        //if(Input.GetKeyDown(KeyCode.D))

        //공격
        //if(Input.GetKeyDown(KeyCode.A))

        //벽 타기
        //if(Input.GetKeyDown(KeyCode.S))

        //구르기
        //if(Input.GetKeyDown(KeyCode.F))

        //일시정지
        //if(Input.GetKeyDown(KeyCode.Escape))

        //지도
        //if(Input.GetKeyDown(KeyCode.Tab))



    }
}