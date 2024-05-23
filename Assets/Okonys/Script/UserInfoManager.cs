using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Data
{
    public UserData userData;
    public OptionData optionData;

    /*    public void SetUserData(UserData userData)
        {
            this.userData = userData ?? new UserData();
        }*/
    public void SetUserData(UserData userData)
    {
        this.userData = userData;
    }

    public void SetOptionData(OptionData optionData)
    {
        this.optionData = optionData;
    }

    public UserData GetUserData()
    {
        return this.userData;
    }

    public OptionData GetOptionData()
    {
        return this.optionData;
    }
}

[System.Serializable]
public class UserData //유저 데이터 클래스
{
    public bool isUserData; // 데이터 유무
    public string UserName; //닉네임
    public string UserId; //고유 아이디
    public int UserHp; // 체력
    public int UserGold; // 골드 재화
    public int SelectCharacter;

    // X : UnitID , Y : UnitRate
    public Vector2[] UserSquad; // 유저 유닛
    public Vector2[] UserInventory; // 유저 인벤토리
    public Vector2[] EnemySquad; // 적 팀 유닛

    public bool isShopArtifactDatas; // 상점 아티팩트 데이터 유무
    public int[] ShopArtifacts;
    public int[] ShopUnits;
    public List<int> UserArtifacts; // 유저 아티팩트

    public int TurnCounter; // 턴 카운트

    public bool isPlateData; // 발판 데이터의 유무
    public int[] PlatesData; // 발판 종류
    public int CurrentPlateNum; // 현재 플레이어가 밟고 있는 발판 번호
    public int CurrentRoadNum; // 현재 플레이어의 진행 길
}

[System.Serializable]
public class OptionData //유저 데이터 클래스
{
    [SerializeField] private float _masterVolume;
    [SerializeField] private float _bgmVolume;
    [SerializeField] private float _sfxVolume;
    [SerializeField] private int _voiceType;

    public void SetMasterVolume(float volume)
    {
        _masterVolume = volume;
    }
    public void SetBgmVolume(float volume)
    {
        _bgmVolume = volume;
    }
    public void SetSfxVolume(float volume)
    {
        _sfxVolume = volume;
    }

    public void SetVoiceType(int i)
    {
        _voiceType = i;
    }

    public float GetMasterVolume()
    {
        return _masterVolume;
    }

    public float GetBgmVolume()
    {
        return _bgmVolume;
    }
    public float GetSfxVolume()
    {
        return _sfxVolume;
    }

    public int GetVoiceType()
    {
        return _voiceType;
    }
}

public class UserInfoManager : MonoBehaviour
{
    public static UserInfoManager Instance;
    public bool DataExist = false;

    private string keyName = "Data"; //키 값
    private string fileName = "Data.ms"; //파일 이름

    public Data data;
    public UserData userData;
    public OptionData optionData;



    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            // 다른 Instance가 존재하면 현재 gameObject를 파괴한다.
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DataLoad();
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {

    }

    public void DataCreate() // 초기 데이터 생성
    {
        UserData userData = new UserData();
        userData.isUserData = false;
        userData.UserHp = 100;
        userData.UserSquad = new Vector2[9];
        userData.UserInventory = new Vector2[12];
        userData.EnemySquad = new Vector2[9];
        userData.isShopArtifactDatas = false;
        userData.ShopArtifacts = new int[6];
        userData.ShopUnits = new int[5];
        userData.UserArtifacts = new List<int>();

        userData.isPlateData = false;
        userData.PlatesData = new int[29];
        userData.CurrentPlateNum = 0;
        userData.CurrentRoadNum = 0;
        userData.TurnCounter = 1;

        OptionData optionData = new OptionData();
        optionData.SetMasterVolume(0.5f);
        optionData.SetBgmVolume(0.5f);
        optionData.SetSfxVolume(0.5f);
        optionData.SetVoiceType(0);

        Data data = new Data();
        data.SetUserData(userData);
        data.SetOptionData(optionData);

        this.userData = userData;
        this.optionData = optionData;
        this.data = data;

        ES3.Save(keyName, data);
    }

    public void GameDataCreate(int index) // 게임 데이터 생성
    {
        UserData userData = new UserData();
        userData.isUserData = true;
        userData.UserHp = 100;
        userData.SelectCharacter = index;
        userData.UserSquad = new Vector2[9];
        userData.UserSquad[0] = new Vector2(index + 1, 0); 
        userData.UserInventory = new Vector2[12];
        userData.EnemySquad = new Vector2[9];
        userData.isShopArtifactDatas = false;
        userData.ShopArtifacts = new int[6];
        userData.ShopUnits = new int[5];
        userData.UserArtifacts = new List<int>();

        userData.isPlateData = false;
        userData.PlatesData = new int[29];
        userData.CurrentPlateNum = 0;
        userData.CurrentRoadNum = 0;
        userData.TurnCounter = 1;

        Data data = new Data();
        data.SetUserData(userData);
        data.SetOptionData(this.optionData);

        this.userData = userData;
        this.data = data;

        ES3.Save(keyName, data);
    }



    public void DataSave() //데이터 저장
    {
        ES3.Save(keyName, data);
    }

    public void UserDataSave()
    {
        data.SetUserData(userData);
        ES3.Save(keyName, data);
    }

    public void OptionDataSave()
    {
        data.SetOptionData(optionData);
        ES3.Save(keyName, data);
    }

    public void DataLoad()
    {
        if (ES3.FileExists(fileName)) //파일 경로에 데이터 있을 경우
        {
            DataExist = true;
            ES3.LoadInto(keyName, data); //로드
            userData = data.GetUserData();
            optionData = data.GetOptionData();
        }
        else
        {
            DataCreate(); //없으면 생성
        }
    }
}
