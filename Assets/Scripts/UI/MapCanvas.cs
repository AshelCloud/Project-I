using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Forest 맵 전용 맵캔버스
/// 다른 맵 추가시 확장성 고려 X
/// </summary>
public class MapCanvas : MonoBehaviour
{
    private Canvas _myCanvas;
    public Canvas _Canvas
    {
        get
        {
            if(_myCanvas == null)
            {
                _myCanvas = GetComponent<Canvas>();
            }
            return _myCanvas;
        }
    }
    public bool ActiveSelf
    {
        private set
        {
            _Canvas.enabled = value;
        }
        get
        {
            return _Canvas.enabled;
        }
    }
    public GameObject playerPosition;

    private Dictionary<string, Transform> mapChildrens;

    private void Start()
    {
        mapChildrens = new Dictionary<string, Transform>();

        foreach(Transform f in GetComponentsInChildren<Transform>())
        {
            if(f == playerPosition.transform) { continue; }

            mapChildrens.Add(f.name, f);
        }

        GameManager.Instance.LoadCompleteMapEvent.AddListener(SetPlayerPosition);
    }

    public void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            ActiveSelf = true;
        }
        else
        {
            ActiveSelf = false;
        }
    }

    public void SetPlayerPosition()
    {
        string mapIndex = GameManager.Instance.CurrentMapName.Split('_')[1];
        playerPosition.transform.SetParent(mapChildrens[mapIndex]);
        playerPosition.transform.localPosition = Vector3.zero;
    }
}
