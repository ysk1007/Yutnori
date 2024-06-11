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
    public TextMeshProUGUI[] _buffDesc;

    public Color[] _Colors;

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

    public void init(int curCount)
    {
        if (!_synergyData) return;

        _synergyIcon.sprite = _synergyData.icon;
        _synergyName.text = _synergyData.synergyName;
        _synergyDesc.text = _synergyData.SynergyDesc;
        for (int i = 0; i < _buffDesc.Length; i++)
        {
            _buffDesc[i].text = (i < _synergyData.RequiredNumber.Length) ? SynergyBuffDesc(_synergyData, i) : null;
            _buffDesc[i].color = (curCount - 2 == i) ? _Colors[1] : _Colors[0];
        }
        //_buffDesc.text = SynergyBuffDesc(_synergyData);
    }

    public Color GetBuffDescColor(int curCount)
    {
        Color color = _Colors[0];
        switch (curCount)
        {
            case 0:
                color = _Colors[0];
                break;
        }
        return color;
    }

    public string SynergyBuffDesc(SynergyData synergyData, int index)
    {
        string Desc = "";
        switch (synergyData.synergyType)
        {
            case SynergyData.SynergyType.Warrior:
                //({0}) ü�� + {1}, ���� + {2}, {3}��ŭ ��ȣ
                Desc = string.Format(_synergyData.BuffDesc,
                    _synergyData.RequiredNumber[index],
                    _synergyData.MaxHp[index],
                    _synergyData.Defense[index],
                    _synergyData.Shield[index]);
                break;

            case SynergyData.SynergyType.Archer:
                // ({0}) {1}�ʸ��� ���ݼӵ� + {2}%
                Desc = string.Format(_synergyData.BuffDesc,
                    _synergyData.RequiredNumber[index],
                    _synergyData.Interval[index],
                    _synergyData.Speed[index] * 100f);
                Desc += (index == _synergyData.RequiredNumber.Length - 1) ? ", ��� �Ʊ� ����" : null;
                break;

            case SynergyData.SynergyType.Wizard:
                // ({0}) ��Ÿ�� ���� +{3}%
                Desc = string.Format(_synergyData.BuffDesc,
                    _synergyData.RequiredNumber[index],
                    _synergyData.CoolTime[index] * 100f);
                Desc += (index == _synergyData.RequiredNumber.Length - 1) ? 
                    ", ��� �Ʊ��� ������ �� ���� ��ų ��Ÿ���� 1�� �����մϴ�." : null;
                break;

            case SynergyData.SynergyType.Assassin:
                // ({0}) ġ��Ÿ Ȯ�� + {1}%, ���ݷ� + {2}
                Desc = string.Format(_synergyData.BuffDesc, 
                    _synergyData.RequiredNumber[index],
                    _synergyData.Critical[index] * 100f,
                    _synergyData.Damage[index]);
                break;

            case SynergyData.SynergyType.Healer:
                // ({0}) {1}�ʸ��� {2}HP ȸ��
                Desc = string.Format(_synergyData.BuffDesc,
                    _synergyData.RequiredNumber[index],
                    _synergyData.Interval[index],
                    _synergyData.Recovery[index]);
                break;

            case SynergyData.SynergyType.Merchant:
                //({0}) ����ǰ �߰� +1
                Desc = string.Format(_synergyData.BuffDesc,
                    _synergyData.RequiredNumber[index]);
                Desc += (index == _synergyData.RequiredNumber.Length - 1) ?
                    ", ������ 100�ɸ��� ���ط��� 1% �����մϴ�." : null;
                break;

            case SynergyData.SynergyType.Human:
                //({0}) ���� ����
                Desc = string.Format(_synergyData.BuffDesc,
                    _synergyData.RequiredNumber[index]);
                switch (index)
                {
                    case 0:
                        Desc += " ü��, ���ݷ� ����";
                        break;
                    case 1:
                        Desc += " ���ݼӵ�, ���� ����";
                        break;
                    case 2:
                        Desc += " ġ��Ÿ Ȯ�� ����, ��Ÿ�� ����";
                        break;
                }
                break;

            case SynergyData.SynergyType.Ghost:
                // ({0}) ���ݷ� +{1}, {2}�� ���� ���� �մϴ�.
                Desc = string.Format(_synergyData.BuffDesc,
                    _synergyData.RequiredNumber[0],
                    _synergyData.Damage[0]);

                switch (index)
                {
                    case 1:
                        Desc += _synergyData.Duration[index] + " �� ���� ���� �մϴ�.";
                        break;
                    case 2:
                        Desc += _synergyData.Duration[index] + " �� ���� ���� �մϴ�.";
                        break;
                }

                break;

        }
        return Desc;
    }
}
