using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Central : MonoBehaviour
{
    public static Central instance;

    public Transform invisibleCard; // 보이지 않는 카드의 위치
    List<Arranger> arrangers;

    Arranger workingArranger;
    public static int oriIndex;
    public static int lastIndex;

    private void Awake()
    {
        instance = this;
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

            if (insert)
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
        if (invisibleCard.parent == transform)
        {
            workingArranger.InsertCard(card, oriIndex);
            workingArranger = null;
            oriIndex = -1;
        }
        else
        {
            SwapCardsInHierarchy(invisibleCard, card);
        }
    }

}