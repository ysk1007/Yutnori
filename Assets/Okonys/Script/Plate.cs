using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plate : MonoBehaviour
{
    public enum PlateType
    {
        Enemy = 0,
        Random = 1,
        Home = 2,
        Boss = 3,
        Chest = 4,
    }

    public Image _plateIcon;
    public PlateType _plateType;
    YutManager _yutManager;

    // Start is called before the first frame update
    void Start()
    {
        _yutManager = YutManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init()
    {
        _plateIcon.sprite = _yutManager._icons[_plateType.GetHashCode()];
        _plateIcon.color = _yutManager._colors[_plateType.GetHashCode()];
    }

    public void init(int i)
    {
        _plateType = (i > 0) ? PlateType.Random : PlateType.Enemy;
        _plateIcon.sprite = _yutManager._icons[i];
        _plateIcon.color = _yutManager._colors[i];
    }
}
