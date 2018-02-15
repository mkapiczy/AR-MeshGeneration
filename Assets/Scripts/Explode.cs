using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {
	
	void Start() {
		Debug.Log ("Explosion");
		ParticleSystem exp = GetComponent<ParticleSystem>();
		exp.Play();
	}
}
