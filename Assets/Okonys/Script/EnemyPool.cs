using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class EnemySquad
{
    [SerializeField] private List<SlotClass> _enemyPool;

    // ������ ����Ʈ�� ��ȯ�մϴ�.
    public List<SlotClass> GetSquad() => _enemyPool;
}

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private int _gameLevel;

    [Header(" # �Ϲ��� �� ������")]
    [SerializeField] private EnemySquad[] _enemySquad_Lv0;
    [SerializeField] private EnemySquad[] _enemySquad_Lv1;
    [SerializeField] private EnemySquad[] _enemySquad_Lv2;
    [SerializeField] private EnemySquad[] _enemySquad_Lv3;
    [SerializeField] private EnemySquad[] _enemySquad_Lv4;

    [Header(" # ����Ʈ�� �� ������")]
    [SerializeField] private EnemySquad[] _eliteSquad_Lv0;
    [SerializeField] private EnemySquad[] _eliteSquad_Lv1;
    [SerializeField] private EnemySquad[] _eliteSquad_Lv2;
    [SerializeField] private EnemySquad[] _eliteSquad_Lv3;
    [SerializeField] private EnemySquad[] _eliteSquad_Lv4;

    [Header(" # ������ �� ������")]
    [SerializeField] private EnemySquad[] _bossSquad;

    [Header(" # ������Ʈ")]
    [SerializeField] private EnemySquad[] _objects;

    private UserInfoManager _userInfoManager;
    private PlayerMove _playerMove;

    private void Awake()
    {
        // �̱��� �ν��Ͻ��� �ʱ�ȭ�մϴ�.
        SoonsoonData.Instance.Enemy_Pool = this;
    }

    private void Start()
    {
        // UserInfoManager �� PlayerMove �ν��Ͻ��� �ʱ�ȭ�մϴ�.
        _userInfoManager = UserInfoManager.Instance;
        _playerMove = SoonsoonData.Instance.Player_Move;

        // ���� ������ �����մϴ�.
        SetGameLevel();
    }

    private void Update()
    {
        SetGameLevel(); 
    }

    // ������ �� �����带 ��ȯ�մϴ�.
    public List<SlotClass> GetRandomEnemy()
    {
        return GetRandomSquad(_gameLevel, _enemySquad_Lv0, _enemySquad_Lv1, _enemySquad_Lv2, _enemySquad_Lv3, _enemySquad_Lv4);
    }

    // ������ ����Ʈ �����带 ��ȯ�մϴ�.
    public List<SlotClass> GetRandomElite()
    {
        return GetRandomSquad(_gameLevel, _eliteSquad_Lv0, _eliteSquad_Lv1, _eliteSquad_Lv2, _eliteSquad_Lv3, _eliteSquad_Lv4);
    }

    // ���� ���� �����带 ��ȯ�մϴ�.
    public List<SlotClass> GetChest()
    {
        _userInfoManager.userData.isEnemyData = true;
        return _objects[0].GetSquad();
    }

    // ���� �����带 ��ȯ�մϴ�.
    public List<SlotClass> GetBossSquad(int index)
    {
        _userInfoManager.userData.isEnemyData = true;
        return _bossSquad[index].GetSquad();
    }

    // ���� ������ �����մϴ�.
    public void SetGameLevel()
    {
        // �� ī���Ϳ� ���� ���� ������ �����մϴ�.
        _gameLevel = Mathf.Clamp(_userInfoManager.userData.TurnCounter / 9, 0, 4);
        _userInfoManager.userData.GameLevel = _gameLevel;
    }

    // ���� ���� ������ ��ȯ�մϴ�.
    public int GetGameLevel() => _gameLevel;

    // Ư�� �Ͽ��� ������ ȣ���մϴ�.
    public void CallBoss()
    {
        _userInfoManager.userData.TurnCounter++;
        _userInfoManager.userData.isCounted = true;
        _userInfoManager.UserDataSave();

        // ������ �̹� ȣ���� ��� �ƹ� �͵� ���� �ʽ��ϴ�.
        if (_userInfoManager.userData.isBossData) return;

        // �Ͽ� ���� ������ ȣ���մϴ�.
        if (_userInfoManager.userData.TurnCounter % 15 == 0)
        {
            _playerMove.BossAppearance();
        }
    }

    // ������ �����带 ��ȯ�մϴ�. ������ ���� �� �����带 �����մϴ�.
    private List<SlotClass> GetRandomSquad(int level, params EnemySquad[][] squads)
    {
        // ������ ��ȿ���� ������ ���� ���� ������ �����մϴ�.
        if (level < 0 || level >= squads.Length)
        {
            level = squads.Length - 1;
        }

        int randomIndex = Random.Range(0, squads[level].Length);
        _userInfoManager.userData.isEnemyData = true;
        return squads[level][randomIndex].GetSquad();
    }
}
