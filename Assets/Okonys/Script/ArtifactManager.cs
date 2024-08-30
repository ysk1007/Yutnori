using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoBehaviour
{
    ItemShop _itemShop;

    public GameObject _artifactPrefab;
    public List<ItemData> _artifacts;
    public Transform _artifactList;

    UserInfoManager _userInfoManager;

    private void Awake()
    {
        SoonsoonData.Instance.Artifact_Manager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
        _itemShop = SoonsoonData.Instance.ItemShop;
        UserArtifactsLoad();
        _itemShop.ShopArtifactsLoad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetArtifact(ItemData itemData)
    {
        GameObject Artifact = Instantiate(_artifactPrefab, _artifactList);

        Artifact.GetComponent<Artifact>()._itemData = itemData;
        Artifact.GetComponent<Artifact>().init();

        Artifact.transform.SetParent(_artifactList);

        _artifacts.Add(itemData);
    }

    public void UserArtifactsLoad()
    {
        for (int i = 0; i < _userInfoManager.userData.UserArtifacts.Count; i++)
        {
            SetArtifact(_itemShop._itemDatas[_userInfoManager.userData.UserArtifacts[i] - 1]);
            _itemShop._itemStock[_userInfoManager.userData.UserArtifacts[i] - 1]._haveStock = false;
        }
    }
}
