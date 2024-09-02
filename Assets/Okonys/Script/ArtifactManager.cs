using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoBehaviour
{
    private ItemShop _itemShop;  // 아이템 상점 인스턴스

    public GameObject _artifactPrefab;  // 아티팩트 프리팹
    public List<ItemData> _artifacts;  // 생성된 아티팩트 목록
    public Transform _artifactList;  // 아티팩트 UI가 배치될 부모 트랜스폼

    private UserInfoManager _userInfoManager;  // 유저 정보 매니저 인스턴스

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        SoonsoonData.Instance.Artifact_Manager = this;
    }

    private void Start()
    {
        _userInfoManager = UserInfoManager.Instance;  // 유저 정보 매니저 초기화
        _itemShop = SoonsoonData.Instance.ItemShop;  // 아이템 상점 초기화
        UserArtifactsLoad();  // 유저의 아티팩트 로드
        _itemShop.ShopArtifactsLoad();  // 상점의 아티팩트 로드
    }

    public void SetArtifact(ItemData itemData)
    {
        // 아티팩트 생성 및 초기화
        GameObject artifact = Instantiate(_artifactPrefab, _artifactList);  // 아티팩트 인스턴스 생성
        Artifact artifactComponent = artifact.GetComponent<Artifact>();  // 아티팩트 컴포넌트 가져오기

        artifactComponent._itemData = itemData;  // 아이템 데이터 설정
        artifactComponent.init();  // 아티팩트 초기화

        _artifacts.Add(itemData);  // 아티팩트를 목록에 추가
    }

    public void UserArtifactsLoad()
    {
        // 유저가 보유한 아티팩트를 로드하고 상점 재고 업데이트
        for (int i = 0; i < _userInfoManager.userData.UserArtifacts.Count; i++)
        {
            int artifactIndex = _userInfoManager.userData.UserArtifacts[i] - 1;  // 유저가 보유한 아티팩트 인덱스
            SetArtifact(_itemShop._itemDatas[artifactIndex]);  // 아티팩트 설정
            _itemShop._itemStock[artifactIndex]._haveStock = false;  // 해당 아티팩트의 상점 재고 업데이트
        }
    }
}
