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
        for(int i = 0; i < btn.Length; i++)
        {
            btn[i] = transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Button>();
            ui[i] = transform.GetChild(0).GetChild(i + 1).gameObject;
        }
        ui[0].SetActive(true);
        ui[1].SetActive(false);
        ui[2].SetActive(false);

        btn[0].onClick.AddListener(() =>
        {
            ui[0].SetActive(true);
            ui[1].SetActive(false);
            ui[2].SetActive(false);
        });

        btn[1].onClick.AddListener(() =>
        {
            ui[0].SetActive(false);
            ui[1].SetActive(true);
            ui[2].SetActive(false);
        });
        btn[2].onClick.AddListener(() =>
        {
            ui[0].SetActive(false);
            ui[1].SetActive(false);
            ui[2].SetActive(true);
        });
    }
}
