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

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _eventPanel = SoonsoonData.Instance.Event_Panel;
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
        _positiveText.text = texts[1];
        _negativeText.text = texts[2];

        switch (buttonTexts.GetButtonType())
        {
            case ButtonTexts.ButtonType.EventContinue:
                _button.onClick.AddListener(() => _eventPanel.EventContinue());
                break;
            case ButtonTexts.ButtonType.EventClose:
                _button.onClick.AddListener(() => _eventPanel.EventClose());
                break;
            case ButtonTexts.ButtonType.Battle:
                _button.onClick.AddListener(() => _eventPanel.EventBattle());
                break;
            case ButtonTexts.ButtonType.BattleRun:
                _button.onClick.AddListener(() => _eventPanel.EventBattleRun());
                break;
            case ButtonTexts.ButtonType.GetSomething:
                _button.onClick.AddListener(() => _eventPanel.EventGetSomething(_buttonIndex));
                break;
            case ButtonTexts.ButtonType.LoseSomething:
                _button.onClick.AddListener(() => _eventPanel.EventLoseSomething(_buttonIndex));
                break;
            case ButtonTexts.ButtonType.GetLose:
                _button.onClick.AddListener(() => _eventPanel.EventGetLose(_buttonIndex));
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
}
