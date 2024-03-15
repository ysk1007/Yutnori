using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unit_SubSet : MonoBehaviour
{
    public List<TextMeshProUGUI> TextList;
    public List<Animator> AnimatorList;
    public Color[] TextColorList;
    public Slider HpSlider; // 체력 슬라이더
    public Slider CTSlider; // 쿨타임 슬라이더
    public Slider SDSlider; // 보호막 슬라이더

    public GameObject Buff_AT_icon;
    public GameObject Buff_AS_icon;
    public GameObject Buff_DF_icon;
    public GameObject Buff_CC_icon;
    public GameObject Debuff_AT_icon;
    public GameObject Debuff_AS_icon;
    public GameObject Debuff_DF_icon;
    public GameObject Debuff_CC_icon;

    public int TextNum;

    Unit unit;
    float _unitMaxHp;
    float _unitSkillCT;
    // Start is called before the first frame update
    void Start()
    {
        unit = this.gameObject.GetComponentInParent<Unit>();
        _unitMaxHp = unit._unitHp;
        _unitSkillCT = unit._unitCT;
        TextNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HpSlider.value = unit._unitHp / _unitMaxHp;
        CTSlider.value = unit._skillTimer / _unitSkillCT;

        if (unit._buffSD > 0)
        {
            SDSlider.gameObject.SetActive(true);
            SDSlider.value = unit._buffSD / unit._unitSD;
        }
        else
        {
            SDSlider.gameObject.SetActive(false);
            unit._unitSD = 0;
        }

        Buff_AT_icon.SetActive(unit._buffAT > 1 ? true : false);
        Buff_AS_icon.SetActive(unit._buffAS > 1 ? true : false);
        Buff_DF_icon.SetActive(unit._buffDF > 1 ? true : false);
        //Buff_CC_icon.SetActive(unit._buffCC > 1 ? true : false);
    }

    public void ShowDamageText(float value)
    {
        TextList[TextNum].text = value.ToString();
        TextList[TextNum].color = TextColorList[0];
        AnimatorList[TextNum].SetTrigger("Show");
        if (TextNum == TextList.Count - 1)
        {
            TextNum = 0;
        }
        else
        {
            TextNum++;
        }
    }

    public void ShowHealText(float value)
    {
        TextList[TextNum].text = value.ToString();
        TextList[TextNum].color = TextColorList[1];
        AnimatorList[TextNum].SetTrigger("Show");
        if (TextNum == TextList.Count - 1)
        {
            TextNum = 0;
        }
        else
        {
            TextNum++;
        }
    }
}
