using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private Image _eventImage;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _mainText;

    UserInfoManager _userInfoManager;
    Popup _thisPopup;
    CanvasManager _canvasManager;
    Unit_Manager _unitManager;

    private void Awake()
    {
        SoonsoonData.Instance.Event_Panel = this;
        for (int i = 0; i < _buttonList.childCount; i++)
        {
            _Buttons.Add(_buttonList.GetChild(i).GetComponent<Button>());
            _choiceButtons.Add(_buttonList.GetChild(i).GetComponent<ChoiceButton>());
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
    }

    public void init()
    {
        _titleText.text = _currentEvent._eventName;
        _mainText.text = _currentEvent._mainText[_currentScene - 1];
        _eventImage.sprite = _currentEvent._eventImage;
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

    public void EventContinue()
    {
        _currentScene++;
        init();
    }

    public void EventClose()
    {
        _canvasManager.FadeImage();
        _thisPopup.ZeroPopup();
    }

    public void EventBattle()
    {
        _thisPopup.ZeroPopup();
        _canvasManager.ShowUi();
        _unitManager._p2unitID = _currentEvent._eventEnemy;
        _unitManager.FieldReset();
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
        _userInfoManager.userData.UserGold += _currentEvent._goldValue[index];
        _userInfoManager.userData.UserHp += _currentEvent._hpValue[index];
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
        _userInfoManager.userData.UserGold += _currentEvent._goldValue[index];
        _userInfoManager.userData.UserHp += _currentEvent._hpValue[index];
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
        _userInfoManager.userData.UserGold += _currentEvent._goldValue[index];
        _userInfoManager.userData.UserHp += _currentEvent._hpValue[index];
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
