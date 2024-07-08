using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitPopup : MonoBehaviour
{
    public Unit _unit;
    public UnitData _unitData;
    [SerializeField] private Sprite[] _rateSprites;
    [SerializeField] private Sprite _nullSprite;
    [SerializeField] private Image _rateSprite;
    [SerializeField] private Image _unitIcon;
    [SerializeField] private TextMeshProUGUI _unitName;

    [SerializeField] private Slider _HpSlider;
    [SerializeField] private Slider _CoolTimeSlider;

    [SerializeField] private TextMeshProUGUI _curHpText;
    [SerializeField] private TextMeshProUGUI _maxHpText;
    [SerializeField] private TextMeshProUGUI _curCTText;
    [SerializeField] private TextMeshProUGUI _maxCTText;

    [SerializeField] private Color[] _synergyColors;
    [SerializeField] private Sprite[] _synergySprites;
    [SerializeField] private Image _synergyBg;
    [SerializeField] private Image _synergyIcon;

    [SerializeField] private Color[] _typeColors;
    [SerializeField] private Sprite[] _typeSprites;
    [SerializeField] private Image _typeBg;
    [SerializeField] private Image _typeIcon;

    [SerializeField] private TextMeshProUGUI _AT_text;
    [SerializeField] private TextMeshProUGUI _DF_text;
    [SerializeField] private TextMeshProUGUI _AS_text;
    [SerializeField] private TextMeshProUGUI _CC_text;
    [SerializeField] private TextMeshProUGUI _AR_text;

    [SerializeField] private Image _skillIcon;
    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI _skillDesc;

    [SerializeField] private float posX;
    [SerializeField] private float posY;

    float screenWidth = Screen.width;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 ������ ��ư Ŭ�� ����
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // ���콺 ��ġ���� ���̸� �߻�
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ���̰� � ������Ʈ�� �浹�ߴ��� Ȯ��
            if (Physics.Raycast(ray, out hit))
            {
                Unit unit = hit.collider.GetComponent<Unit>();
                if (unit != null)
                {
                    _unit = unit;
                    OpenPopUp();
                }
            }
            else
            {
                PopUpClose();
                _unit = null;
            }
        }

        if (!_unit)
        {
            PopUpClose();
            return;
        }
        statusUpdate();
    }

    public void init()
    {
        if (!_unit) return;

        // ���� �κ�
        _unitData = _unit._unitData;
        _rateSprite.sprite = _rateSprites[_unit._unitRate.GetHashCode()];
        _unitIcon.sprite = _unitData.icon;
        _unitName.text = _unitData.UnitName;

        // ��ų �κ�
        _skillIcon.sprite = _unitData._unitSkill.icon;
        _skillName.text = _unitData._unitSkill.SkillName;
        _skillDesc.text = _unitData._unitSkill.SkillDesc;

        // �ó���, Ÿ�� �κ�
        if (_unit._attackType == Unit.AttackType.Object)
        {
            _synergyBg.transform.localScale = Vector3.zero;
            _typeBg.transform.localScale = Vector3.zero;
            _synergyIcon.sprite = _nullSprite;
            _typeIcon.sprite = _nullSprite;
        }
        else
        {
            _synergyBg.transform.localScale = Vector3.one;
            _typeBg.transform.localScale = Vector3.one;
            _synergyIcon.sprite = _synergySprites[_unit._attackType.GetHashCode()];
            _synergyBg.color = _synergyColors[_unit._attackType.GetHashCode()];

            _typeIcon.sprite = _typeSprites[_unit._unitType.GetHashCode()];
            _typeBg.color = _typeColors[_unit._unitType.GetHashCode()];
        }
    }

    string ReturnSynergy(int i)
    {
        string ResultString;
        switch (i)
        {
            case 0:
                ResultString = "����";
                break;
            case 1:
                ResultString = "�ü�";
                break;
            case 2:
                ResultString = "����";
                break;
            case 3:
                ResultString = "�ϻ���";
                break;
            case 4:
                ResultString = "������";
                break;
            case 5:
                ResultString = "����";
                break;
            default:
                ResultString = "null";
                break;
        }
        return ResultString;
    }

    string ReturnType(int i)
    {
        string ResultString;
        switch (i)
        {
            case 0:
                ResultString = "�ΰ�";
                break;
            case 1:
                ResultString = "�䱫";
                break;
            case 2:
                ResultString = "����";
                break;
            case 3:
                ResultString = "�Ǳ�";
                break;
            default:
                ResultString = "null";
                break;
        }
        return ResultString;
    }



    void statusUpdate()
    {
        _HpSlider.value = _unit._unitHp / _unit._unitMaxHp;
        _CoolTimeSlider.value = _unit._skillTimer / _unit._unitCT;

        _curHpText.text = _unit._unitHp.ToString("N0");
        _maxHpText.text = _unit._unitMaxHp.ToString("N0");

        _curCTText.text = _unit._skillTimer.ToString("F1");
        _maxCTText.text = _unit._unitCT.ToString("F1");

        _AT_text.text = (_unit._unitAT + _unit._buffAT - _unit._deBuffAT).ToString("F0");
        _DF_text.text = (_unit._unitDF + _unit._buffDF - +_unit._deBuffDF).ToString("F0");
        _AS_text.text = (_unit._unitAS + _unit._buffAS - _unit._deBuffAS).ToString("F1");
        _CC_text.text = ((_unit._unitCC + _unit._buffCC - _unit._deBuffCC) * 100f).ToString("F0") + "%";
        _AR_text.text = (_unit._unitAR).ToString("F1");
    }

    public void OpenPopUp()
    {
        this.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        init();
    }

    public void PopUpClose()
    {
        this.transform.localScale = Vector3.zero;
    }
}
