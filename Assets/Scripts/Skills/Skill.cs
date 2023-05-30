using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using My;

public class Skill : MonoBehaviour
{
    public SkillData data;
    public int level = 0;
    Weapon weapon;
    Weapon FireBallWeapon;

    public PassiveSkill passiveSkill;
    
    public GameObject FireBallObj;
    public SkillData FireBallData;
    bool bCanfirstAttack;


    float CurrentDamage;
    float nextDamage;
    int nextCount = 0;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[2];
        icon.sprite = data.Skill_Icon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.Skill_Name;

    }

    void Start()
    {
        /** 이 스크립트 지닌 게임오브젝트가 FireBall_Btn이 아니라면 */
        if (this.gameObject.name != "FireBall_Btn")
        {
            /** 파이어볼의 레벨을 가져온다 */
            FireBallData = FireBallObj.GetComponent<Skill>().data;
        }
        else
        {
            FireBallData = this.data;
        }

        /** 시작할 때 레벨 0으로 초기화 */
        // level = 0;
    }

    void OnEnable()
    {
        /** 다음 스킬레벨을 보여준다. */
        textLevel.text = "Lv." + (level + 1);

        /** 스킬별 설명 */
        switch(data.skillType)
        {
            /** 전기구체 data를 보여줌 */
            case SkillData.SkillType.Skill_ElectricBall:
                /** 스킬레벨 0일떄  */
                if (level == 0)
                {
                    textDesc.text = "스킬\n활성화";
                }
                else
                {
                    textDesc.GetComponent<RectTransform>().localPosition = new Vector3(0, -100, 0);

                    /** 스킬 설명을 밑과 같이 적용함 / 전기구체 Count : 구체의 개수 */
                    textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100, data.counts[level]);
                }
                break;

            /** 파이어볼 data를 보여줌 */
            case SkillData.SkillType.Skill_FireBall:

                /** 스킬 설명을 밑과 같이 적용함 / 전기구체 Count : 구체의 개수 */
                textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100, data.counts[level]);
                
                break;
            /** 스킬 속도 증가나 이동 속도 증가를 선택했을때 */
            case SkillData.SkillType.Skill_SkillSpeedUp:
            case SkillData.SkillType.Skill_CharSpeedUp:
                /** 스킬 설명을 밑과 같이 적용함 */
                textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100);
                break;

            /** 크리티컬 확률 증가를 선택했을 때 */
            case SkillData.SkillType.Skill_CriticalUp:

                textName.text = "크리티컬 확률\n 증가";
                /** 스킬 설명을 밑과 같이 적용함 */
                textDesc.text = string.Format(data.Skill_Desc, data.UpRate[level] * 100);
                break;

            /** 크리티컬 데미지 증가를 선택했을 때 */
            case SkillData.SkillType.Skill_CriticalDamageUp:

                textName.text = "크리티컬 데미지\n 증가";

                /** 스킬 설명을 밑과 같이 적용함 */
                textDesc.text = string.Format(data.Skill_Desc, data.UpRate[level] * 100);
                break;

            /** 메테오 스킬을 보여줌 */
            case SkillData.SkillType.Skill_Meteo:
                /** 스킬레벨 0일떄  */
                if (level == 0)
                {
                    textDesc.text = "스킬\n활성화";
                }
                else
                {
                    textDesc.GetComponent<RectTransform>().localPosition = new Vector3(0, -110, 0);
                    textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100, data.counts[level]);
                }
                break;

            /** 아이스 에이지를 data를 보여줌 */
            case SkillData.SkillType.Skill_IceAge:
                if (level == 0)
                {
                    textDesc.text = "스킬\n활성화";
                }
                else
                {
                    textDesc.GetComponent<RectTransform>().localPosition = new Vector3(0, -120, 0);
                    textDesc.text = string.Format(data.Skill_Desc, data.damages[level], data.counts[level]);
                }
                // textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100, data.counts[level]);

                break;

            /** 낙뢰 data를 보여줌 */
            case SkillData.SkillType.Skill_Lightning:
                /** 낙뢰 스킬이 0일 때 */
                if (level == 0)
                {
                    textDesc.text = "스킬\n활성화";
                }
                else
                {
                    textDesc.GetComponent<RectTransform>().localPosition = new Vector3(0, -150, 0);
                    /** 낙뢰 증가 값과 재사용 시간을 보여준다. */
                    textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100, data.counts[level]);
                }
                break;

            /** 토네이도 data를 보여준다. */
            case SkillData.SkillType.Skill_Tornado:
                /** 토네이도 스킬이 0일 때 */
                if (level == 0)
                {
                    textDesc.text = "스킬\n활성화";
                }
                else
                {
                    textDesc.GetComponent<RectTransform>().localPosition = new Vector3(0, -150, 0);
                    /** 토네이도 데미지 증가 값과 재사용 시간을 보여준다. */
                    textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100, data.counts[level]);
                }
                break;

            case SkillData.SkillType.Skill_IceArrow:
                /** 스킬레벨 0일떄  */
                if (level == 0)
                {
                    textDesc.text = "스킬\n활성화";
                }
                else
                {
                    /** 스킬 설명을 밑과 같이 적용함 */
                    textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100, data.counts[level]);
                }
                break;

            /** 최대 체력 증가를 선택했을 때 */
            case SkillData.SkillType.Skill_HpUp:

                textName.text = "최대 체력\n증가";

                /** 스킬 설명을 밑과 같이 적용함 */
                textDesc.text = string.Format(data.Skill_Desc, data.UpRate[level] * 100);
                break;

            /** 스킬 데미지 증가를 선택했을 때 */
            case SkillData.SkillType.Skill_SkillDamageUp:

                textName.text = "스킬 데미지\n증가";

                /** 스킬 설명을 밑과 같이 적용함 */
                textDesc.text = string.Format(data.Skill_Desc, data.UpRate[level] * 100);
                break;

            default:
                textDesc.text = string.Format(data.Skill_Desc);
                break;
        } 
    }

    public void OnClick()
    {
        switch(data.skillType)
        {
            /** 전기구체나 아이스 에로우을 선택했을때 */
            case SkillData.SkillType.Skill_ElectricBall:
            case SkillData.SkillType.Skill_IceArrow:
                /** 만약 스킬레벨이 0이고 파이볼이 아닐때 */
                if (level == 0)
                {
                    /** 만약 스킬 타입이 아이스에로우라면? */
                    if (data.skillType == SkillData.SkillType.Skill_IceArrow)
                    {
                        GameManager.GMInstance.SkillManagerRef.IceArrow.SetActive(true);
                        /** 아이스에로우의 기본 데미지를 GameManager에 전달 */
                        GameManager.GMInstance.SetIceArrowBaseDamage(data.baseDamage);
                    }

                    /** 빈 게임 오브젝트 생성 */
                    GameObject newWeapon = new GameObject();
                    /** weapon은 생성된 newWeapon에 추가해준 Weapon Comoponent를 사용한다. */
                    weapon = newWeapon.AddComponent<Weapon>();
                    /** weapon에 있는 Init함수를 data를 매개변수로 호출한다. */
                    weapon.Init(data);
                    /** 전기구체의 기본 데미지를 GameManager에 전달 */
                    GameManager.GMInstance.SetElectricBallBaseDamage(data.baseDamage);

                    /** 파이어볼 레벨이 0이라면 */
                    //if (FireBallObj.GetComponent<Skill>().level == 0)
                    //{
                    //    /** 빈게임 오브젝트 생성 */
                    //    GameObject FireBallWeaponObj = new GameObject();
                    //    /** 생성된 오브젝트에 Weapon 스크립트 추가 */
                    //    FireBallWeapon = FireBallWeaponObj.AddComponent<Weapon>();
                    //    /** 파이어볼의 레벨을 올린다 */
                    //    FireBallObj.GetComponent<Skill>().level++;
                    //    /** 파이어볼의 기본 데미지 */
                    //    FireBallObj.GetComponent<Skill>().CurrentDamage = FireBallData.baseDamage;
                    //    /** 파이어볼 버튼 오브젝트의 스크립트가 가지고 있는 FireBallWeapon는 이 스크립트가 들고 있는 FireBallWeapon  */
                    //    FireBallObj.GetComponent<Skill>().FireBallWeapon = this.FireBallWeapon;
                    //    FireBallWeapon.Init(FireBallData);
                    //}
                }
                /** 스킬레벨이 0이 아니라면 */
                else
                {
                    /** 전기구체를 눌렀다면 */
                    if (data.skillType == SkillData.SkillType.Skill_ElectricBall)
                    {
                        /** 다음 전기구체의 개수는 Skilldata의 count배열의 적용된 레벨의 크기의 count가 불러와진다. */
                        nextCount = data.counts[level];
                    }

                    /** 만약 현재 데미지가 0이면 */
                    if (CurrentDamage == 0)
                    {
                        /** 다음 데미지는 SkillData에 저장된 데미지에서 저장된 데미지증가만큼 곱한 값으로 적용된다. */
                        nextDamage += data.baseDamage * data.damages[level];
                    }
                    /** 현재 데미지가 0이 아니라면 */
                    else
                    {
                        /** 다음 데미지는 현재 데미지에서 데미지 증가치만큼 곱한 값으로 적용된다. */
                        nextDamage = CurrentDamage * data.damages[level];
                    }

                    /** 현재 데미지는 값이 계산된 nextDamage로 변경 */
                    CurrentDamage = nextDamage;
                    Debug.Log(data.Skill_Name + " " + nextDamage);

                    /** Weapon.cs의 Levelup함수에 nextDamage와 nextCount를 매개변수로 호출 */
                    weapon.Levelup(nextDamage, nextCount);
                }

                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬레벨 증가 */
                level++;
                break;

            /** 파이어볼을 선택했을 때 */
            case SkillData.SkillType.Skill_FireBall:
                if (level == 0)
                {
                    /** 빈 게임 오브젝트 생성 */
                    GameObject FireBallWeaponObj = new GameObject();
                    /** weapon은 생성된 newWeapon에 추가해준 Weapon Comoponent를 사용한다. */
                    FireBallWeapon = FireBallWeaponObj.AddComponent<Weapon>();
                    /** 파이어볼의 기본 데미지를 GameManager에 전달 */
                    GameManager.GMInstance.SetFireBallBaseDamage(data.baseDamage);

                    /** 기본공격 데미지 */
                    CurrentDamage = data.baseDamage;
                    nextCount = data.baseCount;
                    /** weapon에 있는 Init함수를 data를 매개변수로 호출한다. */
                    FireBallWeapon.Init(FireBallData);
                }
                else
                {
                    /** 다음 데미지는 현재 데미지에서 데미지 증가치만큼 곱한 값으로 적용된다. */
                    nextDamage = CurrentDamage * FireBallData.damages[level];

                    /** 현재 데미지는 값이 계산된 nextDamage로 변경 */
                    CurrentDamage = nextDamage;
                    FireBallWeapon.Levelup(CurrentDamage, 0);

                    Debug.Log(data.Skill_Name + " " + nextDamage);

                    /** Weapon.cs의 Levelup함수에 nextDamage와 nextCount를 매개변수로 호출 */
                    // FireBallWeapon.Levelup(nextDamage, nextCount);
                }

                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬레벨 증가 */
                level++;
                break;
                
            /** 스킬 속도 증가나 이동 속도 증가를 선택했을때 */
            case SkillData.SkillType.Skill_SkillSpeedUp:
            case SkillData.SkillType.Skill_CharSpeedUp:
                /** 스킬레벨이 0이라면 */
                if (level == 0)
                {
                    /** newPassvieSkill이라는 빈게임 오브젝트 생성 */
                    GameObject newPassvieSkill = new GameObject();
                    /** 생성된 newPassvieSkill 빈게임 오브젝트에 추가된 PassiveSkill을 passvieSkill에 적용한다. */
                    passiveSkill = newPassvieSkill.AddComponent<PassiveSkill>();
                    /** PassiveSkill의 Init함수에 data를 매개변수로 호출 */
                    passiveSkill.Init(data);

                    /** 파이어볼 레벨이 0이라면 */
                    //if (FireBallObj.GetComponent<Skill>().level == 0)
                    //{
                    //    /** 빈게임 오브젝트 생성 */
                    //    GameObject FireBallWeaponObj = new GameObject();
                    //    /** 생성된 오브젝트에 Weapon 스크립트 추가 */
                    //    FireBallWeapon = FireBallWeaponObj.AddComponent<Weapon>();
                    //    /** 파이어볼의 레벨을 올린다 */
                    //    FireBallObj.GetComponent<Skill>().level++;
                    //    /** 파이어볼의 기본 데미지 */
                    //    FireBallObj.GetComponent<Skill>().CurrentDamage = FireBallData.baseDamage;
                    //    /** 파이어볼 버튼 오브젝트의 스크립트가 가지고 있는 FireBallWeapon는 이 스크립트가 들고 있는 FireBallWeapon  */
                    //    FireBallObj.GetComponent<Skill>().FireBallWeapon = this.FireBallWeapon;
                    //    FireBallWeapon.Init(FireBallData);
                    //}
                }
                /** 레벨이 0이 아니라면 */
                else
                {
                    /** 다음 다음 증가비율은 data에 저장된 값을 불러온다. */
                    float nextRate = data.damages[level];
                    /** PassiveSkill의 LevelUp함수에 nextRate를 매개변수로 호출한다. */
                    passiveSkill.LevelUp(nextRate);
                }
                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;

                level++;
                break;

            /** Hill을 누르면 */
            case SkillData.SkillType.Skill_Hill:
                /** 현재 캐릭터의 체력을 100%로 회복 */
                GameManager.GMInstance.Health = GameManager.GMInstance.MaxHealth;
                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                break;

                /** 메테오를 눌렀다면 */
                /** TODO ## Skill.cs 메테오 설정 로직 */
            case SkillData.SkillType.Skill_Meteo:
                /** 스킬레벨이 0이라면 */
                if(level == 0)
                {
                    /** 메테오의 스킬 타임 */
                    GameManager.GMInstance.SkillManagerRef.MateoSkillCoolTime = data.counts[level];
                    /** SkillManager의 메테오출현 함수를 true로 바꿔준다. */
                    GameManager.GMInstance.SkillManagerRef.bIsMateo = true;

                    /** 메테오의 기본 데미지를 GameManager에 전달 */
                    GameManager.GMInstance.SetMateoBaseDamage(data.baseDamage);

                    /** 초기 메테오 데미지를 설정해 준다. */
                    /** TODO ## Skill.cs 메테오 초기 데미지 설정 */
                    GameManager.GMInstance.SkillManagerRef.MateoDamage = data.baseDamage;
                    /** 현재 데미지는 메테오 SkillManager의 메테오 데미지 */
                    CurrentDamage = GameManager.GMInstance.SkillManagerRef.MateoDamage;


                    ///** 파이어볼 레벨이 0이라면 */
                    //if (FireBallObj.GetComponent<Skill>().level == 0)
                    //{
                    //    /** 빈게임 오브젝트 생성 */
                    //    GameObject FireBallWeaponObj = new GameObject();
                    //    /** 생성된 오브젝트에 Weapon 스크립트 추가 */
                    //    FireBallWeapon = FireBallWeaponObj.AddComponent<Weapon>();
                    //    /** 파이어볼의 레벨을 올린다 */
                    //    FireBallObj.GetComponent<Skill>().level++;
                    //    /** 파이어볼의 기본 데미지 */
                    //    FireBallObj.GetComponent<Skill>().CurrentDamage = FireBallData.baseDamage;
                    //    /** 파이어볼 버튼 오브젝트의 스크립트가 가지고 있는 FireBallWeapon는 이 스크립트가 들고 있는 FireBallWeapon  */
                    //    FireBallObj.GetComponent<Skill>().FireBallWeapon = this.FireBallWeapon;
                    //    FireBallWeapon.Init(FireBallData);
                    //}
                }
                else
                {
                    /** 메테오의 스킬 타임 */
                    GameManager.GMInstance.SkillManagerRef.MateoSkillCoolTime = data.counts[level];
                    /** 다음 레벨 데미지는 메테오 데미지에서 SkillData의 데미지 증가 배열에 있는 값을 곱한 값 */
                    nextDamage = CurrentDamage * data.damages[level];
                    /** 현재레벨은 계산된 nextDamage */
                    CurrentDamage = nextDamage;

                    GameManager.GMInstance.SkillManagerRef.MateoDamage = CurrentDamage;
                    Debug.Log("Mateo : " + CurrentDamage);
                }

                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬레벨 증가 */
                level++;
                break;

            /** 아이스에이지를 눌렸다면? */
            case SkillData.SkillType.Skill_IceAge:
                /** 스킬레벨이 0일때 */
                if (level == 0)
                {
                    /** 아이스 에이지 스킬 활성화를 알려준다. */
                    GameManager.GMInstance.SkillManagerRef.bIsIceAge = true;
                    /** 아이스에이지 쿨타임 시간 설정 */
                    GameManager.GMInstance.SkillManagerRef.IceAgeSkillTime = GameManager.GMInstance.SkillManagerRef.IceAgeSkillCoolTime;
                    /** 아이스 에이지 유지시간 설정 */
                    GameManager.GMInstance.SkillManagerRef.MaxIceAgeOnTime = data.counts[level];

                    /** 아이스에이지의 기본 데미지를 GameManager에 전달 */
                    GameManager.GMInstance.SetIceAgeBaseDamage(data.baseDamage);

                    /** 초기 아이스에이지 유지시간 설정 */
                    GameManager.GMInstance.SkillManagerRef.IceAgeOnTime = GameManager.GMInstance.SkillManagerRef.MaxIceAgeOnTime;
                    /** 아이스 에이지 활성화 시 데미지 */
                    GameManager.GMInstance.SkillManagerRef.IceAgeDamage = data.baseDamage;
                    /** 현재 데미지 */
                    CurrentDamage = GameManager.GMInstance.SkillManagerRef.IceAgeDamage;
                    /** 아이스 에이지 활성화 */
                    GameManager.GMInstance.SkillManagerRef.EnabledIceAge();


                    ///** 파이어볼 레벨이 0이라면 */
                    //if (FireBallObj.GetComponent<Skill>().level == 0)
                    //{
                    //    /** 빈게임 오브젝트 생성 */
                    //    GameObject FireBallWeaponObj = new GameObject();
                    //    /** 생성된 오브젝트에 Weapon 스크립트 추가 */
                    //    FireBallWeapon = FireBallWeaponObj.AddComponent<Weapon>();
                    //    /** 파이어볼의 레벨을 올린다 */
                    //    FireBallObj.GetComponent<Skill>().level++;
                    //    /** 파이어볼의 기본 데미지 */
                    //    FireBallObj.GetComponent<Skill>().CurrentDamage = FireBallData.baseDamage;
                    //    /** 파이어볼 버튼 오브젝트의 스크립트가 가지고 있는 FireBallWeapon는 이 스크립트가 들고 있는 FireBallWeapon  */
                    //    FireBallObj.GetComponent<Skill>().FireBallWeapon = this.FireBallWeapon;
                    //    FireBallWeapon.Init(FireBallData);
                    //}

                }
                else
                {
                    /** 아이스에이지 유지시간 */
                    GameManager.GMInstance.SkillManagerRef.MaxIceAgeOnTime = data.counts[level];
                    /** 레벨업한 아이스에이지 데미지 */
                    nextDamage = CurrentDamage * data.damages[level];
                    /** 현재 데미지값은 레벨업하여 증가된 데미지 */
                    CurrentDamage = nextDamage;
                    /** 아이스에이지 데미지 적용 */
                    GameManager.GMInstance.SkillManagerRef.IceAgeDamage = CurrentDamage;
                    
                }
            
                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬레벨 증가 */
                level++;
                break;

            /** 낙뢰를 선택했을 때 */
            case SkillData.SkillType.Skill_Lightning:
                /** 낙뢰 스킬레벨이 0일 떄 */
                if (level == 0)
                {
                    /** 낙뢰 스킬 활성화 */
                    GameManager.GMInstance.SkillManagerRef.bIsLightning = true;
                    /** 낙뢰 스킬의 기본 데미지 */
                    GameManager.GMInstance.SkillManagerRef.LightningDamage = data.baseDamage;
                    /** 냑뢰의 기본 데미지를 GameManager에 전달 */
                    GameManager.GMInstance.SetLightningBaseDamage(data.baseDamage);
                    /** 현재 낙뢰데미지 */
                    CurrentDamage = GameManager.GMInstance.SkillManagerRef.LightningDamage;
                    /** 스킬 쿨타임 설정 */
                    GameManager.GMInstance.SkillManagerRef.LightningSkillCoolTime = data.counts[level];
                    /** 스킬 시간 감소 적용될 변수 초기화 */
                    GameManager.GMInstance.SkillManagerRef.LightningSkillTime = GameManager.GMInstance.SkillManagerRef.LightningSkillCoolTime;


                    ///** 파이어볼 레벨이 0이라면 */
                    //if (FireBallObj.GetComponent<Skill>().level == 0)
                    //{
                    //    /** 빈게임 오브젝트 생성 */
                    //    GameObject FireBallWeaponObj = new GameObject();
                    //    /** 생성된 오브젝트에 Weapon 스크립트 추가 */
                    //    FireBallWeapon = FireBallWeaponObj.AddComponent<Weapon>();
                    //    /** 파이어볼의 레벨을 올린다 */
                    //    FireBallObj.GetComponent<Skill>().level++;
                    //    /** 파이어볼의 기본 데미지 */
                    //    FireBallObj.GetComponent<Skill>().CurrentDamage = FireBallData.baseDamage;
                    //    /** 파이어볼 버튼 오브젝트의 스크립트가 가지고 있는 FireBallWeapon는 이 스크립트가 들고 있는 FireBallWeapon  */
                    //    FireBallObj.GetComponent<Skill>().FireBallWeapon = this.FireBallWeapon;
                    //    FireBallWeapon.Init(FireBallData);
                    //}
                }
                /** 낙뢰스킬레벨이 0이 아닐 때 */
                else
                {
                    /** 스킬 쿨타임 설정 */
                    GameManager.GMInstance.SkillManagerRef.LightningSkillCoolTime = data.counts[level];

                    /** 레벨업하여 증가된 데미지 */
                    nextDamage = CurrentDamage * data.damages[level];
                    /** 레벨업하여 증가된 현재 데미지 */
                    CurrentDamage = nextDamage;
                    /** 낙뢰의 데미지 값으로 준다 */
                    GameManager.GMInstance.SkillManagerRef.LightningDamage = CurrentDamage;
                }

                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬레벨 증가 */
                level++;
                break;

            /** 토네이도를 선택했을때 */
            case SkillData.SkillType.Skill_Tornado:

                /** 토네이도 스킬레벨이 0일 때 */
                if (level == 0)
                {
                    /** 토네이도 스킬 활성화 */
                    GameManager.GMInstance.SkillManagerRef.bIsTornado = true;
                    /** 토네이도 데미지는 baseDamage에 설정된 값 */
                    GameManager.GMInstance.SkillManagerRef.TornadoDamage = data.baseDamage;
                    /** 토네이토의 기본 데미지를 GameManager에 전달 */
                    GameManager.GMInstance.SetTornadoBaseDamage(data.baseDamage);
                    /** 스킬 쿨타임 설정 */
                    GameManager.GMInstance.SkillManagerRef.TornadoSkillCoolTime = data.counts[level];
                    /** 감소될 시간 값 초기화 */
                    GameManager.GMInstance.SkillManagerRef.TornadoSkillTime = GameManager.GMInstance.SkillManagerRef.TornadoSkillCoolTime;
                    /** 현재 데미지 값 */
                    CurrentDamage = GameManager.GMInstance.SkillManagerRef.TornadoDamage;


                    /** 파이어볼 레벨이 0이라면 */
                    //if (FireBallObj.GetComponent<Skill>().level == 0)
                    //{
                    //    /** 빈게임 오브젝트 생성 */
                    //    GameObject FireBallWeaponObj = new GameObject();
                    //    /** 생성된 오브젝트에 Weapon 스크립트 추가 */
                    //    FireBallWeapon = FireBallWeaponObj.AddComponent<Weapon>();
                    //    /** 파이어볼의 레벨을 올린다 */
                    //    FireBallObj.GetComponent<Skill>().level++;
                    //    /** 파이어볼의 기본 데미지 */
                    //    FireBallObj.GetComponent<Skill>().CurrentDamage = FireBallData.baseDamage;
                    //    FireBallWeapon.Init(FireBallData);
                    //}

                }
                else
                {
                    /** 스킬 쿨타임 설정 */
                    GameManager.GMInstance.SkillManagerRef.TornadoSkillCoolTime = data.counts[level];
                    /** 증가된 데미지 계산 */
                    nextDamage = CurrentDamage * data.damages[level];
                    /** 현재 데미지는 증가된 데미지로 초기화 */
                    CurrentDamage = nextDamage;
                    /** 토네이도 데미지 값 초기화 */
                    GameManager.GMInstance.SkillManagerRef.TornadoDamage = CurrentDamage;
                }

                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬레벨 증가 */
                level++;
                break;

            /** 크리티컬 확률을 선택했을 때 */
            case SkillData.SkillType.Skill_CriticalUp:
                /** 크리티컬 레벨에 따라 확률 증가 */
                float CriticalUpRate = data.UpRate[level];
                /** 플레이씬 매니저에 크리티컬 증가값을 넘겨준다. */
                GameManager.GMInstance.PlaySceneManagerRef.SetPassiveCriticalUpRate(CriticalUpRate);

                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬레벨 증가 */
                level++;
                break;

            /** 크리티컬 데미지 증가를 선택했을 때 */
            case SkillData.SkillType.Skill_CriticalDamageUp:

                /** 크리티컬 데미지 증가 레벨에 따라 확률 증가 */
                float CriticalDamageUpRate = data.UpRate[level];
                /** 플레이씬 매니저에 크리티컬 데미지 증가값을 넘겨준다. */
                GameManager.GMInstance.PlaySceneManagerRef.SetPassiveCriticalDamageUpRate(CriticalDamageUpRate);

                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬레벨 증가 */
                level++;
                break;

            /** 최대 체력 증가를 선택했을 때 */
            case SkillData.SkillType.Skill_HpUp:
                /** 최대 체력 10퍼 증가 */
                GameManager.GMInstance.MaxHealth += GameManager.GMInstance.BaseHp * data.UpRate[level];
                /** 늘어난 체력만큼 현재 체력 보충 */
                GameManager.GMInstance.Health += GameManager.GMInstance.BaseHp * data.UpRate[level];

                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬레벨 증가 */
                level++;
                break;

            /** 스킬 데미지 증가를 선택했을 때 */
            case SkillData.SkillType.Skill_SkillDamageUp:

                GameManager.GMInstance.PlaySceneManagerRef.PassiveSkillDamageUpRate += data.UpRate[level];

                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬레벨 증가 */
                level++;
                break;


            /** TODO ## Skill.cs 아이스 에로우 주석 */
            //case SkillData.SkillType.Skill_IceArrow:
            //     /** 만약 스킬레벨이 0이라면 */
            //    if (level == 0)
            //    {
            //        /** 아이스에로우 활성화 */
            //        GameManager.GMInstance.SkillManagerRef.IceArrow.SetActive(true);
            //    }
            //    /** 스킬레벨이 0이 아니라면 */
            //    else
            //    {
            //        /** 다음 데미지는 현재 데미지에서 데미지 증가치만큼 곱한 값으로 적용된다. */
            //        nextDamage = CurrentDamage * data.damages[level];
            //        /** 현재 데미지는 값이 계산된 nextDamage로 변경 */
            //        CurrentDamage = nextDamage;
            //        Debug.Log(data.Skill_Name + " " + nextDamage);
            //        /** Weapon.cs의 Levelup함수에 nextDamage와 nextCount를 매개변수로 호출 */
            //        weapon.Levelup(nextDamage, nextCount);
            //    }

            //    /** 게임을 실행시킨다. */
            //    GameManager.GMInstance.bIsLive = true;
            //    /** 스킬레벨 증가 */
            //    level++;
            //    break;

            default:
                break;
        }

        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
