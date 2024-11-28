using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
	public AudioClip buttonSound;
	private AudioSource audioSource;
	void Start()
    {
		audioSource = gameObject.AddComponent<AudioSource>();
    }

  public void onButtonSound()
    {
		audioSource.PlayOneShot(buttonSound);
    }
}
