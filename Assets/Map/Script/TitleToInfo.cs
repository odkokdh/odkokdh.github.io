using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleToInfo : MonoBehaviour
{
    public GameObject bg1, bg2, bg3, bg4, bg5, bg6, bg7, bg8;            //배경 판넬 참조할 게임 오브젝트들
    //Image image;                        //판네의 이미지 값을 참조할 이미지 UI

    public Button btnStart;        //아래의 3개는 버튼 클릭시 비활성화 될 UI
    public Text metro;
    public Text surival;

    //public float tum = 0.004f;  //화면 전환용 타임
    public float nextObjectActiveTrue = 0.2f;

    AudioSource audioSource;
    public AudioClip Clip; //씬 전환시 변경될 소리

    // Start is called before the first frame update
    void Start()
    {
        //image = background.GetComponent<Image>();            //참조한 오브젝트의 이미지를 컴포넌트 

        audioSource =GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScene()
    {
        btnStart.gameObject.SetActive(false);       //버튼 클릭시 오브젝트 비활성화
        metro.gameObject.SetActive(false);
        surival.gameObject.SetActive(false);
        //background.SetActive(true);

        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        audioSource.clip = Clip;    //클립 변경
        audioSource.Play();         //그 후 오디오소스 플레이
        //Color color = image.color;                            //color 에 판넬 이미지 참조
      
        yield return new WaitForSecondsRealtime(1f);                 //원래 for문으로 해결해 보려고 했으나 답이 안나와서 이미지를 겹치는 걸로 임시 해결
        bg1.SetActive(true);
        yield return new WaitForSecondsRealtime(nextObjectActiveTrue);                 //일정 시간마다 다음 이미지 활성화
        bg2.SetActive(true);
        yield return new WaitForSecondsRealtime(nextObjectActiveTrue);
        bg3.SetActive(true);
        yield return new WaitForSecondsRealtime(nextObjectActiveTrue);
        bg4.SetActive(true);
        yield return new WaitForSecondsRealtime(nextObjectActiveTrue);
        bg5.SetActive(true);
        yield return new WaitForSecondsRealtime(nextObjectActiveTrue);
        bg6.SetActive(true);
        yield return new WaitForSecondsRealtime(nextObjectActiveTrue);
        bg7.SetActive(true);
        yield return new WaitForSecondsRealtime(nextObjectActiveTrue);
        bg8.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);              //마지막 0.4초 후 Info 씬으로 씬 전환
        SceneManager.LoadScene("Info");
        /*for (int i = 0; i <= 100; i++)                            //for문 100번 반복 0보다 작을 때 까지
        {
            color.a += Time.deltaTime * tum;               //이미지 알파 값을 타임 델타 값

            image.color = color;                                //이미지 컬러에 바뀐 알파값 참조
            if (color.a > 250) {
                SceneManager.LoadScene("Info");                              //씬 전환
            }
        }*/
    }
}
