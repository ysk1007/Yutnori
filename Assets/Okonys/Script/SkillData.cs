using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/SkillData")]
public class SkillData : ScriptableObject
{
    public enum SkillType
    {
        ShortRange, // 근접 스킬
        LongRange, // 원거리 투사체 스킬
        Buff, // 버프
        Heal, // 회복
    }

    public enum TargetType
    {
        None, //없음
        Solo, // 자신
        CurrentTarget, // 현재 타겟
        LeastHp, // 체력이 없는 아군
        LeastHpEnemy, // 체력이 적은 적
        MostHp, // 체력이 많은 아군
        MostHpEnemy, // 체력이 많은 적
        MySquad, // 모든 아군
        EnemySquad, // 모든 적
        FarAwayEnemy, // 가장 먼 적
    }

    [Header(" # Main Info")]
    public int SkillID;
    public string SkillName;
    public SkillType skillType;
    public TargetType targetType = TargetType.None;

    [Header(" # Status Info")]
    public float Damage; // 피해량
    public float Recovery; // 회복량
    public float Speed; // 속도
    public float Defense; // 방어
    public float Shield; // 보호막
    public float xRange; // x축 사거리
    public float yRange; // y축 사거리
    public float Duration; // 지속 시간
    public float DamageInterval; // 데미지 간격

    [TextArea]
    public string SkillDesc;

    [Header(" # Skill icon")]
    public Sprite icon;
}
