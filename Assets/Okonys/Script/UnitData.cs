using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Object/UnitData")]
public class UnitData : ScriptableObject
{
    public enum RateType
    {
        lower = 0,
        middle = 1,
        upper = 2,
    }

    [Header(" # Main Info")]
    public int UnitID;
    public string UnitName;
    public Unit.AttackType UnitType;

    [Header(" # Status Info")]
    public RateType _unitRate; // 등급
    public float[] _unitMaxHp; // 최대 체력
    public float[] _unitAT; // 공격력
    public float[] _unitAR; // 사정거리
    public float[] _unitAS; // 공격 속도
    public float[] _unitMS; // 이동 속도
    public float[] _unitDF; // 방어력
    public float[] _unitCC; // 크리티컬 찬스
    public float[] _unitCT; // 쿨타임
    public float[] _unitFR; // 유닛 서칭 범위
    public SkillData _unitSkill; // 유닛 스킬

    [TextArea]
    public string UnitDesc;

    [Header(" # Unit icon")]
    public Sprite icon;
}
