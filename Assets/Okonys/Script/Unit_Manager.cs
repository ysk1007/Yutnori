using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Unit_Manager : MonoBehaviour
{
    public int UserPopulation;
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

    public List<SlotClass> _p1unitID = new List<SlotClass>(); // 추후 게임 매니저로 이동
    public List<SlotClass> _p2unitID = new List<SlotClass>();
    public List<SlotClass> _userInvenUnit = new List<SlotClass>();

    UserInfoManager _userInfoManager;
    UnitPool Unit_pool;
    InventoryManager im;
    BattleReward _battleReward;
    PlayerMove _playerMove;
    CanvasManager _canvasManager;

    void Awake()
    {
        SoonsoonData.Instance.Unit_Manager = this;
        _p1unitDeploy = _unitPool[0].GetComponent<UnitDeploy>();
        _p2unitDeploy = _unitPool[1].GetComponent<UnitDeploy>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
        Unit_pool = SoonsoonData.Instance.Unit_pool;
        im = SoonsoonData.Instance.Inventory_Manager;
        _playerMove = SoonsoonData.Instance.Player_Move;
        _battleReward = SoonsoonData.Instance.Battle_Reward;
        _canvasManager = SoonsoonData.Instance.Canvas_Manager;
        UserUnitDataLoad();
    }

    // Update is called once per frame
    void Update()
    {
        UserPopulation = _p1UnitList.Count;
    }

    //유닛 정보 연결
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

    // 가장 가까운 타겟 찾기
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

    // 가장 먼 타겟 찾기
    public Unit GetFarAwayTraget(Unit unit)
    {
        Unit tUnit = null;

        List<Unit> tList = new List<Unit>();
        switch (unit.tag) // 유닛의 태그에 따라
        {
            case "P1": tList = _p2UnitList; break; // 타겟 리스트를 반대 태그의 리스트로 할당
            case "P2": tList = _p1UnitList; break;
        }

        float tSDis = 0;

        for (int i = 0; i < tList.Count; i++)
        {
            float tDis = ((Vector2)tList[i].transform.localPosition - (Vector2)unit.transform.localPosition).sqrMagnitude; // sqrMagnitude 루트 처리가 되지 않은 거리를 찾는것은 연산이 가볍다
            if (tDis <= unit._unitFR * unit._unitFR) // 유닛의 서칭 범위를 제곱하여
            {
                if (tList[i].gameObject.activeInHierarchy) // 하이어라키 창에서 오브젝트 active 가 true 인가
                {
                    if (tList[i]._unitState != Unit.UnitState.death) // 유닛이 죽은 상태가 아니면
                    {
                        if (tDis > tSDis) // 범위 안에 들어온 오브젝트들 중에서 가장 가까운 거리의 오브젝트를 타겟으로 설정
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

    // 최대 체력이 가장 높은 타겟 찾기
    public Unit GetGreatestHpTarget(Unit unit, bool Enemy)
    {
        Unit tUnit = null;

        List<Unit> tList = new List<Unit>();
        switch (unit.tag) // 유닛의 태그에 따라
        {
            case "P1": tList = Enemy ? _p2UnitList : _p1UnitList; break; // 타겟 리스트 같은 태그의 리스트로 할당
            case "P2": tList = Enemy ? _p1UnitList : _p2UnitList; break;
        }

        float GreatestHp = 0;

        for (int i = 0; i < tList.Count; i++)
        {
            if (tList[i].gameObject.activeInHierarchy) // 하이어라키 창에서 오브젝트 active 가 true 인가
            {
                if (tList[i]._unitState != Unit.UnitState.death) // 유닛이 죽은 상태가 아니면
                {
                    float curHp = tList[i].GetComponent<Unit>()._unitMaxHp;
                    if (curHp > GreatestHp) // 범위 안에 들어온 오브젝트들 중에서 가장 가까운 거리의 오브젝트를 타겟으로 설정
                    {
                        tUnit = tList[i];
                        GreatestHp = curHp;
                    }
                }
            }

        }
        return tUnit;
    }

    // 체력 비율이 가장 적은 아군 찾기
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

    // 체력 가장 적은 적군 찾기
    public Unit GetLeastEnemy(Unit unit)
    {
        Unit tUnit = null;

        List<Unit> tList = new List<Unit>();
        switch (unit.tag) // 유닛의 태그에 따라
        {
            case "P1": tList = _p2UnitList; break; // 타겟 리스트 같은 태그의 리스트로 할당
            case "P2": tList = _p1UnitList; break;
        }

        float LeastHp = 999999;

        for (int i = 0; i < tList.Count; i++)
        {
            if (tList[i].gameObject.activeInHierarchy) // 하이어라키 창에서 오브젝트 active 가 true 인가
            {
                if (tList[i]._unitState != Unit.UnitState.death) // 유닛이 죽은 상태가 아니면
                {
                    float curHp = tList[i].GetComponent<Unit>()._unitHp;
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

    // 팀 찾기
    public List<Unit> GetSquadTeam(Unit unit, bool Enemy)
    {
        List<Unit> returnList = new List<Unit>();
        List<Unit> tList = new List<Unit>();
        switch (unit.tag) // 유닛의 태그에 따라
        {
            case "P1": tList = Enemy ? _p2UnitList : _p1UnitList; break; // 타겟 리스트 같은 태그의 리스트로 할당
            case "P2": tList = Enemy ? _p1UnitList : _p2UnitList; break;
        }      

        for (int i = 0; i < tList.Count; i++)
        {
            if (tList[i].gameObject.activeInHierarchy) // 하이어라키 창에서 오브젝트 active 가 true 인가
            {
                if (tList[i]._unitState != Unit.UnitState.death) // 유닛이 죽은 상태가 아니면
                {
                    returnList.Add(tList[i]);
                }
            }

        }
        return returnList;
    }

    // 상자 체력
    public float GetChestHp()
    {

        List<Unit> tList = _p2UnitList;

        float ChestHp = 0;

        for (int i = 0; i < tList.Count; i++)
        {
            if (tList[i].gameObject.activeInHierarchy) // 하이어라키 창에서 오브젝트 active 가 true 인가
            {
                if (tList[i]._unitState != Unit.UnitState.death) // 유닛이 죽은 상태가 아니면
                {
                    ChestHp = tList[i].GetComponent<Unit>()._unitHp;
                    tList[i].GetComponent<Unit>().SetDeath();
                }
            }

        }
        return ChestHp;
    }

    // 모든 유닛이 죽었는지 확인하고 승패의 결과를 체크
    public void CheckGameResult(Unit unit)
    {
        List<Unit> tList = new List<Unit>();
        switch (unit.tag) // 유닛의 태그에 따라
        {
            case "P1": 
                tList = _p1UnitList;
                for (int i = 0; i < tList.Count; i++) // 죽은 유닛의 스쿼드를 전부 탐색하여
                {
                    if (tList[i].gameObject.activeInHierarchy) // 하이어라키 창에서 오브젝트 active 가 true 인가
                    {
                        /*if (tList[i]._unitState != Unit.UnitState.death) // 유닛이 죽은 상태가 아니면
                        {
                        }*/
                        return; // 하나라도 생존해 있다면 return
                    }
                }

                // P1의 유닛이 전부 사망
                GameLose();
                break; // 타겟 리스트 태그의 리스트로 할당

            case "P2": 
                tList = _p2UnitList;
                for (int i = 0; i < tList.Count; i++) // 죽은 유닛의 스쿼드를 전부 탐색하여
                {
                    if (tList[i].gameObject.activeInHierarchy) // 하이어라키 창에서 오브젝트 active 가 true 인가
                    {
                        /*if (tList[i]._unitState != Unit.UnitState.death) // 유닛이 죽은 상태가 아니면
                        {
                        }*/
                        return; // 하나라도 생존해 있다면 return
                    }
                }

                // P2의 유닛이 전부 사망
                GameWin();
                break;
        }

        for (int i = 0; i < tList.Count; i++) // 죽은 팀 스쿼드를 전부 탐색하여
        {
            if (tList[i].gameObject.activeInHierarchy) // 하이어라키 창에서 오브젝트 active 가 true 인가
            {
                /*if (tList[i]._unitState != Unit.UnitState.death) // 유닛이 죽은 상태가 아니면
                {
                }*/
                return;
            }
        }
    }

    public void GameWin()
    {
        _gamePause = true;
        _canvasManager.TimerStop();

        _userInfoManager.userData.isEnemyData = false;
        _userInfoManager.userData.EnemySquad = new Vector2[9];
        _userInfoManager.UserDataSave();

        // 보스전 보상
        if (_playerMove.bossMeet)
        {
            _playerMove.BossDead();
            _battleReward.NormalBattleReward();
            return;
        }

        switch (_playerMove.CurrentPlateType())
        {
            case Plate.PlateType.Enemy:
                Debug.Log("노말 적 보상");
                _battleReward.NormalBattleReward();
                break;
            case Plate.PlateType.Random:
                Debug.Log("랜덤 적 보상");
                _battleReward.NormalBattleReward();
                break;
            case Plate.PlateType.Home:
                break;
            case Plate.PlateType.Elite:
                Debug.Log("엘리트 적 보상");
                _battleReward.NormalBattleReward();
                break;
            case Plate.PlateType.Chest:
                Debug.Log("상자 보상");
                _battleReward.ChestReward(GetChestHp());
                break;
            default:
                break;
        }
    }

    public void GameLose()
    {
        _gamePause = true;
        _canvasManager.TimerStop();
        SoonsoonData.Instance.LogPopup.ShowLog("패배");
    }

    public void GameResume()
    {
        _gamePause = false;
        for (int i = 0; i < _unitSynergy.Count; i++)
        {
            _unitSynergy[i].SetActive(true);
        }
        SoonsoonData.Instance.Damage_Measure.MeasureStart();
        _canvasManager.TimerStart();
    }

    public void FieldReset()
    {
        _p1UnitList.RemoveRange(0,_p1UnitList.Count);
        _p2UnitList.RemoveRange(0, _p2UnitList.Count);
        UnitDataUpdate();
        _p1unitDeploy.UnitDeployment();
        _p2unitDeploy.UnitDeployment();

        EnemyUnitsSave();

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

        _userInfoManager.UserDataSave();
    }

    public void UnitRelocation() // p1 유닛 재배치
    {
        _p1UnitList.RemoveRange(0, _p1UnitList.Count);
        UnitDataUpdate();
        _p1unitDeploy.UnitDeployment(); 
        
        for (int i = 0; i < _unitSynergy.Count; i++)
        {
            _unitSynergy[i].SetActive(false);
        }

        SoonsoonData.Instance.UnitInventory.initInventory();
        SoonsoonData.Instance.Synergy_Manager.CheckSynergy();
        SoonsoonData.Instance.Damage_Measure.MeasureReset();
    }

    void UnitDataUpdate()
    {
        for (int i = 0; i < _p1unitID.Count; i++)
        {
            _p1unitID[i] = im._userSquad[i];
        }
    }

    void UserUnitDataLoad()
    {
        // 플레이어 유닛 로드
        for (int i = 0; i < _p1unitID.Count; i++)
        {
            if (_userInfoManager.userData.UserSquad[i].x == 0)
            {
                _p1unitID[i].Clear();
                continue;
            }

            _p1unitID[i] = new SlotClass(Unit_pool._unitDatas[(int)_userInfoManager.userData.UserSquad[i].x - 1], (int)_userInfoManager.userData.UserSquad[i].y);
        }

        // 플레이어 인벤토리 유닛 로드
        for (int i = 0; i < _userInvenUnit.Count; i++)
        {
            if (_userInfoManager.userData.UserInventory[i].x == 0)
            {
                _userInvenUnit[i].Clear();
                continue;
            }

            _userInvenUnit[i] = new SlotClass(Unit_pool._unitDatas[(int)_userInfoManager.userData.UserInventory[i].x - 1], (int)_userInfoManager.userData.UserInventory[i].y);
        }

        // 적 유닛 로드
        for (int i = 0; i < _p2unitID.Count; i++)
        {
            if (_userInfoManager.userData.EnemySquad[i].x == 0)
            {
                _p2unitID[i].Clear();
                continue;
            }

            _p2unitID[i] = ((int)_userInfoManager.userData.EnemySquad[i].x < 100) ? 
                new SlotClass(Unit_pool._unitDatas[(int)_userInfoManager.userData.EnemySquad[i].x - 1], (int)_userInfoManager.userData.EnemySquad[i].y)
                : new SlotClass(Unit_pool._objectDatas[(int)_userInfoManager.userData.EnemySquad[i].x - 101], (int)_userInfoManager.userData.EnemySquad[i].y);
        }
    }

    public void EnemyUnitsSave()
    {
        for (int i = 0; i < _p2unitID.Count; i++)
        {
            if (_p2unitID[i]?._unitData == null)
            {
                _userInfoManager.userData.EnemySquad[i] = Vector2.zero;
                continue;
            }

            int UnitNumber = _p2unitID[i].GetUnitData().UnitID;
            int UnitRate = _p2unitID[i].GetUnitRate();

            _userInfoManager.userData.EnemySquad[i] = new Vector2(UnitNumber, UnitRate);
        }
    }
}
