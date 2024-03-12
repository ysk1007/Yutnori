using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Manager : MonoBehaviour
{
    public float _findTimer;

    public List<Transform> _unitPool = new List<Transform>();

    public List<Unit> _p1UnitList = new List<Unit>();

    public List<Unit> _p2UnitList = new List<Unit>();

    void Awake()
    {
        SoonsoonData.Instance.Unit_Manager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUnitList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //유닛 정보 연결
    void SetUnitList()
    {
        _p1UnitList.Clear();
        _p2UnitList.Clear();

        for (int i = 0; i < _unitPool.Count; i++)
        {
            for (int j = 0; j < _unitPool[i].childCount; j++)
            {
                switch (i)
                {
                    case 0:
                        _p1UnitList.Add(_unitPool[i].GetChild(j).GetComponent<Unit>());
                        _unitPool[i].GetChild(j).gameObject.tag = "P1";
                        break;
                    case 1:
                        _p2UnitList.Add(_unitPool[i].GetChild(j).GetComponent<Unit>());
                        _unitPool[i].GetChild(j).gameObject.tag = "P2";
                        break;
                }
            }
        }
    }

    public Unit GetTraget(Unit unit)
    {
        Unit tUnit = null;

        List<Unit> tList = new List<Unit>();
        switch (unit.tag) // 유닛의 태그에 따라
        {
            case "P1": tList = _p2UnitList; break; // 타겟 리스트를 반대 태그의 리스트로 할당
            case "P2": tList = _p1UnitList; break;
        }

        float tSDis = 999999;

        /*int num = 0;
        if (unit._attackType == Unit.AttackType.Assassin)
            num = 1;*/

        for (int i = 0; i < tList.Count; i++)
        {
            float tDis = ((Vector2)tList[i].transform.localPosition - (Vector2)unit.transform.localPosition).sqrMagnitude; // sqrMagnitude 루트 처리가 되지 않은 거리를 찾는것은 연산이 가볍다
            if (tDis <= unit._unitFR * unit._unitFR) // 유닛의 서칭 범위를 제곱하여
            {
                if (tList[i].gameObject.activeInHierarchy) // 하이어라키 창에서 오브젝트 active 가 true 인가
                {
                    if (tList[i]._unitState != Unit.UnitState.death) // 유닛이 죽은 상태가 아니면
                    {
                        if (tDis < tSDis) // 범위 안에 들어온 오브젝트들 중에서 가장 가까운 거리의 오브젝트를 타겟으로 설정
                        {
                            tUnit = tList[i];
                            tSDis = tDis;
                        }
                    }
                }
            }
        }
        return tUnit;
    }

    // 체력이 가장 적은 아군 찾기
    public Unit GetLeastTeam(Unit unit)
    {
        Unit tUnit = null;

        List<Unit> tList = new List<Unit>();
        switch (unit.tag) // 유닛의 태그에 따라
        {
            case "P1": tList = _p1UnitList; break; // 타겟 리스트 같은 태그의 리스트로 할당
            case "P2": tList = _p2UnitList; break;
        }

        float LeastHp = 999999;

        for (int i = 0; i < tList.Count; i++)
        {
            if (tList[i].gameObject.activeInHierarchy) // 하이어라키 창에서 오브젝트 active 가 true 인가
            {
                if (tList[i]._unitState != Unit.UnitState.death) // 유닛이 죽은 상태가 아니면
                {
                    float curHp = tList[i].GetComponent<Unit>()._unitHp / tList[i].GetComponent<Unit>()._unitMaxHp;
                    if (curHp < LeastHp) // 범위 안에 들어온 오브젝트들 중에서 가장 가까운 거리의 오브젝트를 타겟으로 설정
                    {
                        tUnit = tList[i];
                        LeastHp = curHp;
                    }
                }
            }
            
        }
        return tUnit;
    }
}
