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
public class UserData //���� ������ Ŭ����
{
    public bool isUserData; // ������ ����
    public string UserName; //�г���
    public string UserId; //���� ���̵�
    public int UserHp; // ü��
    public int UserGold; // ��� ��ȭ
    public int SelectCharacter;

    // X : UnitID , Y : UnitRate
    public Vector2[] UserSquad; // ���� ����
    public Vector2[] UserInventory; // ���� �κ��丮
    public Vector2[] EnemySquad; // �� �� ����

    public bool isShopArtifactDatas; // ���� ��Ƽ��Ʈ ������ ����
    public int[] ShopArtifacts;
    public int[] ShopUnits;
    public List<int> UserArtifacts; // ���� ��Ƽ��Ʈ

    public int TurnCounter; // �� ī��Ʈ

    public bool isPlateData; // ���� �������� ����
    public int[] PlatesData; // ���� ����
    public int CurrentPlateNum; // ���� �÷��̾ ��� �ִ� ���� ��ȣ
    public int CurrentRoadNum; // ���� �÷��̾��� ���� ��
}

[System.Serializable]
public class OptionData //���� ������ Ŭ����
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

    private string keyName = "Data"; //Ű ��
    private string fileName = "Data.ms"; //���� �̸�

    public Data data;
    public UserData userData;
    public OptionData optionData;



    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            // �ٸ� Instance�� �����ϸ� ���� gameObject�� �ı��Ѵ�.
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

    public void DataCreate() // �ʱ� ������ ����
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

    public void GameDataCreate(int index) // ���� ������ ����
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



    public void DataSave() //������ ����
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
        if (ES3.FileExists(fileName)) //���� ��ο� ������ ���� ���
        {
            DataExist = true;
            ES3.LoadInto(keyName, data); //�ε�
            userData = data.GetUserData();
            optionData = data.GetOptionData();
        }
        else
        {
            DataCreate(); //������ ����
        }
    }
}
