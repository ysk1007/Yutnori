using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using static Unit;

public class SynergyManager : MonoBehaviour
{
    [Header("0 전사 1 궁수 2 도사 3 암살자 4 힐러 5 상인")]
    public int[] _p1AttackTypeSynergyList = new int[0];
    public int[] _p2AttackTypeSynergyList = new int[0];

    // 아티팩트로 얻는 추가 시너지
    public int[] _AttackTypeAddArtifact = new int[0];

    [Header("0 인간 1 요괴 2 위인 3 악귀 4 사신수")]
    public int[] _p1UnitTypeSynergyList = new int[0];
    public int[] _p2UnitTypeSynergyList = new int[0];

    // 아티팩트로 얻는 추가 시너지
    public int[] _UnitTypeAddArtifact = new int[0];

    public HashSet<UnitData> _p1UnitDataList = new HashSet<UnitData>();
    public HashSet<UnitData> _p2UnitDataList = new HashSet<UnitData>();

    public float[] _synergyBuffHP;
    public float[] _synergyBuffAT;
    public float[] _synergyBuffAS;
    public float[] _synergyBuffCT;
    public float[] _synergyBuffRC;

    public Color[] _synergyColorList;

    int AttackType;
    int UnitType;

    Unit_Manager _unit_Manager;

    void Awake()
    {
        SoonsoonData.Instance.Synergy_Manager = this;

        AttackType = Enum.GetValues(typeof(Unit.AttackType)).Length; // 타입의 개수를 가져와 배열의 길이로 지정
        _p1AttackTypeSynergyList = new int[AttackType]; 
        _p2AttackTypeSynergyList = new int[AttackType];
        _AttackTypeAddArtifact = new int[AttackType];

        UnitType = Enum.GetValues(typeof(Unit.UnitType)).Length; // 타입의 개수를 가져와 배열의 길이로 지정
        _p1UnitTypeSynergyList = new int[UnitType];
        _p2UnitTypeSynergyList = new int[UnitType];
        _UnitTypeAddArtifact = new int[UnitType];
    }

    // Start is called before the first frame update
    void Start()
    {
        _unit_Manager = SoonsoonData.Instance.Unit_Manager;
        CheckSynergy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckSynergy()
    {
        _unit_Manager = SoonsoonData.Instance.Unit_Manager;

        _p1AttackTypeSynergyList = new int[AttackType]; // 타입의 개수를 가져와 배열의 길이로 지정
        _p2AttackTypeSynergyList = new int[AttackType]; // 타입의 개수를 가져와 배열의 길이로 지정
        _p1UnitTypeSynergyList = new int[UnitType]; // 타입의 개수를 가져와 배열의 길이로 지정
        _p2UnitTypeSynergyList = new int[UnitType]; // 타입의 개수를 가져와 배열의 길이로 지정

        _p1UnitDataList = new HashSet<UnitData>();
        _p2UnitDataList = new HashSet<UnitData>();

        for (int i = 0; i < _unit_Manager._p1UnitList.Count; i++)
        {
            _p1UnitDataList.Add(_unit_Manager._p1UnitList[i]._unitData);
        }

        for (int i = 0; i < _unit_Manager._p2UnitList.Count; i++)
        {
            _p2UnitDataList.Add(_unit_Manager._p2UnitList[i]._unitData);
        }

        foreach (UnitData unitData in _p1UnitDataList)
        {
            switch (unitData.AttackType)
            {
                case Unit.AttackType.Warrior:
                    _p1AttackTypeSynergyList[0]++;
                    break;
                case Unit.AttackType.Archer:
                    _p1AttackTypeSynergyList[1]++;
                    break;
                case Unit.AttackType.Wizard:
                    _p1AttackTypeSynergyList[2]++;
                    break;
                case Unit.AttackType.Assassin:
                    _p1AttackTypeSynergyList[3]++;
                    break;
                case Unit.AttackType.Healer:
                    _p1AttackTypeSynergyList[4]++;
                    break;
                case Unit.AttackType.Merchant:
                    _p1AttackTypeSynergyList[5]++;
                    break;
                case Unit.AttackType.Object:
                    break;
                default:
                    break;
            }

            switch (unitData.UnitType)
            {
                case Unit.UnitType.Human:
                    _p1UnitTypeSynergyList[0]++;
                    break;
                case Unit.UnitType.Ghost:
                    _p1UnitTypeSynergyList[1]++;
                    break;
                case Unit.UnitType.Great:
                    _p1UnitTypeSynergyList[2]++;
                    break;
                case Unit.UnitType.Devil:
                    _p1UnitTypeSynergyList[3]++;
                    break;
                case Unit.UnitType.God:
                    _p1UnitTypeSynergyList[4]++;
                    break;
                default:
                    break;
            }
        }

        foreach (UnitData unitData in _p2UnitDataList)
        {
            switch (unitData.AttackType)
            {
                case Unit.AttackType.Warrior:
                    _p2AttackTypeSynergyList[0]++;
                    break;
                case Unit.AttackType.Archer:
                    _p2AttackTypeSynergyList[1]++;
                    break;
                case Unit.AttackType.Wizard:
                    _p2AttackTypeSynergyList[2]++;
                    break;
                case Unit.AttackType.Assassin:
                    _p2AttackTypeSynergyList[3]++;
                    break;
                case Unit.AttackType.Healer:
                    _p2AttackTypeSynergyList[4]++;
                    break;
                case Unit.AttackType.Merchant:
                    _p2AttackTypeSynergyList[5]++;
                    break;
                case Unit.AttackType.Object:
                    break;
                default:
                    break;
            }

            switch (unitData.UnitType)
            {
                case Unit.UnitType.Human:
                    _p2UnitTypeSynergyList[0]++;
                    break;
                case Unit.UnitType.Ghost:
                    _p2UnitTypeSynergyList[1]++;
                    break;
                case Unit.UnitType.Great:
                    _p2UnitTypeSynergyList[2]++;
                    break;
                case Unit.UnitType.Devil:
                    _p2UnitTypeSynergyList[3]++;
                    break;
                case Unit.UnitType.God:
                    _p2UnitTypeSynergyList[4]++;
                    break;
                default:
                    break;
            }
        }

        /*for (int i = 0; i < _p1UnitList.Count; i++)
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
        }*/
    }
}
