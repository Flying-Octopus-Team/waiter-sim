using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class MainMenuAudio : MonoBehaviour
{
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioClip[] musicClips;
	
	private void Start(){
		StartPlayingRandomMusic();
	}
	
	private void StartPlayingRandomMusic() {
		int idx = Random.Range(0, musicClips.Length);
		musicSource.clip = musicClips[idx];
		musicSource.PlayDelayed(1);
	}
}