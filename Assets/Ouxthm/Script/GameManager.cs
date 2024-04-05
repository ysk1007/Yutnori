using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerMove playerMv;

    public float rand;
    public int nowYut;      // 지금 윷 번호

    public Button btn;
    private void Awake()
    {
        instance = this;    
        btn.onClick.AddListener(() => 
        {
            ThrowYut();
            StartCoroutine(playerMv.PlayerMoveOnMap());
        });
    }

    public void ThrowYut()
    {
        rand = Random.Range(0f, 1f);
        if(rand <= 0.2f)
        {
            Debug.Log("결과는 '도'입니다");
            nowYut = 1;
        }
        else if(0.2f < rand && rand <= 0.5f)
        {
            Debug.Log("결과는 '개'입니다");
            nowYut = 2;
        }
        else if(0.5f < rand && rand <= 0.7f)
        {
            Debug.Log("결과는 '걸'입니다");
            nowYut = 3;
        }
        else if(0.7f < rand && rand <= 0.85f)
        {
            Debug.Log("결과는 '윷'입니다");
            nowYut = 4;
        }
        else
        {
            Debug.Log("결과는 '모'입니다");
            nowYut = 5;
        }
    }
}
