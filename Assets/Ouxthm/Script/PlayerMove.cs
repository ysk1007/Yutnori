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
    public int nowPlateNum = 0;     // ���� ��� �ִ� �� ��ȣ
    public int nowRoadNum = 0; // ���� ���� ��

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

    public IEnumerator PlayerMoveOnMap()    // �÷��̾� �⹰ �̵�
    {
        // �� Ž��
        RouteFind();

        // �̵� �Ÿ���ŭ �ݺ�
        for (int i = 0; i < yutManager._moveDistance; i++)
        {
            // ���� ����� ���� ĭ �ε����� ������
            int currentIndex = road[nowRoadNum][nowPlateNum];

            // 
            Vector2 targetPosition = plate[currentIndex + i].anchoredPosition; // ��ǥ ��ġ�� �����մϴ�.
            nowPlateNum++;
            while (Vector2.Distance(player.anchoredPosition, targetPosition) > 0.1f)
            {
                player.anchoredPosition = Vector2.Lerp(player.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(player.anchoredPosition, targetPosition) <= 0.1f)
                {
                    // ���� �������� ��, �÷��̾��� ��ġ�� ��ǥ ��ġ�� ��������� ����
                    player.anchoredPosition = targetPosition;
                }
                yield return null;
            }
            yield return null;
        }
    }

    public void Move()
    {
        // �̵� �Ÿ���ŭ �ݺ�
        for (int i = 1; i < yutManager._moveDistance + 1; i++)
        {
            nowPlateNum += i;
            // ���� ����� ���� ĭ �ε����� ������
            currentIndex = road[nowRoadNum][nowPlateNum];

            Vector2 targetPosition = plate[currentIndex].anchoredPosition; // ��ǥ ��ġ�� �����մϴ�.
            player.anchoredPosition = targetPosition;
        }
        // �� Ž��
        RouteFind();
    }

    void RouteFind() // ���� ��� ã��
    {
        // ���� ����� ���� ĭ �ε����� ������
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
