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
        Buff, // ����
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

    [Header(" # Status Info")]
    public float Damage; // ���ط�
    public float Recovery; // ȸ����
    public float Speed; // �ӵ�
    public float Defense; // ���
    public float Shield; // ��ȣ��
    public float xRange; // x�� ��Ÿ�
    public float yRange; // y�� ��Ÿ�
    public float Duration; // ���� �ð�
    public float DamageInterval; // ������ ����

    [TextArea]
    public string SkillDesc;

    [Header(" # Skill icon")]
    public Sprite icon;
}
