using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffSet
{
    public bool _isDebuff = false;

    public float _buffAT = 0; // ���ݷ� ����
    public float _buffAS = 0; // ���� �ӵ� ����
    public float _buffDF = 0; // ���� ����
    public float _buffCC = 0; // ġ��Ÿ Ȯ�� ����
    public float _buffSD = 0; // ��ȣ�� ����
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
        use = 0, // �Ҹ�ǰ
        artifact = 1, // ��Ƽ��Ʈ
    }

    public enum itemRate
    {
        rare = 0,
        epic = 1,
        legendary = 2
    }

    public enum functionType
    {
        use = 0, // 1ȸ��
        active = 1, // ��Ƽ�� 
        passive = 2 // �нú�
    }

    [Header(" # Main Info")]
    public int ItemID;
    public string _itemName;

    public ItemType _itemType; // ������ Ÿ��

    [Header(" # Status Info")]
    public itemRate _itemRate; // ������ ���
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
