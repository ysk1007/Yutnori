using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    GameManager gm;
    public RectTransform player;

    public RectTransform[] plate = new RectTransform[24];
    public int nowPlateNum;     // 현재 밟고 있는 발판 번호
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

    public IEnumerator PlayerMoveOnMap()    // 플레이어 기물 이동
    {
        for (int i = 0; i <= gm.rand; i++)
        {
            Vector2 targetPosition = plate[nowPlateNum + i].anchoredPosition; // 목표 위치를 설정합니다.

            while (Vector2.Distance(player.anchoredPosition, targetPosition) > 0.1f)
            {
                player.anchoredPosition = Vector2.Lerp(player.anchoredPosition, targetPosition, 5f * Time.deltaTime);
                if (Vector2.Distance(player.anchoredPosition, targetPosition) <= 0.1f)
                {
                    // 거의 도달했을 때, 플레이어의 위치를 목표 위치로 명시적으로 설정
                    player.anchoredPosition = targetPosition;
                }
                yield return null;
            }
            yield return new WaitForSeconds(0.2f);
        }
        nowPlateNum += gm.rand;
    }

}
