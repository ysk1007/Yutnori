using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Popup _battleStartBtn;  // ���� ���� ��ư �˾�
    [SerializeField] private Popup _battleEndBtn;    // ���� ���� ��ư �˾�
    [SerializeField] private Animator _bebAnimator;   // ���� ���� �ִϸ�����

    [SerializeField] private Animator _canvasAnimator; // ��ü ĵ���� �ִϸ�����

    [SerializeField] private Animator _hpTextAnimator; // HP �ؽ�Ʈ �ִϸ�����
    [SerializeField] private TextMeshProUGUI _hpText; // HP �ؽ�Ʈ

    [SerializeField] private Animator _goldTextAnimator; // ��� �ؽ�Ʈ �ִϸ�����
    [SerializeField] private TextMeshProUGUI _goldText; // ��� �ؽ�Ʈ

    [SerializeField] private Animator _saveTextAnimator; // ������ ���� �ؽ�Ʈ �ִϸ�����

    [SerializeField] private Animator _bossLabelAnimator; // ���� �� �ִϸ�����
    [SerializeField] private TextMeshProUGUI _bossNameText; // ���� �̸� �ؽ�Ʈ
    [SerializeField] private Image _bossIcon; // ���� ������

    [SerializeField] private Image _fadeImage; // ���̵� �̹���

    public TutorialHand _tutorialHand; // Ʃ�丮�� �ڵ�

    [SerializeField] private Animator _gameOutAnimator; // ���� ���� �ִϸ�����
    [SerializeField] private Popup _gameOutPopup; // ���� ���� �˾�
    [SerializeField] private Animator _gameOutPopupAnimator; // ���� ���� �˾� �ִϸ�����

    public Shop _shop; // ����
    [SerializeField] private GameObject _uiCanvas; // UI ĵ����
    [SerializeField] private GameObject _yutCanvas; // �� ĵ����
    [SerializeField] private Transform _units; // ���� ��ȯ
    [SerializeField] private Timer _timer; // Ÿ�̸�

    private UserInfoManager _userInfoManager; // ����� ���� �Ŵ���
    private EnemyPool _enemyPool; // �� ���� Ǯ

    private void Awake()
    {
        // Singleton ����
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
        // UI�� ǥ���ϰ� �ִϸ��̼��� Ʈ����
        _canvasAnimator.SetTrigger("Show");
        _yutCanvas.SetActive(false);
        _units.localScale = Vector3.one;
        _timer.TimerReset();
        Invoke("ShowBattleStartBtn", 1f); // 1�� �� ���� ���� ��ư ǥ��
    }

    public void FadeUi()
    {
        // UI�� ���̵� �ƿ��ϰ� �ִϸ��̼��� Ʈ����
        _canvasAnimator.SetTrigger("Fade");
        _yutCanvas.SetActive(true);
        _units.localScale = Vector3.zero;
        _tutorialHand.ThrowGuide(); // Ʃ�丮�� ���̵� ǥ��
    }

    public void FadeImage()
    {
        // �̹��� ���̵� �ִϸ��̼��� Ʈ����
        _canvasAnimator.SetTrigger("FadeImage");
    }

    public void GetGoldAnimation(int value)
    {
        // ��� �ִϸ��̼� ǥ��
        if (value == 0) return;

        _goldText.text = (value > 0) ? "+" + value.ToString() : value.ToString();
        _goldTextAnimator.SetTrigger(value > 0 ? "Get" : "Lose");
    }

    public void GetHpAnimation(int value)
    {
        // HP �ִϸ��̼� ǥ��
        if (value == 0) return;

        _hpText.text = (value > 0) ? "+" + value.ToString() : value.ToString();
        _hpTextAnimator.SetTrigger(value > 0 ? "Get" : "Lose");
    }

    public void DataSaveText()
    {
        // ������ ���� �ؽ�Ʈ �ִϸ��̼� ǥ��
        _saveTextAnimator.SetTrigger("Show");
    }

    public void BossCall(UnitData unit)
    {
        // ���� ���� UI ������Ʈ
        _bossIcon.sprite = unit.icon;
        _bossNameText.text = "����\n" + unit.UnitName;
        _bossLabelAnimator.SetTrigger("Show");
    }

    public void TimerStart()
    {
        // Ÿ�̸� ����
        _timer.TimerStart();
    }

    public void TimerStop()
    {
        // Ÿ�̸� ����
        _timer.TimerStop();
    }

    public void ShowBattleEndBtn()
    {
        // ���� ���� ��ư ǥ��
        if (_userInfoManager.userData.GetUserHp() > 0)
        {
            _battleEndBtn.OnePopup();
            _bebAnimator.enabled = true;
        }
    }

    public void ShowBattleStartBtn()
    {
        // ���� ���� ��ư ǥ��
        _battleStartBtn.OnePopup();
    }

    public void HideBattleStartBtn()
    {
        // ���� ���� ��ư �����
        _battleStartBtn.ZeroPopup();
    }

    public void GameEnd()
    {
        // ���� ���� ����
        Invoke("GameEndSetting", 1f); // 1�� �� ���� ����
    }

    private void GameEndSetting()
    {
        // ���� ���� �ִϸ��̼� �� �˾� ǥ��
        _gameOutAnimator.SetTrigger("Show");
        _gameOutPopupAnimator.SetTrigger("Show");
        _fadeImage.enabled = false;
        _canvasAnimator.SetTrigger("Fade");
        _gameOutPopup.OnePopup();
    }
}
