using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
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
        _cursor.SetActive(_isMoving);
        _cursor.transform.position = Input.mousePosition;
        if (_isMoving)
            _cursor.GetComponent<Image>().sprite = _movingSlot.GetUnitData().icon;

        if (Input.GetMouseButtonDown(0))
        {
            if (_isMoving)
            {
                EndMove();
            }
            else
            {
                BeginMove();
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
        if (GetClosestSlot() == null) // ������ ����
        {
            _movingSlot.Clear();
        }
        else // ������ ����
        {
            _originalSlot.Clear();
            _originalSlot = GetClosestSlot(); // ���콺�� ����� ������ ã��
            if (_originalSlot.GetUnitData() != null) // ���Կ� ������ �̹� ����
            {
                _tempSlot = new SlotClass(_originalSlot); // �ӽ� ���Կ� �̹� �ִ� ������ ������

                _originalSlot = new SlotClass(_movingSlot); // ������ ���Կ� ��ġ
                SquadAdd(_originalSlot, _curindex); // �����忡 �߰�
                SquadAdd(_tempSlot, _lastindex);

                _movingSlot.Clear();
                RefreshUi(); 
                _isMoving = false;
                return true;
            }
            else // ���Կ� ������ ����
            {
                _originalSlot = new SlotClass(_movingSlot); // ������ ���Կ� ��ġ
                _movingSlot.Clear(); // ��ġ ������ �ʱ�ȭ
                SquadAdd(_originalSlot, _curindex); // �����忡 �߰�
            }
        }
        _isMoving = false;
        RefreshUi();
        return true;
    }

    private SlotClass GetClosestSlot()
    {
        if (Vector2.Distance(_squadSlotHoloder.transform.position, Input.mousePosition) < Vector2.Distance(_invenSlotHoloder.transform.position, Input.mousePosition))
        {
            for (int i = 0; i < _userSquad.Length; i++)
            {
                if (Vector2.Distance(_Slots[i].transform.position, Input.mousePosition) <= 48)
                {
                    _curindex = i;
                    if (!_isMoving) _lastindex = i;
                    return _userSquad[i];
                }
            }

            _curindex = -1;
            return null;
        }
        else
        {
            for (int i = 0; i < _userInventory.Length; i++)
            {
                if (Vector2.Distance(_Slots[i].transform.position, Input.mousePosition) <= 48)
                {
                    _curindex = i;
                    if (!_isMoving) _lastindex = i;
                    return _userInventory[i];
                }
            }

            _curindex = -1;
            return null;
        }
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

        SoonsoonData.Instance.Unit_Manager.UnitRelocation();
    }

    public void InventoryAdd(SlotClass slot)
    {
        for (int i = 0; i < _userInventory.Length; i++)
        {
            if (_userInventory[i] == null) // unit id �� ����
            {
                _userInventory[i] = slot;
            }
        }
        RefreshUi();
    }

    public void InventoryRemove(int index)
    {
        _userInventory[index] = null;
        RefreshUi();
    }

    public void SquadAdd(SlotClass slot)
    {
        for (int i = 0; i < _userSquad.Length; i++)
        {
            if (_userSquad[i]._unitData == null) // unit �����Ͱ� ����
            {
                _userSquad[i] = slot;
                break;
            }
        }
        RefreshUi();
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
}
