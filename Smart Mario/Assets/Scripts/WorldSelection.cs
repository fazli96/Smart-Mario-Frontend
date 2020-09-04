using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSelection : MonoBehaviour
{
    public void PlayWorld1()
    {
        SceneManager.LoadScene("World1");
    }
    public void PlayWorld2()
    {
        SceneManager.LoadScene("World2");
    }
}
