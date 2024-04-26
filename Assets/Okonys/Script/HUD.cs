using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum UiType
    {
        WarriorCount = 0,
        ArcherCount = 1,
        WizardCount = 2,
        AssassinCount = 3,
        HealerCount = 4,
        MerchantCount = 5,
        HumanCount = 6,
        GhostCount = 7,
        GreatCount = 8,
        DevilCount = 9,
        UserGold = 10,
        UserHp = 11,
        UserPopulation = 12,
        MaxPopulation = 13,
    }
    public UiType _uiType = UiType.WarriorCount;
    private SynergyManager SM;
    UserInfoManager _userInfoManager;
    InventoryManager _inventoryManager;
    Unit_Manager _unitManager;
    TextMeshProUGUI _thisText;
    Image _thisImage;

    private void Start()
    {
        // "0 전사 1 궁수 2 도사 3 암살자 4 힐러 5 상인"
        SM = SoonsoonData.Instance.Synergy_Manager;

        _unitManager = SoonsoonData.Instance.Unit_Manager;
        _inventoryManager = SoonsoonData.Instance.Inventory_Manager;
        _userInfoManager = UserInfoManager.Instance;

        _thisText = this.GetComponent<TextMeshProUGUI>();
        _thisImage = this.GetComponent<Image>();
    }

    void Update()
    {
        switch (_uiType)
        {
            case UiType.WarriorCount:
                _thisText.text = (SM._p1AttackTypeSynergyList[0] + SM._AttackTypeAddArtifact[0]).ToString();
                break;
            case UiType.ArcherCount:
                _thisText.text = (SM._p1AttackTypeSynergyList[1] + SM._AttackTypeAddArtifact[1]).ToString();
                break;
            case UiType.WizardCount:
                _thisText.text = (SM._p1AttackTypeSynergyList[2] + SM._AttackTypeAddArtifact[2]).ToString();
                break;
            case UiType.AssassinCount:
                _thisText.text = (SM._p1AttackTypeSynergyList[3] + SM._AttackTypeAddArtifact[3]).ToString();
                break;
            case UiType.HealerCount:
                _thisText.text = (SM._p1AttackTypeSynergyList[4] + SM._AttackTypeAddArtifact[4]).ToString();
                break;
            case UiType.MerchantCount:
                _thisText.text = (SM._p1AttackTypeSynergyList[5] + SM._AttackTypeAddArtifact[5]).ToString();
                break;
            case UiType.HumanCount:
                _thisText.text = SM._p1UnitTypeSynergyList[0].ToString();
                break;
            case UiType.GhostCount:
                _thisText.text = SM._p1UnitTypeSynergyList[1].ToString();
                break;
            case UiType.GreatCount:
                break;
            case UiType.DevilCount:
                break;
            case UiType.UserGold:
                _thisText.text = _userInfoManager.userData.UserGold.ToString();
                break;
            case UiType.UserHp:
                _thisText.text = _userInfoManager.userData.UserHp.ToString();
                break;
            case UiType.UserPopulation:
                _thisText.text = _unitManager.UserPopulation.ToString();
                break;
            case UiType.MaxPopulation:
                _thisText.text = _inventoryManager.MaxPopulation.ToString();
                break;
            default:
                break;
        }
    }
}
