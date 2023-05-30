using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My;
using UnityEngine.UI;

public class AcherSkill : MonoBehaviour
{
    public SkillData data;
    public int level = 0;
    Weapon weapon;
    Weapon ArrowWeapon;

    public GameObject ArrowObj;
    public SkillData ArrowData;

    public PassiveSkill passiveSkill;
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
        /** 이 스크립트 지닌 게임오브젝트가 Arrow_Btn이 아니라면 */
        if (this.gameObject.name != "Arrow_Btn")
        {
            /** 파이어볼의 레벨을 가져온다 */
            ArrowData = ArrowObj.GetComponent<AcherSkill>().data;
        }
        else
        {
            ArrowData = this.data;
        }
    }

    void OnEnable()
    {
        /** 다음 스킬레벨을 보여준다. */
        textLevel.text = "Lv." + (level + 1);

        /** 스킬별 설명 */
        switch (data.skillType)
        {
            case SkillData.SkillType.Skill_Arrow:
                /** 스킬 설명을 밑과 같이 적용함 / 전기구체 Count*/
                textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100);
                break;

            case SkillData.SkillType.Skill_Vortex:
                /** 볼텍스 스킬이 0일 때 */
                if (level == 0)
                {
                    textDesc.text = "스킬\n활성화";
                }
                else
                {
                    /** 볼텍스 데미지 증가 값과 재사용 시간을 보여준다. */
                    textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100, data.counts[level]);
                }
                break;

            /** 허리케인 data를 보여줌 */
            case SkillData.SkillType.Skill_Huricane:
                if (level == 0)
                {
                    textDesc.text = "스킬\n활성화";
                }
                else
                {
                    textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100);
                }
                // textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100);

                break;

            /** 바람정령 data를 보여줌 */
            case SkillData.SkillType.Skill_WindSpirit:
                if (level == 0)
                {
                    textDesc.text = "스킬\n활성화";
                }
                else
                {
                    textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100);
                }
                // textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100);
                break;

            /** 트랩 data를 보여줌 */
            case SkillData.SkillType.Skill_Trap:
                if (level == 0)
                {
                    textDesc.text = "스킬\n활성화";
                }
                else
                {
                    textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100);
                }
                // textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100);
                break;

            /** 화살 비 data를 보여줌 */
            case SkillData.SkillType.Skill_ArrowRain:
                /** 화살 비 스킬이 0일 때 */
                if (level == 0)
                {
                    textDesc.text = "스킬\n활성화";
                }
                else
                {
                    /** 화살 비 증가 값과 재사용 시간을 보여준다. */
                    textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100, data.counts[level]);
                }
                break;

            /** 폭탄화살 data를 보여줌 */
            case SkillData.SkillType.Skill_BombArrow:
                /** 화살 비 스킬이 0일 때 */
                if (level == 0)
                {
                    textDesc.text = "스킬\n활성화";
                }
                else
                {
                    /** 화살 비 증가 값과 재사용 시간을 보여준다. */
                    textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100, data.counts[level]);
                }
                break;

            /** 이동 속도 증가를 선택했을때 */
            case SkillData.SkillType.Skill_CharSpeedUp:
                /** 스킬 설명을 밑과 같이 적용함 */
                textDesc.text = string.Format(data.Skill_Desc, data.damages[level] * 100);
                break;

            /** 크리티컬 확률 증가를 선택했을 때 */
            case SkillData.SkillType.Skill_CriticalUp:

                textName.text = "크리티컬 확률\n증가";
                /** 스킬 설명을 밑과 같이 적용함 */
                textDesc.text = string.Format(data.Skill_Desc, data.UpRate[level] * 100);
                break;

            /** 크리티컬 데미지 증가를 선택했을 때 */
            case SkillData.SkillType.Skill_CriticalDamageUp:

                textName.text = "크리티컬 데미지\n증가";

                /** 스킬 설명을 밑과 같이 적용함 */
                textDesc.text = string.Format(data.Skill_Desc, data.UpRate[level] * 100);
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
        switch (data.skillType)
        {
            /** 화살공격 클릭 시 */
            case SkillData.SkillType.Skill_Arrow:
                 if (level == 0)
                {
                    /** 빈 게임 오브젝트 생성 */
                    GameObject ArrowWeaponObj = new GameObject();
                    /** weapon은 생성된 newWeapon에 추가해준 Weapon Comoponent를 사용한다. */
                    ArrowWeapon = ArrowWeaponObj.AddComponent<Weapon>();
                    /** 화살 공격의 기본 데미지를 GameManager에 전달 */
                    GameManager.GMInstance.SetArrowBaseDamage(data.baseDamage);
                    /** 기본공격 데미지 */
                    CurrentDamage = data.baseDamage;
                    nextCount = data.baseCount;
                    /** weapon에 있는 Init함수를 data를 매개변수로 호출한다. */
                    ArrowWeapon.Init(ArrowData);
                }
                else
                {
                    /** 다음 데미지는 현재 데미지에서 데미지 증가치만큼 곱한 값으로 적용된다. */
                    nextDamage = CurrentDamage * ArrowData.damages[level];
                    /** 현재 데미지는 값이 계산된 nextDamage로 변경 */
                    CurrentDamage = nextDamage;
                    ArrowWeapon.Levelup(CurrentDamage, 0);

                    Debug.Log(data.Skill_Name + " " + nextDamage);

                    /** Weapon.cs의 Levelup함수에 nextDamage와 nextCount를 매개변수로 호출 */
                }
                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬 레벨 증가 */
                level++;
                break;

            /** 볼텍스를 클릭했을 때 */
            case SkillData.SkillType.Skill_Vortex:
                /** 볼텍스 스킬레벨이 0일 때 */
                if (level == 0)
                {
                    /** 볼텍스 스킬 활성화 */
                    GameManager.GMInstance.SkillManagerRef.bIsVortex = true;
                    /** 볼텍스 데미지는 baseDamage에 설정된 값 */
                    GameManager.GMInstance.SkillManagerRef.VortexDamage = data.baseDamage;
                    /** 볼텍스의 기본 데미지를 GameManager에 전달 */
                    GameManager.GMInstance.SetVortexBaseDamage(data.baseDamage);
                    /** 스킬 쿨타임 설정 */
                    GameManager.GMInstance.SkillManagerRef.VortexSkillCoolTime = data.counts[level];
                    /** 감소될 시간 값 초기화 */
                    GameManager.GMInstance.SkillManagerRef.VortexSkillTime = GameManager.GMInstance.SkillManagerRef.VortexSkillCoolTime;
                    /** 현재 데미지 값 */
                    CurrentDamage = GameManager.GMInstance.SkillManagerRef.VortexDamage;

                }
                else
                {
                    /** 스킬 쿨타임 설정 */
                    GameManager.GMInstance.SkillManagerRef.VortexSkillCoolTime = data.counts[level];
                    /** 증가된 데미지 계산 */
                    nextDamage = CurrentDamage * data.damages[level];
                    /** 현재 데미지는 증가된 데미지로 초기화 */
                    CurrentDamage = nextDamage;
                    /** 볼텍스 데미지 값 초기화 */
                    GameManager.GMInstance.SkillManagerRef.VortexDamage = CurrentDamage;
                }
                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬 레벨 증가 */
                level++;
                break;

            /** 허리케인 선택 */
            case SkillData.SkillType.Skill_Huricane:
                /** 스킬레벨이 0일때 */
                if (level == 0)
                {
                    /** 허리케인 스킬 활성화를 알려준다. */
                    GameManager.GMInstance.SkillManagerRef.bIsHuricane = true;
                    /** 허리케인 쿨타임 시간 설정 */
                    GameManager.GMInstance.SkillManagerRef.HuricaneSkillTime = GameManager.GMInstance.SkillManagerRef.HuricaneSkillCoolTime;
                    /** 허리케인 유지시간 설정 */
                    GameManager.GMInstance.SkillManagerRef.MaxHuricaneOnTime = data.counts[level];

                    /** 허리케인의 기본 데미지를 GameManager에 전달 */
                    GameManager.GMInstance.SetHuricaneBaseDamage(data.baseDamage);

                    /** 초기 허리케인 유지시간 설정 */
                    GameManager.GMInstance.SkillManagerRef.HuricaneOnTime = GameManager.GMInstance.SkillManagerRef.MaxHuricaneOnTime;
                    /** 허리케인 활성화 시 데미지 */
                    GameManager.GMInstance.SkillManagerRef.HuricaneDamage = data.baseDamage;
                    /** 현재 데미지 */
                    CurrentDamage = GameManager.GMInstance.SkillManagerRef.HuricaneDamage;
                    /** 허리케인 활성화 */
                    GameManager.GMInstance.SkillManagerRef.EnabledHuricane();
                }
                else
                {
                    /** 허리케인 유지시간 */
                    GameManager.GMInstance.SkillManagerRef.MaxHuricaneOnTime = data.counts[level];
                    /** 레벨업한 허리케인 데미지 */
                    nextDamage = CurrentDamage * data.damages[level];
                    /** 현재 데미지값은 레벨업하여 증가된 데미지 */
                    CurrentDamage = nextDamage;
                    /** 허리케인 데미지 적용 */
                    GameManager.GMInstance.SkillManagerRef.HuricaneDamage = CurrentDamage;
                }
                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬레벨 증가 */
                level++;
                break;

            /** 바람 정령 선택 시 */
            case SkillData.SkillType.Skill_WindSpirit:
                /** 만약 스킬레벨이 0이고 파이볼이 아닐때 */
                if (level == 0)
                {
                    GameManager.GMInstance.SkillManagerRef.WindSpirit.SetActive(true);
                    /** 빈 게임 오브젝트 생성 */
                    GameObject newWeapon = new GameObject();
                    /** weapon은 생성된 newWeapon에 추가해준 Weapon Comoponent를 사용한다. */
                    weapon = newWeapon.AddComponent<Weapon>();
                    /** weapon에 있는 Init함수를 data를 매개변수로 호출한다. */
                    weapon.Init(data);
                    /** 현재 데미지 저장 */
                    CurrentDamage = data.baseDamage;
                    /** 바람 정령의 기본 데미지를 GameManager에 전달 */
                    GameManager.GMInstance.SetWindSpiritBaseDamage(data.baseDamage);
                }
                /** 스킬레벨이 0이 아니라면 */
                else
                {
                    /** 다음 데미지는 현재 데미지에서 데미지 증가치만큼 곱한 값으로 적용된다. */
                    nextDamage = CurrentDamage * data.damages[level];
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


            /** 트랩 선택할 시 */
            case SkillData.SkillType.Skill_Trap:
                /** 스킬레벨이 0이라면 */
                if (level == 0)
                {
                    /** 트랩의 스킬 타임 */
                    GameManager.GMInstance.SkillManagerRef.TrapCoolTime = data.counts[level];
                    /** SkillManager의 트랩 출현 함수를 true로 바꿔준다. */
                    GameManager.GMInstance.SkillManagerRef.bIsTrap = true;
                    /** 트랩의 기본 데미지를 GameManager에 전달 */
                    GameManager.GMInstance.SetTrapBaseDamage(data.baseDamage);
                    /** 초기 트랩 데미지를 설정해 준다. */
                    /** TODO ## Skill.cs 메테오 초기 데미지 설정 */
                    GameManager.GMInstance.SkillManagerRef.TrapDamage = data.baseDamage;
                    /** 현재 데미지는 트랩 SkillManager의 트랩 데미지 */
                    CurrentDamage = GameManager.GMInstance.SkillManagerRef.TrapDamage;
                }
                else
                {
                    /** 트랩의 스킬 타임 */
                    GameManager.GMInstance.SkillManagerRef.TrapCoolTime = data.counts[level];
                    /** 다음 레벨 데미지는 트랩 데미지에서 SkillData의 데미지 증가 배열에 있는 값을 곱한 값 */
                    nextDamage = CurrentDamage * data.damages[level];
                    /** 현재레벨은 계산된 nextDamage */
                    CurrentDamage = nextDamage;
                    GameManager.GMInstance.SkillManagerRef.TrapDamage = CurrentDamage;
                    Debug.Log("Trap : " + CurrentDamage);
                }
                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬레벨 증가 */
                level++;
                break;

            /** 화살 비를 선택했을 때 */
            case SkillData.SkillType.Skill_ArrowRain:
                /** 화살비 스킬레벨이 0일 떄 */
                if (level == 0)
                {
                    /** 화살 비 스킬 활성화 */
                    GameManager.GMInstance.SkillManagerRef.bIsArrowRain = true;
                    /** 화살 비 스킬의 기본 데미지 */
                    GameManager.GMInstance.SkillManagerRef.ArrowRainDamage = data.baseDamage;
                    /** 화살비의 기본 데미지를 GameManager에 전달 */
                    GameManager.GMInstance.SetArrowRainBaseDamage(data.baseDamage);
                    /** 현재 화살 비 데미지 */
                    CurrentDamage = GameManager.GMInstance.SkillManagerRef.ArrowRainDamage;
                    /** 스킬 쿨타임 설정 */
                    GameManager.GMInstance.SkillManagerRef.ArrowRainSkillCoolTime = data.counts[level];
                    /** 스킬 시간 감소 적용될 변수 초기화 */
                    GameManager.GMInstance.SkillManagerRef.ArrowRainSkillTime = GameManager.GMInstance.SkillManagerRef.ArrowRainSkillCoolTime;
                }
                /** 화살 비 스킬레벨이 0이 아닐 때 */
                else
                {
                    /** 스킬 쿨타임 설정 */
                    GameManager.GMInstance.SkillManagerRef.ArrowRainSkillCoolTime = data.counts[level];

                    /** 레벨업하여 증가된 데미지 */
                    nextDamage = CurrentDamage * data.damages[level];
                    /** 레벨업하여 증가된 현재 데미지 */
                    CurrentDamage = nextDamage;
                    /** 화살 비의 데미지 값으로 준다 */
                    GameManager.GMInstance.SkillManagerRef.ArrowRainDamage = CurrentDamage;
                }

                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬레벨 증가 */
                level++;
                break;

            /** 폭탄화살 클릭 시 */
            case SkillData.SkillType.Skill_BombArrow:
                if (level == 0)
                {
                    /** 빈 게임 오브젝트 생성 */
                    GameObject newWeapon = new GameObject();
                    /** weapon은 생성된 newWeapon에 추가해준 Weapon Comoponent를 사용한다. */
                    weapon = newWeapon.AddComponent<Weapon>();
                    /** weapon에 있는 Init함수를 data를 매개변수로 호출한다. */
                    weapon.Init(data);

                    GameManager.GMInstance.SkillManagerRef.BombArrowDamage = data.baseDamage;
                    /** 현재 데미지 저장 */
                    CurrentDamage = data.baseDamage;
                    /** 바람 정령의 기본 데미지를 GameManager에 전달 */
                    GameManager.GMInstance.SetBombArrowBaseDamage(data.baseDamage);
                }
                else
                {
                    /** 다음 데미지는 현재 데미지에서 데미지 증가치만큼 곱한 값으로 적용된다. */
                    nextDamage = CurrentDamage * data.damages[level];
                    /** 현재 데미지는 값이 계산된 nextDamage로 변경 */
                    CurrentDamage = nextDamage;
                    // Debug.Log(data.Skill_Name + " " + nextDamage);
                    /** Weapon.cs의 Levelup함수에 nextDamage와 nextCount를 매개변수로 호출 */

                    GameManager.GMInstance.SkillManagerRef.BombArrowDamage = CurrentDamage;
                }
                /** 게임을 실행시킨다. */
                GameManager.GMInstance.bIsLive = true;
                /** 스킬 레벨 증가 */
                level++;
                break;

            /** 이동 속도 증가를 선택했을때 */
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

            default:
                break;

        }

        /** 최대 레벨 도달 시 */
        if (level == data.damages.Length)
        {
            /** 버튼 누를 수 없게 비활성화 */
            GetComponent<Button>().interactable = false;
        }
    }
}

