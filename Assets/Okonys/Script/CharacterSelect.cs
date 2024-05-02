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
                _typeText.text = "인간";
                _typeText.color = _typeColor[0];
                _typeDescText.text = "전투가 시작되면 랜덤하게 리더가 선택 됩니다. 리더는 강력한 스텟 버프를 얻습니다.";
                break;
            case 1:
                _typeText.text = "요괴";
                _typeText.color = _typeColor[1];
                _typeDescText.text = "전투가 시작될 때 아군 요괴가 적 인간보다 많다면 능력치가 상승됩니다. 요괴는 쉽게 죽지 않습니다.";
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
                _synergyText.text = "전사";
                _synergyText.color = _synergyColor[0];
                _synergyDescText.text = "아군의 체력을 상승 시키고, 아군을 보호 합니다.";
                break;
            case 1:
                _synergyText.text = "궁수";
                _synergyText.color = _synergyColor[1];
                _synergyDescText.text = "멀리서 적을 공격하며, 일정 시간마다 공격속도가 상승 합니다.";
                break;
            case 2:
                _synergyText.text = "도사";
                _synergyText.color = _synergyColor[2];
                _synergyDescText.text = "강력한 마법을 사용하며, 스킬 쿨타임이 감소 합니다.";
                break;
            case 3:
                _synergyText.text = "암살자";
                _synergyText.color = _synergyColor[3];
                _synergyDescText.text = "적 진영 후방으로 침투하여 공격 합니다. 치명타 확률이 높습니다.";
                break;
            case 4:
                _synergyText.text = "지원가";
                _synergyText.color = _synergyColor[4];
                _synergyDescText.text = "전투에 도움이 되는 버프들을 사용 합니다.";
                break;
            case 5:
                _synergyText.text = "상인";
                _synergyText.color = _synergyColor[5];
                _synergyDescText.text = "골드를 더 많이 획득 할 수 있습니다.";
                break;
        }
    }
}
