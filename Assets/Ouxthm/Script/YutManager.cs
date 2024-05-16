using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YutManager : MonoBehaviour
{
    public static YutManager instance;
    public PlayerMove playerMv;

    public float rand;
    public int _moveDistance;      // 윷 이동 거리

    public Button btn;
    private void Awake()
    {
        instance = this;
    }

    public void ThrowYut()
    {
        rand = Random.Range(0f, 1f);
        if(rand <= 0.2f)
        {
            Debug.Log("결과는 '도'입니다");
            _moveDistance = 1;
        }
        else if(0.2f < rand && rand <= 0.5f)
        {
            Debug.Log("결과는 '개'입니다");
            _moveDistance = 2;
        }
        else if(0.5f < rand && rand <= 0.7f)
        {
            Debug.Log("결과는 '걸'입니다");
            _moveDistance = 3;
        }
        else if(0.7f < rand && rand <= 0.85f)
        {
            Debug.Log("결과는 '윷'입니다");
            _moveDistance = 4;
        }
        else
        {
            Debug.Log("결과는 '모'입니다");
            _moveDistance = 5;
        }
    }

    public void SetYut(int i)
    {
        Debug.Log(i+"칸 이동");
        _moveDistance = i;
    }
}
