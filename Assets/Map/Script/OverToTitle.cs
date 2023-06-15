using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OverToTitle : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip clip;

    public GameObject btn_ReStart;
    public GameObject btn_Title;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(LoadButton());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReStart()
    {
        StartCoroutine(LoadingGame());
    }

    public void BackTitle()
    {
        StartCoroutine(LoadingTitle());
    }

    IEnumerator LoadingTitle() {
        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Title");
    }

    IEnumerator LoadingGame()
    {
        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Play Scene");
    }

    IEnumerator LoadButton() { 
        yield return new WaitForSecondsRealtime(2.5f);
        btn_ReStart.SetActive(true);
        btn_Title.SetActive(true);
    }
}
