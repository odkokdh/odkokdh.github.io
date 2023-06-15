using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OriginalREv2 : OriginalRPC
{
    Rigidbody2D rigid;
    Animator anim;
    BoxCollider2D boxCollider; //죽었을 때 물리엔진이 일어나지 않게 막기위한 콜라이더 변수
    AudioSource audios;

    Vector3 startPos; //자신의 시작 위치를 저장하기 위한 변수
    float xPos; //지정 범위내에 무작위로 정해진 x좌표를 저장하기 위한 변수
    float yPos; //지정 범위내에 무작위로 정해진 y좌표를 저장하기 위한 변수
    Vector3 randomPos; //무작위로 정해진 좌표 값을 저장하는 변수

    Coroutine returnMove; //코루틴 시작을 체크하는 변수

    float attackDis; //플레이어를 공격하는 거리를 저장하는 변수

    public float patrolRanged; //자신의 배회 범위를 정하는 변수

    private bool moving; //자신이 현재 이동중인지 체크하는 불리언
    private bool randoming = false; //좌표값 무작위 선정을 false일 동안만 이루어지도록 만든 불리언
    bool dead = false; //죽었는지 안죽었는지 체크하는 불리언
    bool waiting = false; //게임 첫 시작 때 움직여서 오류를 방지하기 위한 불리언
    bool returnPos = false; //리턴이 시작되는것을 알려주는 불리언

    public GameObject projectilePrefab; //원거리 공격시 투사체를 생성하게 만들어주는 변수
    public GameObject projectile; //Instantiate()메서드로 생선된 투사체를 담는 게임 오브젝트 
    public float speed;
    private float runSpeed = 18f;

    public int minMoney; //드랍하는 돈 최저치
    public int maxMoney; //드랍하는 돈 최대치

    private bool atkMotion; //공격을 실행했는지 확인하는 불리언

    public float MaxHP = 40f; //최대체력을 정함
    public float currentHP; //현재 체력을 담는 변수

    Canvas canvas; //자식 오브젝트중 Canvas를 찾아 저장할 변수
    public GameObject hpBarBackground; //적의 체력을 구현해주는 변수
    public Image hpBarFilled; //공격을 받았을 경우 hp감소를 구현하기 위한 변수

    public AudioClip attackAudio;
    public AudioClip deadAudio;

    //이동이 끝나고 상하좌우로 벽이 있는지 확인을 하고 정보를 담는 변수
    RaycastHit2D rayHitLeft;
    RaycastHit2D rayHitRigth;
    RaycastHit2D rayHitDown;
    RaycastHit2D rayHitUp;

    bool contactWall; //벽에 닿음을 알려주는 불리언
    bool loopFor; //for문에서 코드가 한번 실행됬다는 것을 알려주는 불리언

    //물리 공격에 관한 대미지 쿨타임에 대한 선언
    float meleeDamageCool;
    float swordDamageCoolTime = 1f;
    float axeDamageCoolTime = 1f;
    float batDamageCoolTime = 1f;
    float panDamageCoolTime = 1f;

    //원거리 공격에 관한 대미지 쿨타임에 대한 선언
    float bulletDamageCool;
    float bulletDamageCoolTime = 0.01f;

    public ItemDataBase itemDataBase;
    public GameObject medikit;
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        audios = GetComponent<AudioSource>();

        startPos = transform.position; //자신의 시작 위치를 저장

        moving = false; //이동하지 않았기에 false로 지정한다
        atkMotion = false; //공격을 하지 않았기에 false로 지정한다

        //자식 오브젝트중 Canvas를 찾아 변수에 저장한다
        canvas = GetComponentInChildren<Canvas>();
        //Canvas의 월드 카메라를 플레이 씬의 mainCamera로 지정한다
        canvas.worldCamera = Camera.main;

        //현재 체력은 가득 찬 상태로 만들어 준다.
        currentHP = MaxHP;

        StartCoroutine(WaitTime()); //바로 움직이지 않게 막아준다
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting == false) return; //쿨타임이 끝나지 않았을 경우 아래의 코드를 실행하지 않는다.

        if (meleeDamageCool >= 0)
        {
            //쿨타임 시간을 감소시킨다
            meleeDamageCool -= Time.deltaTime;
        }

        if (bulletDamageCool >= 0)
        {
            //쿨타임 시간을 감소시킨다
            bulletDamageCool -= Time.deltaTime;
        }

        //적과 적이 부딪히거나 할때 밀림을 방지하기 위한 코드
        rigid.velocity = Vector3.zero;

        //현재 체력 상태를 통해 체력바를 구현한다.
        hpBarFilled.fillAmount = currentHP / MaxHP;

        //만약 현재 체력이 0보다 작게되면
        if (currentHP <= 0f)
        {
            if (dead == false)
            {
                //지정 범위 값을 랜덤으로 정한 다음
                int randomMoney = Random.Range(minMoney, maxMoney);

                //해당 수치만큼의 돈을 최종 관리자에게 넣어준다.
                ItemDataBase.instance.money += randomMoney;

                //일정 수치를 랜덤하게 얻는다
                int dropItem1 = Random.Range(0, 101);
                int dropItem2 = Random.Range(0, 101);

                //만약 0이상 20 미만의 값을 얻었을 경우
                if (0 <= dropItem1 && dropItem1 < 20)
                {
                    Instantiate(bullet, transform.position, transform.rotation);
                }

                //만약 30이상 70미만의 값을 얻었을 경우
                else if (0 <= dropItem2 && dropItem2 < 40)
                {
                    //아이템 2를 생성한다
                    Instantiate(medikit, transform.position, transform.rotation);
                }

                audios.PlayOneShot(deadAudio);

                dead = true;
            }

            StartCoroutine(DeadEnemy());
        }

        if (dead == true) return; //죽은 상태일 경우 아래 코드는 실행하지 않는다.

        if (chase == false)
        {
            //체력바가 현재 구현된 상태이면
            if (hpBarBackground.activeSelf == true)
            {
                //추적이 끝난 상태이기에 안 보이게 만든다
                hpBarBackground.SetActive(false);
            }

            if (returnPos == false)
            {
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
                    //만약 벽과 닿았을 경우
                    if (contactWall == true)
                    {
                        //아래 코드를 싫행하지 않는다.
                        return;
                    }

                    anim.SetBool("walk", true);

                    //현재 위치와 정해진 좌표 값의 거리를 구해 이동한다.
                    transform.position = Vector3.MoveTowards(transform.position, randomPos, speed * Time.deltaTime);

                    audios.Play();

                    //이동 지점과 현재 위치를 계산해서 0보다 클 경우 왼쪽을 향해 움직이는 것이기 때문에
                    if (randomPos.x - transform.position.x < 0)
                    {
                        //이미지가 왼쪽을 향하게 한다
                        transform.localScale = new Vector3(8f, 8f, 1);
                    }

                    //이동 지점과 현재 위치를 계산해서 0보다 클 경우 오른쪽을 향해 움직이는 것이기 때문에
                    else if (randomPos.x - transform.position.x > 0)
                    {
                        //이미지가 오른쪽을 향하게 한다
                        transform.localScale = new Vector3(-8f, 8f, 1);
                    }

                    //만약 현재 좌표와 목표 좌표가 일치할 경우
                    if (randomPos == transform.position)
                    {
                        anim.SetBool("walk", false);
                        //다시 이동이 이루어질 수 있게 불리언을 바꿔주는 함수를 부른다.
                        Invoke("Switching", Random.Range(3, 6));
                        //이동이 완료 되었음을 표시한다.
                        moving = true;
                    }
                }
            }

            //시작 지점과 현재 지점의 거리를 계산한다
            float distanceStartNow = Vector3.Distance(transform.position, startPos);

            //비 추격 상태에서 몬스터가 정해진 배회 범위를 벗어났을 경우
            if (distanceStartNow >= 15f)
            {
                returnPos = true;

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
                //StopCoroutine(Return());

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

            //플레이어와 거리를 계산한다
            attackDis = Vector3.Distance(target.transform.position, transform.position);

            //플레이어가 너무 가까이에 있을 경우
            if (attackDis < 3)
            {
                //이동 애니메이션을 멈춘다
                anim.SetBool("walk", false);

                float dirX = target.transform.position.x - transform.position.x;
                float dirY = target.transform.position.y - transform.position.y;

                Vector3 run;

                //dirX가 음수이고 dirY가 음수면 플레이어는 왼쪽 아래에 위치
                if (dirX < 0 && dirY < 0)
                {
                    run = Vector3.MoveTowards(rigid.position, new Vector3(transform.position.x + 5, transform.position.y + 5), runSpeed * Time.deltaTime);
                    rigid.MovePosition(run);
                }

                //dirX가 양수이고 dirY가 음수면 플레이어는 오른쪽 아래에 위치
                else if (dirX > 0 && dirY < 0)
                {
                    run = Vector3.MoveTowards(rigid.position, new Vector3(transform.position.x - 5, transform.position.y + 5), runSpeed * Time.deltaTime);
                    rigid.MovePosition(run);
                }

                //dirX가 음수이고 dirY가 양수면 플레이어는 왼쪽 위에 위치
                else if (dirX < 0 && dirY > 0)
                {
                    run = Vector3.MoveTowards(rigid.position, new Vector3(transform.position.x + 5, transform.position.y - 5), runSpeed * Time.deltaTime);
                    rigid.MovePosition(run);
                }

                //dirX가 양수이고 dirY가 양수면 플레이어는 오른쪽 위에 위치
                else if (dirX > 0 && dirY > 0)
                {
                    run = Vector3.MoveTowards(rigid.position, new Vector3(transform.position.x - 5, transform.position.y - 5), runSpeed * Time.deltaTime);
                    rigid.MovePosition(run);
                }

                return;
            }

            //플레이어가 적당한 공격 범위에 있을 경우
            else if (attackDis >= 3 && attackDis <= 7)
            {
                //공격을 실행한다
                StartCoroutine(Attack());

                //이동 애니메이션을 멈춘다
                anim.SetBool("walk", false);

                return;
            }

            //플레이어를 추적을 하지만 공격 범위안에 없을 경우
            else if (attackDis > 7 && attackDis <= 10)
            {
                //플레이어의 현재 위치를 목표 지점으로 삼아 움직인다
                transform.position = Vector3.MoveTowards(rigid.position, target.position, speed * Time.deltaTime);
            }

            //만약 인지 범위 전부 벗어나버릴 경우
            else if (attackDis > 10)
            {
                //추적을 그만한다
                chase = false;

                //먼 곳에서 비추적 상태였다가 다시 추적 상태로 돌아갔을 경우를 대비한 코드
                //moving = false;
                //randoming = false;
            }

            anim.SetBool("walk", true);

            //플레이어의 현재 위치를 목표 지점으로 삼아 움직인다
            transform.position = Vector3.MoveTowards(rigid.position, target.position, speed * Time.deltaTime);

            //이동 지점과 현재 위치를 계산해서 0보다 클 경우 왼쪽을 향해 움직이는 것이기 때문에
            if (target.position.x - transform.position.x < 0)
            {
                //이미지가 왼쪽을 향하게 한다
                transform.localScale = new Vector3(8f, 8f, 1);

                hpBarBackground.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            //이동 지점과 현재 위치를 계산해서 0보다 클 경우 오른쪽을 향해 움직이는 것이기 때문에
            else if (target.position.x - transform.position.x > 0)
            {
                //이미지가 오른쪽을 향하게 한다
                transform.localScale = new Vector3(-8f, 8f, 1);

                hpBarBackground.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
    }

    public IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(2f);
        waiting = true;
    }

    //처음 시작 위치로 되돌리는 코루틴
    public IEnumerator Return()
    {
        anim.SetBool("walk", false);

        //불리언을 전부 사용했다고 표시하기 위해 true로 표시
        moving = true;
        randoming = true;

        yield return new WaitForSeconds(5f);

        //원래 자리 이동을 딜레이 후 실행한다
        PosReset();
    }

    public void PosReset()
    {
        //만약 다시 플레이어가 사정거리 안으로 들어오면 제자리 이동을 멈춘다
        if (chase == true) return;

        //자기 위치를 시작 위치로 바꾼다
        transform.position = startPos;

        //다시 이동 루틴이 일어나도록 불리언 스위칭을 해준다
        Switching();
    }


    //플레이어를 공격하는 모션이 실행되는 코드
    public IEnumerator Attack()
    {
        //공격이 아직 이루어지지 않았을 경우
        if (atkMotion == false)
        {
            //공격 모션을 실행하고
            anim.SetTrigger("attack");

            audios.PlayOneShot(attackAudio);

            //플레이어 공격에 대한 코드 작성 필요

            //Instantiate()로 투사체를 복제 생성
            projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

            //공격이 이루어졌으므로 true로 바꿔준다
            atkMotion = true;

            //다시 공격을 할 수 있도록 3초의 딜레이를 준다
            Invoke("atkDelay", 3f);
        }
        yield return new WaitForSeconds(3f);
    }

    //불리언을 바꿔주는 함수
    public void Switching()
    {
        //다시 실행할 수 있도록 false로 바꿔준다
        moving = false;
        randoming = false;
        contactWall = false;
        loopFor = false;
        returnPos = false;
    }

    //3초의 쿨타임 이후 다시 atkMotion을 false로 바꾼다
    void atkDelay()
    {
        //쿨타임이 돌아 또 다시 공격이 이루어질 수 있게 만든다
        atkMotion = false;
    }

    private void OnDamageSword()
    {
        anim.SetTrigger("hunted");
        //정해진 대미지 값으로 현재 체력을 깎는다.
        currentHP -= 9f;
    }

    private void OnDamageAxe()
    {
        anim.SetTrigger("hunted");
        //정해진 대미지 값으로 현재 체력을 깎는다.
        currentHP -= 16f;
    }

    private void OnDamageBat()
    {
        anim.SetTrigger("hunted");
        //정해진 대미지 값으로 현재 체력을 깎는다.
        currentHP -= 13f;
    }

    private void OnDamagePan()
    {
        anim.SetTrigger("hunted");
        //정해진 대미지 값으로 현재 체력을 깎는다.
        currentHP -= 6f;
    }

    private void OnDamageBullet()
    {
        anim.SetTrigger("hunted");
        //정해진 대미지 값으로 현재 체력을 깎는다.
        currentHP -= 5f;
    }

    private void OnDamageBullet2()
    {
        anim.SetTrigger("hunted");
        //정해진 대미지 값으로 현재 체력을 깎는다.
        currentHP -= 10f;
    }

    private void ONDamageSG()
    {
        anim.SetTrigger("hunted");
        //정해진 대미지 값으로 현재 체력을 깎는다.
        currentHP -= 6f;
    }

    public IEnumerator DeadEnemy()
    {
        if (dead == true)
        {
            //원래 크기가 큰 상태이기에 적당한 크기로 바꿔준다
            transform.localScale = new Vector3(4, 4, 4);

            //물리엔진이 일어나지 않도록, 부딪혀서 이동이 불가능하지 않도록 콜라이더를 끈다
            boxCollider.enabled = false;

            //폭발 애니메이션을 실행한다.
            anim.SetTrigger("dead");

            //바로 사라지지 않도록 잠시 시간을 둔다.
            yield return new WaitForSeconds(2.7f);

            //해당 오브젝트를 파괴한다.
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //벽과 충돌을 했을 경우
        if (collision.gameObject.tag == "Wall")
        {
            anim.SetBool("walk", false);

            //현재 이동을 멈춘다.
            moving = true;
            //벽과 닿았음을 표시해준다.
            contactWall = true;

            //레이캐스트를 정면으로 2의 값으로 발사하며 범위내에 Wall이란 Layer값을 가진 것을 발견하면 해당 값을 저장한다.
            rayHitLeft = Physics2D.Raycast(rigid.position, Vector2.left, 1.5f, LayerMask.GetMask("Wall"));
            rayHitRigth = Physics2D.Raycast(rigid.position, Vector2.right, 1.5f, LayerMask.GetMask("Wall"));
            rayHitDown = Physics2D.Raycast(rigid.position, Vector2.down, 1.5f, LayerMask.GetMask("Wall"));
            rayHitUp = Physics2D.Raycast(rigid.position, Vector2.up, 1.5f, LayerMask.GetMask("Wall"));

            //배열에 해당 변수들을 담는다.
            RaycastHit2D[] raycastHit2Ds = { rayHitLeft, rayHitRigth, rayHitDown, rayHitUp };
            //변수와 함께 함수를 실행 시켜준다.
            BackStep(raycastHit2Ds);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //검과 닿았을 경우
        if (collision.tag == "Sword")
        {
            //만약 대미지 입는 함수의 쿨타임이 0일 경우
            if (meleeDamageCool <= 0)
            {
                //대미지를 처리하는 함수를 실행한다.
                OnDamageSword();

                //한번의 공격으로 연속적으로 대미지가 일어나지 않게 대미지 쿨타임을 적용 시킨다.
                meleeDamageCool = swordDamageCoolTime;
            }
        }

        //도끼와 닿았을 경우
        if (collision.tag == "Axe")
        {
            //만약 대미지 입는 함수의 쿨타임이 0일 경우
            if (meleeDamageCool <= 0)
            {
                //대미지를 처리하는 함수를 실행한다.
                OnDamageAxe();

                //한번의 공격으로 연속적으로 대미지가 일어나지 않게 대미지 쿨타임을 적용 시킨다.
                meleeDamageCool = axeDamageCoolTime;
            }
        }

        //방망이와 닿았을 경우
        if (collision.tag == "Bat")
        {
            //만약 대미지 입는 함수의 쿨타임이 0일 경우
            if (meleeDamageCool <= 0)
            {
                //대미지를 처리하는 함수를 실행한다.
                OnDamageBat();

                //한번의 공격으로 연속적으로 대미지가 일어나지 않게 대미지 쿨타임을 적용 시킨다.
                meleeDamageCool = batDamageCoolTime;
            }
        }

        //프라이팬과 닿았을 경우
        if (collision.tag == "Pan")
        {
            //만약 대미지 입는 함수의 쿨타임이 0일 경우
            if (meleeDamageCool <= 0)
            {
                //대미지를 처리하는 함수를 실행한다.
                OnDamagePan();

                //한번의 공격으로 연속적으로 대미지가 일어나지 않게 대미지 쿨타임을 적용 시킨다.
                meleeDamageCool = panDamageCoolTime;
            }
        }

        //HG, SMG, SG의 총알과 부딪혔을 경우
        if (collision.tag == "PlayerBullet")
        {
            if (bulletDamageCool <= 0)
            {
                //대미지를 처리하는 함수를 실행한다.
                OnDamageBullet();

                //한번의 공격으로 연속적으로 대미지가 일어나지 않게 대미지 쿨타임을 적용 시킨다.
                bulletDamageCool = bulletDamageCoolTime;
            }
        }

        //AR의 총알과 부딪혔을 경우
        if (collision.tag == "PlayerBullet2")
        {
            if (bulletDamageCool <= 0)
            {
                //대미지를 처리하는 함수를 실행한다
                OnDamageBullet2();

                //한번의 공격으로 연속적으로 대미지가 일어나지 않게 대미지 쿨타임을 적용 시킨다
                bulletDamageCool = bulletDamageCoolTime;
            }
        }

        //SG의 총알과 부딪혔을 경우
        if (collision.tag == "SGBullet")
        {
            //대미지 처리 함수 실행
            ONDamageSG();
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
