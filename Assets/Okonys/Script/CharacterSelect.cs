using System;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] private Transform _basicCharacters;
    [SerializeField] private List<GameObject> _characters;

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

    int _slectType = 0;
    int _slectSynergy = 0;

    Vector3 _selectSize = new Vector3(3f, 3f, 3f);

    private void Awake()
    {
        int childCount = _basicCharacters.childCount;
        for (int i = 0; i < childCount; i++)
        {
            _characters.Add(_basicCharacters.GetChild(i).gameObject);
            _basicCharacters.GetChild(i).localScale = Vector3.zero;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckTypeUpdate(0);
        CheckSynergyUpdate(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckTypeUpdate(int index)
    {
        _slectType = index;
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

    public void CheckSynergyUpdate(int index)
    {
        _slectSynergy = index;
        for (int i = 0; i < _checkSynergy.Length; i++)
        {
            _checkSynergy[i].color = (i == _slectSynergy) ? _checkColors[1] : _checkColors[0];
            _characters[i].transform.localScale = ((i == _slectSynergy) ? _selectSize : Vector3.zero);
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
    }
}
