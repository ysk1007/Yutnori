using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynergyPopup : MonoBehaviour
{
    public SynergyData _synergyData;

    public Image _synergyIcon;
    public TextMeshProUGUI _synergyName;
    public TextMeshProUGUI _synergyDesc;
    public TextMeshProUGUI _buffDesc;

    public Color[] _rateColor;

    [SerializeField] private float posX;
    [SerializeField] private float posY;

    float screenWidth = Screen.width;

    private void Awake()
    {
        SoonsoonData.Instance.SynergyPopup = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!_synergyData) transform.localScale = Vector3.zero;
        else
        {
            transform.localScale = Vector3.one;

            this.transform.position = (Input.mousePosition.x < screenWidth / 2) ?
                new Vector3(Input.mousePosition.x + posX, Input.mousePosition.y + posY, 0f) :
                new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y + posY, 0f);
        }
    }

    public void init()
    {
        if (!_synergyData) return;

        _synergyIcon.sprite = _synergyData.icon;
        _synergyName.text = _synergyData.synergyName;
        _synergyDesc.text = _synergyData.SynergyDesc;
        _buffDesc.text = SynergyBuffDesc(_synergyData);
    }

    public string SynergyBuffDesc(SynergyData synergyData)
    {
        string Desc = "";
        switch (synergyData.synergyType)
        {
            case SynergyData.SynergyType.Warrior:
                //({0}) 체력 + {1}, 방어력 + {2}, {3}만큼 보호
                Desc = string.Format(_synergyData.BuffDesc,
                    _synergyData.RequiredNumber[0], _synergyData.RequiredNumber[1], _synergyData.RequiredNumber[2],
                    _synergyData.MaxHp[0], _synergyData.MaxHp[1], _synergyData.MaxHp[2],
                    _synergyData.Defense[0], _synergyData.Defense[1], _synergyData.Defense[2],
                    _synergyData.Shield[0], _synergyData.Shield[1], _synergyData.Shield[2]);
                break;

            case SynergyData.SynergyType.Archer:
                // ({0}) {1}초마다 공격속도 + {2}%
                Desc = string.Format(_synergyData.BuffDesc,
                    _synergyData.RequiredNumber[0], _synergyData.RequiredNumber[1], _synergyData.RequiredNumber[2],
                    _synergyData.Interval[0], _synergyData.Interval[1], _synergyData.Interval[2],
                    _synergyData.Speed[0] * 100f, _synergyData.Speed[1] * 100f, _synergyData.Speed[2] * 100f);
                break;

            case SynergyData.SynergyType.Wizard:
                // ({0}) 쿨타임 감소 +{3}%
                Desc = string.Format(_synergyData.BuffDesc,
                    _synergyData.RequiredNumber[0], _synergyData.RequiredNumber[1], _synergyData.RequiredNumber[2],
                    _synergyData.CoolTime[0] * 100f, _synergyData.CoolTime[1] * 100f, _synergyData.CoolTime[2] * 100f);
                break;

            case SynergyData.SynergyType.Assassin:
                // ({0}) 치명타 확률 + {1}%, 공격력 + {2}
                Desc = string.Format(_synergyData.BuffDesc, 
                    _synergyData.RequiredNumber[0], _synergyData.RequiredNumber[1], _synergyData.RequiredNumber[2],
                    _synergyData.Critical[0] * 100f, _synergyData.Critical[1] * 100f, _synergyData.Critical[2] * 100f,
                    _synergyData.Damage[0], _synergyData.Damage[1], _synergyData.Damage[2]);
                break;

            case SynergyData.SynergyType.Healer:
                // ({0}) {1}초마다 {2}HP 회복
                Desc = string.Format(_synergyData.BuffDesc,
                    _synergyData.RequiredNumber[0], _synergyData.RequiredNumber[1], _synergyData.RequiredNumber[2],
                    _synergyData.Interval[0], _synergyData.Interval[1], _synergyData.Interval[2],
                    _synergyData.Recovery[0], _synergyData.Recovery[1], _synergyData.Recovery[2]);
                break;

            case SynergyData.SynergyType.Merchant:
                //({0}) 전리품 추가 +1
                Desc = string.Format(_synergyData.BuffDesc,
                    _synergyData.RequiredNumber[0], _synergyData.RequiredNumber[1], _synergyData.RequiredNumber[2], _synergyData.RequiredNumber[3]);
                break;

            case SynergyData.SynergyType.Human:
                //({0}) 버프 내용
                Desc = string.Format(_synergyData.BuffDesc,
                    _synergyData.RequiredNumber[0], _synergyData.RequiredNumber[1], _synergyData.RequiredNumber[2]);
                break;

            case SynergyData.SynergyType.Ghost:
                // ({0}) 공격력 +{1}, {2}초 동안 복수 합니다.
                Desc = string.Format(_synergyData.BuffDesc,
                    _synergyData.RequiredNumber[0], _synergyData.RequiredNumber[1], _synergyData.RequiredNumber[2],
                    _synergyData.Damage[0], _synergyData.Damage[1], _synergyData.Damage[2],
                    _synergyData.Duration[1], _synergyData.Duration[2]);
                break;

        }
        return Desc;
    }
}
