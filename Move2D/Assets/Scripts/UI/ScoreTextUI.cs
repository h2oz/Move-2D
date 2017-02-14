﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextUI : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		if (GameManager.singleton == null)
			this.GetComponent<Text> ().text = "/";
		else
			this.GetComponent<Text> ().text = GameManager.singleton.score.ToString();
	}
}