using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class EnemySquad
{
    [SerializeField] private List<SlotClass> _enemyPool;

    // 스쿼드 리스트를 반환합니다.
    public List<SlotClass> GetSquad() => _enemyPool;
}

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private int _gameLevel;

    [Header(" # 일반전 적 스쿼드")]
    [SerializeField] private EnemySquad[] _enemySquad_Lv0;
    [SerializeField] private EnemySquad[] _enemySquad_Lv1;
    [SerializeField] private EnemySquad[] _enemySquad_Lv2;
    [SerializeField] private EnemySquad[] _enemySquad_Lv3;
    [SerializeField] private EnemySquad[] _enemySquad_Lv4;

    [Header(" # 엘리트전 적 스쿼드")]
    [SerializeField] private EnemySquad[] _eliteSquad_Lv0;
    [SerializeField] private EnemySquad[] _eliteSquad_Lv1;
    [SerializeField] private EnemySquad[] _eliteSquad_Lv2;
    [SerializeField] private EnemySquad[] _eliteSquad_Lv3;
    [SerializeField] private EnemySquad[] _eliteSquad_Lv4;

    [Header(" # 보스전 적 스쿼드")]
    [SerializeField] private EnemySquad[] _bossSquad;

    [Header(" # 오브젝트")]
    [SerializeField] private EnemySquad[] _objects;

    private UserInfoManager _userInfoManager;
    private PlayerMove _playerMove;

    private void Awake()
    {
        // 싱글톤 인스턴스를 초기화합니다.
        SoonsoonData.Instance.Enemy_Pool = this;
    }

    private void Start()
    {
        // UserInfoManager 및 PlayerMove 인스턴스를 초기화합니다.
        _userInfoManager = UserInfoManager.Instance;
        _playerMove = SoonsoonData.Instance.Player_Move;

        // 게임 레벨을 설정합니다.
        SetGameLevel();
    }

    private void Update()
    {
        SetGameLevel(); 
    }

    // 무작위 적 스쿼드를 반환합니다.
    public List<SlotClass> GetRandomEnemy()
    {
        return GetRandomSquad(_gameLevel, _enemySquad_Lv0, _enemySquad_Lv1, _enemySquad_Lv2, _enemySquad_Lv3, _enemySquad_Lv4);
    }

    // 무작위 엘리트 스쿼드를 반환합니다.
    public List<SlotClass> GetRandomElite()
    {
        return GetRandomSquad(_gameLevel, _eliteSquad_Lv0, _eliteSquad_Lv1, _eliteSquad_Lv2, _eliteSquad_Lv3, _eliteSquad_Lv4);
    }

    // 보물 상자 스쿼드를 반환합니다.
    public List<SlotClass> GetChest()
    {
        _userInfoManager.userData.isEnemyData = true;
        return _objects[0].GetSquad();
    }

    // 보스 스쿼드를 반환합니다.
    public List<SlotClass> GetBossSquad(int index)
    {
        _userInfoManager.userData.isEnemyData = true;
        return _bossSquad[index].GetSquad();
    }

    // 게임 레벨을 설정합니다.
    public void SetGameLevel()
    {
        // 턴 카운터에 따라 게임 레벨을 설정합니다.
        _gameLevel = Mathf.Clamp(_userInfoManager.userData.TurnCounter / 9, 0, 4);
        _userInfoManager.userData.GameLevel = _gameLevel;
    }

    // 현재 게임 레벨을 반환합니다.
    public int GetGameLevel() => _gameLevel;

    // 특정 턴에서 보스를 호출합니다.
    public void CallBoss()
    {
        _userInfoManager.userData.TurnCounter++;
        _userInfoManager.userData.isCounted = true;
        _userInfoManager.UserDataSave();

        // 보스를 이미 호출한 경우 아무 것도 하지 않습니다.
        if (_userInfoManager.userData.isBossData) return;

        // 턴에 따라 보스를 호출합니다.
        if (_userInfoManager.userData.TurnCounter % 15 == 0)
        {
            _playerMove.BossAppearance();
        }
    }

    // 무작위 스쿼드를 반환합니다. 레벨에 따라 적 스쿼드를 선택합니다.
    private List<SlotClass> GetRandomSquad(int level, params EnemySquad[][] squads)
    {
        // 레벨이 유효하지 않으면 가장 높은 레벨로 설정합니다.
        if (level < 0 || level >= squads.Length)
        {
            level = squads.Length - 1;
        }

        int randomIndex = Random.Range(0, squads[level].Length);
        _userInfoManager.userData.isEnemyData = true;
        return squads[level][randomIndex].GetSquad();
    }
}
