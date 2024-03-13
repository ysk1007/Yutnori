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
    public RateType _unitRate; // ���
    public float[] _unitMaxHp; // �ִ� ü��
    public float[] _unitAT; // ���ݷ�
    public float[] _unitAR; // �����Ÿ�
    public float[] _unitAS; // ���� �ӵ�
    public float[] _unitMS; // �̵� �ӵ�
    public float[] _unitDF; // ����
    public float[] _unitCC; // ũ��Ƽ�� ����
    public float[] _unitCT; // ��Ÿ��
    public float[] _unitFR; // ���� ��Ī ����
    public SkillData _unitSkill; // ���� ��ų

    [TextArea]
    public string UnitDesc;

    [Header(" # Unit icon")]
    public Sprite icon;
}
