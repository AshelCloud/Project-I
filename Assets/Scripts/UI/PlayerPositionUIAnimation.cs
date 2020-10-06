using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPositionUIAnimation : MonoBehaviour
{
    public List<Sprite> animationImages;

    public float animationSpeed = 1f;

    private int _index;
    private int Index
    {
        set
        {
            _index = value;
            if(_index >= animationImages.Count)
            {
                _index = 0;
            }
        }
        get
        {
            return _index;
        }
    }

    private Image target;

    private void Awake()
    {
        target = GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(AnimationStart());
    }

    private IEnumerator AnimationStart()
    {
        while (true)
        {
            Index++;
            target.sprite = animationImages[Index];
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}
