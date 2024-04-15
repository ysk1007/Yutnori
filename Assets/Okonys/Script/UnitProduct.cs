using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnitData;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UnitProduct : MonoBehaviour
{
    public UnitData _unitData;

    public UnitData.nobleRate _nobleRate;
    public UnitData.RateType _rateType;


    public Image _productBg;
    public Image _characterIcon;
    public Image _synergyIcon;
    public Image _synergyBg;
    public Image _typeIcon;
    public Image _typeBg;

    public Image _unitRate; // ��� ���
    public Image _productRate; // ������ ���

    public TextMeshProUGUI _productPrice;
    public TextMeshProUGUI _unitName;

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

    public void init()
    {
        float randomNumber = Random.value; // 0���� 1 ������ ���� ��

        if (randomNumber < 0.667f) // 66.7%
        {
            _rateType = RateType.lower;
            _productRate.sprite = _unitShop._rateSprites[0];
        }
        else if (randomNumber < 0.889f) // 22.2%
        {
            _rateType = RateType.middle;
            _productRate.sprite = _unitShop._rateSprites[1];
        }
        else // 11.1%
        {
            _rateType = RateType.upper;
            _productRate.sprite = _unitShop._rateSprites[2];
        }
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
                _productPrice.text = "1";
                _unitRate.color = _unitShop._rateColor[0];
                _productBg.color = _unitShop._rateColor[0];
                break;
            case UnitData.nobleRate.uncommon:
                _productPrice.text = "2";
                _unitRate.color = _unitShop._rateColor[1];
                _productBg.color = _unitShop._rateColor[1];
                break;
            case UnitData.nobleRate.rare:
                _productPrice.text = "3";
                _unitRate.color = _unitShop._rateColor[2];
                _productBg.color = _unitShop._rateColor[2];
                break;
            case UnitData.nobleRate.epic:
                _productPrice.text = "4";
                _unitRate.color = _unitShop._rateColor[3];
                _productBg.color = _unitShop._rateColor[3];
                break;
            case UnitData.nobleRate.legendary:
                _productPrice.text = "5";
                _unitRate.color = _unitShop._rateColor[4];
                _productBg.color = _unitShop._rateColor[4];
                break;
            default:
                break;
        }
    }
}
