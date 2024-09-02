using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    public int _buttonIndex; // 버튼 인덱스

    [SerializeField] private Button _button; // 버튼 컴포넌트
    [SerializeField] private TextMeshProUGUI _behaviorText; // 행동 텍스트
    [SerializeField] private TextMeshProUGUI _positiveText; // 긍정적 텍스트
    [SerializeField] private TextMeshProUGUI _negativeText; // 부정적 텍스트

    private EventPanel _eventPanel; // 이벤트 패널
    private UserInfoManager _userInfoManager; // 사용자 정보 매니저

    private void Awake()
    {
        _button = GetComponent<Button>(); // 버튼 컴포넌트 초기화
    }

    private void Start()
    {
        // 이벤트 패널 및 사용자 정보 매니저 초기화
        _eventPanel = SoonsoonData.Instance.Event_Panel;
        _userInfoManager = UserInfoManager.Instance;
    }

    // 버튼을 초기화하고 텍스트를 설정합니다.
    public void Init(ButtonTexts buttonTexts)
    {
        _button.onClick.RemoveAllListeners(); // 기존의 모든 클릭 리스너 제거
        List<string> texts = buttonTexts.GetButtonTexts(); // 버튼 텍스트 가져오기

        _behaviorText.text = texts[0]; // 행동 텍스트 설정

        // 버튼 타입에 따라 적절한 클릭 리스너 및 텍스트 설정
        switch (buttonTexts.GetButtonType())
        {
            case ButtonTexts.ButtonType.EventContinue:
                SetButtonAction(() => _eventPanel.EventContinue(), texts[1], texts[2]);
                break;
            case ButtonTexts.ButtonType.EventClose:
                SetButtonAction(() => _eventPanel.EventClose(), texts[1], texts[2]);
                break;
            case ButtonTexts.ButtonType.Battle:
                SetButtonAction(() => _eventPanel.EventBattle(), texts[1], texts[2]);
                break;
            case ButtonTexts.ButtonType.BattleRun:
                SetButtonAction(() => _eventPanel.EventBattleRun(), texts[1], texts[2]);
                break;
            case ButtonTexts.ButtonType.GetSomething:
                SetButtonAction(() => _eventPanel.EventGetSomething(_buttonIndex), GetSomeType(), texts[2]);
                break;
            case ButtonTexts.ButtonType.LoseSomething:
                SetButtonAction(() => _eventPanel.EventLoseSomething(_buttonIndex), texts[1], LoseSomeType());
                break;
            case ButtonTexts.ButtonType.GetLose:
                SetButtonAction(() => _eventPanel.EventGetLose(_buttonIndex), GetSomeType(), LoseSomeType());
                break;
            case ButtonTexts.ButtonType.GetChance:
                SetButtonAction(() => _eventPanel.EventGetChance(), string.Empty, string.Empty);
                break;
            case ButtonTexts.ButtonType.LoseChance:
                SetButtonAction(() => _eventPanel.EventLoseChance(), string.Empty, string.Empty);
                break;
            case ButtonTexts.ButtonType.GetLoseChance:
                SetButtonAction(() => _eventPanel.EventGetLoseChance(), texts[1], texts[2]);
                break;
        }
    }

    // 버튼 클릭 동작과 텍스트를 설정합니다.
    private void SetButtonAction(UnityEngine.Events.UnityAction action, string positiveText, string negativeText)
    {
        _button.onClick.AddListener(action);
        _positiveText.text = positiveText;
        _negativeText.text = negativeText;
    }

    // 얻는 항목의 텍스트를 생성합니다.
    public string GetSomeType()
    {
        EventData eventData = _eventPanel.GetEventData();
        if (eventData._goldValue[_buttonIndex] > 0)
        {
            return $"{eventData._goldValue[_buttonIndex]}냥을 얻습니다.";
        }
        else if (eventData._hpValue[_buttonIndex] > 0)
        {
            return $"체력을 {eventData._hpValue[_buttonIndex]} 얻습니다.";
        }
        else if (eventData._someUnit?[_buttonIndex] != null)
        {
            return $"[{_eventPanel.GetEventUnit(_buttonIndex).GetUnitData().UnitName}]을(를) 얻습니다.";
        }
        else if (eventData._someItem?[_buttonIndex] != null)
        {
            return $"[{eventData._someItem[_buttonIndex]._itemName}]을(를) 얻습니다.";
        }
        else
        {
            return string.Empty;
        }
    }

    // 잃는 항목의 텍스트를 생성합니다.
    public string LoseSomeType()
    {
        EventData eventData = _eventPanel.GetEventData();
        if (eventData._goldValue[_buttonIndex] < 0)
        {
            if (_userInfoManager.userData.GetUserGold() >= -eventData._goldValue[_buttonIndex])
            {
                return $"{-eventData._goldValue[_buttonIndex]}냥을 잃습니다.";
            }
            else
            {
                SetButtonAction(() => _eventPanel.EventFail(), string.Empty, "(돈이 부족하다..) 체력 -25");
                return string.Empty;
            }
        }
        else if (eventData._hpValue[_buttonIndex] < 0)
        {
            return $"체력을 {-eventData._hpValue[_buttonIndex]} 잃습니다.";
        }
        else if (eventData._someUnit?[_buttonIndex] != null)
        {
            if (_eventPanel.GetEventUnit(_buttonIndex)?.GetUnitData() != null)
            {
                return $"[{_eventPanel.GetEventUnit(_buttonIndex).GetUnitData().UnitName}]을(를) 잃습니다.";
            }
            else
            {
                SetButtonAction(() => _eventPanel.EventFail(), string.Empty, "(동료가 없다..) 체력 -25");
                return string.Empty;
            }
        }
        else if (eventData._someItem?[_buttonIndex] != null)
        {
            return $"[{eventData._someItem[_buttonIndex]._itemName}]을(를) 잃습니다.";
        }
        else
        {
            return string.Empty;
        }
    }
}
