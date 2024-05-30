using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    public int _buttonIndex;

    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _behaviorText;
    [SerializeField] private TextMeshProUGUI _positiveText;
    [SerializeField] private TextMeshProUGUI _negativeText;

    EventPanel _eventPanel;
    UserInfoManager _userInfoManager;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _eventPanel = SoonsoonData.Instance.Event_Panel;
        _userInfoManager = UserInfoManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(ButtonTexts buttonTexts)
    {
        _button.onClick.RemoveAllListeners();
        List<string> texts = buttonTexts.GetButtonTexts();

        _behaviorText.text = texts[0];

        switch (buttonTexts.GetButtonType())
        {
            case ButtonTexts.ButtonType.EventContinue:
                _button.onClick.AddListener(() => _eventPanel.EventContinue());
                _positiveText.text = texts[1];
                _negativeText.text = texts[2];
                break;
            case ButtonTexts.ButtonType.EventClose:
                _button.onClick.AddListener(() => _eventPanel.EventClose());
                _positiveText.text = texts[1];
                _negativeText.text = texts[2];
                break;
            case ButtonTexts.ButtonType.Battle:
                _button.onClick.AddListener(() => _eventPanel.EventBattle());
                _positiveText.text = texts[1];
                _negativeText.text = texts[2];
                break;
            case ButtonTexts.ButtonType.BattleRun:
                _button.onClick.AddListener(() => _eventPanel.EventBattleRun());
                _positiveText.text = texts[1];
                _negativeText.text = texts[2];
                break;
            case ButtonTexts.ButtonType.GetSomething:
                _button.onClick.AddListener(() => _eventPanel.EventGetSomething(_buttonIndex));
                _positiveText.text = GetSomeType();
                _negativeText.text = texts[2];
                break;
            case ButtonTexts.ButtonType.LoseSomething:
                _button.onClick.AddListener(() => _eventPanel.EventLoseSomething(_buttonIndex));
                _positiveText.text = texts[1];
                _negativeText.text = LoseSomeType();
                break;
            case ButtonTexts.ButtonType.GetLose:
                _button.onClick.AddListener(() => _eventPanel.EventGetLose(_buttonIndex));
                _positiveText.text = GetSomeType();
                _negativeText.text = LoseSomeType();
                break;
            case ButtonTexts.ButtonType.GetChance:
                _button.onClick.AddListener(() => _eventPanel.EventGetChance());
                break;
            case ButtonTexts.ButtonType.LoseChance:
                _button.onClick.AddListener(() => _eventPanel.EventLoseChance());
                break;
            case ButtonTexts.ButtonType.GetLoseChance:
                _button.onClick.AddListener(() => _eventPanel.EventGetLoseChance());
                break;
        }
    }

    public string GetSomeType()
    {
        EventData eventData = _eventPanel.GetEventData();
        if (eventData._goldValue[_buttonIndex] > 0)
        {
            return string.Format("{0}냥을 얻습니다.", eventData._goldValue[_buttonIndex]);
        }
        else if (eventData._hpValue[_buttonIndex] > 0)
        {
            return string.Format("체력을 {0} 얻습니다.", eventData._hpValue[_buttonIndex]);
        }
        else if (eventData._someUnit?[_buttonIndex] != null)
        {
            return string.Format("[{0}]을(를) 얻습니다.", _eventPanel.GetEventUnit(_buttonIndex).GetUnitData().UnitName);
        }
        else if (eventData._someItem?[_buttonIndex] != null)
        {
            return string.Format("[{0}]을(를) 얻습니다.", eventData._someItem[_buttonIndex]._itemName);
        }
        else
        {
            return string.Format("");
        }
    }

    public string LoseSomeType()
    {
        EventData eventData = _eventPanel.GetEventData();
        if (eventData._goldValue[_buttonIndex] < 0)
        {
            if(_userInfoManager.userData.GetUserGold() >= eventData._goldValue[_buttonIndex] * -1)
            {
                return string.Format("{0}냥을 잃습니다.", eventData._goldValue[_buttonIndex] * -1);
            }
            else
            {
                _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(() => _eventPanel.EventFail());
                _positiveText.text = "";
                return string.Format("(돈이 부족하다..) 체력 -25");
            }
        }
        else if (eventData._hpValue[_buttonIndex] < 0)
        {
            return string.Format("체력을 {0} 잃습니다.", eventData._hpValue[_buttonIndex]*-1);
        }
        else if (eventData._someUnit?[_buttonIndex] != null)
        {
            if (_eventPanel.GetEventUnit(_buttonIndex)?.GetUnitData() != null)
            {
                return string.Format("[{0}]을(를) 잃습니다.", _eventPanel.GetEventUnit(_buttonIndex).GetUnitData().UnitName);
            }
            else
            {
                _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(() => _eventPanel.EventFail());
                _positiveText.text = "";
                return string.Format("(동료가 없다..) 체력 -25");
            }
        }
        else if (eventData._someItem?[_buttonIndex] != null)
        {
            return string.Format("[{0}]을(를) 잃습니다.", eventData._someItem[_buttonIndex]._itemName);
        }
        else
        {
            return string.Format("");
        }
    }
}
