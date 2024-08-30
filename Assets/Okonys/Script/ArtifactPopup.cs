using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArtifactPopup : MonoBehaviour
{
    public ItemData _itemData;

    public TextMeshProUGUI _artifactName;
    public TextMeshProUGUI _artifactDesc;

    public Color[] _rateColor;

    [SerializeField]  private float posX;
    [SerializeField]  private float posY;

    float screenWidth = Screen.width;

    private void Awake()
    {
        SoonsoonData.Instance.ArtifactPopup = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_itemData) transform.localScale = Vector3.zero;
        else
        {
            transform.localScale = Vector3.one;

            this.transform.position = (Input.mousePosition.x < screenWidth / 2) ? 
                new Vector3(Input.mousePosition.x + posX, Input.mousePosition.y + posY, 0f) : 
                new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y + posY, 0f);
        }
    }

    public void init()
    {
        if (!_itemData) return;

        _artifactName.text = _itemData._itemName;
        _artifactName.color = _rateColor[_itemData._itemRate.GetHashCode()];
        _artifactDesc.text = _itemData._itemDesc;
    }
}
