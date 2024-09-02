using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;  // 싱글톤 인스턴스

    public AudioMixerGroup audioMixerGroup;  // 오디오 믹서 그룹
    public AudioMixerGroup audioMixerBGM;  // 배경음 믹서 그룹
    public AudioMixerGroup audioMixerSFX;  // 효과음 믹서 그룹

    [Header("# 배경음악")]
    public AudioClip bgmClip;  // 배경음 클립
    public float bgmVolume;  // 배경음 볼륨
    private AudioSource bgmPlayer;  // 배경음 플레이어
    private AudioHighPassFilter bgmEffect;  // 배경음 필터 효과

    [Header("# 효과음")]
    public AudioClip[] sfxClips;  // 효과음 클립 배열
    public float sfxVolume;  // 효과음 볼륨
    public int channels;  // 사용할 채널 수
    private AudioSource[] sfxPlayers;  // 효과음 플레이어 배열
    private int channelIndex;  // 현재 효과음을 재생할 채널 인덱스

    [Header("# 스킬 효과음")]
    public AudioClip[] vfxClips;  // 스킬 효과음 클립 배열
    public int vfxChannels;  // 스킬 효과음 채널 수
    private AudioSource[] vfxPlayers;  // 스킬 효과음 플레이어 배열
    private int vfxChannelIndex;  // 현재 스킬 효과음을 재생할 채널 인덱스

    [Header("# 미사일 효과음")]
    public AudioClip[] missileClips;  // 미사일 효과음 클립 배열

    [Header("# 피격 효과음")]
    public AudioClip[] effectClips;  // 피격 효과음 클립 배열
    public int effectChannels;  // 피격 효과음 채널 수
    private AudioSource[] effectPlayers;  // 피격 효과음 플레이어 배열
    private int effectChannelIndex;  // 현재 피격 효과음을 재생할 채널 인덱스

    private UserInfoManager _userInfoManager;  // 유저 정보 매니저

    public enum Sfx
    {
        YutSounds,
        Do = 5, Gae = 7, Geol = 9, Yut = 11, Mo = 13,
        Select = 15, Buy = 16, Sell = 17, Restock = 18
    }

    private void Awake()
    {
        instance = this;  // 싱글톤 인스턴스 설정
        Init();  // 초기화 함수 호출
    }

    private void Start()
    {
        _userInfoManager = UserInfoManager.Instance;  // 유저 정보 매니저 초기화
        PlayBgm(true);  // 배경음 재생 시작
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        bgmPlayer = CreateAudioSource("BgmPlayer", transform, bgmVolume, audioMixerBGM, true, bgmClip);
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        sfxPlayers = CreateAudioSourceArray("SfxPlayer", transform, channels, sfxVolume, audioMixerSFX);

        // 스킬 효과음 플레이어 초기화
        vfxPlayers = CreateAudioSourceArray("VfxPlayer", transform, vfxChannels, sfxVolume, audioMixerSFX);

        // 피격 효과음 플레이어 초기화
        effectPlayers = CreateAudioSourceArray("EffectPlayer", transform, effectChannels, sfxVolume, audioMixerSFX);
    }

    // 배경음 재생 함수
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
            bgmPlayer.Play();
        else
            bgmPlayer.Stop();
    }

    // 배경음 필터 효과 적용 함수
    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    // 효과음 재생 함수
    public void PlaySfx(Sfx sfx)
    {
        PlayClip(sfxPlayers, sfxClips, (int)sfx, ref channelIndex, sfx == Sfx.YutSounds,
            sfx == Sfx.Do || sfx == Sfx.Gae || sfx == Sfx.Geol || sfx == Sfx.Yut || sfx == Sfx.Mo);
    }

    // 스킬 효과음 재생 함수
    public void PlayVfx(int index)
    {
        PlayClip(vfxPlayers, vfxClips, index, ref vfxChannelIndex, index == 0);
    }

    // 피격 효과음 재생 함수
    public void PlayEffect(int index)
    {
        PlayClip(effectPlayers, effectClips, index, ref effectChannelIndex, index == 0);
    }

    // 오디오 소스 배열을 생성하는 헬퍼 함수
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

    // 단일 오디오 소스를 생성하는 헬퍼 함수
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

    // 재생 가능한 오디오 클립을 찾고 재생하는 헬퍼 함수
    private void PlayClip(AudioSource[] players, AudioClip[] clips, int index, ref int channelIndex, bool randomize, bool isGendered = false)
    {
        for (int i = 0; i < players.Length; i++)
        {
            int loopIndex = randomize ? Random.Range(0, 5) : (i + channelIndex) % players.Length;

            if (players[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;  // 채널 인덱스 갱신

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
