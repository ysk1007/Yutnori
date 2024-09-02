using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Artifact : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData _itemData;  // ������ ������

    public Image _itemIcon;  // ������ ������
    public TextMeshProUGUI _countText;  // ������ ���� �ؽ�Ʈ

    private ArtifactPopup _popup;  // �˾� UI

    void Start()
    {
        _popup = SoonsoonData.Instance.ArtifactPopup;  // �˾� �ʱ�ȭ
    }

    public void init()
    {
        if (_itemData == null) return;  // ������ �����Ͱ� ������ ����

        _itemIcon.sprite = _itemData._itemicon;  // ������ ������ ����

        if (_itemData._synergySet.Count > 0)
        {
            // �ó��� ��Ʈ�� �����ϴ� ���, �ش� �ó����� �߰��մϴ�.
            int attackTypeHash = _itemData._synergySet[0]._AttackTypeNumber.GetHashCode();
            SoonsoonData.Instance.Synergy_Manager._AttackTypeAddArtifact[attackTypeHash]
                += _itemData._synergySet[0]._addSynergy;
        }
    }

    // ���콺�� ������Ʈ ���� �ö��� �� ȣ��Ǵ� �Լ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        _popup._itemData = _itemData;  // �˾��� ������ ����
        _popup.init();
    }

    // ���콺�� ������Ʈ���� ����� �� ȣ��Ǵ� �Լ�
    public void OnPointerExit(PointerEventData eventData)
    {
        _popup._itemData = null;  // �˾� ������ �ʱ�ȭ
    }
}
