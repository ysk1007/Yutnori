using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    EnemyPool _enemyPool;
    EventPanel _eventPanel;
    public RectTransform player;
    public bool _isMove = false;
    public bool _isMarket = false;
    [SerializeField] private Transform players;
    public SPUM_Prefabs _playerPref;

    public Transform plates;
    public List<RectTransform> platePos;
    public List<Plate> plate;

    public Button _yutThrowBtn;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _marketVisitButton;

    public int currentIndex;
    public int nowPlateNum = 0;     // 현재 밟고 있는 길 번호
    public int nowRoadNum = 0; // 현재 진행 길

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
        _enemyPool = SoonsoonData.Instance.Enemy_Pool;
        _userInfoManager = UserInfoManager.Instance;
        yutManager = YutManager.instance;
        canvasManager = SoonsoonData.Instance.Canvas_Manager;
        _unit_Manager = SoonsoonData.Instance.Unit_Manager;
        _eventPanel = SoonsoonData.Instance.Event_Panel;

        players.GetChild(_userInfoManager.userData.SelectCharacter).gameObject.SetActive(true);
        player = players.GetChild(_userInfoManager.userData.SelectCharacter).gameObject.GetComponent<RectTransform>();
        _playerPref = player.GetComponent<SPUM_Prefabs>();

        yutManager._plateList = plate;
        yutManager.SetPlate();
        UserPosSetting();
        MarketVisit();
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
        _userInfoManager.userData.TurnCounter++;
        _userInfoManager.UserDataSave();

        yield return new WaitForSeconds(0.5f);
        _yutThrowBtn.gameObject.SetActive(true);
        PlateEvent();
    }

    public void MarketVisit()
    {
        if (currentIndex != 0) return;

        _isMarket = true;
        _playerPref.PlayAnimation(0);
        _continueButton.SetActive(true);
        _marketVisitButton.SetActive(true);
    }

    public void MarketExit()
    {
        _isMarket = false;
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
                break;
            case Plate.PlateType.Boss:
                break;
            case Plate.PlateType.Chest:
                break;
        }
    }

    public Plate.PlateType CurrentPlateType()
    {
        return plate[currentIndex]._plateType;
    }
}
