using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public RectTransform player;
    public bool _isMove = false;
    public SPUM_Prefabs _playerPref;

    public Transform plates;
    public List<RectTransform> platePos;
    public List<Plate> plate;

    public Button _yutThrowBtn;

    public int currentIndex;
    public int nowPlateNum = 0;     // 현재 밟고 있는 길 번호
    public int nowRoadNum = 0; // 현재 진행 길

    public int moveSpeed = 10;
    private void Awake()
    {
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
        yutManager = YutManager.instance;
        canvasManager = SoonsoonData.Instance.Canvas_Manager;
        yutManager._plateList = plate;
        yutManager.SetPlate();
    }

    private void Update()
    {

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
        }
        _playerPref.PlayAnimation(0);
        // 길 탐색
        RouteFind();
        yield return new WaitForSeconds(0.5f);
        _yutThrowBtn.gameObject.SetActive(true);
        PlateEvent();
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
                break;
            case Plate.PlateType.Random:
                canvasManager.ShowUi();
                break;
            case Plate.PlateType.Home:
                break;
            case Plate.PlateType.Boss:
                break;
            case Plate.PlateType.Chest:
                break;
        }
    }
}
