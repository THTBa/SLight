﻿using UnityEngine;
using System.Collections;

public class PlayModel : MonoBehaviour {
    public string levelName;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public 
    void ChangeLevel()
    {
         GameKernel.levelManager.ChangeLevel(
            GameKernel.levelManager.CreateCommonLevel(levelName), 
            true
         );
    }
}
