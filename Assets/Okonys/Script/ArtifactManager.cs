using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoBehaviour
{
    private ItemShop _itemShop;  // ������ ���� �ν��Ͻ�

    public GameObject _artifactPrefab;  // ��Ƽ��Ʈ ������
    public List<ItemData> _artifacts;  // ������ ��Ƽ��Ʈ ���
    public Transform _artifactList;  // ��Ƽ��Ʈ UI�� ��ġ�� �θ� Ʈ������

    private UserInfoManager _userInfoManager;  // ���� ���� �Ŵ��� �ν��Ͻ�

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        SoonsoonData.Instance.Artifact_Manager = this;
    }

    private void Start()
    {
        _userInfoManager = UserInfoManager.Instance;  // ���� ���� �Ŵ��� �ʱ�ȭ
        _itemShop = SoonsoonData.Instance.ItemShop;  // ������ ���� �ʱ�ȭ
        UserArtifactsLoad();  // ������ ��Ƽ��Ʈ �ε�
        _itemShop.ShopArtifactsLoad();  // ������ ��Ƽ��Ʈ �ε�
    }

    public void SetArtifact(ItemData itemData)
    {
        // ��Ƽ��Ʈ ���� �� �ʱ�ȭ
        GameObject artifact = Instantiate(_artifactPrefab, _artifactList);  // ��Ƽ��Ʈ �ν��Ͻ� ����
        Artifact artifactComponent = artifact.GetComponent<Artifact>();  // ��Ƽ��Ʈ ������Ʈ ��������

        artifactComponent._itemData = itemData;  // ������ ������ ����
        artifactComponent.init();  // ��Ƽ��Ʈ �ʱ�ȭ

        _artifacts.Add(itemData);  // ��Ƽ��Ʈ�� ��Ͽ� �߰�
    }

    public void UserArtifactsLoad()
    {
        // ������ ������ ��Ƽ��Ʈ�� �ε��ϰ� ���� ��� ������Ʈ
        for (int i = 0; i < _userInfoManager.userData.UserArtifacts.Count; i++)
        {
            int artifactIndex = _userInfoManager.userData.UserArtifacts[i] - 1;  // ������ ������ ��Ƽ��Ʈ �ε���
            SetArtifact(_itemShop._itemDatas[artifactIndex]);  // ��Ƽ��Ʈ ����
            _itemShop._itemStock[artifactIndex]._haveStock = false;  // �ش� ��Ƽ��Ʈ�� ���� ��� ������Ʈ
        }
    }
}
