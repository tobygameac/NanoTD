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

  public static void PlayAudioClip(AudioClip audioClip) {
    GameObject audioClipGameObject = new GameObject("Audio Clip : " + audioClip.name);
    audioClipGameObject.transform.position = Vector3.zero;

    audioClipGameObject.AddComponent<AudioSource>();
    AudioSource audioSource = audioClipGameObject.GetComponent<AudioSource>();
    audioSource.playOnAwake = false;
    audioSource.rolloffMode = AudioRolloffMode.Linear;
    audioSource.loop = false;
    audioSource.clip = audioClip;
    audioSource.volume = 1.0f;
    audioSource.Play();

    Destroy(audioClipGameObject, audioClip.length);
  }

  public static void PlayLoopAudioClip(AudioClip audioClip) {
    GameObject audioClipGameObject = new GameObject("Audio Clip : " + audioClip.name);
    audioClipGameObject.transform.position = Vector3.zero;

    audioClipGameObject.AddComponent<AudioSource>();
    AudioSource audioSource = audioClipGameObject.GetComponent<AudioSource>();
    audioSource.playOnAwake = false;
    audioSource.rolloffMode = AudioRolloffMode.Linear;
    audioSource.loop = true;
    audioSource.clip = audioClip;
    audioSource.volume = 1.0f;
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
