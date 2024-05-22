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
    private float[] _itemProbability; // 아이템 등급 등장 확률

    UserInfoManager _userInfo;

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
        _userInfo = UserInfoManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShopArtifactsLoad()
    {
        // 상점에 아티팩트 데이터가 없으면 새로 생성
        if (!_userInfo.userData.isShopArtifactDatas)
        {
            newItem();
            _userInfo.userData.isShopArtifactDatas = true;
            _userInfo.UserDataSave();
        }

        // 있으면 로드 합니다.
        else
        {
            for (int i = 0; i < _itemProducts.Count; i++)
            {
                if (_userInfo.userData.ShopArtifacts[i] == 0)
                {
                    _itemProducts[i].transform.localScale = Vector3.zero;
                    _itemProducts[i]._itemData = null;
                    _itemProducts[i].init();
                    continue;
                }

                int itemNum = _userInfo.userData.ShopArtifacts[i] - 1;

                _itemProducts[i]._itemData = _itemStock[itemNum].GetItemData();
                _itemProducts[i].init();
                // 해당 아이템의 재고 여부를 false로 설정하여 더 이상 등장하지 않도록 합니다.
                _itemStock[itemNum].SetHaveStock(false);
            }
        }
    }

    public void newItem()
    {
        for (int i = 0; i < _itemProducts.Count; i++) // 재고 정리
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

        for (int i = 0; i < _itemProducts.Count; i++) // 상품 갯수 만큼
        {
            bool hasStock = false; // 재고가 있는지 여부를 나타내는 변수

            // 모든 아이템의 재고 여부를 확인하여 재고가 있는 경우 hasStock을 true로 설정합니다.
            foreach (var item in _itemStock)
            {
                if (item.GetItemRate())
                {
                    hasStock = true;
                    break;
                }
            }

            // 재고가 없으면 함수를 종료합니다.
            if (!hasStock)
            {
                SoonsoonData.Instance.LogPopup.ShowLog("아이템이 모두 떨어졌어요!");
                _itemProducts[i].transform.localScale = Vector3.zero;
                _itemProducts[i]._itemData = null;
                _itemProducts[i].init();
                continue;
            }

            float randomNumber = Random.value; // 0부터 1 사이의 랜덤한 값

            float cumulativeProbability = 0f; // 누적 확률
            int selectedGrade = 0; // 선택된 등급

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
                case 0: // 레어 등급
                    selectedStock = GetRandomStock(_itemStock.Where(item => _rareItems.Contains(item.GetItemData())).ToList());
                    break;
                case 1: // 에픽 등급
                    selectedStock = GetRandomStock(_itemStock.Where(item => _epicItems.Contains(item.GetItemData())).ToList());
                    break;
                case 2: // 전설 등급
                    selectedStock = GetRandomStock(_itemStock.Where(item => _legendaryitems.Contains(item.GetItemData())).ToList());
                    break;
            }

            if (selectedStock != null)
            {
                _itemProducts[i]._itemData = selectedStock.GetItemData();
                _itemProducts[i].init();
                // 해당 아이템의 재고 여부를 false로 설정하여 더 이상 등장하지 않도록 합니다.
                selectedStock.SetHaveStock(false);
                _userInfo.userData.ShopArtifacts[i] = _itemProducts[i]._itemData.ItemID;
            }
            else
            {
                // 해당 등급의 재고가 없는 경우 다른 등급의 아이템을 선택하도록 합니다.
                i--; // 다시 한 번 시도하도록 인덱스를 감소시킵니다.
                continue;
            }
        }
        _userInfo.UserDataSave();
    }

    // 주어진 리스트에서 재고가 있는 아이템 중에서 랜덤하게 선택하는 메서드
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

    void ItemSort() // 등급 별 정리
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
        if (_userInfo.userData.UserGold < _itemProducts[_selectItemIndex]._itemData._itemPrice)
        {
            SoonsoonData.Instance.LogPopup.ShowLog("골드가 부족 합니다.");
            return;
        }

        _userInfo.userData.UserGold -= _itemProducts[_selectItemIndex]._itemData._itemPrice;
        SoonsoonData.Instance.Artifact_Manager.SetArtifact(_itemProducts[_selectItemIndex]._itemData);
        _userInfo.userData.UserArtifacts.Add(_itemProducts[_selectItemIndex]._itemData.ItemID);
        _userInfo.userData.ShopArtifacts[_selectItemIndex] = 0;
        _itemProducts[_selectItemIndex]._itemData = null;

        _itemProducts[_selectItemIndex]._isSell = true;
        _itemProducts[_selectItemIndex].transform.localScale = Vector3.zero;
        _selectItemIndex = -1;
        _userInfo.UserDataSave();
    }
}
