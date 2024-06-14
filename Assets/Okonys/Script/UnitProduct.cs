using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnitData;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class UnitProduct : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnitData _unitData;

    public UnitData.nobleRate _nobleRate;
    public UnitData.RateType _rateType;

    public int _productIndex;
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

    Vector3 _originalSize = new Vector3(0.7f, 0.7f, 1f);
    Vector3 _SelectSize = new Vector3(0.9f, 0.9f, 1f);
    UnitShop _unitShop;

    // Start is called before the first frame update
    void Start()
    {
        _unitShop = SoonsoonData.Instance.UnitShop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 마우스가 오브젝트 위에 올라갔을 때 호출되는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSell) return;

        _unitShop._selectProductIndex = _productIndex;
        transform.localScale = _SelectSize;
    }

    // 마우스가 오브젝트에서 벗어났을 때 호출되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSell) return;

        _unitShop._selectProductIndex = -1;
        transform.localScale = _originalSize;
    }

    public void init()
    {
        _isSell = false;
        UnitRate();

        _nobleRate = _unitData._nobleRate;
        _unitName.text = _unitData.UnitName;
        _characterIcon.sprite = _unitData.icon;

        switch (_unitData.AttackType)
        {
            case Unit.AttackType.Warrior:
                _synergyIcon.sprite = _unitShop._synergySprites[0];
                _synergyBg.color = _unitShop._synergyColor[0];
                break;
            case Unit.AttackType.Archer:
                _synergyIcon.sprite = _unitShop._synergySprites[1];
                _synergyBg.color = _unitShop._synergyColor[1];
                break;
            case Unit.AttackType.Wizard:
                _synergyIcon.sprite = _unitShop._synergySprites[2];
                _synergyBg.color = _unitShop._synergyColor[2];
                break;
            case Unit.AttackType.Assassin:
                _synergyIcon.sprite = _unitShop._synergySprites[3];
                _synergyBg.color = _unitShop._synergyColor[3];
                break;
            case Unit.AttackType.Healer:
                _synergyIcon.sprite = _unitShop._synergySprites[4];
                _synergyBg.color = _unitShop._synergyColor[4];
                break;
            case Unit.AttackType.Merchant:
                _synergyIcon.sprite = _unitShop._synergySprites[5];
                _synergyBg.color = _unitShop._synergyColor[5];
                break;
            default:
                break;
        }

        switch (_unitData.UnitType)
        {
            case Unit.UnitType.Human:
                _typeIcon.sprite = _unitShop._typeSprites[0];
                _typeBg.color = _unitShop._typeColor[0];
                break;
            case Unit.UnitType.Ghost:
                _typeIcon.sprite = _unitShop._typeSprites[1];
                _typeBg.color = _unitShop._typeColor[1];
                break;
            case Unit.UnitType.Great:
                break;
            case Unit.UnitType.Devil:
                break;
            default:
                break;
        }

        switch (_unitData._nobleRate)
        {
            case UnitData.nobleRate.common:
                _productPrice = 1;
                _productPriceTag.text = "1";
                _unitRate.color = _unitShop._rateColor[0];
                _productBg.color = _unitShop._rateColor[0];
                break;
            case UnitData.nobleRate.uncommon:
                _productPrice = 2;
                _productPriceTag.text = "2";
                _unitRate.color = _unitShop._rateColor[1];
                _productBg.color = _unitShop._rateColor[1];
                break;
            case UnitData.nobleRate.rare:
                _productPrice = 3;
                _productPriceTag.text = "3";
                _unitRate.color = _unitShop._rateColor[2];
                _productBg.color = _unitShop._rateColor[2];
                break;
            case UnitData.nobleRate.epic:
                _productPrice = 4;
                _productPriceTag.text = "4";
                _unitRate.color = _unitShop._rateColor[3];
                _productBg.color = _unitShop._rateColor[3];
                break;
            case UnitData.nobleRate.legendary:
                _productPrice = 5;
                _productPriceTag.text = "5";
                _unitRate.color = _unitShop._rateColor[4];
                _productBg.color = _unitShop._rateColor[4];
                break;
            default:
                break;
        }
    }

    private void UnitRate()
    {
        float randomNumber = Random.value; // 0부터 1 사이의 랜덤 값
        if (randomNumber < 0.667f) // 66.7%
        {
            _rateType = RateType.lower;
            if (!_unitShop) _unitShop = SoonsoonData.Instance.UnitShop;
            _productRate.sprite = _unitShop._rateSprites[0];
        }
        else if (randomNumber < 0.889f) // 22.2%
        {
            _rateType = RateType.middle;
            if (!_unitShop) _unitShop = SoonsoonData.Instance.UnitShop;
            _productRate.sprite = _unitShop._rateSprites[1];
        }
        else // 11.1%
        {
            _rateType = RateType.upper;
            if (!_unitShop) _unitShop = SoonsoonData.Instance.UnitShop;
            _productRate.sprite = _unitShop._rateSprites[2];
        }
    }
}
