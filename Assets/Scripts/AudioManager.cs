using UnityEngine;

/// <summary>
/// ����� ���� �Ŵ����Դϴ�.
/// �̱������� �����˴ϴ�.
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
    public int channels;                // ȿ������ ����� �� �ִ� ä�� ��
    private AudioSource[] sfxPlayers;
    int channelIndex;                   // ���� ��� ������ ȿ���� ä�� �ε���

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
    /// ������ǰ� ȿ������ ����ϱ� ���� �� ���� �ʱ�ȭ�� �մϴ�.
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
    /// ȿ������ ����մϴ�. (�ִ� 16ä��)
    /// </summary>
    /// <param name="sfx">�����Ϳ��� ������ ȿ���� �迭�� ������ ������ Enum�Դϴ�. ������ ȿ������ ����Ǹ� �����ؾ� �ϹǷ�, ���� ������ ���� ������ ���� �ʿ��մϴ�.</param>
    /// <param name="customVolume">ȿ������ ����� �� �Ŵ������� ������ ������ ������ �� ����� ������ ���� �뷱���� ���߱� ���� ���� �����ϴ� ���Դϴ�.</param>
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
    /// ������� ����մϴ�. (1ä��)
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
