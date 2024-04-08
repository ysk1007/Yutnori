using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    GameManager gm;
    public RectTransform player;

    public RectTransform[] plate = new RectTransform[24];
    public int nowPlateNum;     // ���� ��� �ִ� ���� ��ȣ

    public int moveSpeed = 10;
    private void Awake()
    {
        player = transform.GetChild(4).GetComponent<RectTransform>();
        nowPlateNum = 0;
        for(int i = 0; i < plate.Length; i++)
        {
            plate[i] = transform.GetChild(1).GetChild(i).GetComponent< RectTransform>();
        }
    }

    void Start()
    {
        gm = GameManager.instance;
    }

    public IEnumerator PlayerMoveOnMap()    // �÷��̾� �⹰ �̵�
    {
        for (int i = 0; i < gm.nowYut; i++)
        {
            Vector2 targetPosition = plate[nowPlateNum + i].anchoredPosition; // ��ǥ ��ġ�� �����մϴ�.

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
        nowPlateNum += gm.nowYut;
    }

}
