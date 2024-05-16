using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    List<List<int>> road = new List<List<int>> {
        new List<int>{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19},
        new List<int>{ 5, 20, 21, 22, 23, 24, 15, 16, 17, 18, 19},
        new List<int>{ 10, 25, 26, 22, 28, 29},
        new List<int>{ 22, 28, 29}
    };

    YutManager yutManager;
    public RectTransform player;

    public Transform plates;
    public List<RectTransform> plate;

    public int currentIndex;
    public int nowPlateNum = 0;     // 현재 밟고 있는 길 번호
    public int nowRoadNum = 0; // 현재 진행 길

    public int moveSpeed = 10;
    private void Awake()
    {
        nowPlateNum = 0;
        plate = new List<RectTransform>(plates.childCount);
        for (int i = 0; i < plates.childCount; i++)
        {
            plate.Add(plates.GetChild(i).GetComponent<RectTransform>());
        }
    }

    void Start()
    {
        yutManager = YutManager.instance;
    }

    public IEnumerator PlayerMoveOnMap()    // 플레이어 기물 이동
    {
        // 길 탐색
        RouteFind();

        // 이동 거리만큼 반복
        for (int i = 0; i < yutManager._moveDistance; i++)
        {
            // 현재 경로의 현재 칸 인덱스를 가져옴
            int currentIndex = road[nowRoadNum][nowPlateNum];

            // 
            Vector2 targetPosition = plate[currentIndex + i].anchoredPosition; // 목표 위치를 설정합니다.
            nowPlateNum++;
            while (Vector2.Distance(player.anchoredPosition, targetPosition) > 0.1f)
            {
                player.anchoredPosition = Vector2.Lerp(player.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(player.anchoredPosition, targetPosition) <= 0.1f)
                {
                    // 거의 도달했을 때, 플레이어의 위치를 목표 위치로 명시적으로 설정
                    player.anchoredPosition = targetPosition;
                }
                yield return null;
            }
            yield return null;
        }
    }

    public void Move()
    {
        // 이동 거리만큼 반복
        for (int i = 1; i < yutManager._moveDistance + 1; i++)
        {
            nowPlateNum += i;
            // 현재 경로의 다음 칸 인덱스를 가져옴
            currentIndex = road[nowRoadNum][nowPlateNum];

            Vector2 targetPosition = plate[currentIndex].anchoredPosition; // 목표 위치를 설정합니다.
            player.anchoredPosition = targetPosition;
        }
        // 길 탐색
        RouteFind();
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
}
