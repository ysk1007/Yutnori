using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeasureObj : MonoBehaviour
{
    public DamageMeasure _damageMeasure;
    public Unit _unit;
    public Slider _damageSlider;
    public TextMeshProUGUI _damageText;
    public float _damageValue;
    public Image _uniticon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _damageValue = _unit._damageinflicted;
        _damageText.text = _damageValue.ToString();
        if (_damageMeasure._highestDamage > 0) _damageSlider.value = _damageValue / _damageMeasure._highestDamage;
    }

    public void init(DamageMeasure damageMeasure, Unit unit)
    {
        _damageMeasure = damageMeasure;
        _unit = unit;
        _uniticon.sprite = unit._unitData.icon;
    }
}
