using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arranger : MonoBehaviour
{
    public static Arranger instance;

    public List<Transform> children;
    public int index;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        children = new List<Transform>();

        UpdateChildren();
    }

    public void UpdateChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == children.Count)
            {
                children.Add(null);
            }

            var child = transform.GetChild(i);

            if (child != children[i])
            {
                index = i;
                children[i] = child;
            }

        }

        children.RemoveRange(transform.childCount, children.Count - transform.childCount);
    }

    public void InsertCard(Transform card, int index)
    {
        children.Add(card);
        card.SetParent(transform, true);
        card.SetSiblingIndex(index);
        UpdateChildren();
    }


    public int GetIndexByPosition(Transform card, int skipIndex = -1)
    {
        int result = 0;

        for (int i = 0; i < children.Count; i++)
        {
            if (card.position.x < children[i].position.x)
            {
                break;
            }
            else if (skipIndex != i)
            {
                result++;
            }
        }

        return result;
    }

    public void SwapCard(int index01, int index02)
    {
        Central.SwapCards(children[index01], children[index02]);
        UpdateChildren();
    }


}