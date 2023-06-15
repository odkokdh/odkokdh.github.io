using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
	public static Player instance;

	//이동
    public static float moveSpeed = 8.0f;
	public Rigidbody2D rb;
	public Vector2 vector;
	public Vector2 playerPos;

	private bool moveCorrect = true;


	//애니메이션
	Animator anim;
	SpriteRenderer spriteRenderer;

	//원거리공격
	public GameObject PistolSpawnPos;
	public GameObject SMGSpawnPos;
	public GameObject ARSpawnPos;
	public GameObject SGSpawnPos;

	public GameObject PistolBullet;
	public GameObject SMGBullet;
	public GameObject ARBullet;
	public GameObject SGBullet;
	public Vector3 dir;
	public Vector3 ShootLocation;
	Camera cam;

	private float PistolCool;
	private float PistolCoolLeft = 1.0f;
	private float SMGCool;
	private float SMGCoolLeft = 0.5f;
	private float ARCool;
	private float ARCoolLeft = 1.0f;
	private float SGCool;
	private float SGCoolLeft = 1.0f;

	//원거리무기
	public GameObject Pistol;
	public GameObject SMG;
	public GameObject AR;
	public GameObject SG;

	public GameObject SGB1;
	public GameObject SGB2;
	public GameObject SGB3;
	public GameObject SGB4;

	public GameObject PF;
	public GameObject SMGF;
	public GameObject ARF;
	public GameObject SGF;

	private bool isPistol = false;
	private bool isSMG = false;
	private bool isAR = false;
	private bool isSG = false;

	public int bullet = 0;
	public Text bulletAmountUI;

	//근거리공격
	public bool isAttacking = false;

	private float SwordCool;
	private float SwordCoolLeft = 1.0f;
	private float AxeCool;
	private float AxeCoolLeft = 2.5f;
	private float BatCool;
	private float BatCoolLeft = 1.75f;
	private float PanCool;
	private float PanCoolLeft = 0.5f;

	public GameObject SwordLeft;
	public GameObject SwordLeftAttackBox;
	public GameObject SwordRight;
	public GameObject SwordRightAttackBox;
	public GameObject SwordUp;
	public GameObject SwordUpAttackBox;
	public GameObject SwordDown;
	public GameObject SwordDownAttackBox;


	public GameObject Axe;
	public GameObject AxeAttackBox;
	public GameObject Bat;
	public GameObject BatAttackBox;
	public GameObject Pan;
	public GameObject PanAttackBox;

	private bool isSword = false;
	private bool isAxe = false;
	private bool isBat = false;
	private bool isPan = false;

	private IEnumerator PanCoroutine;

	//피격
	private float HitDelay;
	private float HitDelayLeft = 0.3f;
	private bool isHit = false;

	//구르기
	private bool isInvincible = false;
	private float invincibleCool;
	private float invincibleLeft = 1.8f;

	//회복
	public int mediKitAmount = 0;
	public Text mediKitAmountUI;

	private bool isRAZero = false;

	//방독면
	public bool mask = false;
	public float maskDurability = 100;
	public GameObject maskUI;
	private bool stopMask;

	private IEnumerator RACoroutine;
	private IEnumerator RAHPCoroutine;
	private IEnumerator PistolCoroutine;
	private IEnumerator SMGCoroutine;
	private IEnumerator ARCoroutine;
	private IEnumerator SGCoroutine;

	private IEnumerator MaskCoroutine;

	//현재근거리무기
	public List<Item> meleeWeapon = new List<Item>();

	//현재원거리무기
	public List<Item> rangedWeapon = new List<Item>();


	void Awake()
	{
		instance = this;
	}


	void Start()
    {
		//컴포넌트가져오기
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		cam = Camera.main;
		spriteRenderer = GetComponent<SpriteRenderer>();

		SwordLeft.gameObject.SetActive(false);
		SwordRight.gameObject.SetActive(false);
		SwordUp.gameObject.SetActive(false);
		SwordDown.gameObject.SetActive(false);

		Axe.gameObject.SetActive(false);
		Bat.gameObject.SetActive(false);
		Pan.gameObject.SetActive(false);

		SwordLeftAttackBox.gameObject.SetActive(false);
		SwordRightAttackBox.gameObject.SetActive(false);
		SwordUpAttackBox.gameObject.SetActive(false);
		SwordDownAttackBox.gameObject.SetActive(false);

		AxeAttackBox.gameObject.SetActive(false);
		BatAttackBox.gameObject.SetActive(false);
		PanAttackBox.gameObject.SetActive(false);

		Pistol.gameObject.SetActive(false);
		SMG.gameObject.SetActive(false);
		AR.gameObject.SetActive(false);
		SG.gameObject.SetActive(false);

		PF.gameObject.SetActive(false);
		SMGF.gameObject.SetActive(false);
		ARF.gameObject.SetActive(false);
		SGF.gameObject.SetActive(false);

		bullet = 0;

		isPistol = true;
		isSword = true;

		RACoroutine = RA(1);
		RAHPCoroutine = RAHPDecrease(3);

		PanCoroutine = PanSwing();

		PistolCoroutine = PistolShooting();
		SMGCoroutine = SMGShooting();
		ARCoroutine = ARShooting();
		SGCoroutine = SGShooting();
		MaskCoroutine = MaskActive();

		mask = false;

		StartCoroutine(RAAutoDecrease());
	}

    void Update()
    {
		//플레이어 이동
		Move();

		//플레이어 구르기
		Roll();

		MeleeAttack();

		dir = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z));

		playerPos = this.gameObject.transform.position;

		RangedAttack();

		PlayerHit();

		Heal();

		mediKitAmountUI.text = mediKitAmount.ToString();
		bulletAmountUI.text = bullet.ToString();

		if (maskDurability <= 0)
		{
			Debug.Log("마스크 끝");
			stopMask = true;
			maskUI.gameObject.SetActive(false);
			mask = false;
			maskDurability = 100;
		}
	}

	/*
   public void PistolSwitching()
   {
      isPistol = true;
      isSMG = false;
        isAR = false;
      isSG = false;
    }
   */
	public void SMGSwitching()
	{
		isPistol = false;
		isSMG = true;
		isAR = false;
		isSG = false;
		rangedWeapon[0] = ItemDataBase.instance.itemDB[1];
	}

	public void ARSwitching()
	{
		isPistol = false;
		isSMG = false;
		isAR = true;
		isSG = false;
		rangedWeapon[0] = ItemDataBase.instance.itemDB[4];
	}

	public void SGSwitching()
	{
		isPistol = false;
		isSMG = false;
		isAR = false;
		isSG = true;
		rangedWeapon[0] = ItemDataBase.instance.itemDB[5];
	}
	/*
	public void SwordSwitching()
	{
	   isSword = true;
	   isAxe = false;
	   isBat = false;
	   isPan = false;
	 }
	*/
	public void AxeSwitching()
	{
		isSword = false;
		isAxe = true;
		isBat = false;
		isPan = false;
		meleeWeapon[0] = ItemDataBase.instance.itemDB[6];
	}

	public void BatSwitching()
	{
		isSword = false;
		isAxe = false;
		isBat = true;
		isPan = false;
		meleeWeapon[0] = ItemDataBase.instance.itemDB[7];
	}

	public void PanSwitching()
	{
		isSword = false;
		isAxe = false;
		isBat = false;
		isPan = true;
	}



	private void Move()
	{
		vector.x = Input.GetAxisRaw("Horizontal");
		vector.y = Input.GetAxisRaw("Vertical");

		rb.velocity = vector * moveSpeed;

		if (vector.x > 0)
		{
			transform.eulerAngles = new Vector3(0, 180, 0);
			spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
			moveCorrect = false;
		}

		else if (vector.x < 0)
		{
			transform.eulerAngles = new Vector3(0, 0, 0);
			moveCorrect = true;
		}

		if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
		{
			anim.SetBool("Side", true);
		}

		else
		{
			anim.SetBool("Side", false);
		}
	}

	private void Roll()
	{
		if (invincibleCool <= 0)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (isInvincible == false)
				{
					StartCoroutine(Rolling());
					Debug.Log("구르기");

					invincibleCool = invincibleLeft;
				}
			}
		}

		else
		{
			invincibleCool -= Time.deltaTime;
		}

	}



	private void MeleeAttack()
	{
		if(isSword == true)
		{
			if (SwordCool <= 0)
			{
				if (Input.GetMouseButtonDown(0))
				{
					float GetAngle(Vector3 start, Vector3 end)
					{
						Vector2 v2 = end - start;
						return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
					}

					if (GetAngle(playerPos, dir) > 30 && GetAngle(playerPos, dir) < 150)
					{
						StartCoroutine(SwordUpSwing());
					}

					else if (GetAngle(playerPos, dir) > -150 && GetAngle(playerPos, dir) < -30)
					{
						StartCoroutine(SwordDownSwing());
					}

					else if (GetAngle(playerPos, dir) >= 0 && GetAngle(playerPos, dir) < 30)
					{
						if (moveCorrect == true)
						{
							StartCoroutine(SwordRightSwing());
						}

						else if (moveCorrect == false)
						{
							StartCoroutine(SwordLeftSwing());
						}
					}

					else if (GetAngle(playerPos, dir) > -30 && GetAngle(playerPos, dir) <= 0)
					{
						if (moveCorrect == true)
						{
							StartCoroutine(SwordRightSwing());
						}

						else if (moveCorrect == false)
						{
							StartCoroutine(SwordLeftSwing());
						}
					}

					else if (GetAngle(playerPos, dir) > 150 && GetAngle(playerPos, dir) <= 180)
					{
						if (moveCorrect == true)
						{
							StartCoroutine(SwordLeftSwing());
						}

						else if (moveCorrect == false)
						{
							StartCoroutine(SwordRightSwing());
						}
					}

					else if (GetAngle(playerPos, dir) >= -180 && GetAngle(playerPos, dir) < -150)
					{
						if (moveCorrect == true)
						{
							StartCoroutine(SwordLeftSwing());
						}

						else if (moveCorrect == false)
						{
							StartCoroutine(SwordRightSwing());
						}
					}

					SwordCool = SwordCoolLeft;
				}
			}

			else
			{
				SwordCool -= Time.deltaTime;
			}
		}

		if(isAxe == true)
		{
			if (AxeCool <= 0)
			{
				if (Input.GetMouseButtonDown(0))
				{
					StartCoroutine(AxeSwing());

					AxeCool = AxeCoolLeft;
				}
			}

			else
			{
				AxeCool -= Time.deltaTime;
			}
		}

		if (isBat == true)
		{
			if (BatCool <= 0)
			{
				if (Input.GetMouseButtonDown(0))
				{
					StartCoroutine(BatSwing());

					BatCool = BatCoolLeft;
				}
			}

			else
			{
				BatCool -= Time.deltaTime;
			}
		}

		if (isPan == true)
		{
			if (PanCool <= 0)
			{
				if (Input.GetMouseButtonDown(0))
				{
					Pan.gameObject.SetActive(true);
					
					StartCoroutine(PanCoroutine);

					PanCool = PanCoolLeft;
				}
			}

			if (Input.GetMouseButtonUp(0))
			{
				StopCoroutine(PanCoroutine);
				Pan.gameObject.SetActive(false);
			}

			else
			{
				PanCool -= Time.deltaTime;
			}
		}
	}

	private void RangedAttack()
	{
		if(bullet > 0)
		{
			if (isPistol == true)
			{
				if (PistolCool <= 0) //남은 쿨타임이 0일때
				{
					if (Input.GetMouseButtonDown(1))    //우클릭시
					{
						bullet -= 1;
						StartCoroutine(PistolShooting());

						PistolCool = PistolCoolLeft;  //쿨타임 적용
					}
				}

				else    //쿨타임이 아직 도는 중이라면
				{
					PistolCool -= Time.deltaTime;    //쿨타임 감소
				}
			}

			if (isSMG == true)
			{
				if (SMGCool <= 0)
				{
					if (Input.GetMouseButtonDown(1))    //우클릭시
					{
						SMG.gameObject.SetActive(true);
						StartCoroutine(SMGCoroutine);

						SMGCool = SMGCoolLeft;
					}
				}

				if (Input.GetMouseButtonUp(1))
				{
					StopCoroutine(SMGCoroutine);
					SMG.gameObject.SetActive(false);
				}

				else    //쿨타임이 아직 도는 중이라면
				{
					SMGCool -= Time.deltaTime;    //쿨타임 감소
				}
			}

			if (isAR == true)
			{
				if (ARCool <= 0)
				{
					if (Input.GetMouseButtonDown(1))    //우클릭시
					{
						AR.gameObject.SetActive(true);
						StartCoroutine(ARCoroutine);

						ARCool = ARCoolLeft;
					}
				}

				if (Input.GetMouseButtonUp(1))
				{
					StopCoroutine(ARCoroutine);
					AR.gameObject.SetActive(false);
				}

				else    //쿨타임이 아직 도는 중이라면
				{
					ARCool -= Time.deltaTime;    //쿨타임 감소
				}
			}

			if (isSG == true)
			{
				if (SGCool <= 0) //남은 쿨타임이 0일때
				{
					if (Input.GetMouseButtonDown(1))    //우클릭시
					{
						StartCoroutine(SGShooting());

						SGCool = SGCoolLeft;  //쿨타임 적용
					}
				}

				else    //쿨타임이 아직 도는 중이라면
				{
					SGCool -= Time.deltaTime;    //쿨타임 감소
				}
			}
		}

		else
		{
			if (Input.GetMouseButtonDown(1))
			{
				Debug.Log("총알이 부족합니다.");
			}
		}
	}





	private void PlayerHit()
	{
		if(HitDelay <= 0)
		{
			if(isHit == true)
			{
				if(isInvincible == false)
				{
					PlayerHP.instance.currentHP -= 1;

					HitDelay = HitDelayLeft;
				}
			}
		}

		else
		{
			HitDelay -= Time.deltaTime;
		}
	}

	private void Heal()
	{
		if (Input.GetKeyDown(KeyCode.H))
		{
			if(PlayerHP.instance.currentHP < PlayerHP.instance.HPMax)
			{
				if (mediKitAmount > 0)
				{
					Debug.Log("메디킷을 사용했다.");
					PlayerHP.instance.currentHP += 1;
					mediKitAmount -= 1;
				}

				else
				{
					Debug.Log("메디킷이 부족합니다 !");
				}
			}

			else if(PlayerHP.instance.currentHP == PlayerHP.instance.HPMax)
			{
				Debug.Log("이미 체력이 완전히 회복되어 있습니다.");
			}
			
		}
	}



	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			Debug.Log("충돌");
			StartCoroutine(Hitted());
		}

		if (other.gameObject.CompareTag("MediKit"))
		{
			Debug.Log("메디킷 획득");
			mediKitAmount += 1;
		}

		if (other.gameObject.tag == "RAMedicine")
		{
			RAPillUse();
			Destroy(other.gameObject);
		}

		if (other.gameObject.CompareTag("Ammo"))
		{
			Debug.Log("탄약 획득");
			bullet += 10;
			Destroy(other.gameObject);
		}

		if (other.gameObject.CompareTag("Mask"))
		{
			Debug.Log("마스크 획득");
			MaskUse();
			Destroy(other.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "RA_Fog")
		{
			StartCoroutine(RACoroutine);
		}

		if (collision.tag == "Enemy")
		{
			Debug.Log("충돌");
			StartCoroutine(Hitted());
		}


	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "RA_Fog")
		{
			Debug.Log("방사능 끝");
			StopCoroutine(RACoroutine);
			StopCoroutine(RAHPCoroutine);
		}
	}



	public void MaskUse()
	{
		mask = true;
		stopMask = false;
		MaskCoroutine = MaskActive();
		StartCoroutine(MaskCoroutine);
		maskUI.gameObject.SetActive(true);
	}

	public void RAPillUse()
	{
		PlayerRA.instance.currentRA += 30;
	}



	IEnumerator SwordLeftSwing()
	{
		SwordLeft.gameObject.SetActive(true);
		SwordLeftAttackBox.gameObject.SetActive(true);
		isAttacking = true;
		yield return new WaitForSecondsRealtime(0.5f);
		isAttacking = false;
		SwordLeft.gameObject.SetActive(false);
		SwordLeftAttackBox.gameObject.SetActive(false);
	}

	IEnumerator SwordRightSwing()
	{
		SwordRight.gameObject.SetActive(true);
		SwordRightAttackBox.gameObject.SetActive(true);
		isAttacking = true;
		yield return new WaitForSecondsRealtime(0.5f);
		isAttacking = false;
		SwordRight.gameObject.SetActive(false);
		SwordRightAttackBox.gameObject.SetActive(false);
	}

	IEnumerator SwordUpSwing()
	{
		SwordUp.gameObject.SetActive(true);
		SwordUpAttackBox.gameObject.SetActive(true);
		isAttacking = true;
		yield return new WaitForSecondsRealtime(0.5f);
		isAttacking = false;
		SwordUp.gameObject.SetActive(false);
		SwordUpAttackBox.gameObject.SetActive(false);
	}

	IEnumerator SwordDownSwing()
	{
		SwordDown.gameObject.SetActive(true);
		SwordDownAttackBox.gameObject.SetActive(true);
		isAttacking = true;
		yield return new WaitForSecondsRealtime(0.5f);
		isAttacking = false;
		SwordDown.gameObject.SetActive(false);
		SwordDownAttackBox.gameObject.SetActive(false);
	}

	IEnumerator AxeSwing()
	{
		Axe.gameObject.SetActive(true);
		AxeAttackBox.gameObject.SetActive(true);
		isAttacking = true;
		yield return new WaitForSecondsRealtime(0.7f);
		isAttacking = false;
		Axe.gameObject.SetActive(false);
		AxeAttackBox.gameObject.SetActive(false);
	}

	IEnumerator BatSwing()
	{
		Bat.gameObject.SetActive(true);
		BatAttackBox.gameObject.SetActive(true);
		isAttacking = true;
		yield return new WaitForSecondsRealtime(0.5f);
		isAttacking = false;
		Bat.gameObject.SetActive(false);
		BatAttackBox.gameObject.SetActive(false);
	}

	IEnumerator PanSwing()
	{
		while (true)
		{
			PanAttackBox.gameObject.SetActive(true);
			isAttacking = true;
			yield return new WaitForSecondsRealtime(0.3f);
			isAttacking = false;
			PanAttackBox.gameObject.SetActive(false);
		}
	}



	IEnumerator PistolShooting()
	{
		Pistol.gameObject.SetActive(true);
		PF.gameObject.SetActive(true);
		ShootLocation = (dir - PistolSpawnPos.transform.position);
		GameObject bullet = Instantiate(PistolBullet);
		bullet.transform.position = PistolSpawnPos.transform.position;
		yield return new WaitForSecondsRealtime(0.1f);
		PF.gameObject.SetActive(false);
		yield return new WaitForSecondsRealtime(0.4f);
		Pistol.gameObject.SetActive(false);
	}

	IEnumerator SMGShooting()
	{
		while (true)
		{
			bullet -= 1;
			SMGF.gameObject.SetActive(true);
			ShootLocation = (dir - SMGSpawnPos.transform.position);
			GameObject bullet1 = Instantiate(PistolBullet);
			bullet1.transform.position = SMGSpawnPos.transform.position;
			yield return new WaitForSecondsRealtime(0.1f);
			SMGF.gameObject.SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
		}
	}

	IEnumerator ARShooting()
	{
		while (true)
		{
			bullet -= 3;
			ARF.gameObject.SetActive(true);
			ShootLocation = (dir - ARSpawnPos.transform.position);
			GameObject bullet1 = Instantiate(ARBullet);
			bullet1.transform.position = ARSpawnPos.transform.position;
			GameObject bullet2 = Instantiate(ARBullet);
			bullet2.transform.position = ARSpawnPos.transform.position;
			yield return new WaitForSecondsRealtime(0.1f);
			GameObject bullet3 = Instantiate(ARBullet);
			bullet3.transform.position = ARSpawnPos.transform.position;
			yield return new WaitForSecondsRealtime(0.1f);
			ARF.gameObject.SetActive(false);
			yield return new WaitForSecondsRealtime(0.9f);
		}
	}

	IEnumerator SGShooting()
	{
		bullet -= 3;
		SG.gameObject.SetActive(true);
		ShootLocation = (dir - SGSpawnPos.transform.position);

		int rand = UnityEngine.Random.Range(0, 4);

		if (rand == 0)
		{
			GameObject bullet = Instantiate(SGB1);
			bullet.transform.position = SMGSpawnPos.transform.position;
		}

		else if (rand == 1)
		{
			GameObject bullet = Instantiate(SGB2);
			bullet.transform.position = SMGSpawnPos.transform.position;
		}

		else if (rand == 2)
		{
			GameObject bullet = Instantiate(SGB3);
			bullet.transform.position = SMGSpawnPos.transform.position;
		}

		else if (rand == 3)
		{
			GameObject bullet = Instantiate(SGB4);
			bullet.transform.position = SMGSpawnPos.transform.position;
		}

		yield return new WaitForSecondsRealtime(0.3f);
		SG.gameObject.SetActive(false);
	}



	IEnumerator Hitted()
	{
		isHit = true;
		yield return new WaitForSecondsRealtime(0.1f); 
		isHit = false;
	}

	IEnumerator Rolling()
	{
		isInvincible = true;
		Color color = spriteRenderer.color;
		color.a = 0.5f;
		spriteRenderer.color = color;
		yield return new WaitForSecondsRealtime(0.8f);
		color.a = 1.0f;
		spriteRenderer.color = color;
		isInvincible = false;
	}



	IEnumerator RA(float delay)
	{
		while (true)
		{
			if(mask == false)
			{
				PlayerRA.instance.currentRA -= 10;
				if (PlayerRA.instance.currentRA <= 0)
				{
					if (isRAZero == false)
					{
						StartCoroutine(RAHPCoroutine);
						isRAZero = true;
					}
				}

				else if (PlayerRA.instance.currentRA > 0)
				{
					if (isRAZero == true)
					{
						StopCoroutine(RAHPCoroutine);
						isRAZero = false;
					}
				}
			}

			yield return new WaitForSecondsRealtime(delay);
		}
	}

	IEnumerator RAHPDecrease(float delay)
	{
		while (true)
		{
			PlayerHP.instance.currentHP -= 1;
			yield return new WaitForSecondsRealtime(delay);
		}
	}

	IEnumerator RAAutoDecrease()
	{
		yield return new WaitForSecondsRealtime(10);

		while (true)
		{
			if(mask == false)
			{
				PlayerRA.instance.currentRA -= 5;

				if (PlayerRA.instance.currentRA <= 0)
				{
					if (isRAZero == false)
					{
						StartCoroutine(RAHPCoroutine);
						isRAZero = true;
					}
				}

				else if (PlayerRA.instance.currentRA > 0)
				{
					if (isRAZero == true)
					{
						StopCoroutine(RAHPCoroutine);
						isRAZero = false;
					}
				}
			}
			
			yield return new WaitForSecondsRealtime(10);
		}
	}

	IEnumerator MaskActive()
	{
		yield return new WaitForSecondsRealtime(5);

		while (!stopMask)
		{
			maskDurability -= 5;
			yield return new WaitForSecondsRealtime(5);
		}
	}
}
