using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;  // �̱��� �ν��Ͻ�

    public AudioMixerGroup audioMixerGroup;  // ����� �ͼ� �׷�
    public AudioMixerGroup audioMixerBGM;  // ����� �ͼ� �׷�
    public AudioMixerGroup audioMixerSFX;  // ȿ���� �ͼ� �׷�

    [Header("# �������")]
    public AudioClip bgmClip;  // ����� Ŭ��
    public float bgmVolume;  // ����� ����
    private AudioSource bgmPlayer;  // ����� �÷��̾�
    private AudioHighPassFilter bgmEffect;  // ����� ���� ȿ��

    [Header("# ȿ����")]
    public AudioClip[] sfxClips;  // ȿ���� Ŭ�� �迭
    public float sfxVolume;  // ȿ���� ����
    public int channels;  // ����� ä�� ��
    private AudioSource[] sfxPlayers;  // ȿ���� �÷��̾� �迭
    private int channelIndex;  // ���� ȿ������ ����� ä�� �ε���

    [Header("# ��ų ȿ����")]
    public AudioClip[] vfxClips;  // ��ų ȿ���� Ŭ�� �迭
    public int vfxChannels;  // ��ų ȿ���� ä�� ��
    private AudioSource[] vfxPlayers;  // ��ų ȿ���� �÷��̾� �迭
    private int vfxChannelIndex;  // ���� ��ų ȿ������ ����� ä�� �ε���

    [Header("# �̻��� ȿ����")]
    public AudioClip[] missileClips;  // �̻��� ȿ���� Ŭ�� �迭

    [Header("# �ǰ� ȿ����")]
    public AudioClip[] effectClips;  // �ǰ� ȿ���� Ŭ�� �迭
    public int effectChannels;  // �ǰ� ȿ���� ä�� ��
    private AudioSource[] effectPlayers;  // �ǰ� ȿ���� �÷��̾� �迭
    private int effectChannelIndex;  // ���� �ǰ� ȿ������ ����� ä�� �ε���

    private UserInfoManager _userInfoManager;  // ���� ���� �Ŵ���

    public enum Sfx
    {
        YutSounds,
        Do = 5, Gae = 7, Geol = 9, Yut = 11, Mo = 13,
        Select = 15, Buy = 16, Sell = 17, Restock = 18
    }

    private void Awake()
    {
        instance = this;  // �̱��� �ν��Ͻ� ����
        Init();  // �ʱ�ȭ �Լ� ȣ��
    }

    private void Start()
    {
        _userInfoManager = UserInfoManager.Instance;  // ���� ���� �Ŵ��� �ʱ�ȭ
        PlayBgm(true);  // ����� ��� ����
    }

    void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        bgmPlayer = CreateAudioSource("BgmPlayer", transform, bgmVolume, audioMixerBGM, true, bgmClip);
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // ȿ���� �÷��̾� �ʱ�ȭ
        sfxPlayers = CreateAudioSourceArray("SfxPlayer", transform, channels, sfxVolume, audioMixerSFX);

        // ��ų ȿ���� �÷��̾� �ʱ�ȭ
        vfxPlayers = CreateAudioSourceArray("VfxPlayer", transform, vfxChannels, sfxVolume, audioMixerSFX);

        // �ǰ� ȿ���� �÷��̾� �ʱ�ȭ
        effectPlayers = CreateAudioSourceArray("EffectPlayer", transform, effectChannels, sfxVolume, audioMixerSFX);
    }

    // ����� ��� �Լ�
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
            bgmPlayer.Play();
        else
            bgmPlayer.Stop();
    }

    // ����� ���� ȿ�� ���� �Լ�
    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    // ȿ���� ��� �Լ�
    public void PlaySfx(Sfx sfx)
    {
        PlayClip(sfxPlayers, sfxClips, (int)sfx, ref channelIndex, sfx == Sfx.YutSounds,
            sfx == Sfx.Do || sfx == Sfx.Gae || sfx == Sfx.Geol || sfx == Sfx.Yut || sfx == Sfx.Mo);
    }

    // ��ų ȿ���� ��� �Լ�
    public void PlayVfx(int index)
    {
        PlayClip(vfxPlayers, vfxClips, index, ref vfxChannelIndex, index == 0);
    }

    // �ǰ� ȿ���� ��� �Լ�
    public void PlayEffect(int index)
    {
        PlayClip(effectPlayers, effectClips, index, ref effectChannelIndex, index == 0);
    }

    // ����� �ҽ� �迭�� �����ϴ� ���� �Լ�
    private AudioSource[] CreateAudioSourceArray(string name, Transform parent, int count, float volume, AudioMixerGroup mixerGroup)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = parent;

        AudioSource[] sources = new AudioSource[count];
        for (int i = 0; i < count; i++)
        {
            sources[i] = obj.AddComponent<AudioSource>();
            sources[i].playOnAwake = false;
            sources[i].bypassListenerEffects = true;
            sources[i].volume = volume;
            sources[i].outputAudioMixerGroup = mixerGroup;
        }
        return sources;
    }

    // ���� ����� �ҽ��� �����ϴ� ���� �Լ�
    private AudioSource CreateAudioSource(string name, Transform parent, float volume, AudioMixerGroup mixerGroup, bool loop, AudioClip clip)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = parent;

        AudioSource source = obj.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.volume = volume;
        source.loop = loop;
        source.clip = clip;
        source.outputAudioMixerGroup = mixerGroup;

        return source;
    }

    // ��� ������ ����� Ŭ���� ã�� ����ϴ� ���� �Լ�
    private void PlayClip(AudioSource[] players, AudioClip[] clips, int index, ref int channelIndex, bool randomize, bool isGendered = false)
    {
        for (int i = 0; i < players.Length; i++)
        {
            int loopIndex = randomize ? Random.Range(0, 5) : (i + channelIndex) % players.Length;

            if (players[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;  // ä�� �ε��� ����

            if (isGendered)
            {
                players[loopIndex].clip = clips[index + _userInfoManager.optionData.GetVoiceType()];
            }
            else
            {
                players[loopIndex].clip = clips[index];
            }

            players[loopIndex].Play();
            break;
        }
    }
}
