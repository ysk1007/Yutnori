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
    public float[] MaxHp; // ü��
    public float[] Damage; // ���ط�
    public float[] Recovery; // ȸ����
    public float[] Speed; // �ӵ�
    public float[] Defense; // ���
    public float[] Critical; // ũ��Ƽ��
    public float[] Shield; // ��ȣ��
    public float[] CoolTime; // ��ų ��Ÿ��
    public float[] Duration; // ���� �ð�
    public float[] Interval; // ����

    [TextArea]
    public string SynergyDesc;

    [Header(" # Synergy icon")]
    public Sprite icon;
}
