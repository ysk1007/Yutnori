using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnitData;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class UnitCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnitData _unitData;

    public UnitData.nobleRate _nobleRate;
    public UnitData.RateType _rateType;

    public int _unitIndex;
    public int _productPrice;
    public bool _isSell = false;

    public Image _productBg;
    public Image _characterIcon;
    public Image _synergyIcon;
    public Image _synergyBg;
    public Image _typeIcon;
    public Image _typeBg;

    public Image _unitRate; // 고귀 등급
    public Image _productRate; // 상중하 등급

    public TextMeshProUGUI _productPriceTag;
    public TextMeshProUGUI _unitName;
    public TextMeshProUGUI _unitDesc;

    Vector3 _originalSize = new Vector3(0.7f, 0.7f, 1f);
    Vector3 _SelectSize = new Vector3(0.9f, 0.9f, 1f);

    UnitShop _unitShop;
    InventoryManager _inventoryManager;
    // Start is called before the first frame update
    void Start()
    {
        _unitShop = SoonsoonData.Instance.UnitShop;
        _inventoryManager = SoonsoonData.Instance.Inventory_Manager;
    }

    void OnEnable()
    {
        if (!_unitShop) _unitShop = SoonsoonData.Instance.UnitShop;
        if (!_inventoryManager) _inventoryManager = SoonsoonData.Instance.Inventory_Manager;
        init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 마우스가 오브젝트 위에 올라갔을 때 호출되는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSell) return;

        _unitShop._selectInventoryIndex = _unitIndex;
        transform.localScale = _SelectSize;
    }

    // 마우스가 오브젝트에서 벗어났을 때 호출되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSell) return;

        _unitShop._selectInventoryIndex = -1;
        transform.localScale = _originalSize;
    }

    public void init()
    {
        if (!_unitShop) _unitShop = SoonsoonData.Instance.UnitShop;

        _isSell = false;
        _nobleRate = _unitData._nobleRate;
        _unitName.text = _unitData.UnitName;
        _characterIcon.sprite = _unitData.icon;
        _unitDesc.text = _unitData.UnitDesc;

        SynergySetting(_unitData.AttackType.GetHashCode());
        TypeSetting(_unitData.UnitType.GetHashCode());
        ProductSetting(_unitData._nobleRate.GetHashCode());
    }

    // 시너지에 대한 이미지와 색깔 세팅
    void SynergySetting(int i)
    {
        _synergyIcon.sprite = _unitShop._synergySprites[i];
        _synergyBg.color = _unitShop._synergyColor[i];
    }

    // 타입에 대한 이미지와 색깔 세팅
    void TypeSetting(int i)
    {
        _typeIcon.sprite = _unitShop._typeSprites[i];
        _typeBg.color = _unitShop._typeColor[i];
    }

    // 상품등급에 대한 이미지, 가격 세팅
    void ProductSetting(int i)
    {
        _productPrice = i+1;
        _productPriceTag.text = (i+1).ToString();
        if (!_unitShop) _unitShop = SoonsoonData.Instance.UnitShop;
        _unitRate.sprite = _unitShop._rateImage[i];
        _productBg.color = _unitShop._cardColor[i];
        if (!_inventoryManager) _inventoryManager = SoonsoonData.Instance.Inventory_Manager;
        _productRate.sprite = 
            (_unitIndex < _inventoryManager._userSquad.Length) ? 
            _unitShop._rateSprites[_inventoryManager._userSquad[_unitIndex]._unitRate] : 
            _unitShop._rateSprites[_inventoryManager._userInventory[_unitIndex - _inventoryManager._userSquad.Length]._unitRate];
    }
}
