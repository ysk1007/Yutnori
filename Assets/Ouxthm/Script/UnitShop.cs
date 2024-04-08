using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitShop : MonoBehaviour
{
    public Button[] btn = new Button[2];
    public TextMeshProUGUI[] probabilityText = new TextMeshProUGUI[5];
    public int upgradeLv;
    public float[] oneTier;
    public float[] twoTier;
    public float[] threeTier;
    public float[] fourTier;
    public float[] fiveTier;
    private void Awake()
    {
        upgradeLv = 0;
        oneTier = new float[] { 1, 1, 0.65f, 0.5f, 0.37f, 0.245f, 0.2f, 0.15f, 0.1f };
        twoTier = new float[] { 0, 0, 0.3f, 0.35f, 0.35f, 0.35f, 0.3f, 0.25f, 0.15f };
        threeTier = new float[] { 0, 0, 0.05f, 0.15f, 0.25f, 0.3f, 0.33f, 0.33f, 0.33f };
        fourTier = new float[] { 0, 0, 0, 0.03f, 0.03f, 0.1f, 0.15f, 0.2f, 0.3f };
        fiveTier = new float[] { 0, 0, 0, 0, 0, 0.05f, 0.02f, 0.05f, 0.1f };
        for(int i = 0; i < btn.Length; i++)
        {
            btn[i] = transform.GetChild(i + 1).GetComponent<Button>();
        }
        for(int i = 0; i < probabilityText.Length; i++)
        {
            probabilityText[i] = transform.GetChild(0).GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
        }
        UpdateProbability();

        btn[0].onClick.AddListener(() => LevelUP());
        btn[1].onClick.AddListener(() => Reroll());
    }
    public void LevelUP()
    {
        if(upgradeLv < oneTier.Length - 1)
        {
            upgradeLv++;
            UpdateProbability();
        }
    }
    public void Reroll()
    {

    }

    public void UpdateProbability()     // 확률 텍스트 업데이트
    {
        probabilityText[0].text = oneTier[upgradeLv].ToString() + "%";
        probabilityText[1].text = twoTier[upgradeLv].ToString() + "%";
        probabilityText[2].text = threeTier[upgradeLv].ToString() + "%";
        probabilityText[3].text = fourTier[upgradeLv].ToString() + "%";
        probabilityText[4].text = fiveTier[upgradeLv].ToString() + "%";

    }
}