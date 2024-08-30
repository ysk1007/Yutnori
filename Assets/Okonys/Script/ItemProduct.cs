using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemProduct : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemShop _itemShop;
    public ItemData _itemData;
    public int _productIndex;
    public bool _isSell = false;

    public Image _itemIcon;
    public TextMeshProUGUI _itemName;
    public TextMeshProUGUI _priceTag;

    Vector3 _originalSize = new Vector3(1f, 1f, 1f);
    Vector3 _selectSize = new Vector3(1.2f, 1.2f, 1.2f);
    Vector3 _nullSize = Vector3.zero;

    ArtifactPopup _popup;

    void Start()
    {
        _popup = SoonsoonData.Instance.ArtifactPopup;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localScale = (!_itemData) ? _nullSize : _originalSize;
    }

    public void init()
    {
        if (!_itemData) return;

        _isSell = false;
        transform.localScale = _originalSize;
        _itemIcon.sprite = _itemData._itemicon;
        _itemName.text = _itemData._itemName;
        _priceTag.text = _itemData._itemPrice.ToString();
    }

    // 마우스가 오브젝트 위에 올라갔을 때 호출되는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSell) return;

        _itemShop._selectItemIndex = _productIndex;
        transform.localScale = _selectSize;
        _popup._itemData = _itemData;
        _popup.init();
    }

    // 마우스가 오브젝트에서 벗어났을 때 호출되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSell) return;

        _itemShop._selectItemIndex = -1;
        transform.localScale = _originalSize;
        _popup._itemData = null;
    }
}
