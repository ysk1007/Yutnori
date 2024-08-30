using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Popup _battleStartBtn;
    [SerializeField] private Popup _battleEndBtn;
    [SerializeField] private Animator _bebAnimator;

    [SerializeField] private Animator _canvasAnimator;

    [SerializeField] private Animator _hpTextAnimator;
    [SerializeField] private TextMeshProUGUI _hpText;

    [SerializeField] private Animator _goldTextAnimator;
    [SerializeField] private TextMeshProUGUI _goldText;

    [SerializeField] private Animator _saveTextAnimator;

    [SerializeField] private Animator _bossLabelAnimator;
    [SerializeField] private TextMeshProUGUI _bossNameText;
    [SerializeField] private Image _bossIcon;

    [SerializeField] private Image _fadeImage;

    public TutorialHand _tutorialHand;

    [SerializeField] private Animator _gameOutAnimator;
    [SerializeField] private Popup _gameOutPopup;
    [SerializeField] private Animator _gameOutPopupAnimator;

    public Shop _shop;
    [SerializeField] private GameObject _uiCanvas;
    [SerializeField] private GameObject _yutCanvas;
    [SerializeField] private Transform _units;
    [SerializeField] private Timer _timer;

    UserInfoManager _userInfoManager;
    EnemyPool _enemyPool;
    private void Awake()
    {
        SoonsoonData.Instance.Canvas_Manager = this;
        _userInfoManager = UserInfoManager.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        _userInfoManager._canvasManager = this;
        _enemyPool = SoonsoonData.Instance.Enemy_Pool;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowUi()
    {
        _canvasAnimator.SetTrigger("Show");
        _yutCanvas.SetActive(false);
        _units.transform.localScale = Vector3.one;
        _timer.TimerReset();
        Invoke("ShowBattleStartBtn", 1f);
    }

    public void FadeUi()
    {
        _canvasAnimator.SetTrigger("Fade");
        _yutCanvas.SetActive(true);
        _units.transform.localScale = Vector3.zero;
        _tutorialHand.ThrowGuide();
    }

    public void FadeImage()
    {
        _canvasAnimator.SetTrigger("FadeImage");
    }

    public void GetGoldAnimation(int value)
    {
        if (value == 0) return;

        if (value > 0)
        {
            _goldText.text = "+" + value.ToString();
            _goldTextAnimator.SetTrigger("Get");
        }
        else
        {
            _goldText.text = value.ToString();
            _goldTextAnimator.SetTrigger("Lose");
        }
    }

    public void GetHpAnimation(int value)
    {
        if (value == 0) return;

        if(value > 0)
        {
            _hpText.text = "+" + value.ToString();
            _hpTextAnimator.SetTrigger("Get");
        }
        else
        {
            _hpText.text = value.ToString();
            _hpTextAnimator.SetTrigger("Lose");
        }
    }

    public void DataSaveText()
    {
        _saveTextAnimator.SetTrigger("Show");
    }

    public void BossCall(UnitData unit)
    {
        _bossIcon.sprite = unit.icon;
        _bossNameText.text = "º¸½º\n" + unit.UnitName;
        _bossLabelAnimator.SetTrigger("Show");
    }

    public void TimerStart()
    {
        _timer.TimerStart();
    }

    public void TimerStop()
    {
        _timer.TimerStop();
    }

    public void ShowBattleEndBtn()
    {
        if (_userInfoManager.userData.GetUserHp() <= 0) return;

        _battleEndBtn.OnePopup();
        _bebAnimator.enabled = true;
    }

    public void ShowBattleStartBtn()
    {
        _battleStartBtn.OnePopup();
    }

    public void HideBattleStartBtn()
    {
        _battleStartBtn.ZeroPopup();
    }

    public void GameEnd()
    {
        Invoke("GameEndSetting",1f);
    }

    public void GameEndSetting()
    {
        _gameOutAnimator.SetTrigger("Show");
        _gameOutPopupAnimator.SetTrigger("Show");
        _fadeImage.enabled = false;
        _canvasAnimator.SetTrigger("Fade");
        _gameOutPopup.OnePopup();
    }
}
