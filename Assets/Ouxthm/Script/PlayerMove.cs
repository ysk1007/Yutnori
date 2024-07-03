using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMove : MonoBehaviour
{
    List<List<int>> road = new List<List<int>> {
        new List<int>{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19},
        new List<int>{ 5, 20, 21, 22, 23, 24, 15, 16, 17, 18, 19},
        new List<int>{ 10, 25, 26, 22, 27, 28},
        new List<int>{ 22, 27, 28}
    };

    YutManager yutManager;
    CanvasManager canvasManager;
    UserInfoManager _userInfoManager;
    Unit_Manager _unit_Manager;
    UnitPool _unitPool;
    EnemyPool _enemyPool;
    EventPanel _eventPanel;
    public RectTransform player;
    public RectTransform boss;
    public bool _isMove = false;
    public bool _isMarket = false;
    [SerializeField] private Transform players;
    [SerializeField] private Transform bosses;
    public SPUM_Prefabs _playerPref;
    public SPUM_Prefabs _bossPref;

    public Transform plates;
    public List<RectTransform> platePos;
    public List<Plate> plate;

    public Button _yutThrowBtn;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _marketVisitButton;

    public int currentIndex;
    public int nowPlateNum = 0;     // 현재 밟고 있는 길 번호
    public int nowRoadNum = 0; // 현재 진행 길

    public bool bossAlive = false;
    public bool bossMeet = false;
    public int _bossNum;
    public int bossCurrentIndex;
    public int bossPlateNum = 0;     // 현재 밟고 있는 길 번호
    public int bossRoadNum = 0; // 현재 진행 길

    public int moveSpeed = 10;

    private void Awake()
    {
        SoonsoonData.Instance.Player_Move = this;
        nowPlateNum = 0;
        platePos = new List<RectTransform>(plates.childCount);
        for (int i = 0; i < plates.childCount; i++)
        {
            platePos.Add(plates.GetChild(i).GetComponent<RectTransform>());
            plate.Add(plates.GetChild(i).GetComponent<Plate>());
        }
    }

    void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
        _enemyPool = SoonsoonData.Instance.Enemy_Pool;
        _unitPool = SoonsoonData.Instance.Unit_pool;
        yutManager = YutManager.instance;
        canvasManager = SoonsoonData.Instance.Canvas_Manager;
        _unit_Manager = SoonsoonData.Instance.Unit_Manager;
        _eventPanel = SoonsoonData.Instance.Event_Panel;

        // 플레이어 이동 말 할당
        players.GetChild(_userInfoManager.userData.SelectCharacter).gameObject.SetActive(true);
        player = players.GetChild(_userInfoManager.userData.SelectCharacter).gameObject.GetComponent<RectTransform>();
        _playerPref = player.GetComponent<SPUM_Prefabs>();

        yutManager._plateList = plate;
        yutManager.SetPlate();

        canvasManager._tutorialHand.ThrowGuide();

        UserPosSetting();
        // 보스 데이터가 있으면 보스 말 출현
        BossPosSetting();

        MarketVisit();

        BossPlayerMeet();

        // 전투를 마치고 종료하지 않았다면 즉시 전투 이어하기
        BattleContinue();

        // 이벤트를 마치고 종료하지 않았다면 즉시 이벤트 이어하기
        EventContinue();

        // 현재 발판 타입 확인
        CurrentPlateType();

        if (!_userInfoManager.userData.isCounted)
        {
            _userInfoManager.userData.isCounted = true;
            _enemyPool.CallBoss();
        }
    }

    private void Update()
    {

    }

    void UserPosSetting()
    {
        nowPlateNum = _userInfoManager.userData.CurrentPlateNum;
        nowRoadNum = _userInfoManager.userData.CurrentRoadNum;
        RouteFind();
        Vector2 targetPosition = platePos[currentIndex].anchoredPosition; // 목표 위치를 설정합니다.
        player.anchoredPosition = targetPosition;
    }

    public void BossAppearance()
    {
        // 보스 데이터가 있으면 보스 말 출현
        if (_userInfoManager.userData.isBossData)
        {
            BossPosSetting();
        }
        // 없다면 보스 생성
        else
        {
            _enemyPool.SetGameLevel();
            int GameLevel = _enemyPool.GetGameLevel();
            Debug.Log(GameLevel);

            switch (GameLevel)
            {
                case 0:
                    return;
                case 1:
                    _bossNum = Random.Range(0, 2);
                    break;
                case 2:
                    _bossNum = Random.Range(2, 4);
                    break;
                case 3:
                    _bossNum = Random.Range(4, 6);
                    break;
                case 4:
                    _bossNum = 6;
                    break;
            }

            canvasManager.BossCall(_unitPool._bossDatas[_bossNum]);
            _userInfoManager.userData.isBossData = true;
            _userInfoManager.userData.bossNum = _bossNum;
            _userInfoManager.userData.bossCurrentPlateNum = 0;
            _userInfoManager.userData.bossCurrentRoadNum = 3;
            _userInfoManager.UserDataSave();

            BossPosSetting();
        }
    }

    void BossPosSetting()
    {
        if (!_userInfoManager.userData.isBossData)
            return;

        bossAlive = true;
        bossMeet = false;

        int bossNum = _userInfoManager.userData.bossNum;
        _bossNum = bossNum;

        // 보스 이동 말 할당
        bosses.GetChild(bossNum).gameObject.SetActive(true);
        boss = bosses.GetChild(bossNum).gameObject.GetComponent<RectTransform>();
        _bossPref = boss.GetComponent<SPUM_Prefabs>();

        // 데이터 불러오기
        bossPlateNum = _userInfoManager.userData.bossCurrentPlateNum;
        bossRoadNum = _userInfoManager.userData.bossCurrentRoadNum;

        BossRouteFind();
        Vector2 targetPosition = platePos[bossCurrentIndex].anchoredPosition; // 목표 위치를 설정합니다.
        boss.anchoredPosition = targetPosition;
    }

    public void BossDead()
    {
        // 보스 이동 말 할당
        bosses.GetChild(_bossNum).gameObject.SetActive(false);
        boss = null;
        _bossPref = null;

        _bossNum = 0;
        bossAlive = false;
        bossMeet = false;
        bossPlateNum = 0;
        bossRoadNum = 0;

        _userInfoManager.userData.isBossData = false;
        _userInfoManager.userData.bossNum = _bossNum;
        _userInfoManager.userData.bossCurrentPlateNum = bossPlateNum;
        _userInfoManager.userData.bossCurrentRoadNum = bossRoadNum;
        _userInfoManager.UserDataSave();
    }

    public IEnumerator Move()
    {
        _playerPref.PlayAnimation(1);
        // 이동 거리만큼 반복
        for (int i = 0; i < yutManager._moveDistance; i++)
        {
            nowPlateNum++;

            if (road[nowRoadNum].Count - 1 < nowPlateNum)
            {
                nowRoadNum = 0;
                nowPlateNum = 0;
            }

            // 현재 경로의 다음 칸 인덱스를 가져옴
            currentIndex = road[nowRoadNum][nowPlateNum];

            Vector2 targetPosition = platePos[currentIndex].anchoredPosition; // 목표 위치를 설정합니다.
            Vector2 direction = (targetPosition - player.anchoredPosition).normalized;

            _playerPref._anim.transform.localScale = (direction.x >= 0) ? new Vector3(-1, 1, 1) : Vector3.one;

            while (Vector2.Distance(player.anchoredPosition, targetPosition) > 0.1f)
            {
                player.anchoredPosition = Vector2.MoveTowards(player.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(player.anchoredPosition, targetPosition) <= 0.1f)
                {
                    // 거의 도달했을 때, 플레이어의 위치를 목표 위치로 명시적으로 설정
                    player.anchoredPosition = targetPosition;
                }
                yield return null;
            }

            if (currentIndex == 0)
            {
                // 상점 초기화
                canvasManager._shop.ResetShop();

                MarketVisit();
                while (_isMarket)
                {
                    yield return null;
                }
                _playerPref.PlayAnimation(1);
            }
        }
        _playerPref.PlayAnimation(0);
        // 길 탐색
        RouteFind();

        // 현재 발판 번호 저장
        _userInfoManager.userData.CurrentPlateNum = nowPlateNum;
        _userInfoManager.userData.CurrentRoadNum = nowRoadNum;

        yield return new WaitForSeconds(0.5f);

        if (bossAlive)
        {
            StartCoroutine(BossMove());
        }
        else
        {
            _yutThrowBtn.gameObject.SetActive(true);
            PlateEvent();
        }
    }

    public IEnumerator BossMove()
    {
        _bossPref.PlayAnimation(1);
        // 이동 거리만큼 반복
        for (int i = 0; i < yutManager._moveDistance * 2; i++)
        {
            bossPlateNum++;

            if (road[bossRoadNum].Count - 1 < bossPlateNum)
            {
                bossRoadNum = 0;
                bossPlateNum = 0;
            }

            // 현재 경로의 다음 칸 인덱스를 가져옴
            bossCurrentIndex = road[bossRoadNum][bossPlateNum];

            Vector2 targetPosition = platePos[bossCurrentIndex].anchoredPosition; // 목표 위치를 설정합니다.
            Vector2 direction = (targetPosition - boss.anchoredPosition).normalized;

            _bossPref._anim.transform.localScale = (direction.x >= 0) ? new Vector3(-1, 1, 1) : Vector3.one;

            while (Vector2.Distance(boss.anchoredPosition, targetPosition) > 0.1f)
            {
                boss.anchoredPosition = Vector2.MoveTowards(boss.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(boss.anchoredPosition, targetPosition) <= 0.1f)
                {
                    // 거의 도달했을 때, 플레이어의 위치를 목표 위치로 명시적으로 설정
                    boss.anchoredPosition = targetPosition;
                }
                yield return null;
            }

            // 보스와 조우 했는지 확인
            if (BossPlayerMeet())
            {
                bossMeet = true;
                break;
            }

        }
        _bossPref.PlayAnimation(0);
        // 길 탐색
        BossRouteFind();

        // 현재 발판 번호 저장
        _userInfoManager.userData.bossCurrentPlateNum = bossPlateNum;
        _userInfoManager.userData.bossCurrentRoadNum = bossRoadNum;
        _userInfoManager.UserDataSave();

        yield return new WaitForSeconds(0.5f);

        if (bossMeet)
        {
            canvasManager.ShowUi();
            _unit_Manager._p2unitID = _enemyPool.GetBossSquad(_bossNum);
            _unit_Manager.FieldReset();
            _yutThrowBtn.gameObject.SetActive(true);
        }
        else
        {
            _yutThrowBtn.gameObject.SetActive(true);
            PlateEvent();
        }
    }

    public void MarketVisit()
    {
        if (currentIndex != 0) return;

        _isMarket = true;
        _playerPref.PlayAnimation(0);
        Invoke("PlateEvent", 0.2f);
    }

    public void MarketExit()
    {
        _isMarket = false;
    }

    // 마지막 전투를 마치지 않았을 때
    public void BattleContinue()
    {
        if (!_userInfoManager.userData.isEnemyData)
            return;

        canvasManager.ShowUi();
        _unit_Manager.FieldReset();
    }

    // 마지막 이벤트를 마치지 않았을 때
    public void EventContinue()
    {
        if (!_userInfoManager.userData.isEventData)
            return;

        canvasManager.FadeImage();
        _eventPanel.CallEvent(_userInfoManager.userData.EventNum);
    }

    // 보스와 조우 했는지 확인
    public bool BossPlayerMeet()
    {
        return (currentIndex == bossCurrentIndex) ? true : false;
    }

    void RouteFind() // 진행 경로 찾기
    {
        // 현재 경로의 현재 칸 인덱스를 가져옴
        currentIndex = road[nowRoadNum][nowPlateNum];
        switch (currentIndex)
        {
            case 5:
                nowRoadNum = 1;
                nowPlateNum = 0;
                break;

            case 10:
                nowRoadNum = 2;
                nowPlateNum = 0;
                break;

            case 22:
                nowRoadNum = 3;
                nowPlateNum = 0;
                break;
        }
    }

    void BossRouteFind() // 진행 경로 찾기
    {
        // 현재 경로의 현재 칸 인덱스를 가져옴
        bossCurrentIndex = road[bossRoadNum][bossPlateNum];
        switch (bossCurrentIndex)
        {
            case 5:
                bossRoadNum = 1;
                bossPlateNum = 0;
                break;

            case 10:
                bossRoadNum = 2;
                bossPlateNum = 0;
                break;

            case 22:
                bossRoadNum = 3;
                bossPlateNum = 0;
                break;
        }
    }

    void PlateEvent()
    {
        switch (plate[currentIndex]._plateType)
        {
            case Plate.PlateType.Enemy:
                canvasManager.ShowUi();
                _unit_Manager._p2unitID = _enemyPool.GetRandomEnemy();
                _unit_Manager.FieldReset();
                break;
            case Plate.PlateType.Random:
                canvasManager.FadeImage();
                _eventPanel.RandomEvent();
                break;
            case Plate.PlateType.Home:
                _continueButton.SetActive(true);
                _marketVisitButton.SetActive(true);
                canvasManager._tutorialHand.ShopGuide();
                break;
            case Plate.PlateType.Elite:
                canvasManager.ShowUi();
                _unit_Manager._p2unitID = _enemyPool.GetRandomElite();
                _unit_Manager.FieldReset();
                break;
            case Plate.PlateType.Chest:
                canvasManager.ShowUi();
                _unit_Manager._p2unitID = _enemyPool.GetChest();
                _unit_Manager.FieldReset();
                break;
        }
    }

    public Plate.PlateType CurrentPlateType()
    {
        return plate[currentIndex]._plateType;
    }
}
