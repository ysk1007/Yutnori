using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPool : MonoBehaviour
{
    public List<GameObject> _unitPrefabs = new List<GameObject>();
    public List<UnitData> _unitDatas = new List<UnitData>();

    void Awake()
    {
        SoonsoonData.Instance.Unit_pool = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
