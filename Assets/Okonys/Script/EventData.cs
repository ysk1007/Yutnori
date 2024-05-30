using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ButtonTexts
{
    public enum ButtonType
    {
        EventContinue = 0, // ex) 계속하기
        EventClose = 1, // ex) 떠나기
        Battle = 2, // ex) 전투
        BattleRun = 3, // ex) 도망친다

        GetSomething = 4, // ex) 무언가 얻는 이벤트
        LoseSomething = 5, // ex) 무언가 잃는 이벤트
        GetLose = 6, // ex) 희생 이벤트

        GetChance = 7, // ex) 일정 확률로 얻는 이벤트
        LoseChance = 8, // ex) 일정 확률로 잃는 이벤트
        GetLoseChance = 9, // ex) 일정 확률로 얻을수도, 잃을수도 있는 이벤트
    }

    [SerializeField] private ButtonType _buttonType;
    [SerializeField] private string _behaviorText;
    [SerializeField] private string _positiveText;
    [SerializeField] private string _negativeText;

    public List<string> GetButtonTexts()
    {
        List<string> buttonTexts = new List<string>();
        buttonTexts.Add(_behaviorText);
        buttonTexts.Add(_positiveText);
        buttonTexts.Add(_negativeText);

        return buttonTexts;
    }

    public ButtonType GetButtonType()
    {
        return _buttonType;
    }
}

[CreateAssetMenu(fileName = "Event", menuName = "Scriptable Object/EventData")]
public class EventData : ScriptableObject
{
    [Header(" # Main Info")]
    public int _eventID;
    public string _eventName;

    [Header(" # Situation Info")]
    public int _sceneNumber; //  이벤트 씬 갯수
    public int[] _choiceNumber; // 선택지 갯수

    public float _eventSucessProbability; // 선택지 성공 확률

    [TextArea]
    public string[] _mainText;

    public List<ButtonTexts> _buttonText;

    public List<SlotClass> _eventEnemy;

    [Header(" # Something Info")]
    public int[] _goldValue;
    public int[] _hpValue;
    public SlotClass[] _someUnit;
    public ItemData[] _someItem;

    [Header(" # Event Sprite")]
    public Sprite _eventImage;

    public bool EventChance()
    {
        float chance = Random.value;
        bool isSucess = (chance < _eventSucessProbability) ? true : false ;
        SoonsoonData.Instance.LogPopup.ShowLog("실패 했습니다..");
        return isSucess;
    }
}
