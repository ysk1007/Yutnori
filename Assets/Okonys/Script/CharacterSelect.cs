using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {

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
        }

        switch (_slectSynergy)
        {
            case 0:
                _synergyText.text = "����";
                _synergyText.color = _synergyColor[0];
                _synergyDescText.text = "�Ʊ��� ü���� ��� ��Ű��, �Ʊ��� ��ȣ �մϴ�.";
                break;
            case 1:
                _synergyText.text = "�ü�";
                _synergyText.color = _synergyColor[1];
                _synergyDescText.text = "�ָ��� ���� �����ϸ�, ���� �ð����� ���ݼӵ��� ��� �մϴ�.";
                break;
            case 2:
                _synergyText.text = "����";
                _synergyText.color = _synergyColor[2];
                _synergyDescText.text = "������ ������ ����ϸ�, ��ų ��Ÿ���� ���� �մϴ�.";
                break;
            case 3:
                _synergyText.text = "�ϻ���";
                _synergyText.color = _synergyColor[3];
                _synergyDescText.text = "�� ���� �Ĺ����� ħ���Ͽ� ���� �մϴ�. ġ��Ÿ Ȯ���� �����ϴ�.";
                break;
            case 4:
                _synergyText.text = "������";
                _synergyText.color = _synergyColor[4];
                _synergyDescText.text = "������ ������ �Ǵ� �������� ��� �մϴ�.";
                break;
            case 5:
                _synergyText.text = "����";
                _synergyText.color = _synergyColor[5];
                _synergyDescText.text = "��带 �� ���� ȹ�� �� �� �ֽ��ϴ�.";
                break;
        }
    }
}
