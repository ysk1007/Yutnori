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

    UserInfoManager _userInfoManager;

    private void Awake()
    {
        SoonsoonData.Instance.Enemy_Pool = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
        SetGameLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<SlotClass> GetRandomEnemy()
    {
        SetGameLevel();
        int RandomNum;
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

    public List<SlotClass> GetBossSquad(int index)
    {
        return _bossSquad[index].GetSquad();
    }
    
    void SetGameLevel()
    {
        _gameLevel = _userInfoManager.userData.TurnCounter / 10;
        _userInfoManager.userData.GameLevel = _gameLevel;
        _userInfoManager.UserDataSave();
    }
}
