using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] private Transform _basicHumanCharacters;
    [SerializeField] private Transform _basicGhostCharacters;

    [SerializeField] private List<GameObject> _humanCharacters;
    [SerializeField] private List<GameObject> _ghostCharacters;

    [SerializeField] private TextMeshProUGUI _characterName;

    [SerializeField] private Image[] _checkType;
    [SerializeField] private Image[] _checkSynergy;

    [SerializeField] private Color[] _typeColor;
    [SerializeField] private Color[] _synergyColor;


    [SerializeField] private TextMeshProUGUI _typeText;
    [SerializeField] private TextMeshProUGUI _synergyText;

    [SerializeField] private TextMeshProUGUI[] _statusText;

    [SerializeField] private TextMeshProUGUI _typeDescText;
    [SerializeField] private TextMeshProUGUI _synergyDescText;

    [SerializeField] private Button _continueButton;

    [SerializeField] int _slectType = 0;
    [SerializeField] int _slectSynergy = 0;
    [SerializeField] int _slectCharacter = 0;

    Vector3 _selectSize = new Vector3(3f, 3f, 3f);
    UserInfoManager _userInfoManager;
    OptionPopup _optionPopup;

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
        _userInfoManager = UserInfoManager.Instance;
        _optionPopup = OptionPopup.Instance;
        _continueButton.interactable = (_userInfoManager.userData.isUserData)? true : false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckTypeUpdate()
    {
        for (int i = 0; i < _checkType.Length; i++)
        {
            _checkType[i].enabled = (i == _slectType) ? true : false;
        }

        switch (_slectType)
        {
            case 0:
                _typeText.text = "인간";
                _typeText.color = _typeColor[0];
                _typeDescText.text = "인간 중 랜덤하게 리더가 선택 됩니다.\n\n리더는 강력한 스텟 버프를 얻습니다.";
                break;
            case 1:
                _typeText.text = "요괴";
                _typeText.color = _typeColor[1];
                _typeDescText.text = "요괴가 상대 인간보다 많다면 능력치를 얻습니다.\n\n요괴들은 죽기 직전 발악합니다.";
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
                    _checkSynergy[i].enabled = (i == _slectSynergy) ? true : false;
                    _humanCharacters[i].transform.localScale = ((i == _slectSynergy) ? _selectSize : Vector3.zero);
                }

                switch (_slectSynergy)
                {
                    case 0:
                        _synergyText.text = "전사";
                        _characterName.text = "나무꾼";
                        _synergyText.color = _synergyColor[0];
                        _synergyDescText.text = "아군의 체력을 상승 시키고,\n\n아군을 보호 합니다.";
                        SetStatusText(0);
                        break;
                    case 1:
                        _synergyText.text = "궁수";
                        _characterName.text = "사냥꾼";
                        _synergyText.color = _synergyColor[1];
                        _synergyDescText.text = "멀리서 적을 공격하며,\n\n일정 시간마다 공격속도가 상승 합니다.";
                        SetStatusText(1);
                        break;
                    case 2:
                        _synergyText.text = "도사";
                        _characterName.text = "선비";
                        _synergyText.color = _synergyColor[2];
                        _synergyDescText.text = "강력한 마법을 사용하며,\n\n스킬 쿨타임이 감소 합니다.";
                        SetStatusText(2);
                        break;
                    case 3:
                        _synergyText.text = "암살자";
                        _characterName.text = "좀도둑";
                        _synergyText.color = _synergyColor[3];
                        _synergyDescText.text = "적 진영 후방으로 침투하여 공격 합니다.\n\n치명타 확률이 높습니다.";
                        SetStatusText(3);
                        break;
                    case 4:
                        _synergyText.text = "지원가";
                        _characterName.text = "주모";
                        _synergyText.color = _synergyColor[4];
                        _synergyDescText.text = "전투에 도움이 되는 버프들을 사용 합니다.";
                        SetStatusText(4);
                        break;
                    case 5:
                        _synergyText.text = "상인";
                        _characterName.text = "거지";
                        _synergyText.color = _synergyColor[5];
                        _synergyDescText.text = "골드를 더 많이 획득 할 수 있습니다.";
                        SetStatusText(5);
                        break;
                }
                break;
            case 1:
                _basicHumanCharacters.gameObject.SetActive(false);
                _basicGhostCharacters.gameObject.SetActive(true);

                for (int i = 0; i < _checkSynergy.Length; i++)
                {
                    _checkSynergy[i].enabled = (i == _slectSynergy) ? true : false;
                    _ghostCharacters[i].transform.localScale = ((i == _slectSynergy) ? _selectSize : Vector3.zero);
                }

                switch (_slectSynergy)
                {
                    case 0:
                        _synergyText.text = "전사";
                        _characterName.text = "어둑시니";
                        _synergyText.color = _synergyColor[0];
                        _synergyDescText.text = "아군의 체력을 상승 시키고,\n\n아군을 보호 합니다.";
                        SetStatusText(6);
                        break;
                    case 1:
                        _synergyText.text = "궁수";
                        _characterName.text = "득옥";
                        _synergyText.color = _synergyColor[1];
                        _synergyDescText.text = "멀리서 적을 공격하며,\n\n일정 시간마다 공격속도가 상승 합니다.";
                        SetStatusText(7);
                        break;
                    case 2:
                        _synergyText.text = "도사";
                        _characterName.text = "노호정";
                        _synergyText.color = _synergyColor[2];
                        _synergyDescText.text = "강력한 마법을 사용하며,\n\n스킬 쿨타임이 감소 합니다.";
                        SetStatusText(8);
                        break;
                    case 3:
                        _synergyText.text = "암살자";
                        _characterName.text = "귀태";
                        _synergyText.color = _synergyColor[3];
                        _synergyDescText.text = "적 진영 후방으로 침투하여 공격 합니다.\n\n치명타 확률이 높습니다.";
                        SetStatusText(9);
                        break;
                    case 4:
                        _synergyText.text = "지원가";
                        _characterName.text = "우렁각시";
                        _synergyText.color = _synergyColor[4];
                        _synergyDescText.text = "전투에 도움이 되는 버프들을 사용 합니다.";
                        SetStatusText(10);
                        break;
                    case 5:
                        _synergyText.text = "상인";
                        _characterName.text = "꼬마 도깨비";
                        _synergyText.color = _synergyColor[5];
                        _synergyDescText.text = "골드를 더 많이 획득 할 수 있습니다.";
                        SetStatusText(11);
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

    void SetStatusText(int index)
    {
        _slectCharacter = index;
        UnitData unit = SoonsoonData.Instance.Unit_pool._unitDatas[index];
        _statusText[0].text = unit._unitAT[0].ToString("F0");
        _statusText[1].text = unit._unitDF[0].ToString("F0");
        _statusText[2].text = unit._unitAS[0].ToString("F1");
        _statusText[3].text = unit._unitCC[0].ToString("F0") + "%";
        _statusText[4].text = unit._unitAR[0].ToString("F1");
    }

    public void CharacterSelectComplite()
    {
        _userInfoManager.GameDataCreate(_slectCharacter);
        _optionPopup.MainButtonEnable(true);
        LoadingSceneController.LoadScene("inGameScene");
    }

    public void ContinueGame()
    {
        if (!_userInfoManager.userData.isUserData) return;
        LoadingSceneController.LoadScene("inGameScene");
        _optionPopup.MainButtonEnable(true);
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
