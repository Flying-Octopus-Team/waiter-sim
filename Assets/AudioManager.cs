using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
	[SerializeField, Range(0, 20)] private int intervalMin = 4;
	[SerializeField, Range(0, 20)] private int intervalMax = 12;
	
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource ambienceChatterSource;
	[SerializeField] private AudioSource sfxSource;
	
	[SerializeField] private AudioClip[] musicClips;
	[SerializeField] private AudioClip[] sfxClips;
	[SerializeField] private AudioClip[] sfxGameOverClips;
	
	private bool isPlaying;
	
	private void Start(){
		LevelManager.GameOverEvent += GameOver;
		isPlaying = true;
		
		StartPlayingRandomMusic();
		
		StartCoroutine(StartRandomSFX());
	}

	private void GameOver(){
		StopLevelAudio();
		
		int idx = Random.Range(0, sfxGameOverClips.Length);
		sfxSource.clip = sfxGameOverClips[idx];
		sfxSource.Play();
	}

	private void StartPlayingRandomMusic() {
		int idx = Random.Range(0, musicClips.Length);
		musicSource.clip = musicClips[idx];
		musicSource.PlayDelayed(1);
	}

	private void StopLevelAudio(){
		isPlaying = false;
		StartCoroutine(FadeOutThenStop(musicSource));
		StartCoroutine(FadeOutThenStop(ambienceChatterSource));
	}

	private IEnumerator FadeOutThenStop(AudioSource audioSource){
		while (audioSource.volume > 0.01f){
			audioSource.volume -= Time.deltaTime;
			yield return null;	
		}
		audioSource.Stop();
	}

	private IEnumerator StartRandomSFX(){
		while (isPlaying){
			float interval = Random.Range(intervalMin, intervalMax);
			int idx = Random.Range(0, sfxClips.Length);
			sfxSource.clip = sfxClips[idx];
			sfxSource.Play(1);
			interval += sfxSource.clip.length;
			yield return new WaitForSeconds(interval);	
		}
	}
}