using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PuaseMenu : MonoBehaviour
{
    [SerializeField] GameObject BG;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject GeneralPanel;
    [SerializeField] GameObject failPanel;
    [SerializeField] GameObject winPanel;
    public static PuaseMenu instance;
    // Start is called before the first frame update
    void Start()
    {
        //device setting
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;

        GeneralPanel.SetActive(true);
        winPanel.SetActive(false);
        failPanel.SetActive(false);
        instance = this;

        if (PlayerPrefs.GetInt("Index") == SceneManager.sceneCountInBuildSettings)
        {
            levelText.text = " "+(PlayerPrefs.GetInt("IndexNo")).ToString();
        }
        //AudioManager.instance.Play("backGround");
        BG.SetActive(true);
    }
    public void FailInitiate()
    {
        BG.SetActive(false);
        
        StartCoroutine(DelayFail());
    }

    public void WinInitiate()
    {
        BG.SetActive(false);
        StartCoroutine(DelayWin());
    }

    IEnumerator DelayFail()
    {
        yield return new WaitForSeconds(1f);
        GeneralPanel.SetActive(false);
        AudioManager.instance.Play("fail");
        failPanel.SetActive(true);
        Debug.Log("failed called");
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
    }
    IEnumerator DelayWin()
    {
        yield return new WaitForSeconds(1f);
        GeneralPanel.SetActive(false);
        AudioManager.instance.Play("win");
        winPanel.SetActive(true);
        //Time.timeScale = 0;
    }

    public void Reload()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLvl()
    {
     
         
        if (PlayerPrefs.GetInt("Index") == SceneManager.sceneCountInBuildSettings)
        {
            PlayerPrefs.SetInt("IndexNo", (PlayerPrefs.GetInt("IndexNo") + 1));//save index no for looping;
            SceneManager.LoadScene("FirstScene");
        }
        if (PlayerPrefs.GetInt("Index") < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1;
            //save level index
            PlayerPrefs.SetInt("Index", (SceneManager.GetActiveScene().buildIndex) + 1);
            PlayerPrefs.SetInt("IndexNo", SceneManager.GetActiveScene().buildIndex);//save index no for looping;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }
   
    }
}
