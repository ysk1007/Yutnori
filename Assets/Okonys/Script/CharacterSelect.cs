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
                _typeText.text = "�ΰ�";
                _typeText.color = _typeColor[0];
                _typeDescText.text = "�ΰ� �� �����ϰ� ������ ���� �˴ϴ�.\n\n������ ������ ���� ������ ����ϴ�.";
                break;
            case 1:
                _typeText.text = "�䱫";
                _typeText.color = _typeColor[1];
                _typeDescText.text = "�䱫�� ��� �ΰ����� ���ٸ� �ɷ�ġ�� ����ϴ�.\n\n�䱫���� �ױ� ���� �߾��մϴ�.";
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
                        _synergyText.text = "����";
                        _characterName.text = "������";
                        _synergyText.color = _synergyColor[0];
                        _synergyDescText.text = "�Ʊ��� ü���� ��� ��Ű��,\n\n�Ʊ��� ��ȣ �մϴ�.";
                        SetStatusText(0);
                        break;
                    case 1:
                        _synergyText.text = "�ü�";
                        _characterName.text = "��ɲ�";
                        _synergyText.color = _synergyColor[1];
                        _synergyDescText.text = "�ָ��� ���� �����ϸ�,\n\n���� �ð����� ���ݼӵ��� ��� �մϴ�.";
                        SetStatusText(1);
                        break;
                    case 2:
                        _synergyText.text = "����";
                        _characterName.text = "����";
                        _synergyText.color = _synergyColor[2];
                        _synergyDescText.text = "������ ������ ����ϸ�,\n\n��ų ��Ÿ���� ���� �մϴ�.";
                        SetStatusText(2);
                        break;
                    case 3:
                        _synergyText.text = "�ϻ���";
                        _characterName.text = "������";
                        _synergyText.color = _synergyColor[3];
                        _synergyDescText.text = "�� ���� �Ĺ����� ħ���Ͽ� ���� �մϴ�.\n\nġ��Ÿ Ȯ���� �����ϴ�.";
                        SetStatusText(3);
                        break;
                    case 4:
                        _synergyText.text = "������";
                        _characterName.text = "�ָ�";
                        _synergyText.color = _synergyColor[4];
                        _synergyDescText.text = "������ ������ �Ǵ� �������� ��� �մϴ�.";
                        SetStatusText(4);
                        break;
                    case 5:
                        _synergyText.text = "����";
                        _characterName.text = "����";
                        _synergyText.color = _synergyColor[5];
                        _synergyDescText.text = "��带 �� ���� ȹ�� �� �� �ֽ��ϴ�.";
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
                        _synergyText.text = "����";
                        _characterName.text = "��Ͻô�";
                        _synergyText.color = _synergyColor[0];
                        _synergyDescText.text = "�Ʊ��� ü���� ��� ��Ű��,\n\n�Ʊ��� ��ȣ �մϴ�.";
                        SetStatusText(6);
                        break;
                    case 1:
                        _synergyText.text = "�ü�";
                        _characterName.text = "���";
                        _synergyText.color = _synergyColor[1];
                        _synergyDescText.text = "�ָ��� ���� �����ϸ�,\n\n���� �ð����� ���ݼӵ��� ��� �մϴ�.";
                        SetStatusText(7);
                        break;
                    case 2:
                        _synergyText.text = "����";
                        _characterName.text = "��ȣ��";
                        _synergyText.color = _synergyColor[2];
                        _synergyDescText.text = "������ ������ ����ϸ�,\n\n��ų ��Ÿ���� ���� �մϴ�.";
                        SetStatusText(8);
                        break;
                    case 3:
                        _synergyText.text = "�ϻ���";
                        _characterName.text = "����";
                        _synergyText.color = _synergyColor[3];
                        _synergyDescText.text = "�� ���� �Ĺ����� ħ���Ͽ� ���� �մϴ�.\n\nġ��Ÿ Ȯ���� �����ϴ�.";
                        SetStatusText(9);
                        break;
                    case 4:
                        _synergyText.text = "������";
                        _characterName.text = "�췷����";
                        _synergyText.color = _synergyColor[4];
                        _synergyDescText.text = "������ ������ �Ǵ� �������� ��� �մϴ�.";
                        SetStatusText(10);
                        break;
                    case 5:
                        _synergyText.text = "����";
                        _characterName.text = "���� ������";
                        _synergyText.color = _synergyColor[5];
                        _synergyDescText.text = "��带 �� ���� ȹ�� �� �� �ֽ��ϴ�.";
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
