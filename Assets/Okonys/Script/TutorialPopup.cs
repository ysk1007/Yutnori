using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    // 튜토리얼 타입
    public enum TutorialType
    {
        firstRun,
        firstBattle
    }
    [SerializeField] private TutorialType _tutorialType;

    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField] private TypewriterByCharacter _typeWriter;

    [SerializeField][TextArea] private string[] _firstTutorialTexts;
    [SerializeField][TextArea] private string[] _firstBattleTexts;

    UserInfoManager _userInfoManager;
    Popup _popup;

    bool _isFirstRun;
    bool _isFirstBattle;

    [SerializeField]
    private int _currentDialogueIndex = 0;

    private void Awake()
    {
        _popup = GetComponent<Popup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
        _isFirstRun = _userInfoManager.achievementData.isFirstRun();
        _isFirstBattle = _userInfoManager.achievementData.isFirstBattle();

        RunGameTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunGameTutorial()
    {
        if (!_isFirstRun) return;
        _popup.OnePopup();
        _tutorialType = TutorialType.firstRun;
        _currentDialogueIndex = 0;
    }

    public void RunBattleTutorial()
    {
        if (!_isFirstBattle) return;

    }

    public void NextDialogue()
    {
        _currentDialogueIndex++;
        switch (_tutorialType)
        {
            case TutorialType.firstRun:
                if (_currentDialogueIndex > _firstTutorialTexts.Length - 1) 
                    _popup.ZeroPopup();

                _typeWriter.ShowText(_firstTutorialTexts[_currentDialogueIndex]);
                break;
            case TutorialType.firstBattle:
                if (_currentDialogueIndex > _firstBattleTexts.Length - 1)
                    _popup.ZeroPopup();

                _typeWriter.ShowText(_firstBattleTexts[_currentDialogueIndex]);
                break;
            default:
                break;
        }
    }
}
