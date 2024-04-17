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
    public float[] oneTier;
    public float[] twoTier;
    public float[] threeTier;
    public float[] fourTier;
    public float[] fiveTier;
    public float[] _currentProbability;

    public Color[] _synergyColor;
    public Color[] _typeColor;
    public Color[] _rateColor;

    public Sprite[] _synergySprites;
    public Sprite[] _typeSprites;
    public Sprite[] _rateSprites;

    public Transform _untiProductArray;
    UnitProduct[] _unitProducts;
    Vector3 _productSize = new Vector3(0.7f, 0.7f, 1f);

    UnitPool _unitPool;
    InventoryManager _inventoryManager;
    private void Awake()
    {
        SoonsoonData.Instance.UnitShop = this;

        upgradeLv = 0;
        _currentProbability = new float[5];
        oneTier = new float[] { 1, 1, 0.65f, 0.5f, 0.37f, 0.245f, 0.2f, 0.15f, 0.1f };
        twoTier = new float[] { 0, 0, 0.3f, 0.35f, 0.35f, 0.35f, 0.3f, 0.25f, 0.15f };
        threeTier = new float[] { 0, 0, 0.05f, 0.15f, 0.25f, 0.3f, 0.33f, 0.33f, 0.33f };
        fourTier = new float[] { 0, 0, 0, 0.03f, 0.03f, 0.1f, 0.15f, 0.2f, 0.3f };
        fiveTier = new float[] { 0, 0, 0, 0, 0, 0.05f, 0.02f, 0.05f, 0.1f };

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
        _unitPool = SoonsoonData.Instance.Unit_pool;
        _inventoryManager = SoonsoonData.Instance.Inventory_Manager;
        newProduct();
    }

    private void Update()
    {

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
        }
    }

    public void BuyProduct()
    {
        if (_selectProductIndex < 0) return;
        SlotClass product = new SlotClass(_unitProducts[_selectProductIndex]._unitData, _unitProducts[_selectProductIndex]._rateType.GetHashCode());
        _inventoryManager.InventoryAdd(product);
        _unitProducts[_selectProductIndex]._isSell = true;
        _unitProducts[_selectProductIndex].transform.localScale = Vector3.zero;
        _selectProductIndex = -1;
    }

    public void SellProduct()
    {
        if (_selectInventoryIndex < 0) return;
        if (!_isOpenShop) return;

        if (_selectInventoryIndex < _inventoryManager._userSquad.Length)
        {
            _inventoryManager.SquadRemove(_selectInventoryIndex);
        }
        else
        {
            _inventoryManager.InventoryRemove(_selectInventoryIndex - _inventoryManager._userSquad.Length);
        }

        _selectProductIndex = -1;
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
        newProduct();
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