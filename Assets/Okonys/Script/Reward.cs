using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnitData;

public class Reward : MonoBehaviour
{
    public enum RewardType
    {
        money = 0,
        unit = 1,
        use = 2,
        artifact = 3,
    }

    public RewardType _rewardType;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private int _goldValue;
    [SerializeField] private UnitData _unitData;
    [SerializeField] private int _unitRate;
    [SerializeField] private itemStock _itemStock;
    Button _button;

    UserInfoManager _userInfoManager;
    InventoryManager _inventoryManager;
    ArtifactManager _artifactManager;

    private void Awake()
    {
        _button = this.GetComponent<Button>();
    }

    void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
        _inventoryManager = SoonsoonData.Instance.Inventory_Manager;
        _artifactManager = SoonsoonData.Instance.Artifact_Manager;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(RewardType type, Sprite icon = null, string text = null, UnitData unit = null, itemStock stock = null)
    {
        _rewardType = type;

        switch (_rewardType)
        {
            case RewardType.money:
                _goldValue = int.Parse(text);
                _icon.sprite = icon;
                _text.text = text + " 냥";
                break;
            case RewardType.unit:
                float randomNumber = Random.value; // 0부터 1 사이의 랜덤 값
                if (randomNumber < 0.667f) // 66.7%
                {
                    _unitRate = 0;
                }
                else if (randomNumber < 0.889f) // 22.2%
                {
                    _unitRate = 1;
                }
                else // 11.1%
                {
                    _unitRate = 2;
                }
                _unitData = unit;
                _icon.sprite = unit.icon;
                _text.text = unit.UnitName;
                break;
            case RewardType.use:
            case RewardType.artifact:
                _itemStock = stock;
                if (stock == null) return;
                _icon.sprite = stock._itemData._itemicon;
                _text.text = stock._itemData._itemName;
                break;
        }

        gameObject.SetActive(true);
    }

    public void RewardGet()
    {
        switch (_rewardType)
        {
            case Reward.RewardType.money:
                _userInfoManager.userData.UserGold += _goldValue;
                break;

            case Reward.RewardType.use:
                break;

            case Reward.RewardType.unit:
                SlotClass product = new SlotClass(_unitData, _unitRate);
                if (!_inventoryManager.InventoryAdd(product)) return;
                break;

            case Reward.RewardType.artifact:
                _itemStock.SetHaveStock(false);
                _artifactManager.SetArtifact(_itemStock.GetItemData());
                _userInfoManager.userData.UserArtifacts.Add(_itemStock.GetItemData().ItemID);
                break;

            default:
                break;
        }
        gameObject.SetActive(false);

        _unitData = null;
        _itemStock = null;
        _icon.sprite = null;
        _text.text = null;
    }

    public void Reset()
    {
        gameObject.SetActive(false);

        _unitData = null;
        _itemStock = null;
        _icon.sprite = null;
        _text.text = null;
    }
}
