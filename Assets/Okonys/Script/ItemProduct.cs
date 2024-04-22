using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemProduct : MonoBehaviour
{
    public ItemData _itemData;

    public Image _itemIcon;
    public TextMeshProUGUI _itemName;
    public TextMeshProUGUI _priceTag;

    Vector3 _originalSize = new Vector3(1f, 1f, 1f);
    Vector3 _nullSize = new Vector3(0f, 0f, 0f);

    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = (!_itemData) ? _nullSize : _originalSize;
    }

    public void init()
    {
        if (!_itemData) return;

        _itemIcon.sprite = _itemData._itemicon;
        _itemName.text = _itemData._itemName;
        _priceTag.text = _itemData._itemPrice.ToString();
    }
}
