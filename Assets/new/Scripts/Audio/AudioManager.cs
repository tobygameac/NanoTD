using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
  
  private static AudioManager instance;

  public static AudioManager GetInstance() {
    return instance;
  }

  void Awake() {
    if (instance != null && instance != this) {
      Destroy(this.gameObject);
      return;
    }
    instance = this;
  }

  private static List<GameObject> loopingAudioClip;

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

  public static void PlayLoopAudioClip(AudioClip audioClip) {
    if (loopingAudioClip == null) {
      loopingAudioClip = new List<GameObject>();
    }
    PlayLoopAudioClip(audioClip, 1.0f);
  }

  public static void PlayLoopAudioClip(AudioClip audioClip, float volume) {
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

    if (loopingAudioClip == null) {
      loopingAudioClip = new List<GameObject>();
    }
    loopingAudioClip.Add(audioClipGameObject);
  }

  public static void StartAllLoopAudioClip() {
    if (loopingAudioClip == null) {
      return;
    }
    for (int i = 0; i < loopingAudioClip.Count; ++i) {
      if (!loopingAudioClip[i].GetComponent<AudioSource>().isPlaying) {
        loopingAudioClip[i].GetComponent<AudioSource>().Play();
      }
    }
  }

  public static void StopAllLoopAudioClip() {
    if (loopingAudioClip == null) {
      return;
    }
    for (int i = 0; i < loopingAudioClip.Count; ++i) {
      loopingAudioClip[i].GetComponent<AudioSource>().Stop();
    }
  }

  public static void DeleteAllLoopAudioClip() {
    if (loopingAudioClip == null) {
      return;
    }
    for (int i = 0; i < loopingAudioClip.Count; ++i) {
      Destroy(loopingAudioClip[i]);
    }
    loopingAudioClip.Clear();
  }
 }
