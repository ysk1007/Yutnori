using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotClass
{
    public UnitData _unitData;
    public int _unitRate;

    public SlotClass (UnitData unitData, int rate)
    {
        _unitData = unitData;
        _unitRate = rate;
    }

    public SlotClass (SlotClass slotClass)
    {
        _unitData = slotClass.GetUnitData();
        _unitRate = slotClass.GetUnitRate();
    }

    public UnitData GetUnitData () { return  _unitData; }
    public int GetUnitRate () { return _unitRate; }

    public void Clear()
    {
        _unitData = null;
        _unitRate = 0;
    }
}
