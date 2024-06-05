using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public int MaxPopulation;

    [SerializeField] private GameObject _cursor;

    [SerializeField] private GameObject _invenSlotHoloder;
    [SerializeField] private GameObject _squadSlotHoloder;
    [SerializeField] private SlotClass _unitToAdd;
    [SerializeField] private SlotClass _unitToRemove;

    [SerializeField] public SlotClass[] _userSquad;
    [SerializeField] public SlotClass[] _userInventory;

    public GameObject[] _Slots;

    public SlotClass _movingSlot;
    public SlotClass _tempSlot;
    public SlotClass _originalSlot;
    public bool _isMoving;

    public int _curindex;
    public int _lastindex;

    UserInfoManager _userInfoManager;
    UnitPool _unitPool;
    Unit_Manager _unitManager;

    private void Awake()
    {
        SoonsoonData.Instance.Inventory_Manager = this;
        _userSquad = new SlotClass[9];
        _userInventory = new SlotClass[12];
    }

    // Start is called before the first frame update
    void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
        _unitPool = SoonsoonData.Instance.Unit_pool;
        _unitManager = SoonsoonData.Instance.Unit_Manager;

        for (int i = 0; i < _userSquad.Length; i++)
        {
            _userSquad[i] = _unitManager._p1unitID[i];
        }

        for (int i = 0; i < _userInventory.Length; i++)
        {
            _userInventory[i] = _unitManager._userInvenUnit[i];
        }

        _Slots = new GameObject[_squadSlotHoloder.transform.childCount + _invenSlotHoloder.transform.childCount];

        for (int i = 0; i < _squadSlotHoloder.transform.childCount; i++)
        {
            _Slots[i] = _squadSlotHoloder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < _Slots.Length - _squadSlotHoloder.transform.childCount; i++)
        {
            _Slots[i + _squadSlotHoloder.transform.childCount] = _invenSlotHoloder.transform.GetChild(i).gameObject;
        }

        RefreshUi();
    }

    // Update is called once per frame
    void Update()
    {
        if (!SoonsoonData.Instance.Unit_Manager._gamePause) return;

        _cursor.SetActive(_isMoving);
        _cursor.transform.position = Input.mousePosition;
        if (_isMoving)
            _cursor.GetComponent<Image>().sprite = _movingSlot.GetUnitData().icon;

        if (Input.GetMouseButtonDown(0))
        {
            BeginMove();

        }
        if (Input.GetMouseButtonUp(0))
        {
            if (_isMoving)
            {
                EndMove();
            }
        }
    }

    private bool BeginMove()
    {
        _originalSlot = GetClosestSlot();
        if (_originalSlot == null || _originalSlot.GetUnitData() == null)
            return false;

        _movingSlot = new SlotClass(_originalSlot);
        _isMoving = true;
        RefreshUi();
        return true;
    }

    private bool EndMove()
    {
        if (GetClosestSlot() == null) // 슬롯이 없음
        {
            _movingSlot.Clear();
        }
        else // 슬롯이 있음
        {
            _originalSlot.Clear();
            _originalSlot = GetClosestSlot(); // 마우스와 가까운 슬롯을 찾음
            if (_originalSlot.GetUnitData() != null) // 슬롯에 유닛이 이미 있음
            {
                _tempSlot = new SlotClass(_originalSlot); // 임시 슬롯에 이미 있는 유닛을 가져옴

                _originalSlot = new SlotClass(_movingSlot); // 선택한 슬롯에 배치

                if (_curindex < _userSquad.Length) // 선택한 슬롯이 스쿼드
                {
                    SquadAdd(_originalSlot, _curindex); // 스쿼드에 추가

                    if (_lastindex < _userSquad.Length) // 마지막 슬롯이 스쿼드
                        SquadAdd(_tempSlot, _lastindex);

                    else //마지막 슬롯이 인벤토리
                        InventoryAdd(_tempSlot, _lastindex - _userSquad.Length);
                }
                else // 선택한 슬롯이 인벤토리
                {
                    InventoryAdd(_originalSlot, _curindex - _userSquad.Length); // 인벤토리에 추가

                    if (_lastindex < _userSquad.Length) // 마지막 슬롯이 스쿼드
                        SquadAdd(_tempSlot, _lastindex);

                    else //마지막 슬롯이 인벤토리
                        InventoryAdd(_tempSlot, _lastindex - _userSquad.Length);
                }

                _movingSlot.Clear();
                RefreshUi();
                _isMoving = false;
                return true;
            }
            else // 슬롯에 유닛이 없음
            {
                if (_curindex < _userSquad.Length) // 선택한 슬롯이 스쿼드
                {
                    if (_unitManager.UserPopulation >= MaxPopulation) // 최대 인구가 부족함
                    {
                        if (_lastindex < _userSquad.Length) // 마지막 슬롯이 스쿼드
                        {
                            _originalSlot = new SlotClass(_movingSlot); // 선택한 슬롯에 배치
                            _movingSlot.Clear(); // 배치 슬롯은 초기화
                            SquadAdd(_originalSlot, _curindex); // 스쿼드에 추가
                        }
                        else //마지막 슬롯이 인벤토리
                        {
                            InventoryAdd(_movingSlot, _lastindex - _userSquad.Length);
                            SoonsoonData.Instance.LogPopup.ShowLog("더 이상 유닛을 배치할 수 없습니다.");
                        }
                    }
                    else
                    {
                        _originalSlot = new SlotClass(_movingSlot); // 선택한 슬롯에 배치
                        _movingSlot.Clear(); // 배치 슬롯은 초기화
                        SquadAdd(_originalSlot, _curindex); // 스쿼드에 추가
                    }
                }
                else // 선택한 슬롯이 인벤토리
                {
                    _originalSlot = new SlotClass(_movingSlot); // 선택한 슬롯에 배치
                    _movingSlot.Clear(); // 배치 슬롯은 초기화
                    InventoryAdd(_originalSlot, _curindex - _userSquad.Length); // 인벤토리에 추가
                }
            }
        }
        _isMoving = false;
        RefreshUi();
        return true;
    }

    private SlotClass GetClosestSlot()
    {
        for (int i = 0; i < _Slots.Length; i++)
        {
            if (Vector2.Distance(_Slots[i].transform.position, Input.mousePosition) <= 48)
            {
                _curindex = i;
                if (!_isMoving) _lastindex = i;
                return (i < _userSquad.Length) ? _userSquad[i] : _userInventory[i - _userSquad.Length];
            }
        }
        _curindex = -1;
        return null;
    }

    public void RefreshUi()
    {
        for (int i = 0; i < _squadSlotHoloder.transform.childCount; i++)
        {
            try
            {
                _Slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                _Slots[i].transform.GetChild(0).GetComponent<Image>().sprite = _unitPool._unitDatas[_userSquad[i].GetUnitData().UnitID - 1].icon;
            }
            catch
            {
                _Slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                _Slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }

        for (int i = 0; i < _Slots.Length - _squadSlotHoloder.transform.childCount; i++)
        {
            try
            {
                _Slots[i + _squadSlotHoloder.transform.childCount].transform.GetChild(0).GetComponent<Image>().enabled = true;
                _Slots[i + _squadSlotHoloder.transform.childCount].transform.GetChild(0).GetComponent<Image>().sprite = _unitPool._unitDatas[_userInventory[i].GetUnitData().UnitID - 1].icon;
            }
            catch
            {
                _Slots[i + _squadSlotHoloder.transform.childCount].transform.GetChild(0).GetComponent<Image>().sprite = null;
                _Slots[i + _squadSlotHoloder.transform.childCount].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
        UserUnitsSave();
        SoonsoonData.Instance.Unit_Manager.UnitRelocation();
    }

    public bool InventoryAdd(SlotClass slot)
    {
        bool _sucess = false;

        for (int i = 0; i < _userInventory.Length; i++)
        {
            if (_userInventory[i]?._unitData == null) // unit id 가 없음
            {
                _userInventory[i] = slot;
                _sucess = true;
                break;
            }
        }

        if (!_sucess) // 인벤토리가 가득 찼음
        {
            _sucess = SquadAdd(slot);
            if (!_sucess) // 스쿼드도 빈칸이 없다면
            {
                SoonsoonData.Instance.LogPopup.ShowLog("당신의 부대가 정원초과 상태 입니다.");
                return _sucess;
            }
        }

        RefreshUi();
        return _sucess;
    }

    public SlotClass ReturnRandomUnit()
    {
        List<int> numbers = new List<int>();

        for (int i = 0; i <  _userSquad.Length + _userInventory.Length; i++)
        {
            if(i < _userSquad.Length)
            {
                if (_userSquad[i]?.GetUnitData() != null) { numbers.Add(i); }
            }
            else
            {
                if (_userInventory[i - _userSquad.Length]?.GetUnitData() != null) { numbers.Add(i); }
            }
        }

        if (numbers.Count == 0) return null; // 유닛이 없음

        int index = Random.Range(0, numbers.Count);

        if (numbers[index] < _userSquad.Length)
        {
            return _userSquad[numbers[index]];
        }
        else
        {
            return _userInventory[numbers[index] - _userSquad.Length];
        }
    }

    public void InventoryAdd(SlotClass slot, int index)
    {
        _userInventory[index] = slot;
        RefreshUi();
    }

    public void InventoryRemove(int index)
    {
        _userInventory[index] = null;
        RefreshUi();
    }

    public bool SquadAdd(SlotClass slot)
    {

        bool _sucess = false;
        if (_unitManager.UserPopulation >= MaxPopulation) return _sucess;

            for (int i = 0; i < _userSquad.Length; i++)
        {
            if (_userSquad[i]?._unitData == null) // unit 데이터가 없음
            {
                _userSquad[i] = slot;
                _sucess = true;
                break;
            }
        }

        if (!_sucess) // 스쿼드가 가득 찼음
            return _sucess;

        RefreshUi();
        return _sucess;
    }

    public void SquadAdd(SlotClass slot, int index)
    {
        _userSquad[index] = slot;
        RefreshUi();
    }

    public void SquadRemove(int index)
    {
        _userSquad[index] = null;
        RefreshUi();
    }

    public void UnitRemove(SlotClass unit)
    {
        for (int i = 0; i < _userSquad.Length + _userInventory.Length; i++)
        {
            if (i < _userSquad.Length)
            {
                if (_userSquad[i]?.GetUnitData()?.UnitID == unit.GetUnitData().UnitID) 
                { 
                    SquadRemove(i);
                    return;
                }
            }
            else
            {
                if (_userInventory[i - _userSquad.Length]?.GetUnitData()?.UnitID == unit.GetUnitData().UnitID) 
                { 
                    InventoryRemove(i - _userSquad.Length);
                    return;
                }
            }
        }
    }

    public void UserUnitsSave()
    {
        for (int i = 0; i < _userSquad.Length; i++)
        {
            if (_userSquad[i]?._unitData == null)
            {
                _userInfoManager.userData.UserSquad[i] = Vector2.zero;
                continue;
            }

            int UnitNumber = _userSquad[i].GetUnitData().UnitID;
            int UnitRate = _userSquad[i].GetUnitRate();

            _userInfoManager.userData.UserSquad[i] = new Vector2(UnitNumber, UnitRate);
        }

        for (int i = 0; i < _userInventory.Length; i++)
        {
            if (_userInventory[i]?._unitData == null)
            {
                _userInfoManager.userData.UserInventory[i] = Vector2.zero;
                continue;
            }

            int UnitNumber = _userInventory[i].GetUnitData().UnitID;
            int UnitRate = _userInventory[i].GetUnitRate();

            _userInfoManager.userData.UserInventory[i] = new Vector2(UnitNumber, UnitRate);
        }
    }
}
