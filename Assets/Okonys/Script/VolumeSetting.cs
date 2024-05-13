using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;

    [SerializeField] private TextMeshProUGUI _masterVolumeText;
    [SerializeField] private TextMeshProUGUI _bgmVolumeText;
    [SerializeField] private TextMeshProUGUI _sfxVolumeText;

    UserInfoManager _userInfoManager;
    OptionPopup _optionPopup;

    // Start is called before the first frame update
    void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
        _optionPopup = OptionPopup.Instance;

        LoadVolume();

        SetBgmVolume();
        SetBgmVolume();
        SetSfxVolume();
        _optionPopup.OptionSaved();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMasterVolume()
    {
        float volume = _masterSlider.value;
        _audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        _masterVolumeText.text = (volume * 100).ToString("F0") + " %";
    }

    public void SetBgmVolume()
    {
        float volume = _bgmSlider.value;
        _audioMixer.SetFloat("BGM", Mathf.Log10(volume)*20);
        _bgmVolumeText.text = (volume * 100).ToString("F0") + " %";
    }

    public void SetSfxVolume()
    {
        float volume = _sfxSlider.value;
        _audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        _sfxVolumeText.text = (volume * 100).ToString("F0") + " %";
    }

    public void LoadVolume()
    {
        _masterSlider.value = _userInfoManager.optionData.GetMasterVolume();
        _bgmSlider.value = _userInfoManager.optionData.GetBgmVolume();
        _sfxSlider.value = _userInfoManager.optionData.GetSfxVolume();
    }

    public void SaveVolume()
    {
        _userInfoManager.optionData.SetMasterVolume(_masterSlider.value);
        _userInfoManager.optionData.SetBgmVolume(_bgmSlider.value);
        _userInfoManager.optionData.SetSfxVolume(_sfxSlider.value);
        _userInfoManager.OptionDataSave();
    }
}
