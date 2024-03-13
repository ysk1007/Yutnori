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
        SoloBuff, // 1인 버프
        TargetBuff, // 지정 버프
        SquadBuff, // 아군 전체 버프
        LeastHeal, // 체력이 가장 적은 아군 회복
        SquadHeal // 아군 전체 회복
    }

    [Header(" # Main Info")]
    public int SkillID;
    public string SkillName;
    public SkillType skillType;

    [Header(" # Status Info")]
    public float Damage; // 피해량
    public float Speed; // 속도
    public float xRange; // x축 사거리
    public float yRange; // y축 사거리
    public float Duration; // 지속 시간
    public float DamageInterval; // 데미지 간격

    [TextArea]
    public string SkillDesc;

    [Header(" # Skill icon")]
    public Sprite icon;
}
