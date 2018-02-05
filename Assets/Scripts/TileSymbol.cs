using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSymbol : Activator {
	public static int ONE = 1;
	public static int TWO = 2;
	public static int THREE = 3;
	public static int FOUR = 4;
	public static int FIVE = 5;
	public static int SIX = 6;

	public int value = TileSymbol.ONE;
	public bool lit;

	public override bool Test() {
        return this.lit == true;
    }

	public void SetLight(bool lit) {
		this.lit = lit;
		var	go2 = transform.GetChild(1).gameObject;
		if (this.lit) {
			go2.GetComponent<Renderer>().material.SetColor("_Color", new Color(0.125f, 0.125f, 0.125f));
			go2.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.25f, 0.8f, 1f));
		} else {
			go2.GetComponent<Renderer>().material.SetColor("_Color", new Color(0.25f, 0.25f, 0.25f));
			go2.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f));
		}
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
