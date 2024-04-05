using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerMove playerMv;

    public float rand;
    public int nowYut;      // ���� �� ��ȣ

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
/*        switch (rand)
        {
            case 0: Debug.Log("����� '��'�Դϴ�"); break;
            case 2: Debug.Log("����� '��'�Դϴ�"); break;
            case 3: Debug.Log("����� '��'�Դϴ�"); break;
            case 4: Debug.Log("����� '��'�Դϴ�"); break;
            case 5: Debug.Log("����� '��'�Դϴ�"); break;
        }
*/        if(rand <= 0.2f)
        {
            Debug.Log("����� '��'�Դϴ�");
            nowYut = 1;
        }
        else if(0.2f < rand && rand <= 0.5f)
        {
            Debug.Log("����� '��'�Դϴ�");
            nowYut = 2;
        }
        else if(0.5f < rand && rand <= 0.7f)
        {
            Debug.Log("����� '��'�Դϴ�");
            nowYut = 3;
        }
        else if(0.7f < rand && rand <= 0.85f)
        {
            Debug.Log("����� '��'�Դϴ�");
            nowYut = 4;
        }
        else
        {
            Debug.Log("����� '��'�Դϴ�");
            nowYut = 5;
        }
    }
}
