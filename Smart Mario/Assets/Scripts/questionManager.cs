using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class questionManager : MonoBehaviour
{
    public GameObject qnsOne;
    public GameObject qnsTwo;
    public GameObject panelOne;
    public GameObject panelTwo;
        
    void Start()
    {
        qnsOne.SetActive(false);
        qnsTwo.SetActive(false);
        panelOne.SetActive(false);
        panelTwo.SetActive(false);

    }


    public void showQuestion(int choice, int index)
    {
        if (choice == 1)
        {
            panelOne.SetActive(true);
            qnsOne.SetActive(true);
            qnsOne.GetComponent<UnityEngine.UI.Text>().text = index.ToString();
        }
        else if (choice == 2)
        {
            qnsTwo.SetActive(true);
            panelTwo.SetActive(true);
            qnsTwo.GetComponent<UnityEngine.UI.Text>().text = index.ToString();
        }

    }
    public void hideQuestion(int choice, bool fade)
    {
        
        if (fade)
        {
            //qnsOne.GetComponent<questionScript>().Fade();
            //StartCoroutine(WaitForSecond(1));
        }
        if (choice == 1)
        {
            qnsOne.SetActive(false);
            panelOne.SetActive(false);
        }
        else if (choice == 2)
        {
            qnsTwo.SetActive(false);
            panelTwo.SetActive(false);
        }
    }
    IEnumerator WaitForSecond(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
