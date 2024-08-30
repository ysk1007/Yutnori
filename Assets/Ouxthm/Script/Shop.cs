using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Button[] btn = new Button[3];
    public GameObject[] ui = new GameObject[3];
    UnitShop _unitShop;
    ItemShop _itemShop;
    Gambling _gambling;

    private void Awake()
    {
        _gambling = ui[2].GetComponent<Gambling>();

        ui[0].transform.localScale = Vector3.one;
        ui[1].transform.localScale = Vector3.zero;
        ui[2].transform.localScale = Vector3.zero;

        btn[0].onClick.AddListener(() =>
        {
            if (_gambling._doPlay) return;
            ui[0].transform.localScale = Vector3.one;
            ui[1].transform.localScale = Vector3.zero;
            ui[2].transform.localScale = Vector3.zero;
        });

        btn[1].onClick.AddListener(() =>
        {
            if (_gambling._doPlay) return;
            ui[0].transform.localScale = Vector3.zero;
            ui[1].transform.localScale = Vector3.one;
            ui[2].transform.localScale = Vector3.zero;
        });
        btn[2].onClick.AddListener(() =>
        {
            if (_gambling._doPlay) return;
            ui[0].transform.localScale = Vector3.zero;
            ui[1].transform.localScale = Vector3.zero;
            ui[2].transform.localScale = Vector3.one;
        });
    }

    private void Start()
    {
        _unitShop = SoonsoonData.Instance.UnitShop;
        _itemShop = SoonsoonData.Instance.ItemShop;
    }

    public void OpenShop()
    {
        gameObject.transform.localScale = Vector3.one;
        _unitShop._isOpenShop = true;
    }

    public void CloseShop()
    {
        if (_gambling._doPlay) return;

        gameObject.transform.localScale = Vector3.zero;
        _unitShop._isOpenShop = false;
    }

    public void ResetShop()
    {
        _unitShop.newProduct();
        _itemShop.newItem();
    }
}
