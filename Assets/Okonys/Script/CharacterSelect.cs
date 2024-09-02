using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [Header("Character Transforms")]
    [SerializeField] private Transform _basicHumanCharacters; // 기본 인간 캐릭터들
    [SerializeField] private Transform _basicGhostCharacters; // 기본 요괴 캐릭터들

    [Header("Character Lists")]
    [SerializeField] private List<GameObject> _humanCharacters = new List<GameObject>(); // 인간 캐릭터 리스트
    [SerializeField] private List<GameObject> _ghostCharacters = new List<GameObject>(); // 요괴 캐릭터 리스트

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _characterName; // 선택된 캐릭터 이름
    [SerializeField] private Image[] _checkType; // 캐릭터 타입 선택 체크 이미지
    [SerializeField] private Image[] _checkSynergy; // 시너지 선택 체크 이미지
    [SerializeField] private Color[] _typeColor; // 타입 색상 배열
    [SerializeField] private Color[] _synergyColor; // 시너지 색상 배열
    [SerializeField] private TextMeshProUGUI _typeText; // 타입 텍스트
    [SerializeField] private TextMeshProUGUI _synergyText; // 시너지 텍스트
    [SerializeField] private TextMeshProUGUI[] _statusText; // 캐릭터 상태 텍스트
    [SerializeField] private TextMeshProUGUI _typeDescText; // 타입 설명 텍스트
    [SerializeField] private TextMeshProUGUI _synergyDescText; // 시너지 설명 텍스트
    [SerializeField] private Button _continueButton; // 계속 버튼

    [Header("Selected Indexes")]
    [SerializeField] private int _slectType = 0; // 선택된 타입 인덱스
    [SerializeField] private int _slectSynergy = 0; // 선택된 시너지 인덱스
    [SerializeField] private int _slectCharacter = 0; // 선택된 캐릭터 인덱스

    private Vector3 _selectSize = new Vector3(3f, 3f, 3f); // 선택된 캐릭터의 크기
    private UserInfoManager _userInfoManager; // 사용자 정보 매니저
    private OptionPopup _optionPopup; // 옵션 팝업

    private void Awake()
    {
        // 캐릭터 목록 초기화
        InitializeCharacterList(_basicHumanCharacters, _humanCharacters);
        InitializeCharacterList(_basicGhostCharacters, _ghostCharacters);
    }

    private void Start()
    {
        // 사용자 정보 및 옵션 팝업 초기화
        _userInfoManager = UserInfoManager.Instance;
        _optionPopup = OptionPopup.Instance;
        _continueButton.interactable = _userInfoManager.userData.isUserData;
    }

    private void InitializeCharacterList(Transform parentTransform, List<GameObject> characterList)
    {
        // 자식 객체를 리스트에 추가하고 크기를 초기화
        int childCount = parentTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            characterList.Add(parentTransform.GetChild(i).gameObject);
            parentTransform.GetChild(i).localScale = Vector3.zero;
        }
    }

    public void CheckTypeUpdate()
    {
        // 타입 업데이트
        for (int i = 0; i < _checkType.Length; i++)
        {
            _checkType[i].enabled = (i == _slectType);
        }

        switch (_slectType)
        {
            case 0:
                UpdateTypeInfo("인간", _typeColor[0], "인간 중 랜덤하게 리더가 선택 됩니다.\n\n리더는 강력한 스텟 버프를 얻습니다.");
                break;
            case 1:
                UpdateTypeInfo("요괴", _typeColor[1], "요괴가 상대 인간보다 많다면 능력치를 얻습니다.\n\n요괴들은 죽기 직전 발악합니다.");
                break;
        }
    }

    private void UpdateTypeInfo(string typeName, Color typeColor, string typeDescription)
    {
        // 타입 정보를 업데이트
        _typeText.text = typeName;
        _typeText.color = typeColor;
        _typeDescText.text = typeDescription;
    }

    public void CheckSynergyUpdate()
    {
        // 시너지 업데이트
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
        // 요괴 캐릭터들 숨김
        _basicGhostCharacters.gameObject.SetActive(false);

        // 인간 캐릭터들 보이기
        _basicHumanCharacters.gameObject.SetActive(true);

        UpdateCharacterSelection(_humanCharacters, _checkSynergy, _slectSynergy, _selectSize, _slectSynergy + 6);
    }

    private void UpdateGhostCharacters()
    {
        // 인간 캐릭터들 숨김
        _basicHumanCharacters.gameObject.SetActive(false);

        // 요괴 캐릭터들 보이기
        _basicGhostCharacters.gameObject.SetActive(true);

        UpdateCharacterSelection(_ghostCharacters, _checkSynergy, _slectSynergy, _selectSize, _slectSynergy + 6);
    }

    private void UpdateCharacterSelection(List<GameObject> characters, Image[] checkSynergy, int selectedSynergy, Vector3 selectSize, int statusIndexOffset)
    {
        // 시너지 및 캐릭터 선택 상태 업데이트
        for (int i = 0; i < checkSynergy.Length; i++)
        {
            checkSynergy[i].enabled = (i == selectedSynergy);
            characters[i].transform.localScale = (i == selectedSynergy) ? selectSize : Vector3.zero;
        }

        UpdateSynergyInfo(selectedSynergy, statusIndexOffset);
    }

    private void UpdateSynergyInfo(int selectedSynergy, int statusIndexOffset)
    {
        // 시너지 정보 업데이트
        switch (selectedSynergy)
        {
            case 0:
                SetSynergyInfo("전사", "나무꾼", _synergyColor[0], "아군의 체력을 상승 시키고,\n\n아군을 보호 합니다.", statusIndexOffset);
                break;
            case 1:
                SetSynergyInfo("궁수", "사냥꾼", _synergyColor[1], "멀리서 적을 공격하며,\n\n일정 시간마다 공격속도가 상승 합니다.", statusIndexOffset);
                break;
            case 2:
                SetSynergyInfo("도사", "선비", _synergyColor[2], "강력한 마법을 사용하며,\n\n스킬 쿨타임이 감소 합니다.", statusIndexOffset);
                break;
            case 3:
                SetSynergyInfo("암살자", "좀도둑", _synergyColor[3], "적 진영 후방으로 침투하여 공격 합니다.\n\n치명타 확률이 높습니다.", statusIndexOffset);
                break;
            case 4:
                SetSynergyInfo("지원가", "주모", _synergyColor[4], "전투에 도움이 되는 버프들을 사용 합니다.", statusIndexOffset);
                break;
            case 5:
                SetSynergyInfo("상인", "거지", _synergyColor[5], "골드를 더 많이 획득 할 수 있습니다.", statusIndexOffset);
                break;
        }
    }

    private void SetSynergyInfo(string synergyName, string characterName, Color synergyColor, string synergyDescription, int statusIndex)
    {
        // 시너지 정보를 설정
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
        // 캐릭터 상태 텍스트 업데이트
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
        // 캐릭터 선택 완료 및 씬 전환
        _userInfoManager.GameDataCreate(_slectCharacter);
        _optionPopup.MainButtonEnable(true);
        LoadingSceneController.LoadScene("inGameScene");
    }

    public void ContinueGame()
    {
        // 게임 계속하기
        if (!_userInfoManager.userData.isUserData) return;
        LoadingSceneController.LoadScene("inGameScene");
        _optionPopup.MainButtonEnable(true);
    }

    public void GameExit()
    {
        // 게임 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
