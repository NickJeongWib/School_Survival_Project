using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Define;

namespace My
{
    /** TODO ## GamaManager Script */
    public class GameManager : MonoBehaviour
    {
        public float gameTime;
        public float maxGameTime;

        [Header("# Player Info")]
        public float Health;
        public float MaxHealth;
        public int level;
        public int killcount;
        public int exp;
        public int[] nextExp;
        public bool bIsLive;

        /** GameManager타입의 메모리를 미리 확보해 둔다. */
        public static GameManager GMInstance;
        [Header("-----InGameData-----")]
        public float PlayTime;
        public int MagicStone;
        public int Diamond;
        public int SkillTicket;
        public int EnterTicket;

        [Header("-----PlayerData-----")]
        public float BaseHp;
        public float PlayerSpeed;
        float BasePlayerSpeed;
        public float DashSpeed;
        public float CharacterCriticalPercent = 0.15f;
        public float CharacterCriticalDamage = 1.5f;
        public float SkillDamageUp = 0.1f;

        [Header("-----Data-----")]
        /** 몬스터 스폰시간 */
        public float MonsterSpawnTime;
        /** 몬스터 이동속도 */
        public float MonsterSpeed;
        /** TODO ## GameManager 몬스터 Init함수 */
        public float MonsterMaxHp;
        public float MonsterCurrentSpeed;

        [Header("-----ManagerRef-----")]
        public PoolManager PoolManagerRef;
        public PlaySceneManager PlaySceneManagerRef;
        public LobbySceneManager LobbySceneManagerRef;
        public ScrollManager ScrollManagerRef;
        public ImageChange ImageChangeRef;
        public CharacterDataManager CharacterDataManagerRef;
        public SkillManager SkillManagerRef;
        public MonsterMove MonsterMoveRef;
        public Scanner ScannerRef;
        public CreatureScanner CreatureScannerRef;
        public SoundManager SoundManagerRef;
        public Spawner SpawnerRef;
        public UpGradeManager UpGradeManagerRef;
        public CoinManager CoinManagerRef;

        [Header("-----PlaySceneObject-----")]
        GameObject SceneSelectManager;

        [Header("-----CharacterType-----")]
        public ECharacterType CurrentChar;

        [Header("-----SceneType-----")]
        public ESceneType CurrentScene;

        [Header("-----DungeonType-----")]
        public ESelectDungeon SelectDungeonMode;

        [Header("-----GameObject-----")]
        public GameObject Player;
        public LevelUp UiLevelUp;
        public AcherLevelUp AcherLevelUpRef;

        [Header("-----AbilityRate-----")]
        public float SkillDamageUpRate;
        public float MaxHpLevelUpRate;
        public float CharSpeedLevelUpRate;
        public float CharCriticalPerUpRate;
        public float CharCriticalDamageUpRate;

        [Header("-----AbilityLevel-----")]
        /** 능력치 레벨 */
        public int SkillDamageLevel = 1;
        public int MaxHpLevel = 1;
        public int CharSpeedLevel = 1;
        public int CharCriticalPerLevel = 1;
        public int CharCriticalDamageLevel = 1;

        /** 마법사 기본 데미지 저장 (public 제거 가능) */
        [Header("-----WizardSkillBaseDamage-----")]
        float FireBallBaseDamage;
        float ElectricBallBaseDamage;
        float MateoBaseDamage;
        float IceArrowBaseDamage;
        float IceAgeBaseDamage;
        float TornadoBaseDamage;
        float LightningBaseDamage;

        /** 궁수 기본 데미지 저장 (public 제거 가능) */
        [Header("-----AcherSkillBaseDamage-----")]
        public float ArrowBaseDamage;
        public float VortexBaseDamage;
        public float HuricaneBaseDamage;
        public float WindSpiritBaseDamage;
        public float TrapBaseDamage;
        public float ArrowRainBaseDamage;
        public float BombArrowBaseDamage;

        [Header("-----Advertisements-----")]
        public RewardedAdsButton RewardedAdsButtonRef;

        [Header("-----Component-----")]
        public PlayerController playerCtrl;
        public EndGameAdsPanel EndGameAdsPanelRef;
        public EndGameAdsPanel GameFailedAdsPanelRef;

