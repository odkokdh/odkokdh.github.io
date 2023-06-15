using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeElite : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    BoxCollider2D boxCollider; //죽었을 때 물리엔진이 일어나지 않게 막기위한 콜라이더 변수
    AudioSource audios;

    Vector3 startPos; //자신의 시작 위치를 저장하기 위한 변수

    Coroutine returnMove; //코루틴 시작을 체크하는 변수

    public float patrolRanged; //자신의 배회 범위를 정하는 변수

    public Transform attackPos; //공격 위치를 지정해주는 위치 변수
    public Vector2 attackRange; //유니티에서 공격 범위를 설정해 줄 수 있는 변수
    public GameObject attackObj;

    bool dead = false; //죽었는지 안죽었는지 체크하는 불리언

    public float MaxHP = 60f; //최대체력을 정함
    public float currentHP; //현재 체력을 담는 변수
    public float speed;  //이동속도

    public int minMoney; //드랍하는 돈 최저치
    public int maxMoney; //드랍하는 돈 최대치

    private bool atkMotion; //공격을 실행했는지 확인하는 불리언

    Canvas canvas; //자식 오브젝트중 Canvas를 찾아 저장할 변수
    public GameObject hpBarBackground; //적의 체력을 구현해주는 변수
    public Image hpBarFilled; //공격을 받았을 경우 hp감소를 구현하기 위한 변수

    public AudioClip attackAudio;
    public AudioClip deadAudio;

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

    ContactSpwner parent;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        audios = GetComponent<AudioSource>();

        //부모의 스크립트를 받아온다
        parent = GetComponentInParent<ContactSpwner>();

        startPos = transform.position; //자신의 시작 위치를 저장

        atkMotion = false; //공격을 하지 않았기에 false로 지정한다

        //자식 오브젝트중 Canvas를 찾아 변수에 저장한다
        canvas = GetComponentInChildren<Canvas>();
        //Canvas의 월드 카메라를 플레이 씬의 mainCamera로 지정한다
        canvas.worldCamera = Camera.main;

        //현재 체력은 가득 찬 상태로 만들어 준다.
        currentHP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
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
                else if (30 <= dropItem2 && dropItem2 < 49)
                {
                    Instantiate(medikit, transform.position, transform.rotation);
                }

                audios.PlayOneShot(deadAudio);

                dead = true;
            }

            StartCoroutine(DeadEnemy());
        }

        if (dead == true) return; //죽은 상태일 경우 아래 코드는 실행하지 않는다.

        if (parent.chase == false)
        {
            anim.SetBool("walk", false);

            //체력바가 현재 구현된 상태이면
            if (hpBarBackground.activeSelf == true)
            {
                //추적이 끝난 상태이기에 안 보이게 만든다
                hpBarBackground.SetActive(false);
            }
            
            //시작 지점과 현재 지점의 거리를 계산한다
            float distanceStartNow = Vector3.Distance(transform.position, startPos);

            //비 추격 상태에서 몬스터가 정해진 배회 범위를 벗어났을 경우
            if (distanceStartNow >= 15f)
            {
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
        else if (parent.chase == true)
        {
            //추격 중일 때 정해진 위치와 정해진 범위로 닿았을 때 오브젝트를 탐색하는 박스를 만든다
            Collider2D[] colliders = Physics2D.OverlapBoxAll(attackPos.position, attackRange, 0);

            //만약 체력 구현이 안되어있는 상태이면
            if (hpBarBackground.activeSelf == false)
            {
                //체력바를 활성화해준다
                hpBarBackground.SetActive(true);
            }

            anim.SetBool("walk", true);

            //플레이어의 현재 위치를 목표 지점으로 삼아 움직인다
            transform.position = Vector3.MoveTowards(rigid.position, parent.target.position, speed * Time.deltaTime);

            //이동 지점과 현재 위치를 계산해서 0보다 클 경우 왼쪽을 향해 움직이는 것이기 때문에
            if (parent.target.position.x - transform.position.x < 0)
            {
                //이미지가 왼쪽을 향하게 한다
                transform.localScale = new Vector3(20f, 20f, 1);

                hpBarBackground.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            //이동 지점과 현재 위치를 계산해서 0보다 클 경우 오른쪽을 향해 움직이는 것이기 때문에
            else if (parent.target.position.x - transform.position.x > 0)
            {
                //이미지가 오른쪽을 향하게 한다
                transform.localScale = new Vector3(-20f, 20f, 1);

                hpBarBackground.transform.localScale = new Vector3(-1f, 1f, 1f);
            }

            //리스트 안에 넣어진 오브젝트들을 하나씩 탐색한다
            foreach(Collider2D collider in colliders)
            {
                //만약 플레이어와 닿았을 경우
                if (collider.tag == "Player")
                {
                    //가만히 있고
                    rigid.MovePosition(transform.position);

                    //공격을 실행한다
                    StartCoroutine(Attack());

                    //이동 애니메이션을 멈춘다
                    anim.SetBool("walk", false);
                }
            }
        }
    }

    //처음 시작 위치로 되돌리는 코루틴
    public IEnumerator Return()
    {
        anim.SetBool("walk", false);

        yield return new WaitForSeconds(5f);

        //원래 자리 이동을 딜레이 후 실행한다
        PosReset();
    }

    public void PosReset()
    {
        //만약 다시 플레이어가 사정거리 안으로 들어오면 제자리 이동을 멈춘다
        if (parent.chase == true) return;

        //자기 위치를 시작 위치로 바꾼다
        transform.position = startPos;
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
        if (atkMotion == false)
        {
            //공격 모션을 실행하고
            anim.SetTrigger("attack");

            attackObj.SetActive(true);

            //공격이 이루어졌으므로 true로 바꿔준다
            atkMotion = true;

            yield return new WaitForSeconds(0.5f);

            attackObj.SetActive(false);

            //다시 공격을 할 수 있도록 3초의 딜레이를 준다
            Invoke("atkDeliy", 2.5f);
        }
        
    }

    //3초의 쿨타임 이후 다시 atkMotion을 false로 바꾼다
    void atkDeliy()
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //플레이어와 닿았을 경우
        if (collision.gameObject.tag == "Player")
        {
            //플레이어에 의해 밀리지 않도록 X값과 Y값을 고정시켜 준다.
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        //플레이어와 떨어졌을 경우
        if (collision.gameObject.tag == "Player")
        {
            // 회전 Z값만 고정시켜 회전하지 않도록 만든다.
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
