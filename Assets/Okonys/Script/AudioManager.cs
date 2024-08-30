using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixerGroup audioMixerGroup;
    public AudioMixerGroup audioMixerBGM;
    public AudioMixerGroup audioMixerSFX;

    [Header("#�������")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#ȿ����")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    [Header("# ��ų ȿ����")]
    public AudioClip[] vfxClips;
    public int vfxChannels;
    AudioSource[] vfxPlayers;
    int vfxChannelIndex;

    [Header("# �̻��� ȿ����")]
    public AudioClip[] missileClips;

    [Header("# �ǰ� ȿ����")]
    public AudioClip[] effectClips;
    public int effectChannels;
    AudioSource[] effectPlayers;
    int effectChannelIndex;

    UserInfoManager _userInfoManager;

    public enum Sfx { YutSounds, 
        Do = 5,Gae = 7,Geol = 9,Yut = 11,Mo = 13,

        Select = 15, Buy = 16, Sell = 17, Restock = 18
    }

    private void Awake()
    {
        instance = this;
        Init();
    }

    private void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
        PlayBgm(true);
    }

    void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmPlayer.outputAudioMixerGroup = audioMixerBGM;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
            sfxPlayers[i].volume = sfxVolume;
            sfxPlayers[i].outputAudioMixerGroup = audioMixerSFX;
        }

        // ��ų ȿ���� �÷��̾� �ʱ�ȭ
        GameObject vfxObject = new GameObject("VfxPlayer");
        vfxObject.transform.parent = transform;
        vfxPlayers = new AudioSource[vfxChannels];

        for (int i = 0; i < vfxPlayers.Length; i++)
        {
            vfxPlayers[i] = vfxObject.AddComponent<AudioSource>();
            vfxPlayers[i].playOnAwake = false;
            vfxPlayers[i].bypassListenerEffects = true;
            vfxPlayers[i].volume = sfxVolume;
            vfxPlayers[i].outputAudioMixerGroup = audioMixerSFX;
        }

        // �ǰ��� �÷��̾� �ʱ�ȭ
        GameObject effectObject = new GameObject("EffectPlayer");
        effectObject.transform.parent = transform;
        effectPlayers = new AudioSource[effectChannels];

        for (int i = 0; i < effectPlayers.Length; i++)
        {
            effectPlayers[i] = effectObject.AddComponent<AudioSource>();
            effectPlayers[i].playOnAwake = false;
            effectPlayers[i].bypassListenerEffects = true;
            effectPlayers[i].volume = sfxVolume;
            effectPlayers[i].outputAudioMixerGroup = audioMixerSFX;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        //�����ִ� ȿ���� �÷��̾ ã��
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex;

            if (sfx == Sfx.YutSounds) loopIndex = Random.Range(0, 5);
            else
            {
                loopIndex = (i + channelIndex) % sfxPlayers.Length;
            }

            if (sfxPlayers[loopIndex].isPlaying) //�̹� ȿ������ ������̶��
                continue;

            //���� ȿ���� ���� ���
            int ranIndex = 0;
            if (sfx == Sfx.Do || sfx == Sfx.Gae || sfx == Sfx.Geol || sfx == Sfx.Yut || sfx == Sfx.Mo)
            {
                ranIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex; //������ ä�� �ε��� ����

            // ���������� ���� ȿ���� �� ���� ����ȿ�� ����
            if (sfx == Sfx.Do || sfx == Sfx.Gae || sfx == Sfx.Geol || sfx == Sfx.Yut || sfx == Sfx.Mo)
                sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + _userInfoManager.optionData.GetVoiceType()];

            else 
                sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];

            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void PlayVfx(int index)
    {
        //�����ִ� ȿ���� �÷��̾ ã��
        for (int i = 0; i < vfxPlayers.Length; i++)
        {
            int loopIndex;

            if (index == 0) loopIndex = Random.Range(0, 5);
            else
            {
                loopIndex = (i + vfxChannelIndex) % vfxPlayers.Length;
            }

            if (vfxPlayers[loopIndex].isPlaying) //�̹� ȿ������ ������̶��
                continue;

            //���� ȿ���� ���� ���
            vfxChannelIndex = loopIndex; //������ ä�� �ε��� ����
            vfxPlayers[loopIndex].clip = vfxClips[(int)index];

            vfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void PlayEffect(int index)
    {
        //�����ִ� ȿ���� �÷��̾ ã��
        for (int i = 0; i < effectPlayers.Length; i++)
        {
            int loopIndex;

            if (index == 0) loopIndex = Random.Range(0, 5);
            else
            {
                loopIndex = (i + effectChannelIndex) % effectPlayers.Length;
            }

            if (effectPlayers[loopIndex].isPlaying) //�̹� ȿ������ ������̶��
                continue;

            //���� ȿ���� ���� ���
            effectChannelIndex = loopIndex; //������ ä�� �ε��� ����
            effectPlayers[loopIndex].clip = effectClips[(int)index];

            effectPlayers[loopIndex].Play();
            break;
        }
    }
}