        [Header("-----UpGradeData-----")]
        public int UpPriceRate;
        /** 스킬 데미지 증가 관련 */
        public float SkillDamageUpSum;
        public int SkillDamageUpPrice;
        public int SkillDamageUpLevel;
        /** 최대체력 증가 관련 */
        public float MaxHpUpSum;
        public int MaxHpUpPrice;
        public int MaxHpUpLevel;
        /** 이동속도 증가 관련 */
        public float SpeedUpSum;
        public int SpeedUpPrice;
        public int SpeedUpLevel;
        /** 크리티컬 확률 증가 관련 */
        public float CriticalUpSum;
        public int CriticalUpPrice;
        public int CriticalUpLevel;
        /** 크리티컬 데미지 증가 관련 */
        public float CriticalDamageUpSum;
        public int CriticalDamageUpPrice;
        public int CriticalDamageUpLevel;

        /** 공격력 레벨 반환함수 */
        //public int GetAttackAbilityLevel()
        //{
        //    return AttackAbilityLevel;
        //}

        ///** 최대체력증가 레벨 반환함수 */
        //public int GetMaxHpLevel()
        //{
        //    return MaxHpLevel;
        //}

        ///** 크리티컬확률 증가 레벨 반환 함수 */
        //public int GetCharCriticalPerLevel()
        //{
        //    return CharCriticalPerLevel;
        //}

        ///** 크리티컬데미지 증가 레벨 반환 함수 */
        //public int GetCharCriticalDamageLevel()
        //{
        //    return CharCriticalDamageLevel;
        //}

        public float GetPlayerBaseSpeed() 
        {
            return BasePlayerSpeed;
        }

        public void InfoInit()
        {
            level = 1;
            killcount = 0;
            exp = 0;
        }

        public void SetPlayerSpeedInit(float Basespeed)
        {
            PlayerSpeed = Basespeed;
        }

        void Awake()
        {
            /** GMInstance는 이 클래스를 의미한다. */
            GMInstance = this;

            /** 캐릭터 기본이동속도 저장 */
            BasePlayerSpeed = PlayerSpeed;

            /** 화면이 바껴도 클래스 유지 */
            DontDestroyOnLoad(gameObject);    
        }

        void Start()
        {
            InitData();
        }

        void Update()
        {
            /** 플레이 화면이 아니라면 */
            if (SceneManager.GetActiveScene().name != "PlayScene")
            {
                /** 게임 플레이 시간 0으로 */
                gameTime = 0;
                PlayTime = 0;
                return;
            }

            if (bIsLive == false)
            {
                return;
            }

            /** 플레이 시간 증가 */
            gameTime += Time.deltaTime;

            if (gameTime > maxGameTime)
            {
                gameTime = maxGameTime;
            }
        }

        /** 경험치 획득 함수 */
        public void GetExp()
        {
            /** EXP 증가 */
            exp++;

            /**
            만약 EXP가 다음 레벨 업 경험치를 다 획득했다면 
            Mathf.Min(level, nextExp.Length - 1) : 10레벨 이후의 경험치는 똑같음
            */
            if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
            {
                /** 레벨 업 사운드 실행 */
                SoundManagerRef.PlaySFX(SoundManager.SFX.LevelUp);

                /** 레벨 증가 */
                level++;

                /** 경험치 량 초기화 */
                exp = 0;

                if (CurrentChar == ECharacterType.WizardChar)
                {
                    /** 스킬 선택창 오픈 */
                    UiLevelUp.Show();
                }
                else if (CurrentChar == ECharacterType.AcherChar)
                {
                    AcherLevelUpRef.Show();
                }

                /** 광고 버튼 활성화 */
                RewardedAdsButtonRef._showSkillReSelectAdButton.interactable = true;
            }
        }

        /** 플레이화면 정지 */
        public void PlayStop()
        {
            bIsLive = false;

            Time.timeScale = 0;
        }

        /** 플레이화면 다시시작 */
        public void PlayResume()
        {
            bIsLive = true;
            Time.timeScale = 1;
        }

        public void PlaySceneInit(bool _bIsLive, float _gametime, float _maxHp)
        {
            bIsLive = _bIsLive;
            gameTime = _gametime;
            Health = _maxHp;
        }

        /** 마법사 스킬 초기 데미지 설정 */
        #region SetWizardSkillBaseDamage
        public void SetFireBallBaseDamage(float basedamage)
        {
            FireBallBaseDamage = basedamage;
        }

        public void SetElectricBallBaseDamage(float basedamage)
        {
            ElectricBallBaseDamage = basedamage;
        }

        public void SetMateoBaseDamage(float basedamage)
        {
            MateoBaseDamage = basedamage;
        }

        public void SetIceArrowBaseDamage(float basedamage)
        {
            IceArrowBaseDamage = basedamage;
        }

        public void SetIceAgeBaseDamage(float basedamage)
        {
            IceAgeBaseDamage = basedamage;
        }

