using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OriginalMEE : OriginalPC
{
    Rigidbody2D rigid;
    Animator anim;
    BoxCollider2D boxCollider; //�׾��� �� ���������� �Ͼ�� �ʰ� �������� �ݶ��̴� ����
    AudioSource audios;

    Vector3 startPos; //�ڽ��� ���� ��ġ�� �����ϱ� ���� ����

    Coroutine returnMove; //�ڷ�ƾ ������ üũ�ϴ� ����

    public float patrolRanged; //�ڽ��� ��ȸ ������ ���ϴ� ����

    Vector3 endPos; //��ǥ ������ �������ִ� ����

    public Transform attackPos; //���� ��ġ�� �������ִ� ��ġ ����
    public Vector2 attackRange; //����Ƽ���� ���� ������ ������ �� �� �ִ� ����
    public GameObject attackObj;

    bool dead = false; //�׾����� ���׾����� üũ�ϴ� �Ҹ���
    //bool returnPos = false; //������ ���۵Ǵ°��� �˷��ִ� �Ҹ���
    bool waiting = false; //���� ù ���� �� �������� ������ �����ϱ� ���� �Ҹ���

    public float MaxHP = 60f; //�ִ�ü���� ����
    public float currentHP; //���� ü���� ��� ����
    public float speed;  //�̵��ӵ�

    public int minMoney; //����ϴ� �� ����ġ
    public int maxMoney; //����ϴ� �� �ִ�ġ

    private bool atkMotion; //������ �����ߴ��� Ȯ���ϴ� �Ҹ���

    Canvas canvas; //�ڽ� ������Ʈ�� Canvas�� ã�� ������ ����
    public GameObject hpBarBackground; //���� ü���� �������ִ� ����
    public Image hpBarFilled; //������ �޾��� ��� hp���Ҹ� �����ϱ� ���� ����

    public AudioClip attackAudio;
    public AudioClip deadAudio;

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

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        audios = GetComponent<AudioSource>();

        startPos = transform.position; //�ڽ��� ���� ��ġ�� ����

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

        if (bulletDamageCool >= 0)
        {
            //��Ÿ�� �ð��� ���ҽ�Ų��
            bulletDamageCool -= Time.deltaTime;
        }

        //���� ���� �ε����ų� �Ҷ� �и��� �����ϱ� ���� �ڵ�
        rigid.velocity = Vector3.zero;

        //���� ü�� ���¸� ���� ü�¹ٸ� �����Ѵ�.
        hpBarFilled.fillAmount = currentHP / MaxHP;

        //���� ���� ü���� 0���� �۰ԵǸ�
        if (currentHP <= 0f)
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
                else if (0 <= dropItem2 && dropItem2 < 40)
                {
                    //������ 2�� �����Ѵ�
                    Instantiate(medikit, transform.position, transform.rotation);
                }

                audios.PlayOneShot(deadAudio);

                dead = true;
            }

            StartCoroutine(DeadEnemy());
        }

        if (dead == true) return; //���� ������ ��� �Ʒ� �ڵ�� �������� �ʴ´�.

        if (chase == false)
        {
            anim.SetBool("walk", false);

            //ü�¹ٰ� ���� ������ �����̸�
            if (hpBarBackground.activeSelf == true)
            {
                //������ ���� �����̱⿡ �� ���̰� �����
                hpBarBackground.SetActive(false);
            }

            //���� ������ ���� ������ �Ÿ��� ����Ѵ�
            float distanceStartNow = Vector3.Distance(transform.position, startPos);

            //�� �߰� ���¿��� ���Ͱ� ������ ��ȸ ������ ����� ���
            if (distanceStartNow >= 15f)
            {
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
                //�ڷ�ƾ�� �����
                StopCoroutine(Return());

                //null�� �ٲ� ������ġ�� �̵� �ڵ尡 ������� �ʰ� �ٲ۴�
                returnMove = null;
            }
        }

        //�÷��̾ �߰� ���϶��� ����ȴ�
        else if (chase == true)
        {
            //�߰� ���� �� ������ ��ġ�� ������ ������ ����� �� ������Ʈ�� Ž���ϴ� �ڽ��� �����
            Collider2D[] colliders = Physics2D.OverlapBoxAll(attackPos.position, attackRange, 0);

            //���� ü�� ������ �ȵǾ��ִ� �����̸�
            if (hpBarBackground.activeSelf == false)
            {
                //ü�¹ٸ� Ȱ��ȭ���ش�
                hpBarBackground.SetActive(true);
            }

            anim.SetBool("walk", true);

            //�÷��̾��� ���� ��ġ�� ��ǥ �������� ��� �����δ�
            transform.position = Vector3.MoveTowards(rigid.position, target.position, speed * Time.deltaTime);

            //�̵� ������ ���� ��ġ�� ����ؼ� 0���� Ŭ ��� ������ ���� �����̴� ���̱� ������
            if (target.position.x - transform.position.x < 0)
            {
                //�̹����� ������ ���ϰ� �Ѵ�
                transform.localScale = new Vector3(20f, 20f, 1);

                hpBarBackground.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            //�̵� ������ ���� ��ġ�� ����ؼ� 0���� Ŭ ��� �������� ���� �����̴� ���̱� ������
            else if (target.position.x - transform.position.x > 0)
            {
                //�̹����� �������� ���ϰ� �Ѵ�
                transform.localScale = new Vector3(-20f, 20f, 1);

                hpBarBackground.transform.localScale = new Vector3(-1f, 1f, 1f);
            }

            //����Ʈ �ȿ� �־��� ������Ʈ���� �ϳ��� Ž���Ѵ�
            foreach (Collider2D collider in colliders)
            {
                //���� �÷��̾�� ����� ���
                if (collider.tag == "Player")
                {
                    //������ �ְ�
                    rigid.MovePosition(transform.position);

                    //������ �����Ѵ�
                    StartCoroutine(Attack());

                    //�̵� �ִϸ��̼��� �����
                    anim.SetBool("walk", false);
                }
            }
        }
    }

    public IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(2f);
        waiting = true;
    }

    //ó�� ���� ��ġ�� �ǵ����� �ڷ�ƾ
    public IEnumerator Return()
    {
        anim.SetBool("walk", false);

        yield return new WaitForSeconds(5f);

        //���� �ڸ� �̵��� ������ �� �����Ѵ�
        PosReset();
    }

    public void PosReset()
    {
        //���� �ٽ� �÷��̾ �����Ÿ� ������ ������ ���ڸ� �̵��� �����
        if (chase == true) return;

        //�ڱ� ��ġ�� ���� ��ġ�� �ٲ۴�
        transform.position = startPos;
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
        if (atkMotion == false)
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
        if (dead == true)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�˰� ����� ���
        if (collision.tag == "Sword")
        {
            //���� ����� �Դ� �Լ��� ��Ÿ���� 0�� ���
            if (meleeDamageCool <= 0)
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

        //HG, SMG, SG�� �Ѿ˰� �ε����� ���
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
        if (collision.tag == "PlayerBullet2")
        {
            if (bulletDamageCool <= 0)
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //�÷��̾�� ����� ���
        if (collision.gameObject.tag == "Player")
        {
            //�÷��̾ ���� �и��� �ʵ��� X���� Y���� �������� �ش�.
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        //�÷��̾�� �������� ���
        if (collision.gameObject.tag == "Player")
        {
            // ȸ�� Z���� �������� ȸ������ �ʵ��� �����.
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
