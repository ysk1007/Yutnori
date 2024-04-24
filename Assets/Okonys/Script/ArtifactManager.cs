using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoBehaviour
{
    ItemShop _itemShop;

    public GameObject _artifactPrefab;
    public List<ItemData> _artifacts;
    public Transform _artifactList;

    private void Awake()
    {
        SoonsoonData.Instance.Artifact_Manager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _itemShop = SoonsoonData.Instance.ItemShop;
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
}
