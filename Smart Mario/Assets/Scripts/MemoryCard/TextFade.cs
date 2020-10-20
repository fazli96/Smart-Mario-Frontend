using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This text controls the behavior of the match text by fading in and out
/// </summary>
public class TextFade : MonoBehaviour
{
    Text text;
    RectTransform rT;
    public double t = 0.1;
    /// <summary>
    /// This is called before the script initialisation
    /// Invokes a coroutine to fade the text
    /// </summary>
    void Start()
    {
        text = GetComponent<Text>();
        rT = GetComponent<RectTransform>();
        StartCoroutine(FadeImage(t,text));

    }
    /// <summary>
    /// This is called to fade and move the text showing match 
    /// </summary>
    /// <param name="t"></param>
    /// <param name="i"></param>
    /// <returns>Wait for seconds</returns>
    IEnumerator FadeImage(double t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 0.5f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (float)(Time.deltaTime / t));
            rT.Translate(Vector3.up * (float)(Time.deltaTime / t));
            yield return null;
        }
        yield return new WaitForSeconds((float)0.5);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (float)(Time.deltaTime / t));
            yield return null;
        }
    }
 






}
