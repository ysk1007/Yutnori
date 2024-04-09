using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UserData //���� ������ Ŭ����
{
    public string UserName; //�г���
    public string UserId; //���� ���̵�
    public int UserGold; // ��� ��ȭ

    // X : UnitID , Y : UnitRate
    public Vector2[] UserSquad; // ���� ����
    public Vector2[] UserInventory; // ���� �κ��丮

    public int TurnCounter; // �� ī��Ʈ
}

public class UserInfoManager : MonoBehaviour
{
    public static UserInfoManager Instance;
    public bool DataExist = false;

    private string keyName = "UserData"; //Ű ��
    private string fileName = "UserData.ms"; //���� �̸�

    public UserData userData;

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

    public void DataCreate() //������ ����
    {
        userData.UserSquad = new Vector2[9];
        userData.UserInventory = new Vector2[9];
        ES3.Save(keyName, userData);
    }



    public void DataSave() //������ ����
    {
        ES3.Save(keyName, userData);
    }

    public void DataLoad()
    {
        if (ES3.FileExists(fileName)) //���� ��ο� ������ ���� ���
        {
            DataExist = true;
            ES3.LoadInto(keyName, userData); //�ε�
        }
        else
        {
            DataCreate(); //������ ����
        }
    }
}
