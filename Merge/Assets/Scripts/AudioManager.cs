using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicAudioSource;
    public AudioSource gameAudioSource;

    [Header("Audio Clips")]
    public AudioClip popAudioClip;
    public AudioClip dropAudioClip;
    public AudioClip BoardCleared;
    public AudioClip gameOverClip;

    private void OnEnable()
    {
        EventManager.OnPieceUpgraded += PlayPopAudio;
        EventManager.OnPieceDropped += PlayDropAudio;
        EventManager.OnPieceRemoved += PlayRemovedAudio;
        EventManager.OnCleanBoard += PlayBoardCleared;
        EventManager.OnGameOver += PlayGameOver;
    }

    private void OnDisable()
    {
        EventManager.OnPieceUpgraded -= PlayPopAudio;
        EventManager.OnPieceDropped -= PlayDropAudio;
        EventManager.OnPieceRemoved -= PlayRemovedAudio;
        EventManager.OnCleanBoard -= PlayBoardCleared;
        EventManager.OnGameOver -= PlayGameOver;
    }

    public void PlayPopAudio(GameObject obj, GameObject obj2)
    {
        gameAudioSource.PlayOneShot(popAudioClip);
    }

    public void PlayDropAudio(GameObject obj)
    {
        gameAudioSource.PlayOneShot(dropAudioClip);
    }

    public void PlayRemovedAudio()
    {
                gameAudioSource.PlayOneShot(popAudioClip);
    }

    public void PlayBoardCleared()
    {
        gameAudioSource.PlayOneShot(BoardCleared);
    }
    

    public void ChangeMusicVolume(float volume)
    {
        musicAudioSource.volume = Mathf.Clamp01(volume); // Ensure volume is between 0 and 1
    }

    public void ChangeSFXVolume(float volume)
    {
        gameAudioSource.volume = Mathf.Clamp01(volume); // Ensure volume is between 0 and 1
    }

    public void PlayGameOver()
    {
        musicAudioSource.Stop();
        gameAudioSource.PlayOneShot(gameOverClip);
    }


}
