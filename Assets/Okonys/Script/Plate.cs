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
        Elite = 3,
        Chest = 4,
    }

    [SerializeField] private Animator _plateAnimator;
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
        if (_plateType == PlateType.Elite) return;

        _plateIcon.sprite = _yutManager._icons[_plateType.GetHashCode()];
    }

    public void init(int i)
    {
        _plateType = (i > 0) ? PlateType.Random : PlateType.Enemy;
        _plateAnimator.SetTrigger("Show");
    }
}
