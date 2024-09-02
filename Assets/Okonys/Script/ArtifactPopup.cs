using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArtifactPopup : MonoBehaviour
{
    public ItemData _itemData;  // 팝업에 표시할 아이템 데이터

    public TextMeshProUGUI _artifactName;  // 아티팩트 이름 텍스트
    public TextMeshProUGUI _artifactDesc;  // 아티팩트 설명 텍스트

    public Color[] _rateColor;  // 아이템 등급에 따른 색상 배열

    [SerializeField] private float posX;  // 팝업 위치의 X 오프셋
    [SerializeField] private float posY;  // 팝업 위치의 Y 오프셋

    private float screenWidth = Screen.width;  // 화면 너비

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        SoonsoonData.Instance.ArtifactPopup = this;
    }

    private void Update()
    {
        // 아이템 데이터가 없으면 팝업 숨김
        if (_itemData == null)
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            // 아이템 데이터가 있으면 팝업 표시 및 위치 업데이트
            transform.localScale = Vector3.one;
            Vector3 newPosition = Input.mousePosition.x < screenWidth / 2 ?
                new Vector3(Input.mousePosition.x + posX, Input.mousePosition.y + posY, 0f) :
                new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y + posY, 0f);

            transform.position = newPosition;
        }
    }

    public void init()
    {
        // 아이템 데이터가 없으면 초기화 중지
        if (_itemData == null) return;

        // 팝업에 아이템 정보 표시
        _artifactName.text = _itemData._itemName;
        _artifactName.color = _rateColor[_itemData._itemRate.GetHashCode()];
        _artifactDesc.text = _itemData._itemDesc;
    }
}
