using Febucci.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EventPanel : MonoBehaviour
{
    [SerializeField] private EventData[] events;
    [SerializeField] private EventData _currentEvent;
    [SerializeField] int _currentScene;

    [SerializeField] private Transform _buttonList;
    [SerializeField] private List<Button> _Buttons;
    [SerializeField] private List<ChoiceButton> _choiceButtons;
    [SerializeField] private List<Animator> _buttonAnimators;

    [SerializeField] private Image _eventImage;
    [SerializeField] private SlotClass[] _eventUnit;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _mainText;
    [SerializeField] private TypewriterByCharacter _typeWriter;

    int _nextButtonIndex;

    UserInfoManager _userInfoManager;
    Popup _thisPopup;
    CanvasManager _canvasManager;
    Unit_Manager _unitManager;
    InventoryManager _inventoryManager;
    EnemyPool _enemyPool;
    UnitPool _unitPool;

    private void Awake()
    {
        SoonsoonData.Instance.Event_Panel = this;
        for (int i = 0; i < _buttonList.childCount; i++)
        {
            _Buttons.Add(_buttonList.GetChild(i).GetComponent<Button>());
            _choiceButtons.Add(_buttonList.GetChild(i).GetComponent<ChoiceButton>());
            _buttonAnimators.Add(_buttonList.GetChild(i).GetComponent<Animator>());
            _choiceButtons[i]._buttonIndex = i;
        }
        _thisPopup = GetComponent<Popup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
        _canvasManager = SoonsoonData.Instance.Canvas_Manager;
        _unitManager = SoonsoonData.Instance.Unit_Manager;
        _inventoryManager = SoonsoonData.Instance.Inventory_Manager;
        _unitPool = SoonsoonData.Instance.Unit_pool;
        _enemyPool = SoonsoonData.Instance.Enemy_Pool;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomEvent()
    {
        _thisPopup.OnePopup();
        int eventNumber = Random.Range(0, events.Length);
        _currentEvent = events[eventNumber];
        _currentScene = 1;
        init();

        _userInfoManager.userData.isEventData = true;
        _userInfoManager.userData.EventNum = eventNumber;
        _userInfoManager.UserDataSave();
    }

    public void CallEvent(int index)
    {
        _thisPopup.OnePopup();
        _currentEvent = events[index];
        _currentScene = 1;
        init();

        _userInfoManager.userData.isEventData = true;
        _userInfoManager.userData.EventNum = index;
        _userInfoManager.UserDataSave();
    }

    public EventData GetEventData()
    {
        return _currentEvent;
    }

    public SlotClass GetEventUnit(int index)
    {
        return _eventUnit[index];
    }

    public void init()
    {
        _titleText.text = _currentEvent._eventName;
        //_mainText.text = _currentEvent._mainText[_currentScene - 1];
        _typeWriter.ShowText(_currentEvent._mainText[_currentScene - 1]);
        _eventImage.sprite = _currentEvent._eventImage;

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
            else if(_currentEvent._someUnit[i]?.GetUnitData() != null)
            {
                _eventUnit[i] = new SlotClass(_currentEvent._someUnit[i]?.GetUnitData(), _unitPool.RandomUnitRate());
            }
        }

        // 버튼 할당
        for (int i = 0; i < _Buttons.Count; i++)
        {
            if (_currentEvent._choiceNumber[_currentScene - 1] > i)
            {
                _Buttons[i].transform.localScale = Vector3.one;

                // 첫 장면이 아닐때 전 장면의 선택지 만큼 더 해줌
                int plus = (_currentScene - 1 > 0) ? (_currentScene - 1) * _currentEvent._choiceNumber[(_currentScene - 2)] : 0;

                _choiceButtons[i].init(_currentEvent._buttonText[i + plus]) ;
            }
            else
            {
                _Buttons[i].transform.localScale = Vector3.zero;
            }
        }
    }

    public void ButtonShow()
    {
        _nextButtonIndex = 0;
        float delay = 0;
        for (int i = 0; i < _buttonAnimators.Count; i++)
        {
            delay += 0.5f;
            Invoke("ButtonAnimation", delay);
        }
    }

    public void ButtonFade()
    {
        for (int i = 0; i < _buttonAnimators.Count; i++)
        {
            _buttonAnimators[i].SetTrigger("Fade");
        }
    }

    public void ButtonAnimation()
    {
        _buttonAnimators[_nextButtonIndex].SetTrigger("Show");
        _nextButtonIndex++;
    }

    public void EventFail()
    {
        _userInfoManager.userData.UserHp -= 25;
        EventContinue();
    }

    public void EventContinue()
    {
        _currentScene++;
        init();
    }

    public void EventClose()
    {
        _canvasManager.FadeImage();
        _thisPopup.ZeroPopup();

        _enemyPool.CallBoss();

        _userInfoManager.userData.isEventData = false;
        _userInfoManager.userData.EventNum = 0;
        _userInfoManager.UserDataSave();
    }

    public void EventBattle()
    {
        _thisPopup.ZeroPopup();
        _canvasManager.ShowUi();
        _unitManager._p2unitID = _currentEvent._eventEnemy;
        _unitManager.FieldReset();
        _userInfoManager.UserDataSave();
    }

    public void EventBattleRun()
    {
        if (_currentEvent.EventChance())
        {
            EventContinue();
        }
        else
        {
            EventBattle();
        }
    }

    public void EventGetSomething(int index)
    {
        _userInfoManager.userData.SetUserGold(_currentEvent._goldValue[index]);
        _userInfoManager.userData.UserHp += _currentEvent._hpValue[index];

        if(_currentEvent._someUnit?[index] != null)
        {
            if (!_inventoryManager.InventoryAdd(_eventUnit[index])) return;
        }

        EventContinue();
    }

    public void EventGetChance()
    {
        if (_currentEvent.EventChance())
        {
            //EventGetSomething();
            EventContinue();
        }
        else
        {
            EventContinue();
        }
    }

    public void EventLoseSomething(int index)
    {
        _userInfoManager.userData.SetUserGold(_currentEvent._goldValue[index]);
        _userInfoManager.userData.UserHp += _currentEvent._hpValue[index];

        _inventoryManager.UnitRemove(_eventUnit[index]);


        EventContinue();
    }

    public void EventLoseChance()
    {
        if (_currentEvent.EventChance())
        {
            //EventLoseSomething();
            EventContinue();
        }
        else
        {
            EventContinue();
        }
    }

    public void EventGetLose(int index)
    {
        _userInfoManager.userData.SetUserGold(_currentEvent._goldValue[index]);
        _userInfoManager.userData.UserHp += _currentEvent._hpValue[index];


        if (_currentEvent._someUnit[index]?.GetUnitData()?.UnitName == "RandomGet")
        {
            if (!_inventoryManager.InventoryAdd(_eventUnit[index])) return;
        }
        else if (_currentEvent._someUnit[index]?.GetUnitData()?.UnitName == "RandomLose")
        {
            _inventoryManager.UnitRemove(_eventUnit[index]);
        }

        EventContinue();
    }

    public void EventGetLoseChance()
    {
        if (_currentEvent.EventChance())
        {
            //EventGetLose();
            EventContinue();
        }
        else
        {
            EventContinue();
        }
    }
}
