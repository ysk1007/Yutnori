using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    public int _buttonIndex; // ��ư �ε���

    [SerializeField] private Button _button; // ��ư ������Ʈ
    [SerializeField] private TextMeshProUGUI _behaviorText; // �ൿ �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI _positiveText; // ������ �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI _negativeText; // ������ �ؽ�Ʈ

    private EventPanel _eventPanel; // �̺�Ʈ �г�
    private UserInfoManager _userInfoManager; // ����� ���� �Ŵ���

    private void Awake()
    {
        _button = GetComponent<Button>(); // ��ư ������Ʈ �ʱ�ȭ
    }

    private void Start()
    {
        // �̺�Ʈ �г� �� ����� ���� �Ŵ��� �ʱ�ȭ
        _eventPanel = SoonsoonData.Instance.Event_Panel;
        _userInfoManager = UserInfoManager.Instance;
    }

    // ��ư�� �ʱ�ȭ�ϰ� �ؽ�Ʈ�� �����մϴ�.
    public void Init(ButtonTexts buttonTexts)
    {
        _button.onClick.RemoveAllListeners(); // ������ ��� Ŭ�� ������ ����
        List<string> texts = buttonTexts.GetButtonTexts(); // ��ư �ؽ�Ʈ ��������

        _behaviorText.text = texts[0]; // �ൿ �ؽ�Ʈ ����

        // ��ư Ÿ�Կ� ���� ������ Ŭ�� ������ �� �ؽ�Ʈ ����
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

    // ��ư Ŭ�� ���۰� �ؽ�Ʈ�� �����մϴ�.
    private void SetButtonAction(UnityEngine.Events.UnityAction action, string positiveText, string negativeText)
    {
        _button.onClick.AddListener(action);
        _positiveText.text = positiveText;
        _negativeText.text = negativeText;
    }

    // ��� �׸��� �ؽ�Ʈ�� �����մϴ�.
    public string GetSomeType()
    {
        EventData eventData = _eventPanel.GetEventData();
        if (eventData._goldValue[_buttonIndex] > 0)
        {
            return $"{eventData._goldValue[_buttonIndex]}���� ����ϴ�.";
        }
        else if (eventData._hpValue[_buttonIndex] > 0)
        {
            return $"ü���� {eventData._hpValue[_buttonIndex]} ����ϴ�.";
        }
        else if (eventData._someUnit?[_buttonIndex] != null)
        {
            return $"[{_eventPanel.GetEventUnit(_buttonIndex).GetUnitData().UnitName}]��(��) ����ϴ�.";
        }
        else if (eventData._someItem?[_buttonIndex] != null)
        {
            return $"[{eventData._someItem[_buttonIndex]._itemName}]��(��) ����ϴ�.";
        }
        else
        {
            return string.Empty;
        }
    }

    // �Ҵ� �׸��� �ؽ�Ʈ�� �����մϴ�.
    public string LoseSomeType()
    {
        EventData eventData = _eventPanel.GetEventData();
        if (eventData._goldValue[_buttonIndex] < 0)
        {
            if (_userInfoManager.userData.GetUserGold() >= -eventData._goldValue[_buttonIndex])
            {
                return $"{-eventData._goldValue[_buttonIndex]}���� �ҽ��ϴ�.";
            }
            else
            {
                SetButtonAction(() => _eventPanel.EventFail(), string.Empty, "(���� �����ϴ�..) ü�� -25");
                return string.Empty;
            }
        }
        else if (eventData._hpValue[_buttonIndex] < 0)
        {
            return $"ü���� {-eventData._hpValue[_buttonIndex]} �ҽ��ϴ�.";
        }
        else if (eventData._someUnit?[_buttonIndex] != null)
        {
            if (_eventPanel.GetEventUnit(_buttonIndex)?.GetUnitData() != null)
            {
                return $"[{_eventPanel.GetEventUnit(_buttonIndex).GetUnitData().UnitName}]��(��) �ҽ��ϴ�.";
            }
            else
            {
                SetButtonAction(() => _eventPanel.EventFail(), string.Empty, "(���ᰡ ����..) ü�� -25");
                return string.Empty;
            }
        }
        else if (eventData._someItem?[_buttonIndex] != null)
        {
            return $"[{eventData._someItem[_buttonIndex]._itemName}]��(��) �ҽ��ϴ�.";
        }
        else
        {
            return string.Empty;
        }
    }
}
