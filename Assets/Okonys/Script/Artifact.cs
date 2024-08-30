using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Artifact : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData _itemData;

    public Image _itemIcon;
    public TextMeshProUGUI _countText;

    ArtifactPopup _popup;
    // Start is called before the first frame update
    void Start()
    {
        _popup = SoonsoonData.Instance.ArtifactPopup;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init()
    {
        if (!_itemData) return;

        _itemIcon.sprite = _itemData._itemicon;

        if (_itemData._synergySet.Count > 0)
            SoonsoonData.Instance.Synergy_Manager.
                _AttackTypeAddArtifact[_itemData._synergySet[0]._AttackTypeNumber.GetHashCode()] 
                += _itemData._synergySet[0]._addSynergy;
    }


    // ���콺�� ������Ʈ ���� �ö��� �� ȣ��Ǵ� �Լ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        _popup._itemData = _itemData;
        _popup.init();
    }

    // ���콺�� ������Ʈ���� ����� �� ȣ��Ǵ� �Լ�
    public void OnPointerExit(PointerEventData eventData)
    {
        _popup._itemData = null;
    }
}
