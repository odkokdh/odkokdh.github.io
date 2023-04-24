using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//근접 몬스터에 대해 쓰여진 코드

public class MeleeAttackEnemy : PlayerContect
{
    Rigidbody2D rigid;
    Animator anim;

    Vector3 startPos; //자신의 시작 위치를 저장하기 위한 변수
    float xPos; //지정 범위내에 무작위로 정해진 x좌표를 저장하기 위한 변수
    float yPos; //지정 범위내에 무작위로 정해진 y좌표를 저장하기 위한 변수
    Vector3 randomPos; //무작위로 정해진 좌표 값을 저장하는 변수

    Coroutine returnMove; //코루틴 시작을 체크하는 변수

    public float patrolRanged; //자신의 배회 범위를 정하는 변수

    Vector3 endPos; //목표 지점을 저장해주는 변수

    float attackDis; //플레이어를 공격하는 거리를 저장하는 변수
    public Transform attackPos; //공격 위치를 지정해주는 위치 변수
    public Vector2 attackRange; //유니티에서 공격 범위를 설정해 줄 수 있는 변수

    private float damage; //공격을 받았을 경우 공격력 값을 저장하는 변수
    public float hp = 25f; //최대체력
    private float speed = 20f; //이동속도

    private bool moving; //자신이 현재 이동중인지 체크하는 불리언
    private bool randoming = false; //좌표값 무작위 선정을 false일 동안만 이루어지도록 만든 불리언

    private bool atkMotion; //공격을 실행했는지 확인하는 불리언

    Canvas canvas; //자식 오브젝트중 Canvas를 찾아 저장할 변수
    public GameObject hpBarBackground; //적의 체력을 구현해주는 변수
    public Image hpBarFilled; //공격을 받았을 경우 hp감소를 구현하기 위한 변수

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        startPos = transform.position; //자신의 시작 위치를 저장

        moving = false; //움직이지 않았기에 false로 지정한다
        atkMotion = false; //공격을 하지 않았기에 false로 지정한다

        //자식 오브젝트중 Canvas를 찾아 변수에 저장한다
        canvas = GetComponentInChildren<Canvas>();
        //Canvas의 월드 카메라를 플레이 씬의 mainCamera로 지정한다
        canvas.worldCamera = Camera.main;

        //맨 처음 체력은 피해를 입지 않았기에 가득 차있는 상태로 만든다
        hpBarFilled.fillAmount = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //적과 적이 부딪히거나 할때 밀림을 방지하기 위한 코드
        rigid.velocity = Vector3.zero;

        //비추적 상태이기 때문에 체력바는 안보이게 숨긴다.
        //enemyHpbarSlider.gameObject.SetActive(false);

        if(chase==false)
        {
            //체력바가 현재 구현된 상태이면
            if (hpBarBackground.activeSelf == true)
            {
                //추적이 끝난 상태이기에 안 보이게 만든다
                hpBarBackground.SetActive(false);
            }

            if (randoming == false)
            {
                //x좌표와 y좌표 값을 정해진 범위 내에 무작위로 정한뒤 Vector3 형식으로 저장한다
                xPos = Random.Range(startPos.x - patrolRanged, startPos.x + patrolRanged + 1);
                yPos = Random.Range(startPos.y - patrolRanged, startPos.y + patrolRanged + 1);
                randomPos = new Vector3(xPos, yPos, 0);

                //좌표를 무작위로 선정이 실행되었으므로 true로 변경한다
                randoming = true;
            }

            //만약 이동을 아직 실행하지 않았을 경우
            if (moving == false)
            {
                anim.SetBool("walk", true);

                //현재 자신의 위치와 위에서 정해진 무작위 좌표 값을 최종 목표로 정해진 속도 값으로 이동하게 만든다
                endPos = Vector3.MoveTowards(rigid.position, randomPos, speed * Time.deltaTime);

                //이동 지점과 현재 위치를 계산해서 0보다 클 경우 왼쪽을 향해 움직이는 것이기 때문에
                if (endPos.x - transform.position.x < 0)
                {
                    //이미지가 왼쪽을 향하게 한다
                    transform.localScale = new Vector3(-8f, 8f, 1);
                }

                //이동 지점과 현재 위치를 계산해서 0보다 클 경우 오른쪽을 향해 움직이는 것이기 때문에
                else if (endPos.x - transform.position.x > 0)
                {
                    //이미지가 오른쪽을 향하게 한다
                    transform.localScale = new Vector3(8f, 8f, 1);
                }

                //해당 위치로 이동한다
                rigid.MovePosition(endPos);

                //만약 자신의 위치와 목표 위치가 같다면
                if (transform.position == endPos)
                {
                    //이동을 완료하였기에 true로 바꿔주고
                    moving = true;

                    anim.SetBool("walk", false);

                    //랜덤한 딜레이를 줘 함수를 실행하게 한다
                    Invoke("Switching", Random.Range(3, 6));
                }
            }

            //시작 지점과 현재 지점의 거리를 계산한다
            float distanceStartNow = Vector3.Distance(transform.position, startPos);


            //비 추격 상태에서 몬스터가 정해진 배회 범위를 벗어났을 경우
            if (distanceStartNow >= 15f)
            {
                //이동이 끝났다고 표시하기 위해 불리언을 true로 변경한다
                randoming = true;
                moving = true;

                //만약 시작 위치로 옮기는 코드가 실행되지 않았을 경우
                if (returnMove == null)
                {
                    //시작위치로 옮기는 코루틴을 실행한다
                    returnMove = StartCoroutine(Return());


                }
            }

            //시작 위치와 차가 얼마 안 날 경우, 즉 시작 위치에 있을 경우 
            else if (distanceStartNow < 15)
            {
                //코루틴을 멈춘다
                StopCoroutine(Return());

                //null로 바꿔 시작위치로 이동 코드가 실행되지 않게 바꾼다
                returnMove = null;
            }
        }

