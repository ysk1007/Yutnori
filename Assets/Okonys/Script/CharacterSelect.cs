using System;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] private Transform _basicHumanCharacters;
    [SerializeField] private Transform _basicGhostCharacters;

    [SerializeField] private List<GameObject> _humanCharacters;
    [SerializeField] private List<GameObject> _ghostCharacters;

    [SerializeField] private TextMeshProUGUI _characterName;

    [SerializeField] private Color[] _checkColors;
    [SerializeField] private Image[] _checkType;
    [SerializeField] private Image[] _checkSynergy;

    [SerializeField] private Color[] _typeColor;
    [SerializeField] private Color[] _synergyColor;


    [SerializeField] private TextMeshProUGUI _typeText;
    [SerializeField] private TextMeshProUGUI _synergyText;

    [SerializeField] private TextMeshProUGUI _typeDescText;
    [SerializeField] private TextMeshProUGUI _synergyDescText;

    [SerializeField] int _slectType = 0;
    [SerializeField] int _slectSynergy = 0;

    Vector3 _selectSize = new Vector3(3f, 3f, 3f);

    private void Awake()
    {
        int childCount = _basicHumanCharacters.childCount;

        for (int i = 0; i < childCount; i++)
        {
            _humanCharacters.Add(_basicHumanCharacters.GetChild(i).gameObject);
            _basicHumanCharacters.GetChild(i).localScale = Vector3.zero;
        }

        for (int i = 0; i < childCount; i++)
        {
            _ghostCharacters.Add(_basicGhostCharacters.GetChild(i).gameObject);
            _basicGhostCharacters.GetChild(i).localScale = Vector3.zero;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckTypeUpdate()
    {
        for (int i = 0; i < _checkType.Length; i++)
        {
            _checkType[i].color = (i == _slectType) ? _checkColors[1] : _checkColors[0];
        }

        switch (_slectType)
        {
            case 0:
                _typeText.text = "�ΰ�";
                _typeText.color = _typeColor[0];
                _typeDescText.text = "������ ���۵Ǹ� �����ϰ� ������ ���� �˴ϴ�. ������ ������ ���� ������ ����ϴ�.";
                break;
            case 1:
                _typeText.text = "�䱫";
                _typeText.color = _typeColor[1];
                _typeDescText.text = "������ ���۵� �� �Ʊ� �䱫�� �� �ΰ����� ���ٸ� �ɷ�ġ�� ��µ˴ϴ�. �䱫�� ���� ���� �ʽ��ϴ�.";
                break;
        }
    }

    public void CheckSynergyUpdate()
    {
        switch (_slectType)
        {
            case 0:
                _basicGhostCharacters.gameObject.SetActive(false);
                _basicHumanCharacters.gameObject.SetActive(true);

                for (int i = 0; i < _checkSynergy.Length; i++)
                {
                    _checkSynergy[i].color = (i == _slectSynergy) ? _checkColors[1] : _checkColors[0];
                    _humanCharacters[i].transform.localScale = ((i == _slectSynergy) ? _selectSize : Vector3.zero);
                }

                switch (_slectSynergy)
                {
                    case 0:
                        _synergyText.text = "����";
                        _characterName.text = "������";
                        _synergyText.color = _synergyColor[0];
                        _synergyDescText.text = "�Ʊ��� ü���� ��� ��Ű��, �Ʊ��� ��ȣ �մϴ�.";
                        break;
                    case 1:
                        _synergyText.text = "�ü�";
                        _characterName.text = "��ɲ�";
                        _synergyText.color = _synergyColor[1];
                        _synergyDescText.text = "�ָ��� ���� �����ϸ�, ���� �ð����� ���ݼӵ��� ��� �մϴ�.";
                        break;
                    case 2:
                        _synergyText.text = "����";
                        _characterName.text = "����";
                        _synergyText.color = _synergyColor[2];
                        _synergyDescText.text = "������ ������ ����ϸ�, ��ų ��Ÿ���� ���� �մϴ�.";
                        break;
                    case 3:
                        _synergyText.text = "�ϻ���";
                        _characterName.text = "������";
                        _synergyText.color = _synergyColor[3];
                        _synergyDescText.text = "�� ���� �Ĺ����� ħ���Ͽ� ���� �մϴ�. ġ��Ÿ Ȯ���� �����ϴ�.";
                        break;
                    case 4:
                        _synergyText.text = "������";
                        _characterName.text = "�ָ�";
                        _synergyText.color = _synergyColor[4];
                        _synergyDescText.text = "������ ������ �Ǵ� �������� ��� �մϴ�.";
                        break;
                    case 5:
                        _synergyText.text = "����";
                        _characterName.text = "����";
                        _synergyText.color = _synergyColor[5];
                        _synergyDescText.text = "��带 �� ���� ȹ�� �� �� �ֽ��ϴ�.";
                        break;
                }
                break;
            case 1:
                _basicHumanCharacters.gameObject.SetActive(false);
                _basicGhostCharacters.gameObject.SetActive(true);

                for (int i = 0; i < _checkSynergy.Length; i++)
                {
                    _checkSynergy[i].color = (i == _slectSynergy) ? _checkColors[1] : _checkColors[0];
                    _ghostCharacters[i].transform.localScale = ((i == _slectSynergy) ? _selectSize : Vector3.zero);
                }

                switch (_slectSynergy)
                {
                    case 0:
                        _synergyText.text = "����";
                        _characterName.text = "��Ͻô�";
                        _synergyText.color = _synergyColor[0];
                        _synergyDescText.text = "�Ʊ��� ü���� ��� ��Ű��, �Ʊ��� ��ȣ �մϴ�.";
                        break;
                    case 1:
                        _synergyText.text = "�ü�";
                        _characterName.text = "���";
                        _synergyText.color = _synergyColor[1];
                        _synergyDescText.text = "�ָ��� ���� �����ϸ�, ���� �ð����� ���ݼӵ��� ��� �մϴ�.";
                        break;
                    case 2:
                        _synergyText.text = "����";
                        _characterName.text = "��ȣ��";
                        _synergyText.color = _synergyColor[2];
                        _synergyDescText.text = "������ ������ ����ϸ�, ��ų ��Ÿ���� ���� �մϴ�.";
                        break;
                    case 3:
                        _synergyText.text = "�ϻ���";
                        _characterName.text = "����";
                        _synergyText.color = _synergyColor[3];
                        _synergyDescText.text = "�� ���� �Ĺ����� ħ���Ͽ� ���� �մϴ�. ġ��Ÿ Ȯ���� �����ϴ�.";
                        break;
                    case 4:
                        _synergyText.text = "������";
                        _characterName.text = "�췷����";
                        _synergyText.color = _synergyColor[4];
                        _synergyDescText.text = "������ ������ �Ǵ� �������� ��� �մϴ�.";
                        break;
                    case 5:
                        _synergyText.text = "����";
                        _characterName.text = "���� ������";
                        _synergyText.color = _synergyColor[5];
                        _synergyDescText.text = "��带 �� ���� ȹ�� �� �� �ֽ��ϴ�.";
                        break;
                }
                break;
        }
    }

    public void SetType(int index)
    {
        _slectType = index;
    }

    public void SetSynergy(int index)
    {
        _slectSynergy = index;
    }
}