        public void SetTornadoBaseDamage(float basedamage)
        {
            TornadoBaseDamage = basedamage;
        }

        public void SetLightningBaseDamage(float basedamage)
        {
            LightningBaseDamage = basedamage;
        }
        #endregion

        /** 마법사 스킬 초기 데미지 반환 */
        #region GetWizardSkillBaseDamage

        public float GetFireBallBaseDamage()
        {
            return FireBallBaseDamage;
        }

        public float GetElectricBallBaseDamage()
        {
            return ElectricBallBaseDamage;
        }

        public float GetMateoBaseDamage()
        {
            return MateoBaseDamage;
        }

        public float GetIceArrowBaseDamage()
        {
            return IceArrowBaseDamage;
        }

        public float GetIceAgeBaseDamage()
        {
            return IceAgeBaseDamage;
        }

        public float GetTornadoBaseDamage()
        {
            return TornadoBaseDamage;
        }

        public float GetLightningBaseDamage()
        {
            return LightningBaseDamage;
        }

        #endregion

        /** 궁수 스킬 초기 데미지 설정 */
        #region
        /** 초기 볼텍스 데미지 저장 */
        public void SetArrowBaseDamage(float basedamage)
        {
            ArrowBaseDamage = basedamage;
        }

        /** 볼텍스 기본 데미지 저장 */
        public void SetVortexBaseDamage(float basedamage)
        {
            VortexBaseDamage = basedamage;
        }

        /** 허리케인 기본 데미지 저장 */
        public void SetHuricaneBaseDamage(float basedamage)
        {
            HuricaneBaseDamage = basedamage;
        }

        /** 바람정령 기본 데미지 저장 */
        public void SetWindSpiritBaseDamage(float basedamage)
        {
            WindSpiritBaseDamage = basedamage;
        }

        /** 트랩 기본 데미지 저장 */
        public void SetTrapBaseDamage(float basedamage)
        {
            TrapBaseDamage = basedamage;
        }

        /** 화살비 기본 데미지 저장 */
        public void SetArrowRainBaseDamage(float basedamage)
        {
            ArrowRainBaseDamage = basedamage;
        }

        /** 폭탄화살 기본 데미지 저장 */
        public void SetBombArrowBaseDamage(float basedamage)
        {
            BombArrowBaseDamage = basedamage;
        }
        #endregion

        /** 궁수 스킬 초기 데미지 반환 */
        #region
        /** 화살 기본 데미지 반환 */
        public float GetArrowBaseDamage()
        {
            return ArrowBaseDamage;
        }
        /** 볼텍스 기본 데미지 반환 */
        public float GetVortexBaseDamage()
        {
            return VortexBaseDamage;
        }

        /** 허리케인 기본 데미지 반환 */
        public float GetHuricaneBaseDamage()
        {
            return HuricaneBaseDamage;
        }

        /** 바람정령 기본 데미지 반환 */
        public float GetWindSpiritBaseDamage()
        {
            return WindSpiritBaseDamage;
        }

        /** 바람정령 기본 데미지 반환 */
        public float GetTrapBaseDamage()
        {
            return TrapBaseDamage;
        }

        /** 화살비 기본 데미지 반환 */
        public float GetArrowRainBaseDamage()
        {
            return ArrowRainBaseDamage;
        }

        /** 폭탄화살 기본 데미지 반환 */
        public float GetBombArrowBaseDamage()
        {
            return BombArrowBaseDamage;
        }
        #endregion



        /** 크리티컬 관련 반환 적용 함수 */
        #region Critical

        public void SetCriticalPercent(float percent)
        {
            CharacterCriticalPercent = percent;
        }

        public float GetCriticalPercent()
        {
            return CharacterCriticalPercent;
        }

        public void SetCriticaDamage(float damage)
        {
            CharacterCriticalDamage = damage;
        }

        public float GetCriticalDamage()
        {
            return CharacterCriticalDamage;
        }


        #endregion

        /** 스킬 데미지 증가 반환 적용 함수 */
        #region SkillDamage
        public void SetSkillDamageUp(float rate)
        {
            SkillDamageUp = rate;
        }

        public float GetSkillDamageUp()
        {
            return SkillDamageUpSum;
        }

        #endregion

        void InitData()
        {
            /** 데이터 상의 값만큼 증가시켜줌 */
            MaxHealth += MaxHpUpSum;
            PlayerSpeed += SpeedUpSum;
            CharacterCriticalPercent += CriticalUpSum;
            CharacterCriticalDamage += CriticalDamageUpSum;
        }
    }
}

