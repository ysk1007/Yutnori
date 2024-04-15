using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Button[] btn = new Button[3];
    public GameObject[] ui = new GameObject[3];

    private void Awake()
    {
        ui[0].transform.localScale = Vector3.one;
        ui[1].transform.localScale = Vector3.zero;
        ui[2].transform.localScale = Vector3.zero;

        btn[0].onClick.AddListener(() =>
        {
            ui[0].transform.localScale = Vector3.one;
            ui[1].transform.localScale = Vector3.zero;
            ui[2].transform.localScale = Vector3.zero;
        });

        btn[1].onClick.AddListener(() =>
        {
            ui[0].transform.localScale = Vector3.zero;
            ui[1].transform.localScale = Vector3.one;
            ui[2].transform.localScale = Vector3.zero;
        });
        btn[2].onClick.AddListener(() =>
        {
            ui[0].transform.localScale = Vector3.zero;
            ui[1].transform.localScale = Vector3.zero;
            ui[2].transform.localScale = Vector3.one;
        });
    }

    public void OpenShop()
    {
        gameObject.transform.localScale = Vector3.one;
    }

    public void CloseShop()
    {
        gameObject.transform.localScale = Vector3.zero;
    }
}
