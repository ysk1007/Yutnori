using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerMove playerMv;

    public int rand;

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
        rand = Random.Range(0, 5);
        switch (rand)
        {
            case 0: Debug.Log("����� '��'�Դϴ�"); break;
            case 1: Debug.Log("����� '��'�Դϴ�"); break;
            case 2: Debug.Log("����� '��'�Դϴ�"); break;
            case 3: Debug.Log("����� '��'�Դϴ�"); break;
            case 4: Debug.Log("����� '��'�Դϴ�"); break;
        }
    }
}
