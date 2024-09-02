using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Artifact : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData _itemData;  // 아이템 데이터

    public Image _itemIcon;  // 아이템 아이콘
    public TextMeshProUGUI _countText;  // 아이템 수량 텍스트

    private ArtifactPopup _popup;  // 팝업 UI

    void Start()
    {
        _popup = SoonsoonData.Instance.ArtifactPopup;  // 팝업 초기화
    }

    public void init()
    {
        if (_itemData == null) return;  // 아이템 데이터가 없으면 종료

        _itemIcon.sprite = _itemData._itemicon;  // 아이템 아이콘 설정

        if (_itemData._synergySet.Count > 0)
        {
            // 시너지 세트가 존재하는 경우, 해당 시너지를 추가합니다.
            int attackTypeHash = _itemData._synergySet[0]._AttackTypeNumber.GetHashCode();
            SoonsoonData.Instance.Synergy_Manager._AttackTypeAddArtifact[attackTypeHash]
                += _itemData._synergySet[0]._addSynergy;
        }
    }

    // 마우스가 오브젝트 위에 올라갔을 때 호출되는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        _popup._itemData = _itemData;  // 팝업에 데이터 전달
        _popup.init();
    }

    // 마우스가 오브젝트에서 벗어났을 때 호출되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        _popup._itemData = null;  // 팝업 데이터 초기화
    }
}
