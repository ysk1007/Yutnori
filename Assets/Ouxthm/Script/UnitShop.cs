using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitShop : MonoBehaviour
{
    public Button[] btn = new Button[2];
    public TextMeshProUGUI[] probabilityText = new TextMeshProUGUI[5];
    public int _selectProductIndex = -1;
    public int _selectInventoryIndex = -1;
    public bool _isOpenShop = false;
    public int upgradeLv;
    public int _rerollPrice = 1;
    public TextMeshProUGUI _rerollText;
    public float[] oneTier;
    public float[] twoTier;
    public float[] threeTier;
    public float[] fourTier;
    public float[] fiveTier;
    public float[] _currentProbability;

    public Color[] _synergyColor;
    public Color[] _typeColor;
    public Sprite[] _rateImage;
    public Color[] _cardColor;

    public Sprite[] _synergySprites;
    public Sprite[] _typeSprites;
    public Sprite[] _rateSprites;

    public Transform _untiProductArray;

    public HorizontalLayoutGroup _layoutGroup;

    UnitProduct[] _unitProducts;
    Vector3 _productSize = new Vector3(0.7f, 0.7f, 1f);
    Vector3 _sellSize = new Vector3(0.7f, 0f, 1f);

    UnitPool _unitPool;
    InventoryManager _inventoryManager;
    UserInfoManager _userInfo;

    private void Awake()
    {
        SoonsoonData.Instance.UnitShop = this;

        upgradeLv = 0;
        _currentProbability = new float[5];
        oneTier = new float[] { 1, 1, 0.65f, 0.5f, 0.37f, 0.245f, 0.2f, 0.15f, 0.1f, 0f };
        twoTier = new float[] { 0, 0, 0.3f, 0.35f, 0.35f, 0.35f, 0.3f, 0.25f, 0.15f, 0.1f};
        threeTier = new float[] { 0, 0, 0.05f, 0.15f, 0.25f, 0.3f, 0.33f, 0.33f, 0.33f, 0.35f };
        fourTier = new float[] { 0, 0, 0, 0.03f, 0.03f, 0.1f, 0.15f, 0.2f, 0.3f, 0.33f, 0.40f};
        fiveTier = new float[] { 0, 0, 0, 0, 0, 0.05f, 0.02f, 0.05f, 0.1f, 0.15f };

        _unitProducts = new UnitProduct[_untiProductArray.childCount];
        for (int i = 0; i < _untiProductArray.childCount; i++)
        {
            _unitProducts[i] = _untiProductArray.GetChild(i).GetComponent<UnitProduct>();
            _unitProducts[i]._productIndex = i;
            _unitProducts[i].GetComponent<Button>().onClick.AddListener(BuyProduct);
        }

        for (int i = 0; i < probabilityText.Length; i++)
        {
            probabilityText[i] = transform.GetChild(0).GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
        }
        UpdateProbability();

        btn[0].onClick.AddListener(() => LevelUP());
        btn[1].onClick.AddListener(() => Reroll());
    }

    private void Start()
    {
        _userInfo = UserInfoManager.Instance;
        _unitPool = SoonsoonData.Instance.Unit_pool;
        _inventoryManager = SoonsoonData.Instance.Inventory_Manager;
        ShopProductsLoad();
    }

    private void Update()
    {
        _rerollPrice = 2 * (1 + UserInfoManager.Instance.userData.GameLevel);
        _rerollText.text = _rerollPrice.ToString();
        upgradeLv = UserInfoManager.Instance.userData.TurnCounter / 5;
        UpdateProbability();
    }

    public void ShopProductsLoad()
    {
        for (int i = 0; i < _unitProducts.Length; i++)
        {
            if (_userInfo.userData.ShopUnits[i] == 0)
            {
                _unitProducts[i].transform.localScale = _sellSize;
                _unitProducts[i]._unitData = null;
                _unitProducts[i]._isSell = true;
                continue;
            }

            int unitNum = _userInfo.userData.ShopUnits[i] - 1;

            _unitProducts[i]._unitData = _unitPool._unitDatas[unitNum];
            _unitProducts[i].init();
            _unitProducts[i].transform.localScale = _productSize;
        }
        CheckProduct();
    }

    public void newProduct()
    {
        for (int i = 0; i < _unitProducts.Length; i++) // 상품 갯수 만큼
        {
            float randomNumber = Random.value; // 0부터 1 사이의 랜덤한 값

            float cumulativeProbability = 0f; // 누적 확률
            int selectedGrade = 0; // 선택된 등급

            for (int j = 0; j < _currentProbability.Length; j++)
            {
                cumulativeProbability += _currentProbability[j];

                if (randomNumber < cumulativeProbability)
                {
                    selectedGrade = j;
                    break;
                }
            }

            switch (selectedGrade)
            {
                case 0: // 커먼 등급
                    _unitProducts[i]._unitData = _unitPool._commonUnits[Random.Range(0, _unitPool._commonUnits.Count)];
                    _unitProducts[i].init();
                    break;
                case 1: // 언커먼 등급
                    _unitProducts[i]._unitData = _unitPool._uncommonUnits[Random.Range(0, _unitPool._uncommonUnits.Count)];
                    _unitProducts[i].init();
                    break;
                case 2: // 레어 등급
                    _unitProducts[i]._unitData = _unitPool._rareUnits[Random.Range(0, _unitPool._rareUnits.Count)];
                    _unitProducts[i].init();
                    break;
                case 3: // 에픽 등급
                    _unitProducts[i]._unitData = _unitPool._epicUnits[Random.Range(0, _unitPool._epicUnits.Count)];
                    _unitProducts[i].init();
                    break;
                case 4: // 전설 등급
                    _unitProducts[i]._unitData = _unitPool._legendaryUnits[Random.Range(0, _unitPool._legendaryUnits.Count)];
                    _unitProducts[i].init();
                    break;
            }
            _unitProducts[i].transform.localScale = _productSize;
            _userInfo.userData.ShopUnits[i] = _unitProducts[i]._unitData.UnitID;
        }
        _userInfo.UserDataSave();
    }

    public void BuyProduct()
    {
        if (_selectProductIndex < 0) return;
        if (_userInfo.userData.GetUserGold() < _unitProducts[_selectProductIndex]._productPrice)
        {
            SoonsoonData.Instance.LogPopup.ShowLog("골드가 부족 합니다.");
            return;
        }

        SlotClass product = new SlotClass(_unitProducts[_selectProductIndex]._unitData, _unitProducts[_selectProductIndex]._rateType.GetHashCode());

        if (!_inventoryManager.InventoryAdd(product)) return;

        _userInfo.userData.SetUserGold(-_unitProducts[_selectProductIndex]._productPrice);
        _userInfo.userData.ShopUnits[_selectProductIndex] = 0;
        _unitProducts[_selectProductIndex]._isSell = true;
        _unitProducts[_selectProductIndex].transform.localScale = _sellSize;
        _selectProductIndex = -1;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Buy);
        CheckProduct();
        _userInfo.UserDataSave();
    }

    public void SellProduct(UnitCard unitCard)
    {
        if (_selectInventoryIndex < 0) return;
        if (!_isOpenShop) return;

        int _userUnitCount = 0;

        // 스쿼드와 인벤토리에서 유닛을 찾음
        for (int i = 0; i < _inventoryManager._userSquad.Length; i++)
        {
            if (_inventoryManager._userSquad[i]._unitData != null)
            {
                _userUnitCount++;
                if (_userUnitCount >= 2) break;
            }
        }

        // 유닛 개수가 2보다 적으면 인벤토리에서 추가 유닛을 찾음
        if (_userUnitCount < 2)
        {
            for (int i = 0; i < _inventoryManager._userInventory.Length; i++)
            {
                if (_inventoryManager._userInventory[i]._unitData != null)
                {
                    _userUnitCount++;
                    if (_userUnitCount >= 2) break;
                }
            }
        }

        // 유닛이 하나만 있을 경우 디버그 로그 출력
        if (_userUnitCount < 2)
        {
            SoonsoonData.Instance.LogPopup.ShowLog("최소 하나의 유닛을 보유하고 있어야 합니다.");
            return;
        }

        if (_selectInventoryIndex < _inventoryManager._userSquad.Length)
        {
            _userInfo.userData.SetUserGold(unitCard._productPrice);
            _inventoryManager.SquadRemove(_selectInventoryIndex);
        }
        else
        {
            _userInfo.userData.SetUserGold(unitCard._productPrice);
            _inventoryManager.InventoryRemove(_selectInventoryIndex - _inventoryManager._userSquad.Length);
        }

        _selectProductIndex = -1;
        unitCard._unitData = null;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Sell);
    }

    public void LevelUP()
    {
        if(upgradeLv < oneTier.Length - 1)
        {
            upgradeLv++;
            UpdateProbability();
        }
    }
    public void Reroll()
    {
        if (_userInfo.userData.GetUserGold() < _rerollPrice)
        {
            SoonsoonData.Instance.LogPopup.ShowLog("골드가 부족 합니다.");
            return;
        }
        _userInfo.userData.SetUserGold(-_rerollPrice);
        newProduct();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Restock);
    }

    public void CheckProduct() // 제품이 모두 품절인지 체크하는 메소드
    {
        for (int i = 0; i < _unitProducts.Length; i++)
        {
            if (_unitProducts[i]._isSell == false) return;
        }

        newProduct(); // 모두 품절이면 재입고
    }

    public void UpdateProbability()     // 확률 텍스트 업데이트
    {
        _currentProbability = new float[] { oneTier[upgradeLv], twoTier[upgradeLv], threeTier[upgradeLv], fourTier[upgradeLv], fiveTier[upgradeLv] };
        probabilityText[0].text = (oneTier[upgradeLv] * 100f).ToString("F0") + "%";
        probabilityText[1].text = (twoTier[upgradeLv] * 100f).ToString("F0") + "%";
        probabilityText[2].text = (threeTier[upgradeLv] * 100f).ToString("F0") + "%";
        probabilityText[3].text = (fourTier[upgradeLv] * 100f).ToString("F0") + "%";
        probabilityText[4].text = (fiveTier[upgradeLv] * 100f).ToString("F0") + "%";
    }
}