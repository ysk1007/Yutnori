using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

public class SynergyManager : MonoBehaviour
{
    [Header("0 ���� 1 �ü� 2 ���� 3 �ϻ��� 4 ���� 5 ����")]
    public int[] _p1AttackTypeSynergyList = new int[0];
    public int[] _p2AttackTypeSynergyList = new int[0];

    // ��Ƽ��Ʈ�� ��� �߰� �ó���
    public int[] _AttackTypeAddArtifact = new int[0];

    [Header("0 �ΰ� 1 �䱫 2 ���� 3 �Ǳ�")]
    public int[] _p1UnitTypeSynergyList = new int[0];
    public int[] _p2UnitTypeSynergyList = new int[0];

    // ��Ƽ��Ʈ�� ��� �߰� �ó���
    public int[] _UnitTypeAddArtifact = new int[0];

    public List<Unit> _p1UnitList = new List<Unit>();
    public List<Unit> _p2UnitList = new List<Unit>();

    public float[] _synergyBuffHP;
    public float[] _synergyBuffAT;
    public float[] _synergyBuffAS;
    public float[] _synergyBuffCT;
    public float[] _synergyBuffRC;

    public Color[] _synergyColorList;

    int AttackType;
    int UnitType;

    void Awake()
    {
        SoonsoonData.Instance.Synergy_Manager = this;

        AttackType = Enum.GetValues(typeof(Unit.AttackType)).Length; // Ÿ���� ������ ������ �迭�� ���̷� ����
        _p1AttackTypeSynergyList = new int[AttackType]; 
        _p2AttackTypeSynergyList = new int[AttackType];
        _AttackTypeAddArtifact = new int[AttackType];

        UnitType = Enum.GetValues(typeof(Unit.UnitType)).Length; // Ÿ���� ������ ������ �迭�� ���̷� ����
        _p1UnitTypeSynergyList = new int[UnitType];
        _p2UnitTypeSynergyList = new int[UnitType];
        _UnitTypeAddArtifact = new int[UnitType];
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckSynergy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckSynergy()
    {
        _p1AttackTypeSynergyList = new int[AttackType]; // Ÿ���� ������ ������ �迭�� ���̷� ����
        _p2AttackTypeSynergyList = new int[AttackType]; // Ÿ���� ������ ������ �迭�� ���̷� ����
        _p1UnitTypeSynergyList = new int[UnitType]; // Ÿ���� ������ ������ �迭�� ���̷� ����
        _p2UnitTypeSynergyList = new int[UnitType]; // Ÿ���� ������ ������ �迭�� ���̷� ����

        _p1UnitList = SoonsoonData.Instance.Unit_Manager._p1UnitList;
        _p2UnitList = SoonsoonData.Instance.Unit_Manager._p2UnitList;

        for (int i = 0; i < _p1UnitList.Count; i++)
        {
            if (_p1UnitList[i].gameObject.activeInHierarchy)
            {
                _p1AttackTypeSynergyList[_p1UnitList[i]._attackType.GetHashCode()]++;
                _p1UnitTypeSynergyList[_p1UnitList[i]._unitType.GetHashCode()]++;
            }
        }

        for (int i = 0; i < _p2UnitList.Count; i++)
        {
            if (_p2UnitList[i].gameObject.activeInHierarchy)
            {
                _p2AttackTypeSynergyList[_p2UnitList[i]._attackType.GetHashCode()]++;
                _p2UnitTypeSynergyList[_p2UnitList[i]._unitType.GetHashCode()]++;
            }
        }
    }
}
