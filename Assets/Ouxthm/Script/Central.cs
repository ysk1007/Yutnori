using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Central : MonoBehaviour
{
    public static Central instance;

    public Transform invisibleCard; // ������ �ʴ� ī���� ��ġ
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

    void SwapCardsInHierarchy(Transform sour, Transform dest)   // ������ �ʴ� ī��� �巡�� �ϴ� ī���� ���̾��Ű���� ��ġ ��ȯ
    {
        SwapCards(sour, dest);
        arrangers.ForEach(t => t.UpdateChildren());
    }

    bool ContainPos(RectTransform rt, Vector2 pos)      // ���� pos�� rt �ȿ� �ִ��� ���� Ȯ��
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