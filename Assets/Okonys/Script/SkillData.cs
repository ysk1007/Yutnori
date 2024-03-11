using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/SkillData")]
public class SkillData : ScriptableObject
{
    public enum SkillType
    {
        ShortRange,
        LongRange,
        Buff
    }

    [Header(" # Main Info")]
    public int SkillID;
    public string SkillName;
    public SkillType skillType;

    [Header(" # Status Info")]
    public int[] Damage; // 피해량
    public float Speed; // 속도
    public float xRange; // x축 사거리
    public float yRange; // y축 사거리
    public float Duration; // 지속 시간

    [TextArea]
    public string SkillDesc;

    [Header(" # Skill icon")]
    public Sprite icon;
}
