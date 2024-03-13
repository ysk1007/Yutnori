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
        SoloBuff, // 1�� ����
        TargetBuff, // ���� ����
        SquadBuff, // �Ʊ� ��ü ����
        LeastHeal, // ü���� ���� ���� �Ʊ� ȸ��
        SquadHeal // �Ʊ� ��ü ȸ��
    }

    [Header(" # Main Info")]
    public int SkillID;
    public string SkillName;
    public SkillType skillType;

    [Header(" # Status Info")]
    public float Damage; // ���ط�
    public float Speed; // �ӵ�
    public float xRange; // x�� ��Ÿ�
    public float yRange; // y�� ��Ÿ�
    public float Duration; // ���� �ð�
    public float DamageInterval; // ������ ����

    [TextArea]
    public string SkillDesc;

    [Header(" # Skill icon")]
    public Sprite icon;
}
