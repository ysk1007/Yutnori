using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ButtonTexts
{
    public enum ButtonType
    {
        EventContinue = 0, // ex) ����ϱ�
        EventClose = 1, // ex) ������
        Battle = 2, // ex) ����
        BattleRun = 3, // ex) ����ģ��

        GetSomething = 4, // ex) ���� ��� �̺�Ʈ
        LoseSomething = 5, // ex) ���� �Ҵ� �̺�Ʈ
        GetLose = 6, // ex) ��� �̺�Ʈ

        GetChance = 7, // ex) ���� Ȯ���� ��� �̺�Ʈ
        LoseChance = 8, // ex) ���� Ȯ���� �Ҵ� �̺�Ʈ
        GetLoseChance = 9, // ex) ���� Ȯ���� ��������, �������� �ִ� �̺�Ʈ
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
    public int _sceneNumber; //  �̺�Ʈ �� ����
    public int[] _choiceNumber; // ������ ����

    public float _eventSucessProbability; // ������ ���� Ȯ��

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
        SoonsoonData.Instance.LogPopup.ShowLog("���� �߽��ϴ�..");
        return isSucess;
    }
}
