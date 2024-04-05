using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Synergy", menuName = "Scriptable Object/SynergyData")]
public class SynergyData : ScriptableObject
{
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

    public enum Type
    {
        Start = 0,
        Continue = 1
    }

    [Header(" # Main Info")]
    public string synergyName;
    public SynergyType synergyType = SynergyType.Warrior;
    public Type type = Type.Start;
    public int[] RequiredNumber;

    [Header(" # Buff Info")]
    public float[] MaxHp; // 체력
    public float[] Damage; // 피해량
    public float[] Recovery; // 회복량
    public float[] Speed; // 속도
    public float[] Defense; // 방어
    public float[] Critical; // 크리티컬
    public float[] Shield; // 보호막
    public float[] CoolTime; // 스킬 쿨타임
    public float[] Duration; // 지속 시간
    public float[] Interval; // 간격

    [TextArea]
    public string SynergyDesc;

    [Header(" # Synergy icon")]
    public Sprite icon;
}
