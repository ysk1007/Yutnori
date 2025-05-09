using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unit_SubSet : MonoBehaviour
{
    public Transform TextPool;
    public List<TextMeshProUGUI> TextList;
    public List<Image> iconList;
    public List<Animator> AnimatorList;
    public Color[] TextColorList;
    public Color[] SliderColorList;
    public Slider HpSlider; // 체력 슬라이더
    public Image HpSliderFill; // 체력 슬라이더 이미지
    public Slider CTSlider; // 쿨타임 슬라이더
    public Slider SDSlider; // 보호막 슬라이더

    public GameObject Debuff_Stun_icon;
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

    void Awake()
    {
        GetList();
    }

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
        this.gameObject.transform.localScale = (SoonsoonData.Instance.Unit_Manager._gamePause) ? new Vector3(0, 0, 0) : new Vector3(1, 1, 1);
        HpSlider.value = (!unit._ghostDieFlag) ? unit._unitHp / _unitMaxHp : unit._ghostTimer / unit._ghostTime;
        HpSliderFill.color = (!unit._ghostDieFlag) ? SliderColorList[1] : SliderColorList[3];
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

        Debuff_Stun_icon.SetActive(unit._isStun ? true : false);

        Buff_AT_icon.SetActive(unit._buffAT > 1 ? true : false);
        Buff_AS_icon.SetActive(unit._buffAS > 1 ? true : false);
        Buff_DF_icon.SetActive(unit._buffDF > 1 ? true : false);
        Buff_CC_icon.SetActive(unit._buffCC > 1 ? true : false);

        Debuff_AT_icon.SetActive(unit._deBuffAT > 0 ? true : false);
        Debuff_AS_icon.SetActive(unit._deBuffAS > 0 ? true : false);
        Debuff_DF_icon.SetActive(unit._deBuffDF > 0 ? true : false);
        Debuff_CC_icon.SetActive(unit._deBuffCC > 0 ? true : false);
    }

    public void ShowDamageText(float value, bool critical)
    {
        TextList[TextNum].color = critical ? TextColorList[1] : TextColorList[0];
        TextList[TextNum].text = value.ToString("N0");
        AnimatorList[TextNum].SetTrigger("Show");
        iconList[TextNum].enabled = critical ? true : false;
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
        TextList[TextNum].color = TextColorList[2];
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

    void GetList()
    {
        for (int i = 0; i < TextPool.childCount; i++)
        {
            TextMeshProUGUI Text = TextPool.GetChild(i).GetComponent<TextMeshProUGUI>();
            TextList.Add(Text);

            iconList.Add(Text.transform.GetChild(0).GetComponent<Image>());

            Animator Animator = TextPool.GetChild(i).GetComponent<Animator>();
            AnimatorList.Add(Animator);
        }
    }

    public void CT_Update()
    {
        _unitSkillCT = unit._unitCT;
    }

    public void TextListReset()
    {
        for (int i = 0; i < TextList.Count; i++)
        {
            TextList[i].enabled = false;
        }
    }
}
