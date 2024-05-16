using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class YutManager : MonoBehaviour
{
    public static YutManager instance;
    public PlayerMove playerMv;

    public float rand;
    public int _moveDistance;      // �� �̵� �Ÿ�
    public List<Plate> _plateList;

    public float _randomPlateProbability; // ���� ĭ�� ������ Ȯ��

    public Sprite[] _icons;
    public Color[] _colors;

    public Button btn;
    private void Awake()
    {
        instance = this;
    }

    public void RandomPlate()
    {
        for (int i = 0; i < _plateList.Count; i++)
        {
            switch (_plateList[i]._plateType.GetHashCode())
            {
                case 0:
                case 1:
                    int type;
                    float randomNumber = Random.value;
                    type = (_randomPlateProbability > randomNumber) ? 1 : 0;
                    _plateList[i].init(type);
                    break;
                case 2:
                case 3:
                case 4:
                    break;
            }
        }
    }

    public void ThrowYut()
    {
        rand = Random.Range(0f, 1f);
        if(rand <= 0.2f)
        {
            Debug.Log("����� '��'�Դϴ�");
            _moveDistance = 1;
        }
        else if(0.2f < rand && rand <= 0.5f)
        {
            Debug.Log("����� '��'�Դϴ�");
            _moveDistance = 2;
        }
        else if(0.5f < rand && rand <= 0.7f)
        {
            Debug.Log("����� '��'�Դϴ�");
            _moveDistance = 3;
        }
        else if(0.7f < rand && rand <= 0.85f)
        {
            Debug.Log("����� '��'�Դϴ�");
            _moveDistance = 4;
        }
        else
        {
            Debug.Log("����� '��'�Դϴ�");
            _moveDistance = 5;
        }
    }

    public void SetYut(int i)
    {
        _moveDistance = i;
    }
}
