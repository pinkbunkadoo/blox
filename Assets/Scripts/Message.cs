using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour {
	float cooldown = 0;
	Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if (cooldown > 0) {
			cooldown -= Time.deltaTime;	
			if (cooldown <= 0) {
				cooldown = 0;
				text.CrossFadeAlpha(0, 1, true);
			}
		}
	}

	public void Display(string msg) {
		text.CrossFadeAlpha(1.0f, 0, true);
		text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		text.text = msg;
		cooldown = 1;
	}

}
