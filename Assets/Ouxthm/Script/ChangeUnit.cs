using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeUnit : MonoBehaviour
{
    public Transform useArray;      // ��� ���� ���� ����
    public Transform readyArray;        // ��� ���� ���� ���� 
    public List<Button> readyBtns = new List<Button>();
    public List<Button> useBtns = new List<Button>();

    public GameObject selectFirst;      // ������ ���� ù ��°
    public GameObject selectSeconds;    // ������ ���� �� ��°

    public int indexFirst;
    public int indexSeconds;

    public Transform parentFirst;
    public Transform parentSeconds;
    private void Awake()
    {
        useArray = transform.GetChild(0).GetComponent<Transform>();
        readyArray = transform.GetChild(1).GetComponent<Transform>();
        UpdateUnitList();
    }

    void UpdateUnitList()       // ���� ����Ʈ ������Ʈ
    {
        for (int i = 0; i < readyArray.childCount; i++)
        {
            Button btn = readyArray.GetChild(i).GetComponent<Button>();
            readyBtns.Add(btn);
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
            int index = i;
            btn.onClick.AddListener(() =>
            {
                if (selectFirst == null)
                {
                    selectFirst = useBtns[index].gameObject;
                    indexFirst = selectFirst.transform.GetSiblingIndex();
                    parentFirst = selectFirst.transform.parent;
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

    void SwapUnit()     // ���� ��ġ ����
    {
        selectFirst.transform.SetParent(parentSeconds);
        selectFirst.transform.SetSiblingIndex(indexSeconds);

        selectSeconds.transform.SetParent(parentFirst);
        selectSeconds.transform.SetSiblingIndex(indexFirst);

        selectFirst = null;
        selectSeconds = null;
    }
}
