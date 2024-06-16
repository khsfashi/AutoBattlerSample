using UnityEngine;

/// <summary>
/// 오디오 관리 매니저입니다.
/// 싱글톤으로 관리됩니다.
/// </summary>
public class AudioManager : Manager<AudioManager>
{
    [Header("#BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    private AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;                // 효과음을 재생할 수 있는 채널 수
    private AudioSource[] sfxPlayers;
    int channelIndex;                   // 현재 사용 가능한 효과음 채널 인덱스

    public enum Sfx
    {
        ArcherAttack, AxegirlAttack, ButtonClick, ButtonHover, 
        Defeat, GoldSound, ReadyBgm, SkeletonDefeat, SkeletonTalk1, 
        SkeletonTalk2, SkeletonTalk3, SwordmanAttack, UnitTakeDamage, Victory,
        Buff, Debuff
    }

    private new void Awake()
    {
        base.Awake();
        Setup();
        PlayBgmByIndex(0, 1.5f);
    }

    /// <summary>
    /// 배경음악과 효과음을 재생하기 위해 각 변수 초기화를 합니다.
    /// </summary>
    private void Setup()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClips[0];

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    /// <summary>
    /// 효과음을 재생합니다. (최대 16채널)
    /// </summary>
    /// <param name="sfx">에디터에서 전달한 효과음 배열의 종류를 나열한 Enum입니다. 순서나 효과음이 변경되면 수정해야 하므로, 종류 구분을 위한 로직이 따로 필요합니다.</param>
    /// <param name="customVolume">효과음을 재생할 때 매니저에서 설정한 볼륨과 별개로 각 오디오 파일의 사운드 밸런스를 맞추기 위해 따로 지정하는 값입니다.</param>
    public void PlaySfx(Sfx sfx, float customVolume = 1.0f)
    {
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;
            if (sfxPlayers[loopIndex].isPlaying) continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].volume = customVolume * sfxVolume;
            sfxPlayers[loopIndex].Play();
            break;
        }

    }

    /// <summary>
    /// 배경음을 재생합니다. (1채널)
    /// </summary>
    /// <param name="isPlay"></param>
    public void PlayBgm(bool isPlay)
    {
        if (isPlay) bgmPlayer.Play();
        else bgmPlayer.Stop();
    }

    public void PlayBgmByIndex(int index, float customVolume = 1.0f)
    {
        bgmPlayer.Stop();
        bgmPlayer.volume = customVolume * bgmVolume;
        bgmPlayer.clip = bgmClips[index];
        bgmPlayer.Play();
    }
}
