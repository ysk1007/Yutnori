using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    public GameObject[] _unitCards;
    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        _unitCards = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _unitCards[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Size()
    {
        int stack = 0;
        for (int i = 0; i < _unitCards.Length; i++)
        {
            if (_unitCards[i].activeInHierarchy) stack++;
        }
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (stack / 4) * 460f);
    }
}
