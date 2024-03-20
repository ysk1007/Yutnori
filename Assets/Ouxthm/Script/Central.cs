using System.Collections.Generic;
using UnityEngine;

public class Central : MonoBehaviour
{
    public static Central instance;

    public GameObject[] useArranger = new GameObject[3];  // 전투에 사용할 카드 모음
    public Transform invisibleCard; // 보이지 않는 카드의 위치

    public GameObject invisibleSpace;        // 카드 자리 메꾸기
    List<Arranger> arrangers;

    Arranger workingArranger;
    public static int oriIndex;     // 잡은 카드의 자식 번호
    public static int lastIndex;

    public bool create;     // 빈 공간 생성했는지 확인
    private void Awake()
    {
        instance = this;
        create = false;
        for(int i = 0; i < useArranger.Length; i++)
        {
            useArranger[i] = transform.GetChild(0).GetChild(i).gameObject;
        }
        invisibleSpace = Resources.Load<GameObject>("UIPrefabs/invisibelSpace");
    }
    void Start()
    {
        arrangers = new List<Arranger>();
        var arrs = transform.GetComponentsInChildren<Arranger>();
        for (int i = 0; i < arrs.Length; i++)
        {
            arrangers.Add(arrs[i]);
        }
    }

    public static void SwapCards(Transform sour, Transform dest)
    {
        Transform sourParent = sour.parent;
        Transform destParent = dest.parent;

        int sourIndex = sour.GetSiblingIndex();
        int destIndex = dest.GetSiblingIndex();

        sour.SetParent(destParent);
        sour.SetSiblingIndex(destIndex);

        dest.SetParent(sourParent);
        dest.SetSiblingIndex(sourIndex);

    }

    void SwapCardsInHierarchy(Transform sour, Transform dest)   // 보이지 않는 카드와 드래그 하는 카드의 하이어라키상의 위치 변환
    {
        SwapCards(sour, dest);
        arrangers.ForEach(t => t.UpdateChildren());
    }

    bool ContainPos(RectTransform rt, Vector2 pos)      // 현재 pos가 rt 안에 있는지 여부 확인
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rt, pos);
    }
    void AddInvisibleSpaceAtIndex(Arranger workingArranger, int oriIndex, Transform invisibleSpace)     // 빈 공간 오브
    {
        Transform workingArrangerTransform = workingArranger.transform;
        GameObject invisible = Instantiate(invisibleSpace.gameObject, Vector3.zero, Quaternion.identity, workingArrangerTransform);
        invisible.transform.SetSiblingIndex(oriIndex);
    }
    void BeginDrag(Transform card)
    {
        workingArranger = arrangers.Find(t => ContainPos(t.transform as RectTransform, card.position));
        oriIndex = card.GetSiblingIndex();
        lastIndex = oriIndex;
        SwapCardsInHierarchy(invisibleCard, card);
    }

    void Drag(Transform card)
    {
        var whichArrangerCard = arrangers.Find(t => ContainPos(t.transform as RectTransform, card.position));

        if (whichArrangerCard == null)
        {
            if (workingArranger.tag != "ReadyArranger" && !create)
            {
                create = true;
                AddInvisibleSpaceAtIndex(workingArranger, oriIndex, invisibleSpace.transform);
            }

            bool updateChildren = transform != invisibleCard.parent;

            invisibleCard.SetParent(transform);

            if (updateChildren)
            {
                arrangers.ForEach(t => t.UpdateChildren());
            }
        }
        else
        {
            bool insert = invisibleCard.parent == transform;

            if (insert)     // 카드가 다른 Array로 끼어드는 순간
            {
                int index = whichArrangerCard.GetIndexByPosition(card);
                invisibleCard.SetParent(whichArrangerCard.transform);
                whichArrangerCard.InsertCard(invisibleCard, index);
            }
            else
            {
                int invisibleCardIndex = invisibleCard.GetSiblingIndex();
                int targetIndex = whichArrangerCard.GetIndexByPosition(card, invisibleCardIndex);

                if (invisibleCardIndex != targetIndex)
                {
                    whichArrangerCard.SwapCard(invisibleCardIndex, targetIndex);
                }
            }
        }
    }

    void EndDrag(Transform card)
    {
        
        if (invisibleCard.parent == transform)      // 카드를 Array 밖에서 놓았을 때
        {
            workingArranger.InsertCard(card, oriIndex);
            workingArranger = null;
            oriIndex = -1;
        }
        else
        {
            SwapCardsInHierarchy(invisibleCard, card);
            create = false;
        }
    }

}