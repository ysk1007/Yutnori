using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlate : MonoBehaviour
{
    public GameObject[] plate = new GameObject[27];
    GameObject battlePlate;
    GameObject randomPlayte;
    private void Awake()
    {
        for(int i = 0; i < plate.Length; i++)
        {
            plate[i] = transform.GetChild(i).gameObject;
        }
        RnadomDeployment();
    }

    public void RnadomDeployment() // 플레이트 랜덤 배치
    {
        Object[] randPlate = Resources.LoadAll("Plate/", typeof(GameObject));
        for(int i = 0; i < plate.Length; i++)
        {
            if(randPlate.Length > 0 && i != 4 && i != 9 && i != 14)
            {
                int randomIndex = Random.Range(0, randPlate.Length);
                GameObject randomPrefab = (GameObject)randPlate[randomIndex];
                GameObject instance = Instantiate(randomPrefab, Vector3.zero, Quaternion.Euler(30,0,0), plate[i].transform);

                instance.transform.SetParent(plate[i].transform, false);

                // RectTransform을 가져옵니다.
                RectTransform rectTransform = instance.GetComponent<RectTransform>();

                // anchoredPosition을 (0,0)으로 설정합니다.
                rectTransform.anchoredPosition = Vector2.zero;
            }
        }
    }
}
