using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YutManager : MonoBehaviour
{
    public static YutManager instance;
    public PlayerMove playerMv;

    public float rand;
    public int _moveDistance;      // �� �̵� �Ÿ�

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
            Debug.Log("����� '��'�Դϴ�");
            _moveDistance = 1;
        }
        else if(0.2f < rand && rand <= 0.5f)
        {
            Debug.Log("����� '��'�Դϴ�");
            _moveDistance = 2;
        }
        else if(0.5f < rand && rand <= 0.7f)
        {
            Debug.Log("����� '��'�Դϴ�");
            _moveDistance = 3;
        }
        else if(0.7f < rand && rand <= 0.85f)
        {
            Debug.Log("����� '��'�Դϴ�");
            _moveDistance = 4;
        }
        else
        {
            Debug.Log("����� '��'�Դϴ�");
            _moveDistance = 5;
        }
    }

    public void SetYut(int i)
    {
        Debug.Log(i+"ĭ �̵�");
        _moveDistance = i;
    }
}
