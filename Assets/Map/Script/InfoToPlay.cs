using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InfoToPlay : MonoBehaviour
{
    public Button next;
    public Button before;
    public Button startGame;

    public Text info1;
    public Text info2;
    public Text info3;
    public Text info4;

    public GameObject keyBoard;
    public GameObject mouse;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        audioSource.Play();
        next.gameObject.SetActive(false);
        before.gameObject.SetActive(true);
        startGame.gameObject.SetActive(true);

        info1.gameObject.SetActive(false);
        info2.gameObject.SetActive(true);
        info3.gameObject.SetActive(true);
        info4.gameObject.SetActive(true);

        keyBoard.SetActive(false);
        mouse.SetActive(true);
    }

    public void Before()
    {
        audioSource.Play();
        next.gameObject.SetActive(true);
        before.gameObject.SetActive(false);
        startGame.gameObject.SetActive(false);

        info1.gameObject.SetActive(true);
        info2.gameObject.SetActive(false);
        info3.gameObject.SetActive(false);
        info4.gameObject.SetActive(false);

        keyBoard.SetActive(true);
        mouse.SetActive(false);
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Play Scene");
    }
}
