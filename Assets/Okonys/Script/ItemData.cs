using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header(" # Main Info")]
    public int ItemID;
    public string _itemName;

    public ItemType _itemType; // ������ Ÿ��

    [Header(" # Status Info")]
    public itemRate _itemRate; // ������ ���
    public int _itemPrice;

    [TextArea]
    public string _itemDesc;

    [Header(" # icon")]
    public Sprite _itemicon;
}
