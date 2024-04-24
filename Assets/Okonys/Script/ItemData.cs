using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffSet
{
    public bool _isDebuff = false;

    public float _buffAT = 0; // 공격력 버프
    public float _buffAS = 0; // 공격 속도 버프
    public float _buffDF = 0; // 방어력 버프
    public float _buffCC = 0; // 치명타 확률 버프
    public float _buffSD = 0; // 보호막 버프
}

[System.Serializable]
public class SynergySet
{
    public Unit.AttackType _AttackTypeNumber;
    public int _addSynergy;
}

[System.Serializable]
public class TypeSet
{
    public Unit.UnitType _UnitTypeNumber;
    public int _addType;
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        use = 0, // 소모품
        artifact = 1, // 아티팩트
    }

    public enum itemRate
    {
        rare = 0,
        epic = 1,
        legendary = 2
    }

    public enum functionType
    {
        use = 0, // 1회성
        active = 1, // 액티브 
        passive = 2 // 패시브
    }

    [Header(" # Main Info")]
    public int ItemID;
    public string _itemName;

    public ItemType _itemType; // 아이템 타입

    [Header(" # Status Info")]
    public itemRate _itemRate; // 아이템 등급
    public functionType _functionType;
    public int _itemPrice;

    public int _itemCount;
    public float _itemCoolTime;

    public List<BuffSet> _buffSet;
    public List<SynergySet> _synergySet;
    public List<TypeSet> _typeSet;

    [TextArea]
    public string _itemDesc;

    [Header(" # icon")]
    public Sprite _itemicon;
}
