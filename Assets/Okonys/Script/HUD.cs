using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum UiType
    {
        Count = 0,
        SynergyColor = 1
    }

    public enum SynergyType
    {
        Warrior = 0,
        Archer = 1,
        Wizard = 2,
        Assassin = 3,
        Healer = 4,
        Merchant = 5,
        Human = 6,
        Ghost = 7,
        Great = 8,
        Devil = 9,
    }

    public SynergyType _Synergytype = SynergyType.Warrior;
    public UiType _uiType = UiType.Count;
    private SynergyManager SM;
    TextMeshProUGUI _thisText;
    Image _thisImage;

    private void Start()
    {
        // "0 전사 1 궁수 2 도사 3 암살자 4 힐러 5 상인"
        SM = SoonsoonData.Instance.Synergy_Manager;
        _thisText = this.GetComponent<TextMeshProUGUI>();
        _thisImage = this.GetComponent<Image>();
    }

    void Update()
    {
        switch (_uiType)
        {
            case UiType.Count:
                switch (_Synergytype)
                {
                    case SynergyType.Warrior:
                        _thisText.text = (SM._p1AttackTypeSynergyList[0] + SM._AttackTypeAddArtifact[0]).ToString();
                        break;
                    case SynergyType.Archer:
                        _thisText.text = (SM._p1AttackTypeSynergyList[1] + SM._AttackTypeAddArtifact[1]).ToString();
                        break;
                    case SynergyType.Wizard:
                        _thisText.text = (SM._p1AttackTypeSynergyList[2] + SM._AttackTypeAddArtifact[2]).ToString();
                        break;
                    case SynergyType.Assassin:
                        _thisText.text = (SM._p1AttackTypeSynergyList[3] + SM._AttackTypeAddArtifact[3]).ToString();
                        break;
                    case SynergyType.Healer:
                        _thisText.text = (SM._p1AttackTypeSynergyList[4] + SM._AttackTypeAddArtifact[4]).ToString();
                        break;
                    case SynergyType.Merchant:
                        _thisText.text = (SM._p1AttackTypeSynergyList[5] + SM._AttackTypeAddArtifact[5]).ToString();
                        break;
                    case SynergyType.Human:
                        _thisText.text = SM._p1UnitTypeSynergyList[0].ToString();
                        break;
                    case SynergyType.Ghost:
                        _thisText.text = SM._p1UnitTypeSynergyList[1].ToString();
                        break;
                    case SynergyType.Great:
                        break;
                    case SynergyType.Devil:
                        break;
                    default:
                        break;
                }
                break;
            case UiType.SynergyColor:
                switch (_Synergytype)
                {
                    case SynergyType.Warrior:
                        //_thisImage.color = SynergyColor(0);
                        break;
                    case SynergyType.Archer:
                        //_thisImage.color = SynergyColor(1);
                        break;
                    case SynergyType.Wizard:
                        //_thisImage.color = SynergyColor(2);
                        break;
                    case SynergyType.Assassin:
                        //_thisImage.color = SynergyColor(3);
                        break;
                    case SynergyType.Healer:
                        //_thisImage.color = SynergyColor(4);
                        break;
                    case SynergyType.Merchant:
                        //_thisImage.color = SynergyColor(5);
                        break;
                    case SynergyType.Human:
                        break;
                    case SynergyType.Ghost:
                        break;
                    case SynergyType.Great:
                        break;
                    case SynergyType.Devil:
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    Color SynergyColor(int i)
    {
        if (SM._p1AttackTypeSynergyList[i] >= 5) return SM._synergyColorList[3];
        else if (SM._p1AttackTypeSynergyList[i] >= 3) return SM._synergyColorList[2];
        else if (SM._p1AttackTypeSynergyList[i] >= 1) return SM._synergyColorList[1];
        else return SM._synergyColorList[0];
    }
}
