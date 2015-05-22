using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
  
  private static AudioManager instance;

  private static float _volume;
  public static float Volume {
    get {
      return _volume;
    }
    set {
      if (value >= 0 && value <= 1.0f) {
        _volume = value;
      }
    }
  }
  
  void Awake() {
    if (instance != null && instance != this) {
      Destroy(this.gameObject);
      return;
    }
    instance = this;
  }
  
  public static AudioManager GetInstance() {
    return instance;
  }

  public static void PlayAudioClip(AudioClip audioClip) {
    PlayAudioClip(audioClip, 1.0f);
  }

  public static void PlayAudioClip(AudioClip audioClip, float volume) {
    GameObject audioClipGameObject = new GameObject("Audio Clip : " + audioClip.name);
    audioClipGameObject.transform.position = Vector3.zero;

    audioClipGameObject.AddComponent<AudioSource>();
    AudioSource audioSource = audioClipGameObject.GetComponent<AudioSource>();
    audioSource.playOnAwake = false;
    audioSource.rolloffMode = AudioRolloffMode.Linear;
    audioSource.loop = false;
    audioSource.clip = audioClip;
    audioSource.volume = volume * _volume;
    audioSource.Play();

    Destroy(audioClipGameObject, audioClip.length);
  }

  public static GameObject PlayLoopAudioClip(AudioClip audioClip) {
    return PlayLoopAudioClip(audioClip, 1.0f);
  }

  public static GameObject PlayLoopAudioClip(AudioClip audioClip, float volume) {
    GameObject audioClipGameObject = new GameObject("Audio Clip : " + audioClip.name);
    audioClipGameObject.transform.position = Vector3.zero;

    audioClipGameObject.AddComponent<AudioSource>();
    AudioSource audioSource = audioClipGameObject.GetComponent<AudioSource>();
    audioSource.playOnAwake = false;
    audioSource.rolloffMode = AudioRolloffMode.Linear;
    audioSource.loop = true;
    audioSource.clip = audioClip;
    audioSource.volume = volume * _volume;
    audioSource.Play();

    return  audioClipGameObject;
  }
 }
