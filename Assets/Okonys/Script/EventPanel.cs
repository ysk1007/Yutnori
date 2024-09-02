using Febucci.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EventPanel : MonoBehaviour
{
    [SerializeField] private EventData[] events; // �̺�Ʈ ������ �迭
    [SerializeField] private EventData _currentEvent; // ���� �̺�Ʈ ������
    [SerializeField] int _currentScene; // ���� �� ��ȣ

    [SerializeField] private Transform _buttonList; // ��ư ����Ʈ�� Transform
    [SerializeField] private List<Button> _Buttons = new List<Button>(); // ��ư ����Ʈ
    [SerializeField] private List<ChoiceButton> _choiceButtons = new List<ChoiceButton>(); // ���� ��ư ����Ʈ
    [SerializeField] private List<Animator> _buttonAnimators = new List<Animator>(); // ��ư �ִϸ����� ����Ʈ

    [SerializeField] private Image _eventImage; // �̺�Ʈ �̹���
    [SerializeField] private SlotClass[] _eventUnit; // �̺�Ʈ ���� �迭
    [SerializeField] private TextMeshProUGUI _titleText; // �̺�Ʈ ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI _mainText; // �̺�Ʈ ���� �ؽ�Ʈ
    [SerializeField] private TypewriterByCharacter _typeWriter; // Ÿ�� ȿ��

    int _nextButtonIndex; // ���� ��ư �ε���

    UserInfoManager _userInfoManager; // ����� ���� ����
    Popup _thisPopup; // ���� �˾�
    CanvasManager _canvasManager; // ĵ���� ������
    Unit_Manager _unitManager; // ���� ������
    InventoryManager _inventoryManager; // �κ��丮 ������
    EnemyPool _enemyPool; // �� Ǯ
    UnitPool _unitPool; // ���� Ǯ

    private void Awake()
    {
        // �̱��� �������� Event_Panel�� �ʱ�ȭ�մϴ�.
        SoonsoonData.Instance.Event_Panel = this;

        // ��ư�� ���õ� ������Ʈ�� ����Ʈ�� �߰��մϴ�.
        for (int i = 0; i < _buttonList.childCount; i++)
        {
            _Buttons.Add(_buttonList.GetChild(i).GetComponent<Button>());
            _choiceButtons.Add(_buttonList.GetChild(i).GetComponent<ChoiceButton>());
            _buttonAnimators.Add(_buttonList.GetChild(i).GetComponent<Animator>());
            _choiceButtons[i]._buttonIndex = i;
        }

        _thisPopup = GetComponent<Popup>(); // ���� ���� ������Ʈ�� Popup ������Ʈ�� �����ɴϴ�.
    }

    private void Start()
    {
        // �ν��Ͻ����� �ʱ�ȭ�մϴ�.
        _userInfoManager = UserInfoManager.Instance;
        _canvasManager = SoonsoonData.Instance.Canvas_Manager;
        _unitManager = SoonsoonData.Instance.Unit_Manager;
        _inventoryManager = SoonsoonData.Instance.Inventory_Manager;
        _unitPool = SoonsoonData.Instance.Unit_pool;
        _enemyPool = SoonsoonData.Instance.Enemy_Pool;
    }

    // ������ �̺�Ʈ�� �����Ͽ� �ʱ�ȭ�մϴ�.
    public void RandomEvent()
    {
        _thisPopup.OnePopup(); // �˾��� �ϳ� ǥ���մϴ�.
        int eventNumber = Random.Range(0, events.Length); // �������� �̺�Ʈ ��ȣ�� �����մϴ�.
        _currentEvent = events[eventNumber]; // ���� �̺�Ʈ�� �����մϴ�.
        _currentScene = 1; // �� ��ȣ�� 1�� �ʱ�ȭ�մϴ�.
        init(); // �̺�Ʈ�� �ʱ�ȭ�մϴ�.

        // ����� �����Ϳ� �̺�Ʈ ������ �����մϴ�.
        _userInfoManager.userData.isEventData = true;
        _userInfoManager.userData.EventNum = eventNumber;
        _userInfoManager.UserDataSave(); // ����� ������ ����
    }

    // Ư�� �ε����� �̺�Ʈ�� ȣ���Ͽ� �ʱ�ȭ�մϴ�.
    public void CallEvent(int index)
    {
        _thisPopup.OnePopup(); // �˾��� �ϳ� ǥ���մϴ�.
        _currentEvent = events[index]; // ���� �̺�Ʈ�� �����մϴ�.
        _currentScene = 1; // �� ��ȣ�� 1�� �ʱ�ȭ�մϴ�.
        init(); // �̺�Ʈ�� �ʱ�ȭ�մϴ�.

        // ����� �����Ϳ� �̺�Ʈ ������ �����մϴ�.
        _userInfoManager.userData.isEventData = true;
        _userInfoManager.userData.EventNum = index;
        _userInfoManager.UserDataSave(); // ����� ������ ����
    }

    // ���� �̺�Ʈ �����͸� ��ȯ�մϴ�.
    public EventData GetEventData()
    {
        return _currentEvent;
    }

    // Ư�� �ε����� �̺�Ʈ ������ ��ȯ�մϴ�.
    public SlotClass GetEventUnit(int index)
    {
        return _eventUnit[index];
    }

    // �̺�Ʈ�� �ʱ�ȭ�մϴ�.
    public void init()
    {
        _titleText.text = _currentEvent._eventName; // �̺�Ʈ ���� ����
        _typeWriter.ShowText(_currentEvent._mainText[_currentScene - 1]); // Ÿ�� ȿ���� ���� �ؽ�Ʈ ǥ��
        _eventImage.sprite = _currentEvent._eventImage; // �̺�Ʈ �̹��� ����

        // �̺�Ʈ ���� �ʱ�ȭ
        for (int i = 0; i < _currentEvent._someUnit.Length; i++)
        {
            if (_currentEvent._someUnit[i]?.GetUnitData()?.UnitName == "RandomGet")
            {
                _eventUnit[i] = new SlotClass(_unitPool.ReturnRewardUnit(_userInfoManager.userData.GameLevel), _unitPool.RandomUnitRate());
            }
            else if (_currentEvent._someUnit[i]?.GetUnitData()?.UnitName == "RandomLose")
            {
                _eventUnit[i] = _inventoryManager.ReturnRandomUnit();
            }
            else if (_currentEvent._someUnit[i]?.GetUnitData() != null)
            {
                _eventUnit[i] = new SlotClass(_currentEvent._someUnit[i]?.GetUnitData(), _unitPool.RandomUnitRate());
            }
        }

        // ��ư ����
        for (int i = 0; i < _Buttons.Count; i++)
        {
            if (_currentEvent._choiceNumber[_currentScene - 1] > i)
            {
                _Buttons[i].transform.localScale = Vector3.one; // ��ư ũ�⸦ ������� ����

                // ù ����� �ƴ� ��� �� ����� ������ ��ŭ �����ݴϴ�.
                int plus = (_currentScene - 1 > 0) ? (_currentScene - 1) * _currentEvent._choiceNumber[(_currentScene - 2)] : 0;

                _choiceButtons[i].Init(_currentEvent._buttonText[i + plus]); // ��ư �ؽ�Ʈ ����
            }
            else
            {
                _Buttons[i].transform.localScale = Vector3.zero; // ��ư�� ����ϴ�.
            }
        }
    }

    // ��ư �ִϸ��̼��� ������� ǥ���մϴ�.
    public void ButtonShow()
    {
        _nextButtonIndex = 0; // ���� ��ư �ε��� �ʱ�ȭ
        float delay = 0; // ���� �ð� �ʱ�ȭ
        for (int i = 0; i < _buttonAnimators.Count; i++)
        {
            delay += 0.5f; // ���� �ð��� ������ŵ�ϴ�.
            Invoke("ButtonAnimation", delay); // ���� �� ��ư �ִϸ��̼� ȣ��
        }
    }

    // ��ư �ִϸ��̼��� ���̵� �ƿ��մϴ�.
    public void ButtonFade()
    {
        for (int i = 0; i < _buttonAnimators.Count; i++)
        {
            _buttonAnimators[i].SetTrigger("Fade"); // ��ư �ִϸ��̼� Ʈ���Ÿ� �����մϴ�.
        }
    }

    // ��ư �ִϸ��̼��� ǥ���մϴ�.
    public void ButtonAnimation()
    {
        _buttonAnimators[_nextButtonIndex].SetTrigger("Show"); // ��ư �ִϸ��̼� Ʈ���Ÿ� �����մϴ�.
        _nextButtonIndex++; // ���� ��ư �ε����� ������ŵ�ϴ�.
    }

    // �̺�Ʈ ���� ó��
    public void EventFail()
    {
        _userInfoManager.userData.SetUserHp(-25); // ����� HP ����
        EventContinue(); // �̺�Ʈ ��� ����
    }

    // �̺�Ʈ�� ��� �����մϴ�.
    public void EventContinue()
    {
        _currentScene++; // �� ��ȣ�� ������ŵ�ϴ�.
        init(); // �̺�Ʈ�� �ʱ�ȭ�մϴ�.
    }

    // �̺�Ʈ�� �ݽ��ϴ�.
    public void EventClose()
    {
        _canvasManager.FadeImage(); // ĵ���� �̹��� ���̵� �ƿ�
        _thisPopup.ZeroPopup(); // �˾��� �ݽ��ϴ�.

        _enemyPool.CallBoss(); // ���� ȣ��

        // ����� �����Ϳ��� �̺�Ʈ ���� ����
        _userInfoManager.userData.isEventData = false;
        _userInfoManager.userData.EventNum = 0;
        _userInfoManager.UserDataSave(); // ����� ������ ����
    }

    // �̺�Ʈ ������ �����մϴ�.
    public void EventBattle()
    {
        _thisPopup.ZeroPopup(); // �˾��� �ݽ��ϴ�.
        _canvasManager.ShowUi(); // UI�� ǥ���մϴ�.
        _unitManager._p2unitID = _currentEvent._eventEnemy; // �̺�Ʈ �� ����
        _unitManager.FieldReset(); // �ʵ� ����
        _userInfoManager.UserDataSave(); // ����� ������ ����
    }

    // �̺�Ʈ ������ �����ϰų� ����մϴ�.
    public void EventBattleRun()
    {
        if (_currentEvent.EventChance())
        {
            EventContinue(); // ��� ����
        }
        else
        {
            EventBattle(); // ���� ����
        }
    }

    // �̺�Ʈ���� ���𰡸� ����ϴ�.
    public void EventGetSomething(int index)
    {
        if (_currentEvent._goldValue.Length != 0)
            _userInfoManager.userData.SetUserGold(_currentEvent._goldValue[index]); // �� ȹ��

        if (_currentEvent._hpValue.Length != 0)
            _userInfoManager.userData.SetUserHp(_currentEvent._hpValue[index]); // ü�� ����

        if (_currentEvent._someUnit.Length != 0)
        {
            if (!_inventoryManager.InventoryAdd(_eventUnit[index])) return; // �κ��丮�� ���� �߰�
        }

        EventContinue(); // �̺�Ʈ ��� ����
    }

    // �̺�Ʈ���� ���𰡸� ���� ��ȸ
    public void EventGetChance()
    {
        if (_currentEvent.EventChance())
        {
            EventContinue(); // �����ϸ� ��� ����
        }
        else
        {
            EventContinue(); // �����ϸ� ��� ����
        }
    }

    // �̺�Ʈ���� ���𰡸� �ҽ��ϴ�.
    public void EventLoseSomething(int index)
    {
        if (_currentEvent._goldValue.Length != 0)
            _userInfoManager.userData.SetUserGold(-1 * _currentEvent._goldValue[index]); // �� ����

        if (_currentEvent._hpValue.Length != 0)
            _userInfoManager.userData.SetUserHp(-1 * _currentEvent._hpValue[index]); // ü�� ����

        if (_eventUnit[index]._unitData != null)
            _inventoryManager.UnitRemove(_eventUnit[index]); // ���� ����

        EventContinue(); // �̺�Ʈ ��� ����
    }

    // �̺�Ʈ���� ������ ���
    public void EventLoseChance()
    {
        if (_currentEvent.EventChance())
        {
            EventLoseSomething(0); // ��ȸ�� ������ ���𰡸� ����
            EventContinue(); // ��� ����
        }
        else
        {
            EventContinue(); // ��ȸ�� ������ ��� ����
        }
    }

    // �̺�Ʈ���� ���𰡸� ��ų� �ҽ��ϴ�.
    public void EventGetLose(int index)
    {
        if (_currentEvent._goldValue.Length != 0)
            _userInfoManager.userData.SetUserGold(_currentEvent._goldValue[index]); // �� ȹ��

        if (_currentEvent._hpValue.Length != 0)
            _userInfoManager.userData.SetUserHp(_currentEvent._hpValue[index]); // ü�� ����

        if (_currentEvent._someUnit[index]?.GetUnitData()?.UnitName == "RandomGet")
        {
            if (!_inventoryManager.InventoryAdd(_eventUnit[index])) return; // �κ��丮�� ���� �߰�
        }
        else if (_currentEvent._someUnit[index]?.GetUnitData()?.UnitName == "RandomLose")
        {
            _inventoryManager.UnitRemove(_eventUnit[index]); // ���� ����
        }

        EventContinue(); // �̺�Ʈ ��� ����
    }

    // �̺�Ʈ���� ��ų� ����.
    public void EventGetLoseChance()
    {
        if (_currentEvent.EventChance())
        {
            EventGetSomething(0); // ��ȸ�� ������ ���𰡸� ����
            EventContinue(); // ��� ����
        }
        else
        {
            EventLoseSomething(0); // ��ȸ�� ������ ���𰡸� ����
            EventContinue(); // ��� ����
        }
    }
}
