using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header(" # Main Info")]
    public int ItemID;
    public string _itemName;

    public ItemType _itemType; // 아이템 타입

    [Header(" # Status Info")]
    public itemRate _itemRate; // 아이템 등급
    public int _itemPrice;

    [TextArea]
    public string _itemDesc;

    [Header(" # icon")]
    public Sprite _itemicon;
}
