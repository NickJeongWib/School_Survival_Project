using UnityEngine;
using My;
using static Define;
//IEnumerator : 코루틴만의 반환형 인터페이스 = 가져오기 위해 밑에 5,6번 줄 추가
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MonsterMove : MonoBehaviour
{
    /** 물리적으로 따라갈 타겟 */
    public Rigidbody2D Target;

    /** 몬스터 생존여부 */
    public bool bIsLive = true;

    /** 이 클래스를 가진 오브젝트의 Rigid에 접근을 위한 선언 */
    Rigidbody2D rigid;
    /** 이 클래스를 가진 오브젝트 SpritrRenderer접근을 위한 선언 */
    SpriteRenderer sprite;

    public EMonsterType CurrentMonsterType;

    public GameObject PlayerFind;
    public GameObject Potion;

    /** TODO ## MonsterMove.cs 몬스터 체력 설정 재설정 필요 */
    public float CurrentMonsterHp;
    public float MonsterMaxHp;
    public int PotionDropPercent;
    float NextMonsterHp;
    float MonsterSpeed;
    float BaseMonsterHp;
    

    Animator anim;
    WaitForFixedUpdate wait;
    Collider2D coll;
    
    public void MonsterHpUp ()
    {
       
    }

    void Awake()
    {
        wait = new WaitForFixedUpdate();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
    void Start()
    {
        GameManager.GMInstance.MonsterMoveRef = this;
        #region PlayerFind
        //if (playerfind == null)
        //{
        //    /** playercharacter태그를 가진 플레이어를 찾는다. */
        //    playerfind = gameobject.findgameobjectwithtag("playercharacter");

        //    if (playerfind != null)
        //    {
        //        /** 찾은 player의 rigidbody2d컴포넌트를 가져온다. */
        //        target = playerfind.getcomponent<rigidbody2d>();
        //    }
        //}
        #endregion // 안쓰는거 주석
        MonsterSpeed = GameManager.GMInstance.MonsterSpeed;

        /** TODO ## MonsterMove.cs 보스몬스터 체력 고정 */
        #region SaveMonsterHp

        /** TODO ## MonsterMove.cs 보스몬스터가 아닐 시 일반 몬스터 체력저장 */
        /** 몬스터 기본 체력 저장 */
        if (gameObject.name != "Monster_D(Clone)" || gameObject.name != "Boss_Moai(Clone)" || gameObject.name != "Boss_Reaper")
        {
            /** 기본 몬스터 체력 저장 */
            BaseMonsterHp = MonsterMaxHp;
        }
        #endregion

        /** 현재 몬스터 체력은은 몬스터의 최대 체력 */
        CurrentMonsterHp = MonsterMaxHp;

        /** PlaySceneManager에 몬스터의 체력을 증가시키기 위해 초기 체력 초기화 */
        GameManager.GMInstance.PlaySceneManagerRef.BaseMonsterHp = BaseMonsterHp;

        bIsLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        sprite.sortingOrder = 2;
        anim.SetBool("Dead", false);
        sprite.color = new Color(255, 133, 255, 136);   

        //if (GameManager.GMInstance.PlaySceneManagerRef.BaseMonsterHp == BaseMonsterHp)
        //{
        //    return;
        //}
        
    }

    void Update()
    {
        if (!bIsLive && PlayerFind == null)
        {
            return;
        }

        MonsterMaxHp = GameManager.GMInstance.PlaySceneManagerRef.NextMonsterHp;
    }

    void FixedUpdate()
    {
        if (!GameManager.GMInstance.bIsLive)
        {
            return;
        }

        if (!bIsLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }

        /** TODO ## MonsterMove.cs 몬스터 이동 구현 */
        /** 몬스터와 플레이어의 위치 차이(방향값이 나옴) */
        Vector2 DirVec = Target.position - rigid.position;

        /**
        앞으로 가야할 다음 위치 계산 normalized를 사용하여 DirVec를 1로 정규화 해준다.
        피타고라스 정의에 의해 대각선 이동시 크기가 일정하지 핞기 때문에 일정하게 이동할수 있게 해준다.
        */
        Vector2 NextVec = DirVec.normalized * MonsterSpeed * Time.deltaTime;

        /** 이 클래스의 물리적 이동 구현 */
        rigid.MovePosition(rigid.position + NextVec);

        /** 물리적 속력 0으로 */
        rigid.velocity = Vector2.zero;

        /** 몬스터 방향 전환 */
        sprite.flipX = Target.position.x > rigid.position.x;
    }

    
    void OnEnable()
    {
        Target = GameManager.GMInstance.Player.GetComponent<Rigidbody2D>();

        // 재활용하기 위해
        bIsLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        sprite.sortingOrder = 2;
        anim.SetBool("Dead", false);
        CurrentMonsterHp = GameManager.GMInstance.PlaySceneManagerRef.NextMonsterHp;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        /** 몬스터가 살아있지 않다면 */
        if (!bIsLive)
        {
            return;
        }

        /**---------------------WizardSkill--------------------------*/

        /** TODO ## MonsterMove.cs 태그가 Bullet이면 */
        if (collision.CompareTag("Bullet"))
        {
            /** 피격 효과 */
            sprite.color = Color.red;
            Invoke("OnDamage", 0.2f);


            /** 피격 효과음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);

            /** 크리티컬 확률 계산 */
            float Crtical = 100.0f * (Random.Range(0.0f, 1.0f));
            Debug.Log(Crtical);

            /** TODO ## MonsterMove.cs 크리티컬 적용 공식 */
            /** 파이어볼 크리티컬 데미지 계산식 (기본 크리티컬 데미지 + 패시브 스킬로 증가한 크리티컬 데미지) * (능력치 증가로 인한 데미지 증가 + 패시브 스킬 데미지 증가) + 기존 데미지) */
            float FireBallCriticalDamage = (GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalDamageUpRate()) * (((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetFireBallBaseDamage()) + collision.GetComponent<Bullet>().m_Damage);
            /** 파이어볼 일반 공격 데미지 계산식*/
            float FireBallNormalDamage = ((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetFireBallBaseDamage()) + collision.GetComponent<Bullet>().m_Damage;

            Vector3 NowPos = transform.position;

            /** TODO ## MonsterMove.cs 크리티컬 적용 공식 */
            /** 크리티커 확률 적용 되었다면 */
            if (Crtical <= 100.0f * (GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalUpRate()))
            {
                /** 크리티컬 데미지 피해 */
                GetDamage(FireBallCriticalDamage);

                /** 크리티컬 데미지 텍스트 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(FireBallCriticalDamage, NowPos, Color.red);

                /** 데미지 텍스로 변환 후 DamageText의 damage 초기화 */
                // Damage_Text.GetComponent<DamageText>().SetDamageText(FireBallCriticalDamage);

                // Debug.Log("파이어볼 크리티컬 데미지 : " + FireBallCriticalDamage);
            }
            else
            {
                /** 몬스터의 체력은 Bullet 태그를 가진 오브젝트에 닿으면 m_Damage만큼 빼준다. */
                GetDamage(FireBallNormalDamage);

                /** 크리티컬 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(FireBallNormalDamage, NowPos, Color.white);


                /** 데미지 텍스로 변환 후 DamageText의 damage 초기화 */
                // Damage_Text.GetComponent<DamageText>().SetDamageText(FireBallNormalDamage);

                // Debug.Log("파이어볼 일반공격 데미지 : " + FireBallNormalDamage);
            }

            // Debug.Log(CurrentMonsterHp);

            /** health 0보다 크면 몬스터가 살아있다면 */
            if (CurrentMonsterHp > 0)
            {
                // StartCoroutine(OnDaamge());
                //피격 부분에 애니메이터를 호출하여 상태 변경
                anim.SetTrigger("Hit");
            }
            /** 몬스터가 죽으면 */
            else
            {
                // 몬스터가 죽었으므로 비활성화
                bIsLive = false;
                /** 콜라이더 비활성화 */
                coll.enabled = false;
                /** RigidBody2D 비활성화 */
                rigid.simulated = false;
                /** sprite 레이어 1 */
                sprite.sortingOrder = 1;
                // 죽는 애니메이션
                anim.SetBool("Dead", true);
                /** 함수 호출 */
                GameManager.GMInstance.killcount++;
                // 몬스터 사망 시 킬수 증가 함수 호출
                GameManager.GMInstance.GetExp();
                // 몬스터 사망 시 경험치 함수 호출
                Dead();
            }
        }

        /** TODO ## MonsterMove.cs 태그가 IceArrow이면 */
        if (collision.CompareTag("IceArrow"))
        {
            /** 피격 효과 */
            sprite.color = Color.red;
            Invoke("OnDamage", 0.2f);

            /** 크리티컬 확률 계산 */
            float Crtical = 100.0f * (Random.Range(0.0f, 1.0f));
            // Debug.Log(Crtical);

            /** 아이스에로우 크리티컬 데미지 계산식 (기본 크리티컬 데미지 + 패시브 스킬로 증가한 크리티컬 데미지) * (능력치 증가로 인한 데미지 증가 + 기존 데미지) */
            float IceArrowCriticalDamage = (GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalDamageUpRate()) * (((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetIceArrowBaseDamage()) + collision.GetComponent<Bullet>().m_Damage);
            /** 아이스에로우 일반 공격 데미지 계산식*/
            float IceArrowNormalDamage = ((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetIceArrowBaseDamage()) + collision.GetComponent<Bullet>().m_Damage;

            Vector3 NowPos = transform.position;

            /** 크리티커 확률 적용 되었다면 */
            if (Crtical <= 100.0f * (GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalUpRate()))
            {
                GetDamage(IceArrowCriticalDamage);

                /** 크리티컬 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(IceArrowCriticalDamage, NowPos, Color.red);
                // Debug.Log("아이스에로우 크리티컬 데미지 : " + IceArrowCriticalDamage);
            }
            else
            {
                /** 몬스터의 체력은 Bullet 태그를 가진 오브젝트에 닿으면 m_Damage만큼 빼준다. */
                GetDamage(IceArrowNormalDamage);

                /** 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(IceArrowNormalDamage, NowPos, Color.white);

                // Debug.Log("아이스에로우 일반공격 데미지 : " + IceArrowNormalDamage);
            }

            /** 피격 효과음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);

            // StartCoroutine(KnockBack());

            /** health 0보다 크면 몬스터가 살아있다면 */
            if (CurrentMonsterHp > 0)
            {
                //피격 부분에 애니메이터를 호출하여 상태 변경
                anim.SetTrigger("Hit");
            }
            /** 몬스터가 죽으면 */
            else
            {
                // 몬스터가 죽었으므로 비활성화
                bIsLive = false;
                /** 콜라이더 비활성화 */
                coll.enabled = false;
                /** RigidBody2D 비활성화 */
                rigid.simulated = false;
                /** sprite 레이어 1 */
                sprite.sortingOrder = 1;
                // 죽는 애니메이션
                anim.SetBool("Dead", true);
                /** 함수 호출 */
                GameManager.GMInstance.killcount++;
                // 몬스터 사망 시 킬수 증가 함수 호출
                GameManager.GMInstance.GetExp();
                // 몬스터 사망 시 경험치 함수 호출
                Dead();
            }
        }

        /** TODO ## MonsterMove.cs 태그가 ElectricBall이면 */
        if (collision.CompareTag("ElectricBall"))
        {
            /** 피격 효과 */
            sprite.color = Color.red;
            Invoke("OnDamage", 0.2f);

            /** 피격 효과음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);

            /** 크리티컬 확률 계산 */
            float Crtical = 100.0f * (Random.Range(0.0f, 1.0f));
            // Debug.Log(Crtical);

            /** 전기구체 크리티컬 데미지 계산식 (기본 크리티컬 데미지 + 패시브 스킬로 증가한 크리티컬 데미지) * (능력치 증가로 인한 데미지 증가 + 기존 데미지) */
            float ElectricBallCriticalDamage = (GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalDamageUpRate()) * (((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetElectricBallBaseDamage()) + collision.GetComponent<Bullet>().m_Damage);
            /** 전기구체 일반 공격 데미지 계산식*/
            float ElectricBallNormalDamage = ((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetElectricBallBaseDamage()) + collision.GetComponent<Bullet>().m_Damage;

            Vector3 NowPos = transform.position;

            /** TODO ## MonsterMove.cs 크리티컬 적용 공식 */
            /** 크리티커 확률 적용 되었다면 */
            if (Crtical <= 100.0f * (GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalUpRate()))
            {
                GetDamage(ElectricBallCriticalDamage);

                /** 크리티컬 데미지 텍스트 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(ElectricBallCriticalDamage, NowPos, Color.red);

                // Debug.Log("뇌구 크리티컬 데미지 : " + ElectricBallCriticalDamage);
            }
            else
            {
                /** 몬스터의 체력은 Bullet 태그를 가진 오브젝트에 닿으면 m_Damage만큼 빼준다. */
                GetDamage(ElectricBallNormalDamage);

                /** 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(ElectricBallNormalDamage, NowPos, Color.white);

                // Debug.Log("뇌구 일반공격 데미지 : " + ElectricBallNormalDamage);
                // Debug.Log(GameManager.GMInstance.GetSkillDamageUp() * GameManager.GMInstance.GetElectricBallBaseDamage());
            }

            // StartCoroutine(KnockBack());

            /** health 0보다 크면 몬스터가 살아있다면 */
            if (CurrentMonsterHp > 0)
            {
                //피격 부분에 애니메이터를 호출하여 상태 변경
                anim.SetTrigger("Hit");
            }
            /** 몬스터가 죽으면 */
            else
            {
                // 몬스터가 죽었으므로 비활성화
                bIsLive = false;
                /** 콜라이더 비활성화 */
                coll.enabled = false;
                /** RigidBody2D 비활성화 */
                rigid.simulated = false;
                /** sprite 레이어 1 */
                sprite.sortingOrder = 1;
                // 죽는 애니메이션
                anim.SetBool("Dead", true);
                /** 함수 호출 */
                GameManager.GMInstance.killcount++;
                // 몬스터 사망 시 킬수 증가 함수 호출
                GameManager.GMInstance.GetExp();
                // 몬스터 사망 시 경험치 함수 호출
                Dead();
            }
        }

        /** TODO ## MonsterMove.cs 몬스터가 스킬에 닿았다면 */
        if (collision.CompareTag("Mateo"))
        {
            /** 피격 효과 */
            sprite.color = Color.red;
            Invoke("OnDamage", 0.2f);

            /** 피격 효과음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);

            /** 크리티컬 확률 계산 */
            float Crtical = 100.0f * (Random.Range(0.0f, 1.0f));
            // Debug.Log(Crtical);

            /** 메테오 크리티컬 데미지 계산식 (기본 크리티컬 데미지 + 패시브 스킬로 증가한 크리티컬 데미지) * (능력치 증가로 인한 데미지 증가 + 기존 데미지) */
            float MateoCriticalDamage = (GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalDamageUpRate()) * (((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetMateoBaseDamage()) + GameManager.GMInstance.SkillManagerRef.MateoDamage);
            /** 메테오 일반 공격 데미지 계산식*/
            float MateoNormalDamage = (GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetMateoBaseDamage() + GameManager.GMInstance.SkillManagerRef.MateoDamage;

            Vector3 NowPos = transform.position;

            /** TODO ## MonsterMove.cs 크리티컬 적용 공식 */
            /** 크리티커 확률 적용 되었다면 */
            if (Crtical <= 100.0f * (GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalUpRate()))
            {               
                GetDamage(MateoCriticalDamage);

                /** 크리티컬 데미지 텍스트 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(MateoCriticalDamage, NowPos, Color.red);
                // Debug.Log("메테오 크리티컬 데미지 : " + MateoCriticalDamage);
            }
            else
            {
                /** 몬스터의 체력은 Bullet 태그를 가진 오브젝트에 닿으면 m_Damage만큼 빼준다. */
                GetDamage(MateoNormalDamage);

                /** 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(MateoNormalDamage, NowPos, Color.white);

                // Debug.Log("메테오 일반공격 데미지 : " + MateoNormalDamage);

            }


            if (CurrentMonsterHp > 0)
            {
                //피격 부분에 애니메이터를 호출하여 상태 변경
                anim.SetTrigger("Hit");
            }
            /** 몬스터가 죽으면 */
            else
            {
                // 몬스터가 죽었으므로 비활성화
                bIsLive = false;
                /** 콜라이더 비활성화 */
                coll.enabled = false;
                /** RigidBody2D 비활성화 */
                rigid.simulated = false;
                /** sprite 레이어 1 */
                sprite.sortingOrder = 1;
                // 죽는 애니메이션
                anim.SetBool("Dead", true);
                /** 함수 호출 */
                GameManager.GMInstance.killcount++;
                // 몬스터 사망 시 킬수 증가 함수 호출
                GameManager.GMInstance.GetExp();
                // 몬스터 사망 시 경험치 함수 호출
                Dead();
            }
        }

        /** 라이트닝에 닿았다면 */
        if (collision.gameObject.CompareTag("Lightning"))
        {
            /** 피격 효과 */
            sprite.color = Color.red;
            Invoke("OnDamage", 0.2f);

            /** 피격 효과음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);

            /** 크리티컬 확률 계산 */
            float Crtical = 100.0f * (Random.Range(0.0f, 1.0f));
            // Debug.Log(Crtical);

            /** 낙뢰 크리티컬 데미지 계산식 (기본 크리티컬 데미지 + 패시브 스킬로 증가한 크리티컬 데미지) * (능력치 증가로 인한 데미지 증가 + 기존 데미지) */
            float LightningCriticalDamage = (GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalDamageUpRate()) * (((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetLightningBaseDamage()) + GameManager.GMInstance.SkillManagerRef.LightningDamage);
            /** 낙뢰 일반 공격 데미지 계산식*/
            float LightningNormalDamage = (GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetLightningBaseDamage() + GameManager.GMInstance.SkillManagerRef.LightningDamage;

            Vector3 NowPos = transform.position;

            /** 크리티커 확률 적용 되었다면 */
            if (Crtical <= 100.0f * (GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalUpRate()))
            {
                /** 낙뢰 크리티컬 데미지 */
                GetDamage(LightningCriticalDamage);

                /** 크리티컬 데미지 텍스트 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(LightningCriticalDamage, NowPos, Color.red);
                // Debug.Log("낙뢰 크리티컬 데미지 : " + LightningCriticalDamage);
            }
            else
            {
                /** 낙뢰 일반 데미지 */
                GetDamage(LightningNormalDamage);

                /** 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(LightningNormalDamage, NowPos, Color.white);

                // Debug.Log("낙뢰 일반공격 데미지 : " + LightningNormalDamage);

            }


            if (CurrentMonsterHp > 0)
            {        
                //피격 부분에 애니메이터를 호출하여 상태 변경
                anim.SetTrigger("Hit");
            }
            /** 몬스터가 죽으면 */
            else
            {
                // 몬스터가 죽었으므로 비활성화
                bIsLive = false;
                /** 콜라이더 비활성화 */
                coll.enabled = false;
                /** RigidBody2D 비활성화 */
                rigid.simulated = false;
                /** sprite 레이어 1 */
                sprite.sortingOrder = 1;
                // 죽는 애니메이션
                anim.SetBool("Dead", true);
                /** 함수 호출 */
                GameManager.GMInstance.killcount++;
                // 몬스터 사망 시 킬수 증가 함수 호출
                GameManager.GMInstance.GetExp();
                // 몬스터 사망 시 경험치 함수 호출
                Dead();
            }
        }

        if (collision.gameObject.CompareTag("Tornado"))
        {
            /** 피격 효과 */
            sprite.color = Color.red;
            Invoke("OnDamage", 0.2f);

            /** 피격 효과음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);

            /** 크리티컬 확률 계산 */
            float Crtical = 100.0f * (Random.Range(0.0f, 1.0f));
            // Debug.Log(Crtical);

            /** 토네이토 크리티컬 데미지 계산식 (기본 크리티컬 데미지 + 패시브 스킬로 증가한 크리티컬 데미지) * (능력치 증가로 인한 데미지 증가 + 기존 데미지) */
            float TornadoCriticalDamage = (GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalDamageUpRate()) * (((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetTornadoBaseDamage()) + GameManager.GMInstance.SkillManagerRef.TornadoDamage);
            /** 토네이도 일반 공격 데미지 계산식*/
            float TornadoNormalDamage = (GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetTornadoBaseDamage() + GameManager.GMInstance.SkillManagerRef.TornadoDamage;

            Vector3 NowPos = transform.position;

            /** 크리티커 확률 적용 되었다면 */
            if (Crtical <= 100.0f * (GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalUpRate()))
            {
                /** 토네이도 크리티컬 데미지 */
                GetDamage(TornadoCriticalDamage);

                /** 데미지 텍스트 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(TornadoCriticalDamage, NowPos, Color.red);
                // Debug.Log("토네이도 크리티컬 데미지 : " + TornadoCriticalDamage);
            }
            else
            {
                /** 크리티컬 토네이도 일반 데미지 */
                GetDamage(TornadoNormalDamage);

                /** 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(TornadoNormalDamage, NowPos, Color.white);
                // Debug.Log("토네이도 일반공격 데미지 : " + TornadoNormalDamage);
            }


            if (CurrentMonsterHp > 0)
            {

                //피격 부분에 애니메이터를 호출하여 상태 변경
                anim.SetTrigger("Hit");
            }
            /** 몬스터가 죽으면 */
            else
            {
                // 몬스터가 죽었으므로 비활성화
                bIsLive = false;
                /** 콜라이더 비활성화 */
                coll.enabled = false;
                /** RigidBody2D 비활성화 */
                rigid.simulated = false;
                /** sprite 레이어 1 */
                sprite.sortingOrder = 1;
                // 죽는 애니메이션
                anim.SetBool("Dead", true);
                /** 함수 호출 */
                GameManager.GMInstance.killcount++;
                // 몬스터 사망 시 킬수 증가 함수 호출
                GameManager.GMInstance.GetExp();
                // 몬스터 사망 시 경험치 함수 호출
                Dead();
            }
        }

        /** 아이스에이지에 닿았다면 */
        if (collision.gameObject.CompareTag("IceAge"))
        {
            /** 피격 효과 */
            sprite.color = Color.red;
            Invoke("OnDamage", 0.2f);

            /** 피격 효과음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);

            /** 크리티컬 확률 계산 */
            float Crtical = 100.0f * (Random.Range(0.0f, 1.0f));
            // Debug.Log(Crtical);

            /** 아이스에이지 크리티컬 데미지 계산식 (기본 크리티컬 데미지 + 패시브 스킬로 증가한 크리티컬 데미지) * (능력치 증가로 인한 데미지 증가 + 기존 데미지) */
            float IceAgeCriticalDamage = (GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalDamageUpRate()) * (((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetIceAgeBaseDamage()) + GameManager.GMInstance.SkillManagerRef.IceAgeDamage);
            /** 아이스에이지 일반 공격 데미지 계산식*/
            float IceAgeNormalDamage = (GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetIceAgeBaseDamage() + GameManager.GMInstance.SkillManagerRef.IceAgeDamage;

            Vector3 NowPos = transform.position;

            /** 크리티커 확률 적용 되었다면 */
            if (Crtical <= 100.0f * (GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalUpRate()))
            {
                /** 아이스에이지 크리티컬 데미지 */
                GetDamage(IceAgeCriticalDamage);

                /** 데미지 텍스트 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(IceAgeCriticalDamage, NowPos, Color.red);
                // Debug.Log("아이스에이지 크리티컬 데미지 : " + IceAgeCriticalDamage);
            }
            else
            {
                /** 크리티컬 아이스에이지 일반 데미지 */
                GetDamage(IceAgeNormalDamage);

                /** 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(IceAgeNormalDamage, NowPos, Color.white);
                // Debug.Log("아이스에이지 일반공격 데미지 : " + IceAgeNormalDamage);
            }


            if (CurrentMonsterHp > 0)
            {

                //피격 부분에 애니메이터를 호출하여 상태 변경
                anim.SetTrigger("Hit");
            }
            /** 몬스터가 죽으면 */
            else
            {
                // 몬스터가 죽었으므로 비활성화
                bIsLive = false;
                /** 콜라이더 비활성화 */
                coll.enabled = false;
                /** RigidBody2D 비활성화 */
                rigid.simulated = false;
                /** sprite 레이어 1 */
                sprite.sortingOrder = 1;
                // 죽는 애니메이션
                anim.SetBool("Dead", true);
                /** 함수 호출 */
                GameManager.GMInstance.killcount++;
                // 몬스터 사망 시 킬수 증가 함수 호출
                GameManager.GMInstance.GetExp();
                // 몬스터 사망 시 경험치 함수 호출
                Dead();
            }
        }

        /** TODO ## MonsterMove.cs 궁수 스킬에 피격 시 */
        /**---------------------AcherSkill--------------------------*/

        /** 화살 공격에 맞았다면 */
        if (collision.gameObject.CompareTag("Arrow"))
        {
            /** 피격 효과 */
            sprite.color = Color.red;
            Invoke("OnDamage", 0.2f);

            /** 피격 효과음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);

            /** 크리티컬 확률 계산 */
            float Crtical = 100.0f * (Random.Range(0.0f, 1.0f));
            // Debug.Log(Crtical);

            /** 화살 공격 크리티컬 데미지 계산식 (기본 크리티컬 데미지 + 패시브 스킬로 증가한 크리티컬 데미지) * (능력치 증가로 인한 데미지 증가 + 기존 데미지) */
            float ArrowCriticalDamage = (GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalDamageUpRate()) * (((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetArrowBaseDamage()) + collision.GetComponent<Bullet>().m_Damage);
            /** 화살 공격 일반 공격 데미지 계산식*/
            float ArrowNormalDamage = (GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetArrowBaseDamage() + collision.GetComponent<Bullet>().m_Damage;

            Vector3 NowPos = transform.position;

            /** 크리티커 확률 적용 되었다면 */
            if (Crtical <= 100.0f * (GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalUpRate()))
            {
                /** 화살 공격 크리티컬 데미지 */
                GetDamage(ArrowCriticalDamage);

                /** 크리티컬 데미지 텍스트 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(ArrowCriticalDamage, NowPos, Color.red);
                // Debug.Log("화살 공격 크리티컬 데미지 : " + IceAgeCriticalDamage);
            }
            else
            {
                /** 화살 공격 일반 데미지 */
                GetDamage(ArrowNormalDamage);

                /** 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(ArrowNormalDamage, NowPos, Color.white);
                // Debug.Log("화살 공격 일반공격 데미지 : " + IceAgeNormalDamage);
            }


            if (CurrentMonsterHp > 0)
            {

                //피격 부분에 애니메이터를 호출하여 상태 변경
                anim.SetTrigger("Hit");
            }
            /** 몬스터가 죽으면 */
            else
            {
                // 몬스터가 죽었으므로 비활성화
                bIsLive = false;
                /** 콜라이더 비활성화 */
                coll.enabled = false;
                /** RigidBody2D 비활성화 */
                rigid.simulated = false;
                /** sprite 레이어 1 */
                sprite.sortingOrder = 1;
                // 죽는 애니메이션
                anim.SetBool("Dead", true);
                /** 함수 호출 */
                GameManager.GMInstance.killcount++;
                // 몬스터 사망 시 킬수 증가 함수 호출
                GameManager.GMInstance.GetExp();
                // 몬스터 사망 시 경험치 함수 호출
                Dead();
            }
        }

        /** 볼텍스에 맞았다면 */
        if (collision.gameObject.CompareTag("Vortex"))
        {
            /** 피격 효과 */
            sprite.color = Color.red;
            Invoke("OnDamage", 0.2f);

            /** 피격 효과음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);

            /** 크리티컬 확률 계산 */
            float Crtical = 100.0f * (Random.Range(0.0f, 1.0f));
            // Debug.Log(Crtical);

            /** 볼텍스에 크리티컬 데미지 계산식 (기본 크리티컬 데미지 + 패시브 스킬로 증가한 크리티컬 데미지) * (능력치 증가로 인한 데미지 증가 + 기존 데미지) */
            float VortexCriticalDamage = (GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalDamageUpRate()) * (((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetVortexBaseDamage()) + GameManager.GMInstance.SkillManagerRef.VortexDamage);
            /** 볼텍스에 일반 공격 데미지 계산식*/
            float VortexNormalDamage = ((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetVortexBaseDamage()) + GameManager.GMInstance.SkillManagerRef.VortexDamage;

            Vector3 NowPos = transform.position;

            /** 크리티커 확률 적용 되었다면 */
            if (Crtical <= 100.0f * (GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalUpRate()))
            {
                /** 볼텍스 크리티컬 데미지 */
                GetDamage(VortexCriticalDamage);

                /** 크리티컬 데미지 텍스트 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(VortexCriticalDamage, NowPos, Color.red);
                // Debug.Log("볼텍스 크리티컬 데미지 : " + IceAgeCriticalDamage);
            }
            else
            {
                /** 볼텍스 일반 데미지 */
                GetDamage(VortexNormalDamage);

                /** 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(VortexNormalDamage, NowPos, Color.white);
                // Debug.Log("볼텍스 일반공격 데미지 : " + IceAgeNormalDamage);
            }


            if (CurrentMonsterHp > 0)
            {

                //피격 부분에 애니메이터를 호출하여 상태 변경
                anim.SetTrigger("Hit");
            }
            /** 몬스터가 죽으면 */
            else
            {
                // 몬스터가 죽었으므로 비활성화
                bIsLive = false;
                /** 콜라이더 비활성화 */
                coll.enabled = false;
                /** RigidBody2D 비활성화 */
                rigid.simulated = false;
                /** sprite 레이어 1 */
                sprite.sortingOrder = 1;
                // 죽는 애니메이션
                anim.SetBool("Dead", true);
                /** 함수 호출 */
                GameManager.GMInstance.killcount++;
                // 몬스터 사망 시 킬수 증가 함수 호출
                GameManager.GMInstance.GetExp();
                // 몬스터 사망 시 경험치 함수 호출
                Dead();
            }
        } // if (collision.gameObject.CompareTag("Vortex"))

        /** 허리케인에 맞았다면 */
        if (collision.gameObject.CompareTag("Huricane"))
        {
            /** 피격 효과 */
            sprite.color = Color.red;
            Invoke("OnDamage", 0.2f);

            /** 피격 효과음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);

            /** 크리티컬 확률 계산 */
            float Crtical = 100.0f * (Random.Range(0.0f, 1.0f));
            // Debug.Log(Crtical);

            /** TODO ## MonsterMove.cs 크리티컬 적용 공식 */
            /** 파이어볼 크리티컬 데미지 계산식 (기본 크리티컬 데미지 + 패시브 스킬로 증가한 크리티컬 데미지) * (능력치 증가로 인한 데미지 증가 + 기존 데미지) */
            float HuricaneCriticalDamage = (GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalDamageUpRate()) * (((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetHuricaneBaseDamage()) + GameManager.GMInstance.SkillManagerRef.HuricaneDamage);
            /** 파이어볼 일반 공격 데미지 계산식*/
            float HuricaneNormalDamage = ((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetHuricaneBaseDamage()) + GameManager.GMInstance.SkillManagerRef.HuricaneDamage;

            Vector3 NowPos = transform.position;

            /** 크리티커 확률 적용 되었다면 */
            if (Crtical <= 100.0f * (GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalUpRate()))
            {
                /** 허리케인 크리티컬 데미지 */
                GetDamage(HuricaneCriticalDamage);


                /** 크리티컬 데미지 텍스트 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(HuricaneCriticalDamage, NowPos, Color.red);
                // Debug.Log("허리케인 크리티컬 데미지 : " + IceAgeCriticalDamage);
            }
            else
            {
                /** 허리케인 일반 데미지 */
                GetDamage(HuricaneNormalDamage);

                /** 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(HuricaneNormalDamage, NowPos, Color.white);
                // Debug.Log("허리케인 일반공격 데미지 : " + IceAgeNormalDamage);
            }

            if (CurrentMonsterHp > 0)
            {
                //피격 부분에 애니메이터를 호출하여 상태 변경
                anim.SetTrigger("Hit");
            }
            /** 몬스터가 죽으면 */
            else
            {
                // 몬스터가 죽었으므로 비활성화
                bIsLive = false;
                /** 콜라이더 비활성화 */
                coll.enabled = false;
                /** RigidBody2D 비활성화 */
                rigid.simulated = false;
                /** sprite 레이어 1 */
                sprite.sortingOrder = 1;
                // 죽는 애니메이션
                anim.SetBool("Dead", true);
                /** 함수 호출 */
                GameManager.GMInstance.killcount++;
                // 몬스터 사망 시 킬수 증가 함수 호출
                GameManager.GMInstance.GetExp();
                // 몬스터 사망 시 경험치 함수 호출
                Dead();
            }
        } //if (collision.gameObject.CompareTag("Huricane")) 

        /** 바람정령 공격에 맞았다면 */
        if (collision.gameObject.CompareTag("WindSpiritAttack"))
        {
            /** 피격 효과 */
            sprite.color = Color.red;
            Invoke("OnDamage", 0.2f);

            /** 피격 효과음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);

            /** 크리티컬 확률 계산 */
            float Crtical = 100.0f * (Random.Range(0.0f, 1.0f));
            // Debug.Log(Crtical);

            /** 바람정령 크리티컬 데미지 계산식 (기본 크리티컬 데미지 + 패시브 스킬로 증가한 크리티컬 데미지) * (능력치 증가로 인한 데미지 증가 + 기존 데미지) */
            float WindSpiritCriticalDamage = (GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalDamageUpRate()) * (((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetWindSpiritBaseDamage()) + collision.GetComponent<Bullet>().m_Damage);
            /** 바람정령 일반 공격 데미지 계산식*/
            float WindSpiritNormalDamage = ((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetWindSpiritBaseDamage()) + collision.GetComponent<Bullet>().m_Damage;

            Vector3 NowPos = transform.position;

            /** 크리티커 확률 적용 되었다면 */
            if (Crtical <= 100.0f * (GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalUpRate()))
            {
                /** 바람정령 크리티컬 데미지 */
                GetDamage(WindSpiritCriticalDamage);

                /** 크리티컬 데미지 텍스트 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(WindSpiritCriticalDamage, NowPos, Color.red);
                // Debug.Log("바람정령 크리티컬 데미지 : " + IceAgeCriticalDamage);
            }
            else
            {
                /** 바람정령 일반 데미지 */
                GetDamage(WindSpiritCriticalDamage);

                /** 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(WindSpiritCriticalDamage, NowPos, Color.white);
                // Debug.Log("바람정령 일반공격 데미지 : " + IceAgeNormalDamage);
            }


            if (CurrentMonsterHp > 0)
            {
                //피격 부분에 애니메이터를 호출하여 상태 변경
                anim.SetTrigger("Hit");
            }
            /** 몬스터가 죽으면 */
            else
            {
                // 몬스터가 죽었으므로 비활성화
                bIsLive = false;
                /** 콜라이더 비활성화 */
                coll.enabled = false;
                /** RigidBody2D 비활성화 */
                rigid.simulated = false;
                /** sprite 레이어 1 */
                sprite.sortingOrder = 1;
                // 죽는 애니메이션
                anim.SetBool("Dead", true);
                /** 함수 호출 */
                GameManager.GMInstance.killcount++;
                // 몬스터 사망 시 킬수 증가 함수 호출
                GameManager.GMInstance.GetExp();
                // 몬스터 사망 시 경험치 함수 호출
                Dead();
            }
        } // if (collision.gameObject.CompareTag("WindSpiritAttack"))


        /** 트랩폭발 공격에 맞았다면 */
        if (collision.gameObject.CompareTag("Trap"))
        {
            /** 피격 효과 */
            sprite.color = Color.red;
            Invoke("OnDamage", 0.2f);

            /** 피격 효과음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);

            /** 크리티컬 확률 계산 */
            float Crtical = 100.0f * (Random.Range(0.0f, 1.0f));
            // Debug.Log(Crtical);

            /** 트랩폭발에 크리티컬 데미지 계산식 (기본 크리티컬 데미지 + 패시브 스킬로 증가한 크리티컬 데미지) * (능력치 증가로 인한 데미지 증가 + 기존 데미지) */
            float TrapCriticalDamage = (GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalDamageUpRate()) * (((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetTrapBaseDamage()) + GameManager.GMInstance.SkillManagerRef.TrapDamage);
            /** 트랩폭발에 일반 공격 데미지 계산식*/
            float TrapNormalDamage = ((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetTrapBaseDamage()) + GameManager.GMInstance.SkillManagerRef.TrapDamage;

            Vector3 NowPos = transform.position;

            /** 크리티컬 확률 적용 되었다면 */
            if (Crtical <= 100.0f * (GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalUpRate()))
            {
                /** 트랩 폭발 크리티컬 데미지 */
                GetDamage(TrapCriticalDamage);

                /** 크리티컬 데미지 텍스트 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(TrapCriticalDamage, NowPos, Color.red);

                // Debug.Log("트랩폭발 크리티컬 데미지 : " + IceAgeCriticalDamage);
            }
            else
            {
                /** 트랩폭발 일반 데미지 */
                GetDamage(TrapNormalDamage);

                /** 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(TrapNormalDamage, NowPos, Color.white);
                // Debug.Log("트랩폭발 일반공격 데미지 : " + IceAgeNormalDamage);
            }

            if (CurrentMonsterHp > 0)
            {

                //피격 부분에 애니메이터를 호출하여 상태 변경
                anim.SetTrigger("Hit");
            }
            /** 몬스터가 죽으면 */
            else
            {
                // 몬스터가 죽었으므로 비활성화
                bIsLive = false;
                /** 콜라이더 비활성화 */
                coll.enabled = false;
                /** RigidBody2D 비활성화 */
                rigid.simulated = false;
                /** sprite 레이어 1 */
                sprite.sortingOrder = 1;
                // 죽는 애니메이션
                anim.SetBool("Dead", true);
                /** 함수 호출 */
                GameManager.GMInstance.killcount++;
                // 몬스터 사망 시 킬수 증가 함수 호출
                GameManager.GMInstance.GetExp();
                // 몬스터 사망 시 경험치 함수 호출
                Dead();
            }
        } // if (collision.gameObject.CompareTag("Trap"))


        /** 화살 비 공격에 맞았다면 */
        if (collision.gameObject.CompareTag("ArrowRain"))
        {
            /** 피격 효과 */
            sprite.color = Color.red;
            Invoke("OnDamage", 0.2f);

            /** 피격 효과음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);

            /** 크리티컬 확률 계산 */
            float Crtical = 100.0f * (Random.Range(0.0f, 1.0f));
            // Debug.Log(Crtical);

            /** 화살 비 크리티컬 데미지 계산식 (기본 크리티컬 데미지 + 패시브 스킬로 증가한 크리티컬 데미지) * (능력치 증가로 인한 데미지 증가 + 기존 데미지) */
            float ArrowRainCriticalDamage = (GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalDamageUpRate()) * (((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetArrowRainBaseDamage()) + GameManager.GMInstance.SkillManagerRef.ArrowRainDamage);
            /** 화살 비 일반 공격 데미지 계산식*/
            float ArrowRainNormalDamage = ((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetArrowRainBaseDamage()) + GameManager.GMInstance.SkillManagerRef.ArrowRainDamage;

            Vector3 NowPos = transform.position;

            /** 크리티컬 확률 적용 되었다면 */
            if (Crtical <= 100.0f * (GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalUpRate()))
            {
                /** 화살 비 크리티컬 데미지 */
                GetDamage(ArrowRainCriticalDamage);

                /** 크리티컬 데미지 텍스트 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(ArrowRainCriticalDamage, NowPos, Color.red);

                // Debug.Log("화살 비 크리티컬 데미지 : " + IceAgeCriticalDamage);
            }
            else
            {
                /** 화살 비 일반 데미지 */
                GetDamage(ArrowRainNormalDamage);

                /** 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(ArrowRainNormalDamage, NowPos, Color.white);
                // Debug.Log("화살 비 일반공격 데미지 : " + IceAgeNormalDamage);
            }

            if (CurrentMonsterHp > 0)
            {

                //피격 부분에 애니메이터를 호출하여 상태 변경
                anim.SetTrigger("Hit");
            }
            /** 몬스터가 죽으면 */
            else
            {
                // 몬스터가 죽었으므로 비활성화
                bIsLive = false;
                /** 콜라이더 비활성화 */
                coll.enabled = false;
                /** RigidBody2D 비활성화 */
                rigid.simulated = false;
                /** sprite 레이어 1 */
                sprite.sortingOrder = 1;
                // 죽는 애니메이션
                anim.SetBool("Dead", true);
                /** 함수 호출 */
                GameManager.GMInstance.killcount++;
                // 몬스터 사망 시 킬수 증가 함수 호출
                GameManager.GMInstance.GetExp();
                // 몬스터 사망 시 경험치 함수 호출
                Dead();
            }
        } // if (collision.gameObject.CompareTag("ArrowRain"))

        /** 폭발화살에 맞았다면 */
        if (collision.gameObject.CompareTag("BombArrowEffect"))
        {
            /** 피격 효과 */
            sprite.color = Color.red;
            Invoke("OnDamage", 0.2f);

            /** 피격 효과음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);

            /** 크리티컬 확률 계산 */
            float Crtical = 100.0f * (Random.Range(0.0f, 1.0f));
            // Debug.Log(Crtical);

            /** 화살 공격 크리티컬 데미지 계산식 (기본 크리티컬 데미지 + 패시브 스킬로 증가한 크리티컬 데미지) * (능력치 증가로 인한 데미지 증가 + 기존 데미지) */
            float BombArrowCriticalDamage = (GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalDamageUpRate()) * (((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetBombArrowBaseDamage()) + GameManager.GMInstance.SkillManagerRef.BombArrowDamage);
            /** 화살 공격 일반 공격 데미지 계산식*/
            float BombArrowNormalDamage = ((GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate) * GameManager.GMInstance.GetBombArrowBaseDamage()) + GameManager.GMInstance.SkillManagerRef.BombArrowDamage;

            Vector3 NowPos = transform.position;

            /** 크리티컬 확률 적용 되었다면 */
            if (Crtical <= 100.0f * (GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.PlaySceneManagerRef.GetPassiveCriticalUpRate()))
            {
                /** 폭발화살 크리티컬 데미지 */
                GetDamage(BombArrowCriticalDamage);

                /** 크리티컬 데미지 텍스트 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(BombArrowCriticalDamage, NowPos, Color.red);
                // Debug.Log("폭발화살 크리티컬 데미지 : " + BombArrowCriticalDamage);
            }
            else
            {
                /** 폭발화살 일반 데미지 */
                GetDamage(BombArrowNormalDamage);

                /** 데미지 출력 */
                GameManager.GMInstance.PlaySceneManagerRef.DamageTxt(BombArrowNormalDamage, NowPos, Color.white);
                // Debug.Log("폭발화살 일반공격 데미지 : " + BombArrowNormalDamage);
            }


            if (CurrentMonsterHp > 0)
            {

                //피격 부분에 애니메이터를 호출하여 상태 변경
                anim.SetTrigger("Hit");
            }
            /** 몬스터가 죽으면 */
            else
            {
                // 몬스터 사망 시 경험치 함수 호출
                Dead();
                // 몬스터가 죽었으므로 비활성화
                bIsLive = false;
                /** 콜라이더 비활성화 */
                coll.enabled = false;
                /** RigidBody2D 비활성화 */
                rigid.simulated = false;
                /** sprite 레이어 1 */
                sprite.sortingOrder = 1;
                // 죽는 애니메이션
                anim.SetBool("Dead", true);
                /** 함수 호출 */
                GameManager.GMInstance.killcount++;
                // 몬스터 사망 시 킬수 증가 함수 호출
                GameManager.GMInstance.GetExp();        
            }
        }


    }

    /** TODO ## MonsterMove.cs 아이스 에이지에 닿았을 때 데미지 */
    void OnTriggerStay2D(Collider2D collision)
    {
        ///** 아이스에이지에 닿았다면 */
        //if (collision.gameObject.CompareTag("IceAge"))
        //{
        //    CurrentMonsterHp -= Time.deltaTime * GameManager.GMInstance.SkillManagerRef.IceAgeDamage;
        //    // Debug.Log(GameManager.GMInstance.SkillManagerRef.IceAgeDamage);

        //    /** 피격 효과음 재생 */
        //    GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Hit);


        //    if (CurrentMonsterHp > 0)
        //    {

        //        //피격 부분에 애니메이터를 호출하여 상태 변경
        //        anim.SetTrigger("Hit");
        //    }
        //    /** 몬스터가 죽으면 */
        //    else
        //    {
        //        // 몬스터가 죽었으므로 비활성화
        //        bIsLive = false;
        //        /** 콜라이더 비활성화 */
        //        coll.enabled = false;
        //        /** RigidBody2D 비활성화 */
        //        rigid.simulated = false;
        //        /** sprite 레이어 1 */
        //        sprite.sortingOrder = 1;
        //        // 죽는 애니메이션
        //        anim.SetBool("Dead", true);
        //        /** 함수 호출 */
        //        GameManager.GMInstance.killcount++;
        //        // 몬스터 사망 시 킬수 증가 함수 호출
        //        GameManager.GMInstance.GetExp();
        //        // 몬스터 사망 시 경험치 함수 호출
        //        // Dead();
        //    }
        //}
    }

    //public void Init(GameManager Data)
    //{
    //    MonsterMaxHp = Data.MonsterMaxHp;
    //    MonsterSpeed = Data.MonsterCurrentSpeed;
    //}

    IEnumerator KnockBack()
    {
        yield return wait; // 다음 하나의 물리 프레임 딜레이
        Vector3 playerpos = GameManager.GMInstance.Player.transform.position;
        Vector3 dirVec = transform.position - playerpos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        Debug.Log("Coroutain");
    }

    void OnDamage()
    {       
        sprite.color = Color.white;
    }

    /** 죽는 함수 */
    void Dead()
    {
        /** 포션 생성함수 */
        MakePotion();

        /** 게임 오브젝트 비활성화 */
        gameObject.SetActive(false);
        sprite.color = Color.white;

        /** 만약 이 오브젝트 이름이 Monster_D이라면 */
        if (gameObject.name == "Monster_D(Clone)" || gameObject.name == "Boss_Moai(Clone)" || gameObject.name == "Boss_Reaper(Clone)")
        {
            /** 보스 생성 x */
            GameManager.GMInstance.SpawnerRef.SetIsBossSpawn(false);
            // Debug.Log(1);

            Invoke("WaitGameClear", 2.0f);
            // GameManager.GMInstance.PlaySceneManagerRef.GameClear();
        }
    }

    void WaitGameClear()
    {
        GameManager.GMInstance.PlaySceneManagerRef.GameClear();
    }

    void MakePotion()
    {
        /** 랜덤값 생성 */
        int PotionMakePercent = Random.Range(1, 101);

        /** 랜덤 함수가 드랍율보다 적으면 */
        if (PotionMakePercent <= PotionDropPercent)
        {
            /** 포션 오브젝트 생성 */
            GameObject PotionObj = Instantiate(Potion);

            /** 포션의 위치는 몬스터가 죽은 위치로 한다. */
            PotionObj.transform.position = this.transform.position;
        }
    }

    /** 데이미지를 받음 */
    public void GetDamage(float _damage)
    {
        CurrentMonsterHp -= _damage; 
    }
}