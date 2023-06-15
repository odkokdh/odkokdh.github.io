using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� ���Ϳ� ���� ������ �ڵ�

public class MeleeAttackEnemy : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    BoxCollider2D boxCollider; //�׾��� �� ���������� �Ͼ�� �ʰ� �������� �ݶ��̴� ����
    AudioSource audios;

    Vector3 startPos; //�ڽ��� ���� ��ġ�� �����ϱ� ���� ����
    float xPos; //���� �������� �������� ������ x��ǥ�� �����ϱ� ���� ����
    float yPos; //���� �������� �������� ������ y��ǥ�� �����ϱ� ���� ����
    Vector3 randomPos; //�������� ������ ��ǥ ���� �����ϴ� ����

    Coroutine returnMove; //�ڷ�ƾ ������ üũ�ϴ� ����

    public float patrolRanged; //�ڽ��� ��ȸ ������ ���ϴ� ����

    public Transform attackPos; //���� ��ġ�� �������ִ� ��ġ ����
    public Vector2 attackRange; //����Ƽ���� ���� ������ ������ �� �� �ִ� ����
    public GameObject attackObj;

    public float MaxHP = 40f; //�ִ�ü���� ����
    public float currentHP; //���� ü���� ��� ����
    public float speed; //�̵��ӵ�

    public int minMoney; //����ϴ� �� ����ġ
    public int maxMoney; //����ϴ� �� �ִ�ġ

    private bool moving; //�ڽ��� ���� �̵������� üũ�ϴ� �Ҹ���
    private bool randoming = false; //��ǥ�� ������ ������ false�� ���ȸ� �̷�������� ���� �Ҹ���
    bool dead = false; //�׾����� ���׾����� üũ�ϴ� �Ҹ���
    bool waiting = false; //���� ù ���� �� �������� ������ �����ϱ� ���� �Ҹ���
    bool returnPos = false; //������ ���۵Ǵ°��� �˷��ִ� �Ҹ���

    private bool atkMotion; //������ �����ߴ��� Ȯ���ϴ� �Ҹ���

    Canvas canvas; //�ڽ� ������Ʈ�� Canvas�� ã�� ������ ����
    public GameObject hpBarBackground; //���� ü���� �������ִ� ����
    public Image hpBarFilled; //������ �޾��� ��� hp���Ҹ� �����ϱ� ���� ����

    public AudioClip attackAudio;
    public AudioClip deadAudio;

    //�̵��� ������ �����¿�� ���� �ִ��� Ȯ���� �ϰ� ������ ��� ����
    RaycastHit2D rayHitLeft;
    RaycastHit2D rayHitRigth;
    RaycastHit2D rayHitDown;
    RaycastHit2D rayHitUp;

    bool contactWall; //���� ������ �˷��ִ� �Ҹ���
    bool loopFor; //for������ �ڵ尡 �ѹ� ������ٴ� ���� �˷��ִ� �Ҹ���

    //���� ���ݿ� ���� ����� ��Ÿ�ӿ� ���� ����
    float meleeDamageCool;
    float swordDamageCoolTime = 1f;
    float axeDamageCoolTime = 1f;
    float batDamageCoolTime = 1f;
    float panDamageCoolTime = 1f;

    //���Ÿ� ���ݿ� ���� ����� ��Ÿ�ӿ� ���� ����
    float bulletDamageCool;
    float bulletDamageCoolTime = 0.01f;

    public ItemDataBase itemDataBase;
    public GameObject medikit;
    public GameObject bullet;

    ContactSpwner parent;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        audios = GetComponent<AudioSource>();

        //�ش� ������ ��ũ��Ʈ ����� �����Ѵ�
        //noticeUI = GetComponent<NoticeUI>();

        //itemDataBase = GetComponent<ItemDataBase>();

        //�θ��� ��ũ��Ʈ�� �޾ƿ´�
        parent = GetComponentInParent<ContactSpwner>();

        startPos = transform.position; //�ڽ��� ���� ��ġ�� ����

        moving = false; //�������� �ʾұ⿡ false�� �����Ѵ�
        atkMotion = false; //������ ���� �ʾұ⿡ false�� �����Ѵ�

        //�ڽ� ������Ʈ�� Canvas�� ã�� ������ �����Ѵ�
        canvas = GetComponentInChildren<Canvas>();
        //Canvas�� ���� ī�޶� �÷��� ���� mainCamera�� �����Ѵ�
        canvas.worldCamera = Camera.main;

        //���� ü���� ���� �� ���·� ����� �ش�.
        currentHP = MaxHP;

        StartCoroutine(WaitTime()); //�ٷ� �������� �ʰ� �����ش�
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting == false) return; //��Ÿ���� ������ �ʾ��� ��� �Ʒ��� �ڵ带 �������� �ʴ´�.

        if (meleeDamageCool >= 0)
        {
            //��Ÿ�� �ð��� ���ҽ�Ų��
            meleeDamageCool -= Time.deltaTime;
        }

        if(bulletDamageCool >= 0)
        {
            //��Ÿ�� �ð��� ���ҽ�Ų��
            bulletDamageCool -= Time.deltaTime;
        }

        //���� ���� �ε����ų� �Ҷ� �и��� �����ϱ� ���� �ڵ�
        rigid.velocity = Vector3.zero;

        //���� ü�� ���¸� ���� ü�¹ٸ� �����Ѵ�.
        hpBarFilled.fillAmount = currentHP / MaxHP;

        //���� ���� ü���� 0���� �۰ԵǸ�
        if(currentHP <= 0f)
        {
            if (dead == false)
            {
                //���� ���� ���� �������� ���� ����
                int randomMoney = Random.Range(minMoney, maxMoney);

                //�ش� ��ġ��ŭ�� ���� ���� �����ڿ��� �־��ش�.
                ItemDataBase.instance.money += randomMoney;
                
                //���� ��ġ�� �����ϰ� ��´�
                int dropItem1 = Random.Range(0, 101);
                int dropItem2 = Random.Range(0, 101);

                //���� 0�̻� 20 �̸��� ���� ����� ���
                if (0 <= dropItem1 && dropItem1 < 20)
                {
                    Instantiate(bullet, transform.position, transform.rotation);
                }

                //���� 30�̻� 70�̸��� ���� ����� ���
                else if (0 <= dropItem2 && dropItem2 < 20)
                {
                    Instantiate(medikit, transform.position, transform.rotation);
                }

                audios.PlayOneShot(deadAudio);

                dead = true;
            }

            StartCoroutine(DeadEnemy());
        }

        //���� ���� ���¿��� �Ʒ� �ൿ�� �Ͼ��
        if(dead == false)
        {
            if (parent.chase==false)
            {
                //ü�¹ٰ� ���� ������ �����̸�
                if (hpBarBackground.activeSelf == true)
                {
                    //������ ���� �����̱⿡ �� ���̰� �����
                    hpBarBackground.SetActive(false);
                }

                if (returnPos == false)
                {
                    if (randoming == false)
                    {
                        //x��ǥ�� y��ǥ ���� ������ ���� ���� �������� ���ѵ� Vector3 �������� �����Ѵ�
                        xPos = Random.Range(startPos.x - patrolRanged, startPos.x + patrolRanged + 1);
                        yPos = Random.Range(startPos.y - patrolRanged, startPos.y + patrolRanged + 1);
                        randomPos = new Vector3(xPos, yPos, 0);

                        //��ǥ�� �������� ������ ����Ǿ����Ƿ� true�� �����Ѵ�
                        randoming = true;
                    }

                    //���� �̵��� ���� �������� �ʾ��� ���
                    if (moving == false)
                    {
                        //���� ���� ����� ���
                        if (contactWall == true)
                        {
                            //�Ʒ� �ڵ带 �������� �ʴ´�.
                            return;
                        }

                        anim.SetBool("walk", true);

                        //���� ��ġ�� ������ ��ǥ ���� �Ÿ��� ���� �̵��Ѵ�.
                        transform.position = Vector3.MoveTowards(transform.position, randomPos, speed * Time.deltaTime);

                        audios.Play();

                        //�̵� ������ ���� ��ġ�� ����ؼ� 0���� Ŭ ��� ������ ���� �����̴� ���̱� ������
                        if (randomPos.x - transform.position.x < 0)
                        {
                            //�̹����� ������ ���ϰ� �Ѵ�
                            transform.localScale = new Vector3(-8f, 8f, 1f);
                        }

                        //�̵� ������ ���� ��ġ�� ����ؼ� 0���� Ŭ ��� �������� ���� �����̴� ���̱� ������
                        else if (randomPos.x - transform.position.x > 0)
                        {
                            //�̹����� �������� ���ϰ� �Ѵ�
                            transform.localScale = new Vector3(8f, 8f, 1);
                        }

                        //���� ���� ��ǥ�� ��ǥ ��ǥ�� ��ġ�� ���
                        if (randomPos == transform.position)
                        {
                            anim.SetBool("walk", false);
                            //�ٽ� �̵��� �̷���� �� �ְ� �Ҹ����� �ٲ��ִ� �Լ��� �θ���.
                            Invoke("Switching", Random.Range(3, 6));
                            //�̵��� �Ϸ� �Ǿ����� ǥ���Ѵ�.
                            moving = true;
                        }
                    }
                }

                //���� ������ ���� ������ �Ÿ��� ����Ѵ�
                float distanceStartNow = Vector3.Distance(transform.position, startPos);
                    
                //�� �߰� ���¿��� ���Ͱ� ������ ��ȸ ������ ����� ���
                if (distanceStartNow >= 15f)
                {
                    returnPos = true;

                    //���� ���� ��ġ�� �ű�� �ڵ尡 ������� �ʾ��� ���
                    if (returnMove == null)
                    {
                        //������ġ�� �ű�� �ڷ�ƾ�� �����Ѵ�
                        returnMove = StartCoroutine(Return());
                    }
                }

                //���� ��ġ�� ���� �� �� �� ���, �� ���� ��ġ�� ���� ��� 
                else if (distanceStartNow < 15)
                {
                    //null�� �ٲ� ������ġ�� �̵� �ڵ尡 ������� �ʰ� �ٲ۴�
                    returnMove = null;
                }
            }

            //�÷��̾ �߰� ���϶��� ����ȴ�
            else if (parent.chase == true)
            {
                Collider2D[] colliders = Physics2D.OverlapBoxAll(attackPos.position, attackRange, 0);

                //���� ü�� ������ �ȵǾ��ִ� �����̸�
                if (hpBarBackground.activeSelf == false)
                {
                    //ü�¹ٸ� Ȱ��ȭ���ش�
                    hpBarBackground.SetActive(true);
                }

                foreach (Collider2D collider in colliders)
                {
                    if (collider.tag == "Player")
                    {
                        //������ �����Ѵ�
                        StartCoroutine(Attack());
    
                        //�̵� �ִϸ��̼��� �����
                        anim.SetBool("walk", false);
                            
                        return;
                    }
                }

                anim.SetBool("walk", true);
                    
                //�÷��̾��� ���� ��ġ�� ��ǥ �������� ��� �����δ�
                transform.position = Vector3.MoveTowards(rigid.position, parent.target.position, speed * Time.deltaTime);

                //�̵� ������ ���� ��ġ�� ����ؼ� 0���� Ŭ ��� ������ ���� �����̴� ���̱� ������
                if (parent.target.position.x - transform.position.x < 0)
                {
                    //�̹����� ������ ���ϰ� �Ѵ�
                    transform.localScale = new Vector3(-8f, 8f, 1);

                    hpBarBackground.transform.localScale = new Vector3(-1f, 1f, 1f);
                }

                //�̵� ������ ���� ��ġ�� ����ؼ� 0���� Ŭ ��� �������� ���� �����̴� ���̱� ������
                else if (parent.target.position.x - transform.position.x > 0)
                {
                    //�̹����� �������� ���ϰ� �Ѵ�
                    transform.localScale = new Vector3(8f, 8f, 1);

                    hpBarBackground.transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }
        }
    }

    public IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1f);
        waiting = true;
    }

    //ó�� ���� ��ġ�� �ǵ����� �ڷ�ƾ
    public IEnumerator Return()
    {
        anim.SetBool("walk", false);

        //�Ҹ����� ���� ����ߴٰ� ǥ���ϱ� ���� true�� ǥ��
        moving = true;
        randoming = true;

        yield return new WaitForSeconds(5f);

        //���� �ڸ� �̵��� ������ �� �����Ѵ�
        PosReset();
    }

    public void PosReset()
    {
        //���� �ٽ� �÷��̾ �����Ÿ� ������ ������ ���ڸ� �̵��� �����
        if (parent.chase == true) return;

        //�ڱ� ��ġ�� ���� ��ġ�� �ٲ۴�
        transform.position = startPos;
        
        //�ٽ� �̵� ��ƾ�� �Ͼ���� �Ҹ��� ����Ī�� ���ش�
        Switching();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, attackRange);
    }

    //�÷��̾ �����ϴ� ����� ����Ǵ� �ڵ�
    public IEnumerator Attack()
    {
        //������ ���� �̷������ �ʾ��� ���
        if(atkMotion == false)
        {
            //���� ����� �����ϰ�
            anim.SetTrigger("attack");

            audios.PlayOneShot(attackAudio);

            attackObj.SetActive(true);

            //������ �̷�������Ƿ� true�� �ٲ��ش�
            atkMotion = true;

            yield return new WaitForSeconds(0.5f);

            attackObj.SetActive(false);

            //�ٽ� ������ �� �� �ֵ��� 3���� �����̸� �ش�
            Invoke("atkDeliy", 2.5f);
        }
    }

    //3���� ��Ÿ�� ���� �ٽ� atkMotion�� false�� �ٲ۴�
    void atkDeliy()
    {
        //��Ÿ���� ���� �� �ٽ� ������ �̷���� �� �ְ� �����
        atkMotion = false;
    }

    //�Ҹ����� �ٲ��ִ� �Լ�
    public void Switching()
    {
        //�ٽ� ������ �� �ֵ��� false�� �ٲ��ش�
        moving = false;
        randoming = false;
        contactWall = false;
        loopFor = false;
        returnPos = false;
    }

    private void OnDamageSword()
    {
        anim.SetTrigger("hunted");
        //������ ����� ������ ���� ü���� ��´�.
        currentHP -= 9f;
    }

    private void OnDamageAxe()
    {
        anim.SetTrigger("hunted");
        //������ ����� ������ ���� ü���� ��´�.
        currentHP -= 16f;
    }

    private void OnDamageBat()
    {
        anim.SetTrigger("hunted");
        //������ ����� ������ ���� ü���� ��´�.
        currentHP -= 13f;
    }

    private void OnDamagePan()
    {
        anim.SetTrigger("hunted");
        //������ ����� ������ ���� ü���� ��´�.
        currentHP -= 6f;
    }

    private void OnDamageBullet()
    {
        anim.SetTrigger("hunted");
        //������ ����� ������ ���� ü���� ��´�.
        currentHP -= 5f;
    }

    private void OnDamageBullet2()
    {
        anim.SetTrigger("hunted");
        //������ ����� ������ ���� ü���� ��´�.
        currentHP -= 10f;
    }
    
    private void ONDamageSG()
    {
        anim.SetTrigger("hunted");
        //������ ����� ������ ���� ü���� ��´�.
        currentHP -= 6f;
    }

    public IEnumerator DeadEnemy()
    {
        if(dead == true)
        {
            //���� ũ�Ⱑ ū �����̱⿡ ������ ũ��� �ٲ��ش�
            transform.localScale = new Vector3(4, 4, 4);

            //���������� �Ͼ�� �ʵ���, �ε����� �̵��� �Ұ������� �ʵ��� �ݶ��̴��� ����
            boxCollider.enabled = false;

            //���� �ִϸ��̼��� �����Ѵ�.
            anim.SetTrigger("dead");

            //�ٷ� ������� �ʵ��� ��� �ð��� �д�.
            yield return new WaitForSeconds(2.7f);

            //�ش� ������Ʈ�� �ı��Ѵ�.
            Destroy(gameObject);
        }    
    }

    //���� ���� ��� ���߰� �ٸ� ������ ���� ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //���� �浹�� ���� ���
        if (collision.gameObject.tag == "Wall")
        {
            anim.SetBool("walk", false);

            //���� �̵��� �����.
            moving = true;
            //���� ������� ǥ�����ش�.
            contactWall = true;

            //����ĳ��Ʈ�� �������� 2�� ������ �߻��ϸ� �������� Wall�̶� Layer���� ���� ���� �߰��ϸ� �ش� ���� �����Ѵ�.
            rayHitLeft = Physics2D.Raycast(rigid.position, Vector2.left, 1, LayerMask.GetMask("Wall"));
            rayHitRigth = Physics2D.Raycast(rigid.position, Vector2.right, 1, LayerMask.GetMask("Wall"));
            rayHitDown = Physics2D.Raycast(rigid.position, Vector2.down, 1.5f, LayerMask.GetMask("Wall"));
            rayHitUp = Physics2D.Raycast(rigid.position, Vector2.up, 1.5f, LayerMask.GetMask("Wall"));

            //�迭�� �ش� �������� ��´�.
            RaycastHit2D[] raycastHit2Ds = { rayHitLeft, rayHitRigth, rayHitDown, rayHitUp };
            //������ �Բ� �Լ��� ���� �����ش�.
            BackStep(raycastHit2Ds);
        }

        //�÷��̾�� ����� ���
        else if(collision.gameObject.tag =="Player")
        {
            //�÷��̾ ���� �и��� �ʵ��� X���� Y���� �������� �ش�.
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�˰� ����� ���
        if(collision.tag == "Sword")
        {
            //���� ����� �Դ� �Լ��� ��Ÿ���� 0�� ���
            if(meleeDamageCool <= 0)
            {
                //������� ó���ϴ� �Լ��� �����Ѵ�.
                OnDamageSword();

                //�ѹ��� �������� ���������� ������� �Ͼ�� �ʰ� ����� ��Ÿ���� ���� ��Ų��.
                meleeDamageCool = swordDamageCoolTime;
            }
        }

        //������ ����� ���
        if (collision.tag == "Axe")
        {
            //���� ����� �Դ� �Լ��� ��Ÿ���� 0�� ���
            if (meleeDamageCool <= 0)
            {
                //������� ó���ϴ� �Լ��� �����Ѵ�.
                OnDamageAxe();

                //�ѹ��� �������� ���������� ������� �Ͼ�� �ʰ� ����� ��Ÿ���� ���� ��Ų��.
                meleeDamageCool = axeDamageCoolTime;
            }
        }

        //����̿� ����� ���
        if (collision.tag == "Bat")
        {
            //���� ����� �Դ� �Լ��� ��Ÿ���� 0�� ���
            if (meleeDamageCool <= 0)
            {
                //������� ó���ϴ� �Լ��� �����Ѵ�.
                OnDamageBat();

                //�ѹ��� �������� ���������� ������� �Ͼ�� �ʰ� ����� ��Ÿ���� ���� ��Ų��.
                meleeDamageCool = batDamageCoolTime;
            }
        }

        //�������Ұ� ����� ���
        if (collision.tag == "Pan")
        {
            //���� ����� �Դ� �Լ��� ��Ÿ���� 0�� ���
            if (meleeDamageCool <= 0)
            {
                //������� ó���ϴ� �Լ��� �����Ѵ�.
                OnDamagePan();

                //�ѹ��� �������� ���������� ������� �Ͼ�� �ʰ� ����� ��Ÿ���� ���� ��Ų��.
                meleeDamageCool = panDamageCoolTime;
            }
        }

        //HG, SMG�� �Ѿ˰� �ε����� ���
        if (collision.tag == "PlayerBullet")
        {
            if (bulletDamageCool <= 0)
            {
                //������� ó���ϴ� �Լ��� �����Ѵ�.
                OnDamageBullet();

                //�ѹ��� �������� ���������� ������� �Ͼ�� �ʰ� ����� ��Ÿ���� ���� ��Ų��.
                bulletDamageCool = bulletDamageCoolTime;
            }
        }

        //AR�� �Ѿ˰� �ε����� ���
        if(collision.tag == "PlayerBullet2")
        {
            if(bulletDamageCool <= 0)
            {
                //������� ó���ϴ� �Լ��� �����Ѵ�
                OnDamageBullet2();

                //�ѹ��� �������� ���������� ������� �Ͼ�� �ʰ� ����� ��Ÿ���� ���� ��Ų��
                bulletDamageCool = bulletDamageCoolTime;
            }
        }

        //SG�� �Ѿ˰� �ε����� ���
        if (collision.tag == "SGBullet")
        {
            //����� ó�� �Լ� ����
            ONDamageSG();
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        //�÷��̾�� �������� ���
        if(collision.gameObject.tag == "Player")
        {
            // ȸ�� Z���� �������� ȸ������ �ʵ��� �����.
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    //�� ��ġ�� ���� �ڷ� �������� ������ִ� �Լ�
    private void BackStep(RaycastHit2D[] raycastHit2Ds)
    {
        //�ִ� �迭�� ���̸�ŭ, �� �� 4�� �����Ѵ�.
        for (int i = 0; i < raycastHit2Ds.Length; i++)
        {
            //��ǥ ���ġ�� �̷������ �ʾ��� ��� �ٽ� �����Ѵ�.
            if (loopFor == false)
            {
                //�迭 �� ���� ���� ���ʴ�� �˻��Ѵ�. ���� �˻� �ߴµ� ���� ������ ���
                if (raycastHit2Ds[i].collider != null)
                {
                    //ù��° �迭, �� ���ʿ� ���� �ִ�
                    if (i == 0)
                    {
                        //���� ��� �ִ� ���°� ���� �ʵ��� ���������� �ణ ����ش�.
                        transform.position = new Vector2(transform.position.x + 0.3f, transform.position.y);

                        //for������ �ڵ尡 �ѹ� ��������Ƿ� �̸� ǥ�����ش�
                        loopFor = true;

                        //�ٽ� �̵��� �̷���� �� �ְ� �Ҹ����� �ٲ��ִ� �Լ��� �θ���.
                        Invoke("Switching", Random.Range(3, 6));
                    }

                    //�ι�° �迭, �� �����ʿ� ���� �ִ�.
                    else if (i == 1)
                    {
                        if (loopFor == true) return;

                        //���� ��� �ִ� ���°� ���� �ʵ��� �������� �ణ ����ش�.
                        transform.position = new Vector2(transform.position.x - 0.3f, transform.position.y);

                        //for������ �ڵ尡 �ѹ� ��������Ƿ� �̸� ǥ�����ش�
                        loopFor = true;

                        //�ٽ� �̵��� �̷���� �� �ְ� �Ҹ����� �ٲ��ִ� �Լ��� �θ���.
                        Invoke("Switching", Random.Range(3, 6));
                    }

                    //����° �迭, �� �Ʒ��� ���� �ִ�.
                    else if (i == 2)
                    {
                        if (loopFor == true) return;

                        //���� ��� �ִ� ���°� ���� �ʵ��� �������� �ణ ����ش�.
                        transform.position = new Vector2(transform.position.x, transform.position.y + 0.3f);

                        //for������ �ڵ尡 �ѹ� ��������Ƿ� �̸� ǥ�����ش�
                        loopFor = true;

                        //�ٽ� �̵��� �̷���� �� �ְ� �Ҹ����� �ٲ��ִ� �Լ��� �θ���.
                        Invoke("Switching", Random.Range(3, 6));
                    }

                    //�׹�° �迭, �� ���� ���� �ִ�.
                    else if (i == 3)
                    {
                        if (loopFor == true) return;

                        //���� ��� �ִ� ���°� ���� �ʵ��� �Ʒ������� �ణ ����ش�.
                        transform.position = new Vector2(transform.position.x, transform.position.y - 0.3f);

                        //for������ �ڵ尡 �ѹ� ��������Ƿ� �̸� ǥ�����ش�
                        loopFor = true;

                        //�ٽ� �̵��� �̷���� �� �ְ� �Ҹ����� �ٲ��ִ� �Լ��� �θ���.
                        Invoke("Switching", Random.Range(3, 6));
                    }
                }
            }
        }
    }
}
