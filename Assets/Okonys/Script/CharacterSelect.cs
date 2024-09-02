using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [Header("Character Transforms")]
    [SerializeField] private Transform _basicHumanCharacters; // �⺻ �ΰ� ĳ���͵�
    [SerializeField] private Transform _basicGhostCharacters; // �⺻ �䱫 ĳ���͵�

    [Header("Character Lists")]
    [SerializeField] private List<GameObject> _humanCharacters = new List<GameObject>(); // �ΰ� ĳ���� ����Ʈ
    [SerializeField] private List<GameObject> _ghostCharacters = new List<GameObject>(); // �䱫 ĳ���� ����Ʈ

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _characterName; // ���õ� ĳ���� �̸�
    [SerializeField] private Image[] _checkType; // ĳ���� Ÿ�� ���� üũ �̹���
    [SerializeField] private Image[] _checkSynergy; // �ó��� ���� üũ �̹���
    [SerializeField] private Color[] _typeColor; // Ÿ�� ���� �迭
    [SerializeField] private Color[] _synergyColor; // �ó��� ���� �迭
    [SerializeField] private TextMeshProUGUI _typeText; // Ÿ�� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI _synergyText; // �ó��� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI[] _statusText; // ĳ���� ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI _typeDescText; // Ÿ�� ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI _synergyDescText; // �ó��� ���� �ؽ�Ʈ
    [SerializeField] private Button _continueButton; // ��� ��ư

    [Header("Selected Indexes")]
    [SerializeField] private int _slectType = 0; // ���õ� Ÿ�� �ε���
    [SerializeField] private int _slectSynergy = 0; // ���õ� �ó��� �ε���
    [SerializeField] private int _slectCharacter = 0; // ���õ� ĳ���� �ε���

    private Vector3 _selectSize = new Vector3(3f, 3f, 3f); // ���õ� ĳ������ ũ��
    private UserInfoManager _userInfoManager; // ����� ���� �Ŵ���
    private OptionPopup _optionPopup; // �ɼ� �˾�

    private void Awake()
    {
        // ĳ���� ��� �ʱ�ȭ
        InitializeCharacterList(_basicHumanCharacters, _humanCharacters);
        InitializeCharacterList(_basicGhostCharacters, _ghostCharacters);
    }

    private void Start()
    {
        // ����� ���� �� �ɼ� �˾� �ʱ�ȭ
        _userInfoManager = UserInfoManager.Instance;
        _optionPopup = OptionPopup.Instance;
        _continueButton.interactable = _userInfoManager.userData.isUserData;
    }

    private void InitializeCharacterList(Transform parentTransform, List<GameObject> characterList)
    {
        // �ڽ� ��ü�� ����Ʈ�� �߰��ϰ� ũ�⸦ �ʱ�ȭ
        int childCount = parentTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            characterList.Add(parentTransform.GetChild(i).gameObject);
            parentTransform.GetChild(i).localScale = Vector3.zero;
        }
    }

    public void CheckTypeUpdate()
    {
        // Ÿ�� ������Ʈ
        for (int i = 0; i < _checkType.Length; i++)
        {
            _checkType[i].enabled = (i == _slectType);
        }

        switch (_slectType)
        {
            case 0:
                UpdateTypeInfo("�ΰ�", _typeColor[0], "�ΰ� �� �����ϰ� ������ ���� �˴ϴ�.\n\n������ ������ ���� ������ ����ϴ�.");
                break;
            case 1:
                UpdateTypeInfo("�䱫", _typeColor[1], "�䱫�� ��� �ΰ����� ���ٸ� �ɷ�ġ�� ����ϴ�.\n\n�䱫���� �ױ� ���� �߾��մϴ�.");
                break;
        }
    }

    private void UpdateTypeInfo(string typeName, Color typeColor, string typeDescription)
    {
        // Ÿ�� ������ ������Ʈ
        _typeText.text = typeName;
        _typeText.color = typeColor;
        _typeDescText.text = typeDescription;
    }

    public void CheckSynergyUpdate()
    {
        // �ó��� ������Ʈ
        if (_slectType == 0)
        {
            UpdateHumanCharacters();
        }
        else if (_slectType == 1)
        {
            UpdateGhostCharacters();
        }
    }

    private void UpdateHumanCharacters()
    {
        // �䱫 ĳ���͵� ����
        _basicGhostCharacters.gameObject.SetActive(false);

        // �ΰ� ĳ���͵� ���̱�
        _basicHumanCharacters.gameObject.SetActive(true);

        UpdateCharacterSelection(_humanCharacters, _checkSynergy, _slectSynergy, _selectSize, _slectSynergy + 6);
    }

    private void UpdateGhostCharacters()
    {
        // �ΰ� ĳ���͵� ����
        _basicHumanCharacters.gameObject.SetActive(false);

        // �䱫 ĳ���͵� ���̱�
        _basicGhostCharacters.gameObject.SetActive(true);

        UpdateCharacterSelection(_ghostCharacters, _checkSynergy, _slectSynergy, _selectSize, _slectSynergy + 6);
    }

    private void UpdateCharacterSelection(List<GameObject> characters, Image[] checkSynergy, int selectedSynergy, Vector3 selectSize, int statusIndexOffset)
    {
        // �ó��� �� ĳ���� ���� ���� ������Ʈ
        for (int i = 0; i < checkSynergy.Length; i++)
        {
            checkSynergy[i].enabled = (i == selectedSynergy);
            characters[i].transform.localScale = (i == selectedSynergy) ? selectSize : Vector3.zero;
        }

        UpdateSynergyInfo(selectedSynergy, statusIndexOffset);
    }

    private void UpdateSynergyInfo(int selectedSynergy, int statusIndexOffset)
    {
        // �ó��� ���� ������Ʈ
        switch (selectedSynergy)
        {
            case 0:
                SetSynergyInfo("����", "������", _synergyColor[0], "�Ʊ��� ü���� ��� ��Ű��,\n\n�Ʊ��� ��ȣ �մϴ�.", statusIndexOffset);
                break;
            case 1:
                SetSynergyInfo("�ü�", "��ɲ�", _synergyColor[1], "�ָ��� ���� �����ϸ�,\n\n���� �ð����� ���ݼӵ��� ��� �մϴ�.", statusIndexOffset);
                break;
            case 2:
                SetSynergyInfo("����", "����", _synergyColor[2], "������ ������ ����ϸ�,\n\n��ų ��Ÿ���� ���� �մϴ�.", statusIndexOffset);
                break;
            case 3:
                SetSynergyInfo("�ϻ���", "������", _synergyColor[3], "�� ���� �Ĺ����� ħ���Ͽ� ���� �մϴ�.\n\nġ��Ÿ Ȯ���� �����ϴ�.", statusIndexOffset);
                break;
            case 4:
                SetSynergyInfo("������", "�ָ�", _synergyColor[4], "������ ������ �Ǵ� �������� ��� �մϴ�.", statusIndexOffset);
                break;
            case 5:
                SetSynergyInfo("����", "����", _synergyColor[5], "��带 �� ���� ȹ�� �� �� �ֽ��ϴ�.", statusIndexOffset);
                break;
        }
    }

    private void SetSynergyInfo(string synergyName, string characterName, Color synergyColor, string synergyDescription, int statusIndex)
    {
        // �ó��� ������ ����
        _synergyText.text = synergyName;
        _characterName.text = characterName;
        _synergyText.color = synergyColor;
        _synergyDescText.text = synergyDescription;
        SetStatusText(statusIndex);
    }

    public void SetType(int index)
    {
        _slectType = index;
    }

    public void SetSynergy(int index)
    {
        _slectSynergy = index;
    }

    private void SetStatusText(int index)
    {
        // ĳ���� ���� �ؽ�Ʈ ������Ʈ
        _slectCharacter = index;
        UnitData unit = SoonsoonData.Instance.Unit_pool._unitDatas[index];
        _statusText[0].text = unit._unitAT[0].ToString("F0");
        _statusText[1].text = unit._unitDF[0].ToString("F0");
        _statusText[2].text = unit._unitAS[0].ToString("F1");
        _statusText[3].text = unit._unitCC[0].ToString("F0") + "%";
        _statusText[4].text = unit._unitAR[0].ToString("F1");
    }

    public void CharacterSelectComplete()
    {
        // ĳ���� ���� �Ϸ� �� �� ��ȯ
        _userInfoManager.GameDataCreate(_slectCharacter);
        _optionPopup.MainButtonEnable(true);
        LoadingSceneController.LoadScene("inGameScene");
    }

    public void ContinueGame()
    {
        // ���� ����ϱ�
        if (!_userInfoManager.userData.isUserData) return;
        LoadingSceneController.LoadScene("inGameScene");
        _optionPopup.MainButtonEnable(true);
    }

    public void GameExit()
    {
        // ���� ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
