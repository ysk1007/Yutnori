using System.Collections.Generic;
using UnityEngine;

public class Central : MonoBehaviour
{
    public static Central instance;

    public GameObject[] useArranger = new GameObject[3];  // ������ ����� ī�� ����
    public Transform invisibleCard; // ������ �ʴ� ī���� ��ġ

    public GameObject invisibleSpace;        // ī�� �ڸ� �޲ٱ�
    List<Arranger> arrangers;

    Arranger workingArranger;
    public static int oriIndex;     // ���� ī���� �ڽ� ��ȣ
    public static int lastIndex;

    public bool create;     // �� ���� �����ߴ��� Ȯ��
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

    void SwapCardsInHierarchy(Transform sour, Transform dest)   // ������ �ʴ� ī��� �巡�� �ϴ� ī���� ���̾��Ű���� ��ġ ��ȯ
    {
        SwapCards(sour, dest);
        arrangers.ForEach(t => t.UpdateChildren());
    }

    bool ContainPos(RectTransform rt, Vector2 pos)      // ���� pos�� rt �ȿ� �ִ��� ���� Ȯ��
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rt, pos);
    }
    void AddInvisibleSpaceAtIndex(Arranger workingArranger, int oriIndex, Transform invisibleSpace)     // �� ���� ����
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

            if (insert)     // ī�尡 �ٸ� Array�� ������ ����
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
        
        if (invisibleCard.parent == transform)      // ī�带 Array �ۿ��� ������ ��
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