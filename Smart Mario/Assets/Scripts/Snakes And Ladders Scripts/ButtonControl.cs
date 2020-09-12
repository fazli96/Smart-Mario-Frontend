using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    public GameObject exitPanel;
    public GameObject menuPanel;

    void Start(){
        menuPanel.SetActive(false);
        exitPanel.SetActive(false);
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(SceneManager.GetActiveScene().name == "Menu"){
                exitPanel.SetActive(true);
            }
            else{
                menuPanel.SetActive(true);
            }
        }
    }
    public void CancelBtn(){
        menuPanel.SetActive(false);
        exitPanel.SetActive(false);
    }

    public void ResetBtn()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitBtn()
    {
        Application.Quit();
    }

    public void oneP()
    {
        SceneManager.LoadScene("Main");
    }

}
