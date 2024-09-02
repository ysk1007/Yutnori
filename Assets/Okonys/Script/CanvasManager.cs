using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Popup _battleStartBtn;  // 전투 시작 버튼 팝업
    [SerializeField] private Popup _battleEndBtn;    // 전투 종료 버튼 팝업
    [SerializeField] private Animator _bebAnimator;   // 전투 종료 애니메이터

    [SerializeField] private Animator _canvasAnimator; // 전체 캔버스 애니메이터

    [SerializeField] private Animator _hpTextAnimator; // HP 텍스트 애니메이터
    [SerializeField] private TextMeshProUGUI _hpText; // HP 텍스트

    [SerializeField] private Animator _goldTextAnimator; // 골드 텍스트 애니메이터
    [SerializeField] private TextMeshProUGUI _goldText; // 골드 텍스트

    [SerializeField] private Animator _saveTextAnimator; // 데이터 저장 텍스트 애니메이터

    [SerializeField] private Animator _bossLabelAnimator; // 보스 라벨 애니메이터
    [SerializeField] private TextMeshProUGUI _bossNameText; // 보스 이름 텍스트
    [SerializeField] private Image _bossIcon; // 보스 아이콘

    [SerializeField] private Image _fadeImage; // 페이드 이미지

    public TutorialHand _tutorialHand; // 튜토리얼 핸드

    [SerializeField] private Animator _gameOutAnimator; // 게임 종료 애니메이터
    [SerializeField] private Popup _gameOutPopup; // 게임 종료 팝업
    [SerializeField] private Animator _gameOutPopupAnimator; // 게임 종료 팝업 애니메이터

    public Shop _shop; // 상점
    [SerializeField] private GameObject _uiCanvas; // UI 캔버스
    [SerializeField] private GameObject _yutCanvas; // 윷 캔버스
    [SerializeField] private Transform _units; // 유닛 변환
    [SerializeField] private Timer _timer; // 타이머

    private UserInfoManager _userInfoManager; // 사용자 정보 매니저
    private EnemyPool _enemyPool; // 적 유닛 풀

    private void Awake()
    {
        // Singleton 패턴
        SoonsoonData.Instance.Canvas_Manager = this;
        _userInfoManager = UserInfoManager.Instance;
    }

    private void Start()
    {
        _userInfoManager._canvasManager = this;
        _enemyPool = SoonsoonData.Instance.Enemy_Pool;
    }

    public void ShowUi()
    {
        // UI를 표시하고 애니메이션을 트리거
        _canvasAnimator.SetTrigger("Show");
        _yutCanvas.SetActive(false);
        _units.localScale = Vector3.one;
        _timer.TimerReset();
        Invoke("ShowBattleStartBtn", 1f); // 1초 후 전투 시작 버튼 표시
    }

    public void FadeUi()
    {
        // UI를 페이드 아웃하고 애니메이션을 트리거
        _canvasAnimator.SetTrigger("Fade");
        _yutCanvas.SetActive(true);
        _units.localScale = Vector3.zero;
        _tutorialHand.ThrowGuide(); // 튜토리얼 가이드 표시
    }

    public void FadeImage()
    {
        // 이미지 페이드 애니메이션을 트리거
        _canvasAnimator.SetTrigger("FadeImage");
    }

    public void GetGoldAnimation(int value)
    {
        // 골드 애니메이션 표시
        if (value == 0) return;

        _goldText.text = (value > 0) ? "+" + value.ToString() : value.ToString();
        _goldTextAnimator.SetTrigger(value > 0 ? "Get" : "Lose");
    }

    public void GetHpAnimation(int value)
    {
        // HP 애니메이션 표시
        if (value == 0) return;

        _hpText.text = (value > 0) ? "+" + value.ToString() : value.ToString();
        _hpTextAnimator.SetTrigger(value > 0 ? "Get" : "Lose");
    }

    public void DataSaveText()
    {
        // 데이터 저장 텍스트 애니메이션 표시
        _saveTextAnimator.SetTrigger("Show");
    }

    public void BossCall(UnitData unit)
    {
        // 보스 정보 UI 업데이트
        _bossIcon.sprite = unit.icon;
        _bossNameText.text = "보스\n" + unit.UnitName;
        _bossLabelAnimator.SetTrigger("Show");
    }

    public void TimerStart()
    {
        // 타이머 시작
        _timer.TimerStart();
    }

    public void TimerStop()
    {
        // 타이머 정지
        _timer.TimerStop();
    }

    public void ShowBattleEndBtn()
    {
        // 전투 종료 버튼 표시
        if (_userInfoManager.userData.GetUserHp() > 0)
        {
            _battleEndBtn.OnePopup();
            _bebAnimator.enabled = true;
        }
    }

    public void ShowBattleStartBtn()
    {
        // 전투 시작 버튼 표시
        _battleStartBtn.OnePopup();
    }

    public void HideBattleStartBtn()
    {
        // 전투 시작 버튼 숨기기
        _battleStartBtn.ZeroPopup();
    }

    public void GameEnd()
    {
        // 게임 종료 설정
        Invoke("GameEndSetting", 1f); // 1초 후 종료 설정
    }

    private void GameEndSetting()
    {
        // 게임 종료 애니메이션 및 팝업 표시
        _gameOutAnimator.SetTrigger("Show");
        _gameOutPopupAnimator.SetTrigger("Show");
        _fadeImage.enabled = false;
        _canvasAnimator.SetTrigger("Fade");
        _gameOutPopup.OnePopup();
    }
}
