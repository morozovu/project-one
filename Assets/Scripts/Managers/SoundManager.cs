using UnityEngine;

public class SoundManager : MonoBehaviour
{

	public bool musicEnabled = true;
	public bool fxEnabled = true;
	[Range(0f, 1f)]
	public float musicVolume = 1.0f;
	[Range(0f, 1f)]
	public float fxVolume = 1.0f;

	public AudioClip clearRowSound;
	public AudioClip moveSound;
	public AudioClip dropSound;
	public AudioClip gameOverSound;
	public AudioClip errorSound;
	public AudioClip holdSound;
	public AudioSource musicSource;
	public AudioClip[] musicClips;
	public AudioClip[] vocalClips;
	public AudioClip gameOverVocalClip;
	public AudioClip levelUpVocalClip;
	public IconToggle musicIconToggle;
	public IconToggle fxIconToggle;

	AudioClip _randomMusicClip;


	public AudioClip GetRandomClip(AudioClip[] clips)
	{
		AudioClip randomClip = clips[Random.Range(0, clips.Length)];
		return randomClip;
	}

	public void PlayBackgroundMusic(AudioClip musicClip)
	{
		// return if music is disabled or if musicSource is null or is musicClip is null
		if (!musicEnabled || !musicClip || !musicSource)
		{
			return;
		}

		// if music is playing, stop it
		musicSource.Stop();

		musicSource.clip = musicClip;

		// set the music volume
		musicSource.volume = musicVolume;

		// music repeats forever
		musicSource.loop = true;

		// start playing
		musicSource.Play();
	}

	public void ToggleMusic()
	{
		musicEnabled = !musicEnabled;
		UpdateMusic();

		if (musicIconToggle)
		{
			musicIconToggle.ToggleIcon(musicEnabled);
		}
	}

	public void ToggleFX()
	{
		fxEnabled = !fxEnabled;

		if (fxIconToggle)
		{
			fxIconToggle.ToggleIcon(fxEnabled);
		}
	}


	void Start()
	{
		_randomMusicClip = GetRandomClip(musicClips);
		PlayBackgroundMusic(_randomMusicClip);
	}


	void UpdateMusic()
	{
		if (musicSource.isPlaying != musicEnabled)
		{
			if (musicEnabled)
			{
				_randomMusicClip = GetRandomClip(musicClips);
				PlayBackgroundMusic(_randomMusicClip);
			}
			else
			{
				musicSource.Stop();
			}
		}
	}
}
