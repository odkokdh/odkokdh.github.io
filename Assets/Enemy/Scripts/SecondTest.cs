using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondTest : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector3 startPos; //자신의 시작 위치를 저장하기 위한 변수
    float xPos; //지정 범위내에 무작위로 정해진 x좌표를 저장하기 위한 변수
    float yPos; //지정 범위내에 무작위로 정해진 y좌표를 저장하기 위한 변수
    Vector3 randomPos; //무작위로 정해진 좌표 값을 저장하는 변수

    public float patrolRanged; //자신의 배회 범위를 정하는 변수

    private float speed = 10f; //이동속도

    private bool moving; //자신이 현재 이동중인지 체크하는 불리언
    private bool randoming = false; //좌표값 무작위 선정을 false일 동안만 이루어지도록 만든 불리언


    //이동이 끝나고 상하좌우로 벽이 있는지 확인을 하고 정보를 담는 변수
    RaycastHit2D rayHitLeft;
    RaycastHit2D rayHitRigth;
    RaycastHit2D rayHitDown;
    RaycastHit2D rayHitUp;

    bool contactWall; //벽에 닿음을 알려주는 불리언
    bool loopFor; //for문에서 코드가 한번 실행됬다는 것을 알려주는 불리언

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        moving = false; //움직이지 않았기에 false로 지정한다

        startPos = transform.position; //자신의 시작 위치를 저장
    }

    // Update is called once per frame
    void Update()
    {
        //랜덤 좌표를 뽑지 않았을 경우 실행
        if(randoming == false)
        {
            //x좌표와 y좌표 값을 정해진 범위 내에 무작위로 정한뒤 Vector3 형식으로 저장한다
            xPos = Random.Range(startPos.x - patrolRanged, startPos.x + patrolRanged + 1);
            yPos = Random.Range(startPos.y - patrolRanged, startPos.y + patrolRanged + 1);
            randomPos = new Vector3(xPos, yPos, 0);

            //랜덤 좌표를 한번 실행했으므로 true로 바꿔 표시
            randoming = true;
        }

        //아직 이동을 완료하지 않았을 경우 실행
        if (moving == false)
        {
            //만약 벽과 닿았을 경우
            if (contactWall == true)
            {
                //아래 코드를 싫행하지 않는다.
                return;
            }
            
            //현재 위치와 정해진 좌표 값의 거리를 구해 이동한다.
            transform.position = Vector3.MoveTowards(transform.position, randomPos, speed * Time.deltaTime);

            //만약 현재 좌표와 목표 좌표가 일치할 경우
            if(randomPos == transform.position)
            {
                //다시 이동이 이루어질 수 있게 불리언을 바꿔주는 함수를 부른다.
                Invoke("Switching", Random.Range(3, 6));
                //이동이 완료 되었음을 표시한다.
                moving = true;
            }
        }
    }

    public void Switching()
    {
        //다시 실행할 수 있도록 false로 바꿔준다
        moving = false;
        randoming = false;

        contactWall = false;
        loopFor = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //벽과 닿았을 경우
        if (collision.gameObject.tag == "Wall")
        {
            //현재 이동을 멈춘다.
            moving = true;
            //벽과 닿았음을 표시해준다.
            contactWall = true;

            //레이캐스트를 정면으로 2의 값으로 발사하며 범위내에 Wall이란 Layer값을 가진 것을 발견하면 해당 값을 저장한다.
            rayHitLeft = Physics2D.Raycast(rigid.position, Vector2.left, 1, LayerMask.GetMask("Wall"));
            rayHitRigth = Physics2D.Raycast(rigid.position, Vector2.right, 1, LayerMask.GetMask("Wall"));
            rayHitDown = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("Wall"));
            rayHitUp = Physics2D.Raycast(rigid.position, Vector2.up, 1, LayerMask.GetMask("Wall"));

            //배열에 해당 변수들을 담는다.
            RaycastHit2D[] raycastHit2Ds = { rayHitLeft, rayHitRigth, rayHitDown, rayHitUp };
            //변수와 함께 함수를 실행 시켜준다.
            BackStep(raycastHit2Ds);
        }
    }


    //각 위치에 따라 뒤로 물러나게 만들어주는 함수
    private void BackStep(RaycastHit2D[] raycastHit2Ds)
    {
        //최대 배열의 길이만큼, 즉 총 4번 실행한다.
        for (int i = 0; i < raycastHit2Ds.Length; i++)
        {
            //좌표 재배치가 이루어지지 않았을 경우 다시 실행한다.
            if (loopFor == false)
            {
                //배열 앞 순서 부터 차례대로 검사한다. 만약 검사 했는데 벽이 존재할 경우
                if (raycastHit2Ds[i].collider != null)
                {
                    //첫번째 배열, 즉 왼쪽에 벽이 있다
                    if (i == 0)
                    {
                        //벽과 닿아 있는 상태가 되지 않도록 오른쪽으로 약간 띄어준다.
                        transform.position = new Vector2(transform.position.x + 0.3f, transform.position.y);

                        //for문에서 코드가 한번 실행됐으므로 이를 표시해준다
                        loopFor = true;

                        //다시 이동이 이루어질 수 있게 불리언을 바꿔주는 함수를 부른다.
                        Invoke("Switching", Random.Range(3, 6));
                    }

                    //두번째 배열, 즉 오른쪽에 벽이 있다.
                    else if (i == 1)
                    {
                        if (loopFor == true) return;

                        //벽과 닿아 있는 상태가 되지 않도록 왼쪽으로 약간 띄어준다.
                        transform.position = new Vector2(transform.position.x - 0.3f, transform.position.y);

                        //for문에서 코드가 한번 실행됐으므로 이를 표시해준다
                        loopFor = true;

                        //다시 이동이 이루어질 수 있게 불리언을 바꿔주는 함수를 부른다.
                        Invoke("Switching", Random.Range(3, 6));
                    }

                    //세번째 배열, 즉 아래에 벽이 있다.
                    else if (i == 2)
                    {
                        if (loopFor == true) return;

                        //벽과 닿아 있는 상태가 되지 않도록 위쪽으로 약간 띄어준다.
                        transform.position = new Vector2(transform.position.x, transform.position.y + 0.3f);

                        //for문에서 코드가 한번 실행됐으므로 이를 표시해준다
                        loopFor = true;

                        //다시 이동이 이루어질 수 있게 불리언을 바꿔주는 함수를 부른다.
                        Invoke("Switching", Random.Range(3, 6));
                    }

                    //네번째 배열, 즉 위에 벽이 있다.
                    else if (i == 3)
                    {
                        if (loopFor == true) return;

                        //벽과 닿아 있는 상태가 되지 않도록 아래쪽으로 약간 띄어준다.
                        transform.position = new Vector2(transform.position.x, transform.position.y - 0.3f);

                        //for문에서 코드가 한번 실행됐으므로 이를 표시해준다
                        loopFor = true;

                        //다시 이동이 이루어질 수 있게 불리언을 바꿔주는 함수를 부른다.
                        Invoke("Switching", Random.Range(3, 6));
                    }
                }
            }
        }
    }
}
