using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questionScript : MonoBehaviour
{
    Image panel;
    public double t = 1.0;
    // Start is called before the first frame update
    void Start()
    {
        panel = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Fade()
    {
        StartCoroutine(FadeImage(t, panel));
    }
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
