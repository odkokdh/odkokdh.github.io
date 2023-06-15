using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeUI : MonoBehaviour
{
    public GameObject messageBox;
    public Text intheText;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        messageBox.SetActive(false);
    }
    
    public void OnMessage(string message)
    {
        intheText.text = message;
        messageBox.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        messageBox.SetActive(true);
        anim.SetBool("isOn", true);
        yield return new WaitForSeconds(2f);
        anim.SetBool("isOn", false);
        yield return new WaitForSeconds(5f);
        messageBox.SetActive(false);
    }
}
