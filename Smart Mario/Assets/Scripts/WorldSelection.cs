﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSelection : MonoBehaviour
{
    private SceneController scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.getSceneController();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectWorld1()
    {
        scene.PlayWorld1();
    }

    public void SelectWorld2()
    {
        scene.PlayWorld2();
    }

    public void ReturnToMainMenu()
    {
        scene.ToMainMenu();
    }
}
