using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/SkillData")]
public class SkillData : ScriptableObject
{
    public enum SkillType
    {
        ShortRange, // ���� ��ų
        LongRange, // ���Ÿ� ����ü ��ų
        AreaSkill, // ��ġ ���� ��ų
        Buff, // ����
        Debuff, // �����
        Heal, // ȸ��
    }

    public enum TargetType
    {
        None, //����
        Solo, // �ڽ�
        CurrentTarget, // ���� Ÿ��
        LeastHp, // ü���� ���� �Ʊ�
        LeastHpEnemy, // ü���� ���� ��
        MostHp, // ü���� ���� �Ʊ�
        MostHpEnemy, // ü���� ���� ��
        MySquad, // ��� �Ʊ�
        EnemySquad, // ��� ��
        FarAwayEnemy, // ���� �� ��
    }

    [Header(" # Main Info")]
    public int SkillID;
    public string SkillName;
    public SkillType skillType;
    public TargetType targetType = TargetType.None;

    [Header(" # Buff Info")]
    public float Damage; // ���ط�
    public float Recovery; // ȸ����
    public float Speed; // �ӵ�
    public float Defense; // ���
    public float Critical; // ũ��Ƽ��
    public float Shield; // ��ȣ��
    public float xRange; // x�� ��Ÿ�
    public float yRange; // y�� ��Ÿ�
    public float Duration; // ���� �ð�
    public float DamageCount; // ������ Ƚ��
    public float DamageInterval; // ������ ����
    public bool StunCheck; // ���� ����
    public float StunTime; // ���� �ð�
    public Vector3 ColliderPos; // ��ų ��ġ ����
    public float Timing; // ��ų �������� ���� Ÿ�̹�

    [TextArea]
    public string SkillDesc;

    [Header(" # Skill icon")]
    public Sprite icon;
}
