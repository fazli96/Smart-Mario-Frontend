using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{
    Text text;
    RectTransform rT;
    public double t = 0.1;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        rT = GetComponent<RectTransform>();
        StartCoroutine(FadeImage(t,text));

    }

    IEnumerator FadeImage(double t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (float)(Time.deltaTime / t));
            rT.Translate(Vector3.up * (float)(Time.deltaTime / t));
            yield return null;
        }
        yield return new WaitForSeconds((float)0.6);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (float)(Time.deltaTime / t));
            yield return null;
        }
    }
    void Update()
    {

    }






}
