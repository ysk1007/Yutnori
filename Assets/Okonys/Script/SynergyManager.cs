using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyManager : MonoBehaviour
{
    [Header("0 ���� 1 �ü� 2 ���� 3 �ϻ��� 4 ���� 5 ����")]
    public int[] _p1SynergyList = new int[0];
    public int[] _p2SynergyList = new int[0];

    public List<Unit> _p1UnitList = new List<Unit>();
    public List<Unit> _p2UnitList = new List<Unit>();

    public float[] _synergyBuffHP;
    public float[] _synergyBuffAT;
    public float[] _synergyBuffAS;
    public float[] _synergyBuffCT;
    public float[] _synergyBuffRC;

    public Color[] _synergyColorList;

    void Awake()
    {
        SoonsoonData.Instance.Synergy_Manager = this;
        _p1SynergyList = new int[Enum.GetValues(typeof(Unit.AttackType)).Length]; // Ÿ���� ������ ������ �迭�� ���̷� ����
        _p2SynergyList = new int[Enum.GetValues(typeof(Unit.AttackType)).Length]; // Ÿ���� ������ ������ �迭�� ���̷� ����
    }

    // Start is called before the first frame update
    void Start()
    {
        _p1UnitList = SoonsoonData.Instance.Unit_Manager._p1UnitList;
        _p2UnitList = SoonsoonData.Instance.Unit_Manager._p2UnitList;

        for (int i = 0; i < _p1UnitList.Count; i++)
        {
            if(_p1UnitList[i].gameObject.activeInHierarchy)
                _p1SynergyList[_p1UnitList[i]._attackType.GetHashCode()]++;
        }

        for (int i = 0; i < _p2UnitList.Count; i++)
        {
            if (_p2UnitList[i].gameObject.activeInHierarchy)
                _p2SynergyList[_p2UnitList[i]._attackType.GetHashCode()]++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}