using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    public GameObject[] _unitCards;
    RectTransform rectTransform;
    public InventoryManager _inventoryManager;

    private void Awake()
    {
        SoonsoonData.Instance.UnitInventory = this;
        rectTransform = GetComponent<RectTransform>();
        _unitCards = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _unitCards[i] = transform.GetChild(i).gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        initInventory();
    }

    // Update is called once per frame
    void Update()
    {
        Size();
    }

    public void Size()
    {
        int stack = 0;
        for (int i = 0; i < _unitCards.Length; i++)
        {
            if (_unitCards[i].activeInHierarchy) stack++;
        }
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 600f + ((stack / 4) * 400f));
    }

    public void initInventory()
    {
        for (int i = 0; i < _inventoryManager._userSquad.Length + _inventoryManager._userInventory.Length; i++)
        {
            AddUnit(i);
        }
    }

    public void AddUnit(int index)
    {
        if (index < _inventoryManager._userSquad.Length)
        {
            if (_inventoryManager._userSquad[index]?.GetUnitData() == null)
            {
                _unitCards[index].SetActive(false);
                return;
            }

            UnitCard unitCard = _unitCards[index].GetComponent<UnitCard>();
            unitCard._unitData = _inventoryManager._userSquad[index].GetUnitData();
            switch (_inventoryManager._userSquad[index].GetUnitRate())
            {
                case 0:
                    unitCard._unitData._unitRate = UnitData.RateType.lower;
                    break;
                case 1:
                    unitCard._unitData._unitRate = UnitData.RateType.middle;
                    break;
                case 2:
                    unitCard._unitData._unitRate = UnitData.RateType.upper;
                    break;
            }
            unitCard.init();
            unitCard._unitIndex = index;
            _unitCards[index].SetActive(true);
        }
        else
        {
            if (_inventoryManager._userInventory[index - _inventoryManager._userSquad.Length]?.GetUnitData() == null)
            {
                _unitCards[index].SetActive(false);
                return;
            }

            UnitCard unitCard = _unitCards[index].GetComponent<UnitCard>();
            unitCard._unitData = _inventoryManager._userInventory[index - _inventoryManager._userSquad.Length].GetUnitData();
            switch (_inventoryManager._userInventory[index - _inventoryManager._userSquad.Length].GetUnitRate())
            {
                case 0:
                    unitCard._unitData._unitRate = UnitData.RateType.lower;
                    break;
                case 1:
                    unitCard._unitData._unitRate = UnitData.RateType.middle;
                    break;
                case 2:
                    unitCard._unitData._unitRate = UnitData.RateType.upper;
                    break;
            }
            unitCard.init(); 
            unitCard._unitIndex = index;
            _unitCards[index].SetActive(true);
        }
    }

    public void RemoveUnit(int index)
    {
        _unitCards[index].SetActive(false);
    }
}
