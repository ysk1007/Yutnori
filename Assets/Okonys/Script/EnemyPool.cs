using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class EnemySquad
{
    [SerializeField] private List<SlotClass> _enemyPool;

    public List<SlotClass> GetSquad()
    {
        return _enemyPool;
    }
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

    UserInfoManager _userInfoManager;
    PlayerMove _playerMove;

    private void Awake()
    {
        SoonsoonData.Instance.Enemy_Pool = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
        _playerMove = SoonsoonData.Instance.Player_Move;
        SetGameLevel();

    }

    // Update is called once per frame
    void Update()
    {
        SetGameLevel();
    }

    public List<SlotClass> GetRandomEnemy()
    {
        SetGameLevel();
        int RandomNum;
        _userInfoManager.userData.isEnemyData = true;
        switch (_gameLevel)
        {
            case 0:
                RandomNum = Random.Range(0, _enemySquad_Lv0.Length);
                return _enemySquad_Lv0[RandomNum].GetSquad();
            case 1:
                RandomNum = Random.Range(0, _enemySquad_Lv1.Length);
                return _enemySquad_Lv1[RandomNum].GetSquad();
            case 2:
                RandomNum = Random.Range(0, _enemySquad_Lv2.Length);
                return _enemySquad_Lv2[RandomNum].GetSquad();
            case 3:
                RandomNum = Random.Range(0, _enemySquad_Lv3.Length);
                return _enemySquad_Lv3[RandomNum].GetSquad();
            case 4:
                RandomNum = Random.Range(0, _enemySquad_Lv4.Length);
                return _enemySquad_Lv4[RandomNum].GetSquad();
            default:
                RandomNum = Random.Range(0, _enemySquad_Lv4.Length);
                return _enemySquad_Lv4[RandomNum].GetSquad();
        }
    }

    public List<SlotClass> GetRandomElite()
    {
        SetGameLevel();
        int RandomNum;
        _userInfoManager.userData.isEnemyData = true;
        switch (_gameLevel)
        {
            case 0:
                RandomNum = Random.Range(0, _eliteSquad_Lv0.Length);
                return _eliteSquad_Lv0[RandomNum].GetSquad();
            case 1:
                RandomNum = Random.Range(0, _eliteSquad_Lv1.Length);
                return _eliteSquad_Lv1[RandomNum].GetSquad();
            case 2:
                RandomNum = Random.Range(0, _eliteSquad_Lv2.Length);
                return _eliteSquad_Lv2[RandomNum].GetSquad();
            case 3:
                RandomNum = Random.Range(0, _eliteSquad_Lv3.Length);
                return _eliteSquad_Lv3[RandomNum].GetSquad();
            case 4:
                RandomNum = Random.Range(0, _eliteSquad_Lv4.Length);
                return _eliteSquad_Lv4[RandomNum].GetSquad();
            default:
                RandomNum = Random.Range(0, _eliteSquad_Lv4.Length);
                return _eliteSquad_Lv4[RandomNum].GetSquad();
        }
    }

    public List<SlotClass> GetChest()
    {
        _userInfoManager.userData.isEnemyData = true;
        return _objects[0].GetSquad();
    }

    public List<SlotClass> GetBossSquad(int index)
    {
        _userInfoManager.userData.isEnemyData = true;
        return _bossSquad[index].GetSquad();
    }

    public void SetGameLevel()
    {
        _gameLevel = _userInfoManager.userData.TurnCounter / 15;
        _userInfoManager.userData.GameLevel = _gameLevel;
    }

    public int GetGameLevel()
    {
        return _gameLevel;
    }

    public void CallBoss()
    {
        _userInfoManager.userData.TurnCounter++;
        _userInfoManager.UserDataSave();

        if (_userInfoManager.userData.isBossData) return;

        int gameTurn = _userInfoManager.userData.TurnCounter;
        switch (gameTurn)
        {
            case 15:
                _playerMove.BossAppearance();
                break;
            case 30:
                _playerMove.BossAppearance();
                break;
            case 45:
                _playerMove.BossAppearance();
                break;
            case 60:
                _playerMove.BossAppearance();
                break;
        }
    }
}
