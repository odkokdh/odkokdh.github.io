using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleToInfo : MonoBehaviour
{
    public GameObject bg1, bg2, bg3, bg4, bg5, bg6, bg7, bg8;            //��� �ǳ� ������ ���� ������Ʈ��
    //Image image;                        //�ǳ��� �̹��� ���� ������ �̹��� UI

    public Button btnStart;        //�Ʒ��� 3���� ��ư Ŭ���� ��Ȱ��ȭ �� UI
    public Text metro;
    public Text surival;

    //public float tum = 0.004f;  //ȭ�� ��ȯ�� Ÿ��
    public float nextObjectActiveTrue = 0.2f;

    AudioSource audioSource;
    public AudioClip Clip; //�� ��ȯ�� ����� �Ҹ�

    // Start is called before the first frame update
    void Start()
    {
        //image = background.GetComponent<Image>();            //������ ������Ʈ�� �̹����� ������Ʈ 

        audioSource =GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScene()
    {
        btnStart.gameObject.SetActive(false);       //��ư Ŭ���� ������Ʈ ��Ȱ��ȭ
        metro.gameObject.SetActive(false);
        surival.gameObject.SetActive(false);
        //background.SetActive(true);

        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        audioSource.clip = Clip;    //Ŭ�� ����
        audioSource.Play();         //�� �� ������ҽ� �÷���
        //Color color = image.color;                            //color �� �ǳ� �̹��� ����
      
        yield return new WaitForSecondsRealtime(1f);                 //���� for������ �ذ��� ������ ������ ���� �ȳ��ͼ� �̹����� ��ġ�� �ɷ� �ӽ� �ذ�
        bg1.SetActive(true);
        yield return new WaitForSecondsRealtime(nextObjectActiveTrue);                 //���� �ð����� ���� �̹��� Ȱ��ȭ
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

        yield return new WaitForSecondsRealtime(1f);              //������ 0.4�� �� Info ������ �� ��ȯ
        SceneManager.LoadScene("Info");
        /*for (int i = 0; i <= 100; i++)                            //for�� 100�� �ݺ� 0���� ���� �� ����
        {
            color.a += Time.deltaTime * tum;               //�̹��� ���� ���� Ÿ�� ��Ÿ ��

            image.color = color;                                //�̹��� �÷��� �ٲ� ���İ� ����
            if (color.a > 250) {
                SceneManager.LoadScene("Info");                              //�� ��ȯ
            }
        }*/
    }
}
