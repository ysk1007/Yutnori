using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SynergyUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SynergyData _synergyData;

    public TextMeshProUGUI[] _countText;
    public Color[] _colors;

    public TextMeshProUGUI _curCountText;
    public TextMeshProUGUI _typeText;
    public Image _iconSprite;

    SynergyPopup _synergyPopup;
    LayoutElement _layoutElement;

    void Awake()
    {
        _layoutElement = this.GetComponent<LayoutElement>();
        init();
    }

    // Start is called before the first frame update
    void Start()
    {
        _synergyPopup = SoonsoonData.Instance.SynergyPopup;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_synergyData) return;
        switch (_synergyData.RequiredNumber.Length)
        {
            case 1:
                break;
            case 3:
                CountTextUpdate(CheckCount3(int.Parse(_curCountText.text)));
                break;
            case 4:
                CountTextUpdate(CheckCount4(int.Parse(_curCountText.text)));
                break;
        }
    }

    void init()
    {
        if (!_synergyData) return;

        for (int i = 0; i < _countText.Length; i++)
        {
            _countText[i].text = _synergyData.RequiredNumber[i].ToString();
        }
        _typeText.text = _synergyData.synergyName;
        _iconSprite.sprite = _synergyData.icon;
    }

    public int CheckCount3(int i)
    {
        if (i < _synergyData.RequiredNumber[0])
        {
            return 0;
        }
        else if (i >= _synergyData.RequiredNumber[0] && i < _synergyData.RequiredNumber[1])
        {
            return 1;
        }
        else if (i >= _synergyData.RequiredNumber[1] && i < _synergyData.RequiredNumber[2])
        {
            return 2;
        }
        else // i >= _synergyData.RequiredNumber[2]
        {
            return 3;
        }
    }

    public int CheckCount4(int i)
    {
        if (i < _synergyData.RequiredNumber[0])
        {
            return 0;
        }
        else if (i >= _synergyData.RequiredNumber[0] && i < _synergyData.RequiredNumber[1])
        {
            return 1;
        }
        else if (i >= _synergyData.RequiredNumber[1] && i < _synergyData.RequiredNumber[2])
        {
            return 2;
        }
        else if (i >= _synergyData.RequiredNumber[2] && i < _synergyData.RequiredNumber[3])
        {
            return 3;
        }
        else
        {
            return 4;
        }
    }

    void CountTextUpdate(int index)
    {
        if (index == 0)
        {
            _layoutElement.ignoreLayout = true;
            transform.localScale = Vector3.zero;
            for (int i = 0; i < _countText.Length; i++)
            {
                _countText[i].color =  _colors[1];
            }
        }
        else
        {
            _layoutElement.ignoreLayout = false;
            transform.localScale = Vector3.one;
            index -= 1;
            for (int i = 0; i < _countText.Length; i++)
            {
                _countText[i].color = (index == i) ? _colors[0] : _colors[1];
            }
        }
    }

    // 마우스가 오브젝트 위에 올라갔을 때 호출되는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        _synergyPopup._synergyData = this._synergyData;
        _synergyPopup.init();
    }

    // 마우스가 오브젝트에서 벗어났을 때 호출되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        _synergyPopup._synergyData = null;
    }
}