        //플레이어를 추격 중일때만 실행된다
        else if (chase == true)
        {
            //만약 체력 구현이 안되어있는 상태이면
            if (hpBarBackground.activeSelf == false)
            {
                //체력바를 활성화해준다
                hpBarBackground.SetActive(true);
            }

            anim.SetBool("walk", true);

            //플레이어의 현재 위치를 목표 지점으로 삼아 움직인다
            endPos = Vector3.MoveTowards(rigid.position, target.position, speed * Time.deltaTime);

            //이동 지점과 현재 위치를 계산해서 0보다 클 경우 왼쪽을 향해 움직이는 것이기 때문에
            if (endPos.x - transform.position.x < 0)
            {
                //이미지가 왼쪽을 향하게 한다
                transform.localScale = new Vector3(-8f, 8f, 1);
            }

            //이동 지점과 현재 위치를 계산해서 0보다 클 경우 오른쪽을 향해 움직이는 것이기 때문에
            else if (endPos.x - transform.position.x > 0)
            {
                //이미지가 오른쪽을 향하게 한다
                transform.localScale = new Vector3(8f, 8f, 1);
            }

            //해당 위치로 이동한다
            rigid.MovePosition(endPos);

            //플레이어와 거리를 계산한다
            attackDis = Vector3.Distance(target.transform.position, transform.position);

            //플레이어와 거리가 일정 수치 이하라면
            if(attackDis <= 3)
            {
                //가만히 있고
                rigid.MovePosition(transform.position);

                //공격을 실행한다
                StartCoroutine(Attack());

                //이동 애니메이션을 멈춘다
                anim.SetBool("walk",false);
            }
        }
    }

    //처음 시작 위치로 되돌리는 코루틴
    public IEnumerator Return()
    {
        anim.SetBool("walk", false);

        //원래 자리 이동을 딜레이 후 실행한다
        Invoke("PosReset", 7f);

        yield return new WaitForSeconds(1f);
    }

    public void PosReset()
    {
        //자기 위치를 시작 위치로 바꾼다
        transform.position = startPos;
        //불리언을 전부 사용했다고 표시하기 위해 true로 표시
        moving = true;
        randoming = true;
        //목표지점을 시작 위치로 변경한다
        endPos = startPos;
        //다시 이동 루틴이 일어나도록 불리언 스위칭을 해준다
        Switching();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, attackRange);
    }

    //플레이어를 공격하는 모션이 실행되는 코드
    public IEnumerator Attack()
    {
        //공격이 아직 이루어지지 않았을 경우
        if(atkMotion == false)
        {
            //공격 모션을 실행하고
            anim.SetTrigger("attack");

            //플레이어 공격에 대한 코드 작성 필요
            Debug.Log("공격");
            Collider2D[] colliders = Physics2D.OverlapBoxAll(attackPos.position, attackRange, 0);

            foreach(Collider2D collider in colliders)
            {
                if(collider.tag == "Player")
                {
                    Debug.Log("플레이어 공격");
                    //플레이어에 대한 대미지 구현?
                }
            }

            //공격이 이루어졌으므로 true로 바꿔준다
            atkMotion = true;

            //다시 공격을 할 수 있도록 3초의 딜레이를 준다
            Invoke("atkDeliy", 3f);
        }

        yield return new WaitForSeconds(3f);
    }

    //3초의 쿨타임 이후 다시 atkMotion을 false로 바꾼다
    void atkDeliy()
    {
        //쿨타임이 돌아 또 다시 공격이 이루어질 수 있게 만든다
        atkMotion = false;
    }

    //불리언을 바꿔주는 함수
    public void Switching()
    {
        //다시 실행할 수 있도록 false로 바꿔준다
        moving = false;
        randoming = false;
    }

    //벽과 닿을 경우 멈추고 다른 방향을 향해 간다
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //벽과 충돌을 했을 경우
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag=="Enemy")
        {
            anim.SetBool("walk", false);

            //이동을 완료했다고 판단해 true로 바꾸고
            moving = true;
            //목표지점을 자신의 위치로 바꾼다
            //endPos = transform.position;
            //그리고 목표지점으로 이동한다
            //rigid.MovePosition(endPos);

            //이동이 다시 이루어질 수 있도록 딜레이를 줘 함수를 실행한다
            Invoke("Switching", Random.Range(3, 6));
        }

        /*
        //무기에 맞아서
        if(collision.gameObject.tag == "무기")
        {
            //어떤 무기의 공격력이 4 일경우
            damage = 4f;
            //그 공격력만큼 체력을 절감한다
            hp -= damage;
        }
        */
    }
}
