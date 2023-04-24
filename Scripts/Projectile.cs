using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float projectileSpeed = 4f;

    Vector3 target; //플레이어 위치를 얻는 변수

    Vector3 startPosition; //투사체가 발사된 위치를 저장하는 변수
    Vector3 nowPositionl; //날아가는 투사체의 위치를 저장하는 변수

    Vector3 shootLocaition; //쏴지는 좌표를 저장하는 변수

    float distance; //출발 위치와 현재 위치의 거리가 얼마나 벌어졌는지 확인하는 변수

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        //총알이 발사되는 위치 좌표를 저장해둠
        startPosition = transform.position;

        //플레이어의 위치와 나의 위치 좌표를 빼서 어느 방향으로 투사체를 쏠지 정함
        shootLocaition = (target - startPosition);
    }

    // Update is called once per frame
    void Update()
    {
        //발사하는 방향을 정한 좌표와 투사체의 속도, deltaTime을 곱하여 투사체가 정해진 방향과 이동속도로 이동한다
        transform.Translate(shootLocaition * projectileSpeed * Time.deltaTime);

        //자신의 위치를 실시간으로 저장한다
        nowPositionl = transform.position;

        //처음 시작의 위치와 자신의 위치의 차를 구한다
        distance = Vector3.Distance(startPosition, nowPositionl);

        //처음 시작과 자신의 위치 차가 17이상일 경우
        if(distance>=17)
        {
            //오브젝트는 파괴된다
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //벽과 부딪힐 경우
        if(collision.gameObject.tag=="Wall")
        {
            //자신을 파괴
            Destroy(gameObject);
        }

        //플레이어와 부딪힐 경우
        if(collision.gameObject.tag == "Player")
        {

            //자신을 파괴
            Destroy(gameObject);
        }
    }
}
