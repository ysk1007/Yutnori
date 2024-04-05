using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitPopup : MonoBehaviour
{
    public Unit _unit;
    public UnitData _unitData;
    public Sprite[] _rateSprites;
    public Image _rateSprite;
    public Image _unitIcon;
    public TextMeshProUGUI _unitName;
    public TextMeshProUGUI _unitRate;

    public Sprite[] _synergySprites;
    public Image _synergyIcon;
    public TextMeshProUGUI _synergyText;

    public Sprite[] _typeSprites;
    public Image _typeIcon; 
    public TextMeshProUGUI _typeText;

    public TextMeshProUGUI _AT_text;
    public TextMeshProUGUI _DF_text;
    public TextMeshProUGUI _AS_text;
    public TextMeshProUGUI _CC_text;
    public TextMeshProUGUI _AR_text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init()
    {
        if (!_unit) return;
        _unitData = _unit._unitData;
        _rateSprite.sprite = _rateSprites[_unit._unitRate.GetHashCode()];
        _unitIcon.sprite = _unitData.icon;
        _unitName.text = _unitData.name;
        _unitRate.text = _unitData._unitRate.ToString();
        _synergyIcon.sprite = _synergySprites[_unit._attackType.GetHashCode()];
        _typeIcon.sprite = _typeSprites[_unit._unitType.GetHashCode()];
    }
}
