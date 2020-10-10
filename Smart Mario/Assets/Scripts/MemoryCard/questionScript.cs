using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class defines the behaviour of the question cards
/// </summary>
public class questionScript : MonoBehaviour
{
    Image panel;
    public double t = 1.0;
    /// <summary>
    /// This is called at the start of script initialisation
    /// </summary>
    void Start()
    {
        panel = GetComponent<Image>();
    }
    /// <summary>
    /// This starts a fade coroutine
    /// </summary>
    public void Fade()
    {
        StartCoroutine(FadeImage(t, panel));
    }
    /// <summary>
    /// This fades the card over a time period given as the input
    /// </summary>
    /// <param name="t"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    IEnumerator FadeImage(double t, Image i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (float)(Time.deltaTime / t));
            yield return null;
        }
    }
}
