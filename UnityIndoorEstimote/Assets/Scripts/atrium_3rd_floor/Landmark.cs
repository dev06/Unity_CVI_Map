using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmark : MonoBehaviour {

	public int landmarkID = 0;

	public AudioSource landmarkSource;

	public AudioClip clip;

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player")
		{
			landmarkSource.clip = clip;
			landmarkSource.Play();
		}
	}
}
