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
    [SerializeField] private EventData[] events; // 이벤트 데이터 배열
    [SerializeField] private EventData _currentEvent; // 현재 이벤트 데이터
    [SerializeField] int _currentScene; // 현재 씬 번호

    [SerializeField] private Transform _buttonList; // 버튼 리스트의 Transform
    [SerializeField] private List<Button> _Buttons = new List<Button>(); // 버튼 리스트
    [SerializeField] private List<ChoiceButton> _choiceButtons = new List<ChoiceButton>(); // 선택 버튼 리스트
    [SerializeField] private List<Animator> _buttonAnimators = new List<Animator>(); // 버튼 애니메이터 리스트

    [SerializeField] private Image _eventImage; // 이벤트 이미지
    [SerializeField] private SlotClass[] _eventUnit; // 이벤트 유닛 배열
    [SerializeField] private TextMeshProUGUI _titleText; // 이벤트 제목 텍스트
    [SerializeField] private TextMeshProUGUI _mainText; // 이벤트 메인 텍스트
    [SerializeField] private TypewriterByCharacter _typeWriter; // 타자 효과

    int _nextButtonIndex; // 다음 버튼 인덱스

    UserInfoManager _userInfoManager; // 사용자 정보 관리
    Popup _thisPopup; // 현재 팝업
    CanvasManager _canvasManager; // 캔버스 관리자
    Unit_Manager _unitManager; // 유닛 관리자
    InventoryManager _inventoryManager; // 인벤토리 관리자
    EnemyPool _enemyPool; // 적 풀
    UnitPool _unitPool; // 유닛 풀

    private void Awake()
    {
        // 싱글톤 패턴으로 Event_Panel을 초기화합니다.
        SoonsoonData.Instance.Event_Panel = this;

        // 버튼과 관련된 컴포넌트를 리스트에 추가합니다.
        for (int i = 0; i < _buttonList.childCount; i++)
        {
            _Buttons.Add(_buttonList.GetChild(i).GetComponent<Button>());
            _choiceButtons.Add(_buttonList.GetChild(i).GetComponent<ChoiceButton>());
            _buttonAnimators.Add(_buttonList.GetChild(i).GetComponent<Animator>());
            _choiceButtons[i]._buttonIndex = i;
        }

        _thisPopup = GetComponent<Popup>(); // 현재 게임 오브젝트의 Popup 컴포넌트를 가져옵니다.
    }

    private void Start()
    {
        // 인스턴스들을 초기화합니다.
        _userInfoManager = UserInfoManager.Instance;
        _canvasManager = SoonsoonData.Instance.Canvas_Manager;
        _unitManager = SoonsoonData.Instance.Unit_Manager;
        _inventoryManager = SoonsoonData.Instance.Inventory_Manager;
        _unitPool = SoonsoonData.Instance.Unit_pool;
        _enemyPool = SoonsoonData.Instance.Enemy_Pool;
    }

    // 무작위 이벤트를 선택하여 초기화합니다.
    public void RandomEvent()
    {
        _thisPopup.OnePopup(); // 팝업을 하나 표시합니다.
        int eventNumber = Random.Range(0, events.Length); // 무작위로 이벤트 번호를 선택합니다.
        _currentEvent = events[eventNumber]; // 현재 이벤트를 설정합니다.
        _currentScene = 1; // 씬 번호를 1로 초기화합니다.
        init(); // 이벤트를 초기화합니다.

        // 사용자 데이터에 이벤트 정보를 저장합니다.
        _userInfoManager.userData.isEventData = true;
        _userInfoManager.userData.EventNum = eventNumber;
        _userInfoManager.UserDataSave(); // 사용자 데이터 저장
    }

    // 특정 인덱스의 이벤트를 호출하여 초기화합니다.
    public void CallEvent(int index)
    {
        _thisPopup.OnePopup(); // 팝업을 하나 표시합니다.
        _currentEvent = events[index]; // 현재 이벤트를 설정합니다.
        _currentScene = 1; // 씬 번호를 1로 초기화합니다.
        init(); // 이벤트를 초기화합니다.

        // 사용자 데이터에 이벤트 정보를 저장합니다.
        _userInfoManager.userData.isEventData = true;
        _userInfoManager.userData.EventNum = index;
        _userInfoManager.UserDataSave(); // 사용자 데이터 저장
    }

    // 현재 이벤트 데이터를 반환합니다.
    public EventData GetEventData()
    {
        return _currentEvent;
    }

    // 특정 인덱스의 이벤트 유닛을 반환합니다.
    public SlotClass GetEventUnit(int index)
    {
        return _eventUnit[index];
    }

    // 이벤트를 초기화합니다.
    public void init()
    {
        _titleText.text = _currentEvent._eventName; // 이벤트 제목 설정
        _typeWriter.ShowText(_currentEvent._mainText[_currentScene - 1]); // 타자 효과로 메인 텍스트 표시
        _eventImage.sprite = _currentEvent._eventImage; // 이벤트 이미지 설정

        // 이벤트 유닛 초기화
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

        // 버튼 설정
        for (int i = 0; i < _Buttons.Count; i++)
        {
            if (_currentEvent._choiceNumber[_currentScene - 1] > i)
            {
                _Buttons[i].transform.localScale = Vector3.one; // 버튼 크기를 원래대로 설정

                // 첫 장면이 아닐 경우 전 장면의 선택지 만큼 더해줍니다.
                int plus = (_currentScene - 1 > 0) ? (_currentScene - 1) * _currentEvent._choiceNumber[(_currentScene - 2)] : 0;

                _choiceButtons[i].Init(_currentEvent._buttonText[i + plus]); // 버튼 텍스트 설정
            }
            else
            {
                _Buttons[i].transform.localScale = Vector3.zero; // 버튼을 숨깁니다.
            }
        }
    }

    // 버튼 애니메이션을 순서대로 표시합니다.
    public void ButtonShow()
    {
        _nextButtonIndex = 0; // 다음 버튼 인덱스 초기화
        float delay = 0; // 지연 시간 초기화
        for (int i = 0; i < _buttonAnimators.Count; i++)
        {
            delay += 0.5f; // 지연 시간을 증가시킵니다.
            Invoke("ButtonAnimation", delay); // 지연 후 버튼 애니메이션 호출
        }
    }

    // 버튼 애니메이션을 페이드 아웃합니다.
    public void ButtonFade()
    {
        for (int i = 0; i < _buttonAnimators.Count; i++)
        {
            _buttonAnimators[i].SetTrigger("Fade"); // 버튼 애니메이션 트리거를 설정합니다.
        }
    }

    // 버튼 애니메이션을 표시합니다.
    public void ButtonAnimation()
    {
        _buttonAnimators[_nextButtonIndex].SetTrigger("Show"); // 버튼 애니메이션 트리거를 설정합니다.
        _nextButtonIndex++; // 다음 버튼 인덱스를 증가시킵니다.
    }

    // 이벤트 실패 처리
    public void EventFail()
    {
        _userInfoManager.userData.SetUserHp(-25); // 사용자 HP 감소
        EventContinue(); // 이벤트 계속 진행
    }

    // 이벤트를 계속 진행합니다.
    public void EventContinue()
    {
        _currentScene++; // 씬 번호를 증가시킵니다.
        init(); // 이벤트를 초기화합니다.
    }

    // 이벤트를 닫습니다.
    public void EventClose()
    {
        _canvasManager.FadeImage(); // 캔버스 이미지 페이드 아웃
        _thisPopup.ZeroPopup(); // 팝업을 닫습니다.

        _enemyPool.CallBoss(); // 보스 호출

        // 사용자 데이터에서 이벤트 정보 삭제
        _userInfoManager.userData.isEventData = false;
        _userInfoManager.userData.EventNum = 0;
        _userInfoManager.UserDataSave(); // 사용자 데이터 저장
    }

    // 이벤트 전투를 시작합니다.
    public void EventBattle()
    {
        _thisPopup.ZeroPopup(); // 팝업을 닫습니다.
        _canvasManager.ShowUi(); // UI를 표시합니다.
        _unitManager._p2unitID = _currentEvent._eventEnemy; // 이벤트 적 설정
        _unitManager.FieldReset(); // 필드 리셋
        _userInfoManager.UserDataSave(); // 사용자 데이터 저장
    }

    // 이벤트 전투를 진행하거나 계속합니다.
    public void EventBattleRun()
    {
        if (_currentEvent.EventChance())
        {
            EventContinue(); // 계속 진행
        }
        else
        {
            EventBattle(); // 전투 시작
        }
    }

    // 이벤트에서 무언가를 얻습니다.
    public void EventGetSomething(int index)
    {
        if (_currentEvent._goldValue.Length != 0)
            _userInfoManager.userData.SetUserGold(_currentEvent._goldValue[index]); // 돈 획득

        if (_currentEvent._hpValue.Length != 0)
            _userInfoManager.userData.SetUserHp(_currentEvent._hpValue[index]); // 체력 증가

        if (_currentEvent._someUnit.Length != 0)
        {
            if (!_inventoryManager.InventoryAdd(_eventUnit[index])) return; // 인벤토리에 유닛 추가
        }

        EventContinue(); // 이벤트 계속 진행
    }

    // 이벤트에서 무언가를 얻을 기회
    public void EventGetChance()
    {
        if (_currentEvent.EventChance())
        {
            EventContinue(); // 성공하면 계속 진행
        }
        else
        {
            EventContinue(); // 실패하면 계속 진행
        }
    }

    // 이벤트에서 무언가를 잃습니다.
    public void EventLoseSomething(int index)
    {
        if (_currentEvent._goldValue.Length != 0)
            _userInfoManager.userData.SetUserGold(-1 * _currentEvent._goldValue[index]); // 돈 잃음

        if (_currentEvent._hpValue.Length != 0)
            _userInfoManager.userData.SetUserHp(-1 * _currentEvent._hpValue[index]); // 체력 감소

        if (_eventUnit[index]._unitData != null)
            _inventoryManager.UnitRemove(_eventUnit[index]); // 유닛 제거

        EventContinue(); // 이벤트 계속 진행
    }

    // 이벤트에서 실패할 경우
    public void EventLoseChance()
    {
        if (_currentEvent.EventChance())
        {
            EventLoseSomething(0); // 기회가 있으면 무언가를 잃음
            EventContinue(); // 계속 진행
        }
        else
        {
            EventContinue(); // 기회가 없으면 계속 진행
        }
    }

    // 이벤트에서 무언가를 얻거나 잃습니다.
    public void EventGetLose(int index)
    {
        if (_currentEvent._goldValue.Length != 0)
            _userInfoManager.userData.SetUserGold(_currentEvent._goldValue[index]); // 돈 획득

        if (_currentEvent._hpValue.Length != 0)
            _userInfoManager.userData.SetUserHp(_currentEvent._hpValue[index]); // 체력 증가

        if (_currentEvent._someUnit[index]?.GetUnitData()?.UnitName == "RandomGet")
        {
            if (!_inventoryManager.InventoryAdd(_eventUnit[index])) return; // 인벤토리에 유닛 추가
        }
        else if (_currentEvent._someUnit[index]?.GetUnitData()?.UnitName == "RandomLose")
        {
            _inventoryManager.UnitRemove(_eventUnit[index]); // 유닛 제거
        }

        EventContinue(); // 이벤트 계속 진행
    }

    // 이벤트에서 얻거나 잃음.
    public void EventGetLoseChance()
    {
        if (_currentEvent.EventChance())
        {
            EventGetSomething(0); // 기회가 있으면 무언가를 얻음
            EventContinue(); // 계속 진행
        }
        else
        {
            EventLoseSomething(0); // 기회가 없으면 무언가를 잃음
            EventContinue(); // 계속 진행
        }
    }
}
