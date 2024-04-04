using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit_Manager : MonoBehaviour
{
    public bool _gamePause = true;

    public float _findTimer;

    public List<Transform> _unitPool = new List<Transform>();

    public List<Unit> _p1UnitList = new List<Unit>();
    public List<Unit> _p2UnitList = new List<Unit>();

    public List<GameObject> _unitSynergy = new List<GameObject>();

    public Vector2[] _p1fieldPos;
    public Vector2[] _p2fieldPos;

    UnitDeploy _p1unitDeploy;
    UnitDeploy _p2unitDeploy;

    public List<int> _p1unitID = new List<int>(); // ���� ���� �Ŵ����� �̵�
    public List<int> _p2unitID = new List<int>();

    void Awake()
    {
        SoonsoonData.Instance.Unit_Manager = this;
        _p1unitDeploy = _unitPool[0].GetComponent<UnitDeploy>();
        _p2unitDeploy = _unitPool[1].GetComponent<UnitDeploy>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //���� ���� ����
    public void SetUnitList(string tag)
    {
        switch (tag)
        {
            case "P1":
                _p1UnitList = new List<Unit>();
                for (int i = 0; i < _unitPool[0].childCount; i++)
                {
                    _p1UnitList.Add(_unitPool[0].GetChild(i).GetComponent<Unit>());
                    _unitPool[0].GetChild(i).gameObject.tag = "P1";
                }
                break;
            case "P2":
                _p2UnitList = new List<Unit>();
                for (int i = 0; i < _unitPool[1].childCount; i++)
                {
                    _p2UnitList.Add(_unitPool[1].GetChild(i).GetComponent<Unit>());
                    _unitPool[1].GetChild(i).gameObject.tag = "P2";
                }
                break;
        }
    }

    // ���� ����� Ÿ�� ã��
    public Unit GetTraget(Unit unit)
    {
        Unit tUnit = null;

        List<Unit> tList = new List<Unit>();
        switch (unit.tag) // ������ �±׿� ����
        {
            case "P1": tList = _p2UnitList; break; // Ÿ�� ����Ʈ�� �ݴ� �±��� ����Ʈ�� �Ҵ�
            case "P2": tList = _p1UnitList; break;
        }

        float tSDis = 999999;

        for (int i = 0; i < tList.Count; i++)
        {
            float tDis = ((Vector2)tList[i].transform.localPosition - (Vector2)unit.transform.localPosition).sqrMagnitude; // sqrMagnitude ��Ʈ ó���� ���� ���� �Ÿ��� ã�°��� ������ ������
            if (tDis <= unit._unitFR * unit._unitFR) // ������ ��Ī ������ �����Ͽ�
            {
                if (tList[i].gameObject.activeInHierarchy) // ���̾��Ű â���� ������Ʈ active �� true �ΰ�
                {
                    if (tList[i]._unitState != Unit.UnitState.death) // ������ ���� ���°� �ƴϸ�
                    {
                        if (tDis < tSDis) // ���� �ȿ� ���� ������Ʈ�� �߿��� ���� ����� �Ÿ��� ������Ʈ�� Ÿ������ ����
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

    // ���� �� Ÿ�� ã��
    public Unit GetFarAwayTraget(Unit unit)
    {
        Unit tUnit = null;

        List<Unit> tList = new List<Unit>();
        switch (unit.tag) // ������ �±׿� ����
        {
            case "P1": tList = _p2UnitList; break; // Ÿ�� ����Ʈ�� �ݴ� �±��� ����Ʈ�� �Ҵ�
            case "P2": tList = _p1UnitList; break;
        }

        float tSDis = 0;

        for (int i = 0; i < tList.Count; i++)
        {
            float tDis = ((Vector2)tList[i].transform.localPosition - (Vector2)unit.transform.localPosition).sqrMagnitude; // sqrMagnitude ��Ʈ ó���� ���� ���� �Ÿ��� ã�°��� ������ ������
            if (tDis <= unit._unitFR * unit._unitFR) // ������ ��Ī ������ �����Ͽ�
            {
                if (tList[i].gameObject.activeInHierarchy) // ���̾��Ű â���� ������Ʈ active �� true �ΰ�
                {
                    if (tList[i]._unitState != Unit.UnitState.death) // ������ ���� ���°� �ƴϸ�
                    {
                        if (tDis > tSDis) // ���� �ȿ� ���� ������Ʈ�� �߿��� ���� ����� �Ÿ��� ������Ʈ�� Ÿ������ ����
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

    // �ִ� ü���� ���� ���� Ÿ�� ã��
    public Unit GetGreatestHpTarget(Unit unit, bool Enemy)
    {
        Unit tUnit = null;

        List<Unit> tList = new List<Unit>();
        switch (unit.tag) // ������ �±׿� ����
        {
            case "P1": tList = Enemy ? _p2UnitList : _p1UnitList; break; // Ÿ�� ����Ʈ ���� �±��� ����Ʈ�� �Ҵ�
            case "P2": tList = Enemy ? _p1UnitList : _p2UnitList; break;
        }

        float GreatestHp = 0;

        for (int i = 0; i < tList.Count; i++)
        {
            if (tList[i].gameObject.activeInHierarchy) // ���̾��Ű â���� ������Ʈ active �� true �ΰ�
            {
                if (tList[i]._unitState != Unit.UnitState.death) // ������ ���� ���°� �ƴϸ�
                {
                    float curHp = tList[i].GetComponent<Unit>()._unitMaxHp;
                    if (curHp > GreatestHp) // ���� �ȿ� ���� ������Ʈ�� �߿��� ���� ����� �Ÿ��� ������Ʈ�� Ÿ������ ����
                    {
                        tUnit = tList[i];
                        GreatestHp = curHp;
                    }
                }
            }

        }
        return tUnit;
    }

    // ü�� ������ ���� ���� �Ʊ� ã��
    public Unit GetLeastTeam(Unit unit)
    {
        Unit tUnit = null;

        List<Unit> tList = new List<Unit>();
        switch (unit.tag) // ������ �±׿� ����
        {
            case "P1": tList = _p1UnitList; break; // Ÿ�� ����Ʈ ���� �±��� ����Ʈ�� �Ҵ�
            case "P2": tList = _p2UnitList; break;
        }

        float LeastHp = 999999;

        for (int i = 0; i < tList.Count; i++)
        {
            if (tList[i].gameObject.activeInHierarchy) // ���̾��Ű â���� ������Ʈ active �� true �ΰ�
            {
                if (tList[i]._unitState != Unit.UnitState.death) // ������ ���� ���°� �ƴϸ�
                {
                    float curHp = tList[i].GetComponent<Unit>()._unitHp / tList[i].GetComponent<Unit>()._unitMaxHp;
                    if (curHp < LeastHp) // ���� �ȿ� ���� ������Ʈ�� �߿��� ���� ����� �Ÿ��� ������Ʈ�� Ÿ������ ����
                    {
                        tUnit = tList[i];
                        LeastHp = curHp;
                    }
                }
            }
            
        }
        return tUnit;
    }

    // ü�� ���� ���� ���� ã��
    public Unit GetLeastEnemy(Unit unit)
    {
        Unit tUnit = null;

        List<Unit> tList = new List<Unit>();
        switch (unit.tag) // ������ �±׿� ����
        {
            case "P1": tList = _p2UnitList; break; // Ÿ�� ����Ʈ ���� �±��� ����Ʈ�� �Ҵ�
            case "P2": tList = _p1UnitList; break;
        }

        float LeastHp = 999999;

        for (int i = 0; i < tList.Count; i++)
        {
            if (tList[i].gameObject.activeInHierarchy) // ���̾��Ű â���� ������Ʈ active �� true �ΰ�
            {
                if (tList[i]._unitState != Unit.UnitState.death) // ������ ���� ���°� �ƴϸ�
                {
                    float curHp = tList[i].GetComponent<Unit>()._unitHp;
                    if (curHp < LeastHp) // ���� �ȿ� ���� ������Ʈ�� �߿��� ���� ����� �Ÿ��� ������Ʈ�� Ÿ������ ����
                    {
                        tUnit = tList[i];
                        LeastHp = curHp;
                    }
                }
            }

        }
        return tUnit;
    }

    // �� ã��
    public List<Unit> GetSquadTeam(Unit unit, bool Enemy)
    {
        List<Unit> returnList = new List<Unit>();
        List<Unit> tList = new List<Unit>();
        switch (unit.tag) // ������ �±׿� ����
        {
            case "P1": tList = Enemy ? _p2UnitList : _p1UnitList; break; // Ÿ�� ����Ʈ ���� �±��� ����Ʈ�� �Ҵ�
            case "P2": tList = Enemy ? _p1UnitList : _p2UnitList; break;
        }      

        for (int i = 0; i < tList.Count; i++)
        {
            if (tList[i].gameObject.activeInHierarchy) // ���̾��Ű â���� ������Ʈ active �� true �ΰ�
            {
                if (tList[i]._unitState != Unit.UnitState.death) // ������ ���� ���°� �ƴϸ�
                {
                    returnList.Add(tList[i]);
                }
            }

        }
        return returnList;
    }

    public void GameResume()
    {
        _gamePause = false;
        for (int i = 0; i < _unitSynergy.Count; i++)
        {
            _unitSynergy[i].SetActive(true);
        }
        SoonsoonData.Instance.Damage_Measure.MeasureStart();
    }

    public void FieldReset()
    {
        _p1UnitList.RemoveRange(0,_p1UnitList.Count);
        _p2UnitList.RemoveRange(0, _p2UnitList.Count);
        _p1unitDeploy.UnitDeployment();
        _p2unitDeploy.UnitDeployment();

        SoonsoonData.Instance.Missile_Manager.ResetMissile();
        SoonsoonData.Instance.Effect_Manager.ResetEffect();
        SoonsoonData.Instance.Skill_Manager.ResetSkill();
        _gamePause = true;

        for (int i = 0; i < _unitSynergy.Count; i++)
        {
            _unitSynergy[i].SetActive(false);
        }

        SoonsoonData.Instance.Synergy_Manager.CheckSynergy();
        SoonsoonData.Instance.Damage_Measure.MeasureReset();
    }
}
