using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _slotHoloder;
    [SerializeField] private List<Vector2> _unitToAdd = new List<Vector2>();
    [SerializeField] private List<Vector2> _unitToRemove = new List<Vector2>();

    public List<Vector2> _userSquad = new List<Vector2>();
    public List<Vector2> _userInventory = new List<Vector2>();

    private GameObject[] _slots;

    UnitPool _unitPool;
    // Start is called before the first frame update
    void Start()
    {
        _unitPool = SoonsoonData.Instance.Unit_pool;
        _slots = new GameObject[_slotHoloder.transform.childCount];
        for (int i = 0; i < _slotHoloder.transform.childCount; i++)
        {
            _slots[i] = _slotHoloder.transform.GetChild(i).gameObject;
        }
        RefreshUi();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshUi()
    {
        for (int i = 0; i < _slotHoloder.transform.childCount; i++)
        {
            try
            {
                _slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                _slots[i].transform.GetChild(0).GetComponent<Image>().sprite = _unitPool._unitDatas[i].icon;
            }
            catch
            {
                _slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                _slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
    }

    public void InventoryAdd(Vector2 vc)
    {
        _userInventory.Add(vc);
        RefreshUi();
    }

    public void InventoryRemove(Vector2 vc)
    {
        _userInventory.Remove(vc);
        RefreshUi();
    }

    public void SquadAdd(Vector2 vc)
    {
        _userSquad.Add(vc);
        RefreshUi();
    }

    public void SquadRemove(Vector2 vc)
    {
        _userSquad.Remove(vc);
        RefreshUi();
    }
}
