using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArtifactPopup : MonoBehaviour
{
    public ItemData _itemData;  // �˾��� ǥ���� ������ ������

    public TextMeshProUGUI _artifactName;  // ��Ƽ��Ʈ �̸� �ؽ�Ʈ
    public TextMeshProUGUI _artifactDesc;  // ��Ƽ��Ʈ ���� �ؽ�Ʈ

    public Color[] _rateColor;  // ������ ��޿� ���� ���� �迭

    [SerializeField] private float posX;  // �˾� ��ġ�� X ������
    [SerializeField] private float posY;  // �˾� ��ġ�� Y ������

    private float screenWidth = Screen.width;  // ȭ�� �ʺ�

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        SoonsoonData.Instance.ArtifactPopup = this;
    }

    private void Update()
    {
        // ������ �����Ͱ� ������ �˾� ����
        if (_itemData == null)
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            // ������ �����Ͱ� ������ �˾� ǥ�� �� ��ġ ������Ʈ
            transform.localScale = Vector3.one;
            Vector3 newPosition = Input.mousePosition.x < screenWidth / 2 ?
                new Vector3(Input.mousePosition.x + posX, Input.mousePosition.y + posY, 0f) :
                new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y + posY, 0f);

            transform.position = newPosition;
        }
    }

    public void init()
    {
        // ������ �����Ͱ� ������ �ʱ�ȭ ����
        if (_itemData == null) return;

        // �˾��� ������ ���� ǥ��
        _artifactName.text = _itemData._itemName;
        _artifactName.color = _rateColor[_itemData._itemRate.GetHashCode()];
        _artifactDesc.text = _itemData._itemDesc;
    }
}
