using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int rand;
    private void Start()
    {
        ThrowYut();
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
