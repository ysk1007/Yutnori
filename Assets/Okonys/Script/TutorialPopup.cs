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
        none,
        firstRun,
        firstBattle
    }
    [SerializeField] private TutorialType _tutorialType;
    [SerializeField] private TutorialHand _tutorialHand;

    [SerializeField] private GameObject _plateTutorial;
    [SerializeField] private Animator _plateTutorialAnimator;

    [SerializeField] private GameObject _yutTutorial;
    [SerializeField] private Animator _yutTutorialAnimator;

    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField] private TypewriterByCharacter _typeWriter;

    [SerializeField][TextArea] private string[] _firstTutorialTexts;
    [SerializeField] private RectTransform[] _firstTutorialRectTransform;

    [SerializeField][TextArea] private string[] _firstBattleTexts;

    UserInfoManager _userInfoManager;
    Popup _popup;

    [SerializeField] private bool _isFirstRun;
    [SerializeField] private bool _isFirstBattle;
    public bool _textShowed;

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
        _typeWriter.ShowText(_firstTutorialTexts[_currentDialogueIndex]);
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
                else
                {
                    _typeWriter.ShowText(_firstTutorialTexts[_currentDialogueIndex]);
                    SetTutorialHand();
                    SetPlateTutorial();
                    SetYutTutorial();
                }
                break;
            case TutorialType.firstBattle:
                if (_currentDialogueIndex > _firstBattleTexts.Length - 1)
                    _popup.ZeroPopup();
                else
                {
                    _typeWriter.ShowText(_firstBattleTexts[_currentDialogueIndex]);
                    SetTutorialHand();
                }
                break;
            default:
                break;
        }
    }

    public void SetTutorialHand()
    {
        _tutorialHand.SetTargetImage(_firstTutorialRectTransform[_currentDialogueIndex]);
    }

    void SetPlateTutorial()
    {
        if (_currentDialogueIndex < 5 || _currentDialogueIndex > 10)
        {
            _plateTutorial.SetActive(false);
            return;
        }

        _plateTutorial.SetActive(true);
        _plateTutorialAnimator.SetInteger("index", _currentDialogueIndex);
        _plateTutorialAnimator.SetTrigger("Show");
    }

    void SetYutTutorial()
    {
        if (_currentDialogueIndex < 11 || _currentDialogueIndex > 16)
        {
            _yutTutorial.SetActive(false);
            return;
        }
        _yutTutorial.SetActive(true);
        _yutTutorialAnimator.SetTrigger("Show");
    }
}
