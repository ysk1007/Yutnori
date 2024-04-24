using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[System.Serializable]
public class itemStock
{
    public ItemData _itemData;
    public bool _haveStock = true;

    public itemStock(ItemData itemData, bool haveStock)
    {
        _itemData = itemData;
        _haveStock = haveStock;
    }

    public itemStock(itemStock itemStock)
    {
        _itemData = itemStock.GetItemData();
        _haveStock = itemStock.GetItemRate();
    }

    public ItemData GetItemData() { return _itemData; }
    public bool GetItemRate() { return _haveStock; }

    public void Clear()
    {
        _itemData = null;
        _haveStock = true;
    }

    public void SetHaveStock(bool haveStock)
    {
        _haveStock = haveStock;
    }
}

public class ItemShop : MonoBehaviour
{
    [SerializeField]
    public List<itemStock> _itemStock;
    public List<ItemData> _itemDatas;
    public List<ItemData> _rareItems;
    public List<ItemData> _epicItems;
    public List<ItemData> _legendaryitems;

    public Transform _itemProductArray;

    public List<ItemProduct> _itemProducts;

    public int _selectItemIndex = -1;


    [SerializeField]
    private float[] _itemProbability; // ������ ��� ���� Ȯ��

    private void Awake()
    {
        SoonsoonData.Instance.ItemShop = this;
        _itemStock = new List<itemStock>(_itemDatas.Count);
        ItemSort();

        int index = 0;
        foreach (var item in _itemProductArray.GetComponentsInChildren<ItemProduct>())
        {
            _itemProducts.Add(item);
            item.GetComponent<ItemProduct>()._itemShop = this;
            item.GetComponent<ItemProduct>()._productIndex = index;
            index++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        newItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newItem()
    {
        for (int i = 0; i < _itemProducts.Count; i++)
        {
            if (!_itemProducts[i]._itemData) continue;

            foreach (var item in _itemStock)
            {
                if (item.GetItemData() == _itemProducts[i]._itemData)
                {
                    item.SetHaveStock(true);
                    break;
                }
            }

        }

        for (int i = 0; i < _itemProducts.Count; i++) // ��ǰ ���� ��ŭ
        {
            bool hasStock = false; // ��� �ִ��� ���θ� ��Ÿ���� ����

            // ��� �������� ��� ���θ� Ȯ���Ͽ� ��� �ִ� ��� hasStock�� true�� �����մϴ�.
            foreach (var item in _itemStock)
            {
                if (item.GetItemRate())
                {
                    hasStock = true;
                    break;
                }
            }

            // ��� ������ �Լ��� �����մϴ�.
            if (!hasStock)
            {
                Debug.Log("All items are out of stock. Stopping newItem() function.");
                _itemProducts[i]._itemData = null;
                _itemProducts[i].init();
                continue;
            }

            float randomNumber = Random.value; // 0���� 1 ������ ������ ��

            float cumulativeProbability = 0f; // ���� Ȯ��
            int selectedGrade = 0; // ���õ� ���

            for (int j = 0; j < _itemProbability.Length; j++)
            {
                cumulativeProbability += _itemProbability[j];

                if (randomNumber < cumulativeProbability)
                {
                    selectedGrade = j;
                    break;
                }
            }

            itemStock selectedStock = null;

            switch (selectedGrade)
            {
                case 0: // ���� ���
                    selectedStock = GetRandomStock(_itemStock.Where(item => _rareItems.Contains(item.GetItemData())).ToList());
                    break;
                case 1: // ���� ���
                    selectedStock = GetRandomStock(_itemStock.Where(item => _epicItems.Contains(item.GetItemData())).ToList());
                    break;
                case 2: // ���� ���
                    selectedStock = GetRandomStock(_itemStock.Where(item => _legendaryitems.Contains(item.GetItemData())).ToList());
                    break;
            }

            if (selectedStock != null)
            {
                _itemProducts[i]._itemData = selectedStock.GetItemData();
                _itemProducts[i].init();
                // �ش� �������� ��� ���θ� false�� �����Ͽ� �� �̻� �������� �ʵ��� �մϴ�.
                selectedStock.SetHaveStock(false);
            }
            else
            {
                // �ش� ����� ��� ���� ��� �ٸ� ����� �������� �����ϵ��� �մϴ�.
                i--; // �ٽ� �� �� �õ��ϵ��� �ε����� ���ҽ�ŵ�ϴ�.
                continue;
            }
        }
    }

    // �־��� ����Ʈ���� ��� �ִ� ������ �߿��� �����ϰ� �����ϴ� �޼���
    private itemStock GetRandomStock(List<itemStock> itemList)
    {
        List<itemStock> availableItems = itemList.Where(item => item.GetItemRate()).ToList();

        if (availableItems.Count > 0)
        {
            int randomIndex = Random.Range(0, availableItems.Count);
            return availableItems[randomIndex];
        }
        else
        {
            return null;
        }
    }

    void ItemSort() // ��� �� ����
    {
        for (int i = 0; i < _itemDatas.Count; i++)
        {
            _itemStock.Add(new itemStock(_itemDatas[i], true));
            switch (_itemDatas[i]._itemRate)
            {
                case ItemData.itemRate.rare:
                    _rareItems.Add(_itemStock[i].GetItemData());
                    break;
                case ItemData.itemRate.epic:
                    _epicItems.Add(_itemStock[i].GetItemData());
                    break;
                case ItemData.itemRate.legendary:
                    _legendaryitems.Add(_itemStock[i].GetItemData());
                    break;
                default:
                    break;
            }
        }
    }

    public void BuyItem()
    {
        if (_selectItemIndex < 0) return;

        SoonsoonData.Instance.Artifact_Manager.SetArtifact(_itemProducts[_selectItemIndex]._itemData);
        _itemProducts[_selectItemIndex]._itemData = null;

        _itemProducts[_selectItemIndex]._isSell = true;
        _itemProducts[_selectItemIndex].transform.localScale = Vector3.zero;
        _selectItemIndex = -1;
    }
}
