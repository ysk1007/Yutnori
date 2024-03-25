using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeUnit : MonoBehaviour
{
    public GameObject invisible;        // 빈 슬롯
    public Button exceptionBtn;     // 사용 중인 유닛 제거 버튼
    public Transform useArray;      // 사용 중인 유닛 모음
    public Transform readyArray;        // 대기 중인 유닛 모음 
    public List<Button> readyBtns = new List<Button>();
    public List<Button> useBtns = new List<Button>();

    public GameObject selectFirst;      // 선택한 유닛 첫 번째
    public GameObject selectSeconds;    // 선택한 유닛 두 번째

    public int indexFirst;
    public int indexSeconds;

    public Transform parentFirst;
    public Transform parentSeconds;
    private void Awake()
    {
        invisible = Resources.Load<GameObject>("UIPrefabs/InvisibleSlot");
        exceptionBtn = transform.GetChild(2).GetComponent<Button>();
        useArray = transform.GetChild(0).GetComponent<Transform>();
        readyArray = transform.GetChild(1).GetComponent<Transform>();
        UpdateUnitList();
        exceptionBtn.onClick.AddListener(() =>
        {
            selectFirst.transform.SetParent(readyArray);
            selectFirst.transform.SetSiblingIndex(readyArray.childCount - 1);

            GameObject invisibleSlot = Instantiate(invisible, useArray.transform);
            invisibleSlot.transform.SetSiblingIndex(indexFirst);
            selectFirst = null;
            selectSeconds = null;
            indexFirst = -1;
            indexSeconds = -1;
            parentFirst = null;
            parentSeconds = null;   
            readyBtns.Clear();
            useBtns.Clear();
            UpdateUnitList();
            exceptionBtn.gameObject.SetActive(false);
        });
    }

    void UpdateUnitList()       // 유닛 리스트 업데이트
    {
        for (int i = 0; i < readyArray.childCount; i++)
        {
            Button btn = readyArray.GetChild(i).GetComponent<Button>();
            readyBtns.Add(btn);
            btn.onClick.RemoveAllListeners();
            int index = i; 
            btn.onClick.AddListener(() =>
            {
                if (selectFirst == null)
                {
                    selectFirst = readyBtns[index].gameObject;
                    indexFirst = selectFirst.transform.GetSiblingIndex();
                    parentFirst = selectFirst.transform.parent;
                }
                else if (selectSeconds == null)
                {
                    selectSeconds = readyBtns[index].gameObject; 
                    indexSeconds = selectSeconds.transform.GetSiblingIndex();
                    parentSeconds = selectSeconds.transform.parent;
                    if (parentSeconds.name == "ReadyArranger" && parentSeconds == parentFirst)
                    {
                        selectFirst = null;
                        selectSeconds = null;
                    }
                    else
                    {
                        SwapUnit();
                    }
                }
            });
        }
        for (int i = 0; i < useArray.childCount; i++)
        {
            Button btn = useArray.GetChild(i).GetComponent<Button>();
            useBtns.Add(btn);
            btn.onClick.RemoveAllListeners();
            int index = i;
            btn.onClick.AddListener(() =>
            {
                if (selectFirst == null)
                {
                    selectFirst = useBtns[index].gameObject;
                    indexFirst = selectFirst.transform.GetSiblingIndex();
                    parentFirst = selectFirst.transform.parent;
                    if (parentFirst.name == "UseArranger")
                    {
                        exceptionBtn.gameObject.SetActive(true);
                    }
                }
                else if (selectSeconds == null)
                {
                    selectSeconds = useBtns[index].gameObject;
                    indexSeconds = selectSeconds.transform.GetSiblingIndex();
                    parentSeconds = selectSeconds.transform.parent;
                    if (parentSeconds.name == "ReadyArranger" && parentSeconds == parentFirst)
                    {
                        selectFirst = null;
                        selectSeconds = null;
                    }
                    else
                    {
                        SwapUnit();
                    }
                }
            });
        }
        

    }

    void SwapUnit()     // 유닛 위치 변경
    {
        selectFirst.transform.SetParent(parentSeconds);
        selectFirst.transform.SetSiblingIndex(indexSeconds);

        selectSeconds.transform.SetParent(parentFirst);
        selectSeconds.transform.SetSiblingIndex(indexFirst);

        selectFirst = null;
        selectSeconds = null;
        exceptionBtn.gameObject.SetActive(true);
    }
}
