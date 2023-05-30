using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using My;
using static Define;

public class LobbySceneManager : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject ConfigPanel;
    public GameObject MyCharacter;
    
    public GameObject DungeonSelectPanel;
    public GameObject AbilityCheckPanel;
    public GameObject InGameMoneyPanel;
    public GameObject EquipmentGachaPanel;
    public GameObject TicketPanel;

    public GameObject DayQuestPanel;
    public GameObject WeekQuestPanel;

    public GameObject[] SelectCharacterPrefabs;


    /** TODO ## LobbyManager.cs 캐릭터 능력치 증가 변수 */
    public int Up_Hp;
    public int Up_Damage;
    public int Up_Defense;

    [Header("---EconomyText---")]
    public Text MagicStonText;
    public Text DiamondText;
    public Text EnterTicketText;

    //[Header("---Button---")]
    //public Button SkillDamageBtn;
    //public Button MaxHpUpBtn;
    //public Button SpeedUpBtn;
    //public Button CriticalUpBtn;
    //public Button CriticalDamageUpBtn;

    [Header("---Volum---")]
    public Slider BGMVolum;
    public Slider SFXVolum;

    /** 전사 시작 방지용 버튼 */
    public Button ModeSelectBtn;

    [Header("---QuestTime---")]
    public Text DayRemainTime;
    public Text WeekRemainTime;

    /** 캐릭터 능력치 창 Text변수 */
    [Header("---CharInfoText---")]
    public Text AttackText;
    public Text MaxHPText;
    public Text CharSpeedText;
    public Text CharCriticalPer;
    public Text CharCriticalDamage;

    /** 캐릭터 능력치 레벨 텍스트 */
    [Header("---CharAbilityLevelText---")]
    public Text AttackLevelText;
    public Text MaxHpLevelText;
    public Text CharSpeedLevelText;
    public Text CharCriticalPerLevelText;
    public Text CharCriticalDamageLevelText;

    [Header("---Check---")]
    public Image BGM_Off_Check;
    public Image BGM_On_Check;
    public Image EffectSound_On_Check;
    public Image EffectSound_Off_Check;

    [Header("---PriceText---")]
    public Text DamageUpPriceText;
    public Text MaxHpUpPricelText;
    public Text SpeedUpPriceText;
    public Text CriticalUpPriceText;
    public Text CriticalDamageUpPriceText;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.GMInstance.SoundManagerRef.PlayBGM(SoundManager.BGM.Lobby);

        // Time.timeScale = 1;

        GameManager.GMInstance.LobbySceneManagerRef = this;

        /** 현재 화면 로비씬*/
        GameManager.GMInstance.CurrentScene = Define.ESceneType.LobbyScene;

        /** MyCharacter에 GameManager가 저장하고 있는 캐릭터를 소환 */
        // MyCharacter = Instantiate(SelectCharacterPrefabs[(int)GameManager.GMInstance.CurrentChar]);
        /** MyCharacter크기를 3x3x3으로 바꿔준다. */
        // MyCharacter.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);

        /** 만약 배경음이 꺼져있다면 */
        if (GameManager.GMInstance.SoundManagerRef.bIsBGMOn == false)
        {
            /** 배경음 off 체크박스 활성화 */
            BGM_Off_Check.gameObject.SetActive(true);
            /** 배경음 on 체크박스 비활성화 */
            BGM_On_Check.gameObject.SetActive(false);
        }

        /** 만약 효과음이 꺼져있다면 */
        if (GameManager.GMInstance.SoundManagerRef.bIsSFXOn == false)
        {
            /** 효과음 off 체크박스 활성화 */
            EffectSound_Off_Check.gameObject.SetActive(true);
            /** 효과음 on 체크박스 활성화 */
            EffectSound_On_Check.gameObject.SetActive(false);
        }

        /** 초기 배경음 볼륨 값 초기화 */
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.BGMPlayers.Length; i++)
        {
            BGMVolum.value = GameManager.GMInstance.SoundManagerRef.BGMPlayers[i].volume;
        }
        /** 초기 효과음 볼륨 값 초기화 */
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.SFXPlayers.Length; i++)
        {
            SFXVolum.value = GameManager.GMInstance.SoundManagerRef.SFXPlayers[i].volume;
        }

        /** 시작 시 주간 퀘스트 크기 안보이기 */
        WeekQuestPanel.GetComponent<RectTransform>().localScale = Vector3.zero;

        /** 장비와 티켓 상점 스케일 0 */
        EquipmentGachaPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
        TicketPanel.GetComponent<RectTransform>().localScale = Vector3.zero;

        InitText();
        // CharInfoInit();
    }

    void Update()
    {
        /** TODO ## LobbySceneManager.cs 전사 미구현으로 인한 플레이 차단*/
        /** 현재 캐릭터가 전사라면 */
        if (GameManager.GMInstance.CurrentChar == ECharacterType.WorriorChar)
        {
            /** 시작버튼 비활성화 */
            ModeSelectBtn.interactable = false;
        }
        /** 전사가 아니라면 */
        else
        {
            /** 시작버튼 활성화 */
            ModeSelectBtn.interactable = true;
        }

        /** 일일미션 남은 시간 계산 */
        DayRemainTime.text = (23 - GetCurrentHour()).ToString() + ":" + (60 - GetCurrentMinute()).ToString() + ":" + (60 - GetCurrentSecond()).ToString();

        /** 주간미션 남은 시간 계산 */
        WeekRemainTime.text = (7 - GetCurrentDay()).ToString() + ":" + (23 - GetCurrentHour()).ToString() + ":" + (60 - GetCurrentMinute()).ToString();
    }


    public void OnClickMenuBtn()
    {
        /** MenuPanel 활성화 */
        MenuPanel.SetActive(true);
    }

    public void OnClickMenuCloseBtn()
    {
        /** MenuPanel 비활성화 */
        MenuPanel.SetActive(false);
    }

    public void OnClickGameplayBtn()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        DungeonSelectPanel.SetActive(true);
    }

    /** TODO ## LobbySceneManager.cs 캐릭터 선택 관련 */
    #region CharSelectBtn
    /** 다음 캐릭터 버튼 눌렀을 때 */
    public void OnClickNextCharacter()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);


        /** 현재 캐릭터 타입이 마법사라면? */
        if (GameManager.GMInstance.CurrentChar == ECharacterType.WizardChar)
        {
            /** 다음 캐릭터 타입은 궁수가 된다. */
            GameManager.GMInstance.CurrentChar = ECharacterType.AcherChar;
        }
        /** 현재 캐릭터 타입이 궁수라면? */
        else if (GameManager.GMInstance.CurrentChar == ECharacterType.AcherChar)
        {
            /** 다음 캐릭터 타입은 전사가 된다. */
            GameManager.GMInstance.CurrentChar = ECharacterType.WorriorChar;
        }
        /** 현재 캐릭터 타입이 전사라면? */
        else if (GameManager.GMInstance.CurrentChar == ECharacterType.WorriorChar)
        {
            /** 다음 캐릭터 타입은 마법사가 된다. */
            GameManager.GMInstance.CurrentChar = ECharacterType.WizardChar;
        }
    }

    /** 이전 캐릭터 버튼 눌렀을 때 */
    public void OnClickPreviousCharacter()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);


        /** 현재 캐릭터 타입이 마법사라면? */
        if (GameManager.GMInstance.CurrentChar == ECharacterType.WizardChar)
        {
            /** 다음 캐릭터 타입은 전사가 된다. */
            GameManager.GMInstance.CurrentChar = ECharacterType.WorriorChar;
        }
        /** 현재 캐릭터 타입이 궁수라면? */
        else if (GameManager.GMInstance.CurrentChar == ECharacterType.AcherChar)
        {
            /** 다음 캐릭터 타입은 마법사가 된다. */
            GameManager.GMInstance.CurrentChar = ECharacterType.WizardChar;
        }
        /** 현재 캐릭터 타입이 전사라면? */
        else if (GameManager.GMInstance.CurrentChar == ECharacterType.WorriorChar)
        {
            /** 다음 캐릭터 타입은 궁수가 된다. */
            GameManager.GMInstance.CurrentChar = ECharacterType.AcherChar;
        }
    }
#endregion

    /** TODO ## LobbySceneManager.cs 사운드 관련 */
    #region ConfigBtn
    public void OnClickConfigBtn()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);
        /** 환경설정 창 off */
        ConfigPanel.SetActive(true);
    }

    public void OnClickCloseBtn()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        /** 환경설정 창 off */
        ConfigPanel.SetActive(false);
    }
    #endregion

    /** TODO ## LobbySceneManager.cs 사운드 관련 */
    #region Sound
    /** 효과음 off 함수 */
    public void OnClickSoundOff()
    {
        GameManager.GMInstance.SoundManagerRef.bIsSFXOn = false;

        /** 저장된 효과음 개수만큼 반복 */
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.SFXPlayers.Length; i++)
        {
            /** 효과음 소거 */
            GameManager.GMInstance.SoundManagerRef.SFXPlayers[i].mute = true;
        }

        EffectSound_On_Check.gameObject.SetActive(false);
        EffectSound_Off_Check.gameObject.SetActive(true);
    }

    /** 효과음 on 함수 */
    public void OnClickSoundOn()
    {
        GameManager.GMInstance.SoundManagerRef.bIsSFXOn = true;

        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        /** 저장된 효과음 개수만큼 반복 */
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.SFXPlayers.Length; i++)
        {
            /** 효과음 소거 */
            GameManager.GMInstance.SoundManagerRef.SFXPlayers[i].mute = false;
        }

        EffectSound_On_Check.gameObject.SetActive(true);
        EffectSound_Off_Check.gameObject.SetActive(false);
    }

    /** 배경음 on 함수 */
    public void OnClickBGMOn()
    {
        GameManager.GMInstance.SoundManagerRef.bIsBGMOn = true;

        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.BGMPlayers.Length; i++)
        {
            /** 배경음 On */
            GameManager.GMInstance.SoundManagerRef.BGMPlayers[i].mute = false;
        }

        BGM_On_Check.gameObject.SetActive(true);
        BGM_Off_Check.gameObject.SetActive(false);
    }

    /** 배경음 off 함수 */
    public void OnClickBGMOff()
    {
        GameManager.GMInstance.SoundManagerRef.bIsBGMOn = false;

        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.BGMPlayers.Length; i++)
        {
            /** 배경음 off */
            GameManager.GMInstance.SoundManagerRef.BGMPlayers[i].mute = true;
        }

        BGM_On_Check.gameObject.SetActive(false);
        BGM_Off_Check.gameObject.SetActive(true);
    }

    /** TODO ## LobbySceneManager.cs 배경음 볼륨 조절 */
    public void SetBGMVolum(float volum)
    {
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.BGMPlayers.Length; i++)
        {
            GameManager.GMInstance.SoundManagerRef.BGMPlayers[i].volume = volum;
        }
    }

    /** TODO ## LobbySceneManager.cs 효과음 볼륨 조절 */
    public void SetSFXVolum(float volum)
    {
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.SFXPlayers.Length; i++)
        {
            GameManager.GMInstance.SoundManagerRef.SFXPlayers[i].volume = volum;
        }
    }

    #endregion

    /** TODO ## LobbySceneManager.cs 던전 선택 관련 */
    #region SelectDungeon
    public void OnClickCloseSelectDungeonPanel()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        /** 던전 판넬 끄기 */
        DungeonSelectPanel.gameObject.SetActive(false);
    }

    public void OnClickEnterGrassLand()
    {
        /** 티켓이 0이면 return */
        if (GameManager.GMInstance.EnterTicket <= 0)
        {
            return;
        }

        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        /** 게임매니저 모드를 초원지대로 바꾸고 */
        GameManager.GMInstance.SelectDungeonMode = ESelectDungeon.GrassLand;

        /** 티켓 1감소 */
        GameManager.GMInstance.EnterTicket--;

        // GameManager.GMInstance.CoinManagerRef.JsonSave();

        SceneManager.LoadScene("PlayScene");
    }

    public void OnClickEnterRockLand()
    {
        /** 티켓이 0이면 return */
        if (GameManager.GMInstance.EnterTicket <= 0)
        {
            return;
        }

        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        /** 게임매니저 모드를 초원지대로 바꾸고 */
        GameManager.GMInstance.SelectDungeonMode = ESelectDungeon.RockLand;

        /** 티켓 1감소 */
        GameManager.GMInstance.EnterTicket--;

        // GameManager.GMInstance.CoinManagerRef.JsonSave();

        SceneManager.LoadScene("PlayScene");
    }

    public void OnClickEnterDeathLand()
    {
        /** 티켓이 0이면 return */
        if (GameManager.GMInstance.EnterTicket <= 0)
        {
            return;
        }

        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        /** 게임매니저 모드를 초원지대로 바꾸고 */
        GameManager.GMInstance.SelectDungeonMode = ESelectDungeon.DeathLand;

        /** 티켓 1감소 */
        GameManager.GMInstance.EnterTicket--;

        // GameManager.GMInstance.CoinManagerRef.JsonSave();

        SceneManager.LoadScene("PlayScene");
    }
#endregion

    /** TODO ## LobbySceneManager.cs 능력치 업그레이드 버튼 */
    #region AbilityUpGrade
    /** 크리티컬확률 증가 */
    public void OnClickCrticalUp()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        /** 마정석 보유량보다 업그레이드 가격이 높다면? */
        if (GameManager.GMInstance.MagicStone - GameManager.GMInstance.CriticalUpPrice < 0)
        {
            return;
        }

        // -------------------새로 추가-----------------------
        GameManager.GMInstance.MagicStone -= GameManager.GMInstance.CriticalUpPrice;
        /** 스킬데미지 증가의 가격을 UpPriceRate만큼 높임 */
        GameManager.GMInstance.CriticalUpPrice += GameManager.GMInstance.UpPriceRate;
        /** 마정석 텍스트 초기화 */
        MagicStonText.text = GameManager.GMInstance.MagicStone.ToString();
        // -------------------새로 추가-----------------------

        /** 크리티컬 확률 계산 */
        // GameManager.GMInstance.SetCriticalPercent(GameManager.GMInstance.GetCriticalPercent() + GameManager.GMInstance.CharCriticalPerUpRate);
        GameManager.GMInstance.CriticalUpSum += GameManager.GMInstance.CharCriticalPerUpRate;
        GameManager.GMInstance.CharacterCriticalPercent += GameManager.GMInstance.CharCriticalPerUpRate;

        /** 크리티컬 증가확률 레벨 1+ */
        // GameManager.GMInstance.CharCriticalPerLevel++;
        GameManager.GMInstance.CriticalUpLevel++;

        /** 크리티컬 레벨 증가 텍스트 초기화 */
        // CharCriticalPerLevelText.text = "Lv" + GameManager.GMInstance.CharCriticalPerLevel + " 크리티컬 확률 증가";
        CharCriticalPerLevelText.text = "Lv" + GameManager.GMInstance.CriticalUpLevel + " 크리티컬 확률 증가";
        /** 능력치 창 크리티컬 확률 초기화 */
        CharCriticalPer.text = (100 * GameManager.GMInstance.GetCriticalPercent()).ToString("F2") + "%";
        /** 마정석 가격 텍스트 초기화 */
        CriticalUpPriceText.text = GameManager.GMInstance.CriticalUpPrice + " 마정석";

        GameManager.GMInstance.CoinManagerRef.JsonSave();
        GameManager.GMInstance.UpGradeManagerRef.JsonSave();
    }

    /** 크리티컬 데미지 증가 */
    public void OnClickCrticalDamageUp()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        /** 마정석 보유량보다 업그레이드 가격이 높다면? */
        if (GameManager.GMInstance.MagicStone - GameManager.GMInstance.CriticalDamageUpPrice < 0)
        {
            return;
        }

        // -------------------새로 추가-----------------------
        GameManager.GMInstance.MagicStone -= GameManager.GMInstance.CriticalDamageUpPrice;
        /** 스킬데미지 증가의 가격을 UpPriceRate만큼 높임 */
        GameManager.GMInstance.CriticalDamageUpPrice += GameManager.GMInstance.UpPriceRate;
        /** 마정석 텍스트 초기화 */
        MagicStonText.text = GameManager.GMInstance.MagicStone.ToString();
        // -------------------새로 추가-----------------------

        /** 크리티컬 데미지 계산 */
        // GameManager.GMInstance.SetCriticaDamage(GameManager.GMInstance.GetCriticalDamage() + GameManager.GMInstance.CharCriticalDamageUpRate);
        GameManager.GMInstance.CriticalDamageUpSum += GameManager.GMInstance.CharCriticalDamageUpRate;
        GameManager.GMInstance.CharacterCriticalDamage += GameManager.GMInstance.CharCriticalDamageUpRate;

        /** 크리티컬 데미지 증가 레벨 1+ */
        // GameManager.GMInstance.CharCriticalDamageLevel++;
        GameManager.GMInstance.CriticalDamageUpLevel++;
        /** 크리티컬 데미지 레벨 증가 텍스트 초기화 */
        // CharCriticalDamageLevelText.text = "Lv" + GameManager.GMInstance.CharCriticalDamageLevel + " 크리티컬 데미지 증가";
        CharCriticalDamageLevelText.text = "Lv" + GameManager.GMInstance.CriticalDamageUpLevel + " 크리티컬 데미지 증가";
        /** 능력치 창 크리티컬 데미지 수치 초기화 */
        CharCriticalDamage.text = (100 * GameManager.GMInstance.GetCriticalDamage()).ToString("F1") + "%";
        /** 마정석 가격 텍스트 초기화 */
        CriticalDamageUpPriceText.text = GameManager.GMInstance.CriticalDamageUpPrice + " 마정석";

        GameManager.GMInstance.CoinManagerRef.JsonSave();
        GameManager.GMInstance.UpGradeManagerRef.JsonSave();
    }

    /** 체력 증가 */
    public void OnClickMaxHpUp()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        /** 마정석 보유량보다 업그레이드 가격이 높다면? */
        if (GameManager.GMInstance.MagicStone - GameManager.GMInstance.MaxHpUpPrice < 0)
        {
            return;
        }

        // -------------------새로 추가-----------------------
        GameManager.GMInstance.MagicStone -= GameManager.GMInstance.MaxHpUpPrice;
        /** 스킬데미지 증가의 가격을 UpPriceRate만큼 높임 */
        GameManager.GMInstance.MaxHpUpPrice += GameManager.GMInstance.UpPriceRate;
        /** 마정석 텍스트 초기화 */
        MagicStonText.text = GameManager.GMInstance.MagicStone.ToString();
        // -------------------새로 추가-----------------------


        /** 현재 최대체력의 10% 증가 계산 */
        //GameManager.GMInstance.MaxHealth += GameManager.GMInstance.MaxHealth * 0.1f;

        /** 최대체력의 2씩 증가 계산 */
        // GameManager.GMInstance.MaxHealth += GameManager.GMInstance.MaxHpLevelUpRate;
        GameManager.GMInstance.MaxHpUpSum += GameManager.GMInstance.MaxHpLevelUpRate;
        GameManager.GMInstance.MaxHealth += GameManager.GMInstance.MaxHpLevelUpRate;

        /** 캐릭터 최대체력 증가레벨 1+ */
        GameManager.GMInstance.MaxHpLevel++;

        /** 최대체력증가 레벨 텍스트 초기화 */
        MaxHpLevelText.text = "Lv" + GameManager.GMInstance.MaxHpLevel + " 최대 체력 증가";

        /** 능력치 창 이동속도 표시 초기화 */
        MaxHPText.text = GameManager.GMInstance.MaxHealth.ToString("F0") + " HP";
        /** 마정석 가격 텍스트 초기화 */
        MaxHpUpPricelText.text = GameManager.GMInstance.MaxHpUpPrice + " 마정석";

        GameManager.GMInstance.CoinManagerRef.JsonSave();
        GameManager.GMInstance.UpGradeManagerRef.JsonSave();
    }


    /** 이동속도 증가 */
    public void OnClickSpeedUp()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        /** 마정석 보유량보다 업그레이드 가격이 높다면? */
        if (GameManager.GMInstance.MagicStone - GameManager.GMInstance.SpeedUpPrice < 0)
        {
            return;
        }

        // -------------------새로 추가-----------------------
        GameManager.GMInstance.MagicStone -= GameManager.GMInstance.SpeedUpPrice;
        /** 스킬데미지 증가의 가격을 UpPriceRate만큼 높임 */
        GameManager.GMInstance.SpeedUpPrice += GameManager.GMInstance.UpPriceRate;
        /** 마정석 텍스트 초기화 */
        MagicStonText.text = GameManager.GMInstance.MagicStone.ToString();
        // -------------------새로 추가-----------------------

        /** 이동속도 증가 계산 */
        // GameManager.GMInstance.PlayerSpeed += GameManager.GMInstance.CharSpeedLevelUpRate;
        GameManager.GMInstance.SpeedUpSum += GameManager.GMInstance.CharSpeedLevelUpRate;
        GameManager.GMInstance.PlayerSpeed += GameManager.GMInstance.CharSpeedLevelUpRate;

        /** 이동속도 증가 레벨 1+ */
        // GameManager.GMInstance.CharSpeedLevel++;
        GameManager.GMInstance.SpeedUpLevel++;

        /** 이동속도 레벨 텍스트 초기화 */
        // CharSpeedLevelText.text = "Lv" + GameManager.GMInstance.CharSpeedLevel + " 이동 속도 증가";
        CharSpeedLevelText.text = "Lv" + GameManager.GMInstance.SpeedUpLevel + " 이동 속도 증가";

        /** 능력치 창 이동속도 표시 초기화 */
        CharSpeedText.text = (100 * GameManager.GMInstance.PlayerSpeed).ToString("F1") + "%";
        /** 마정석 가격 텍스트 초기화 */
        SpeedUpPriceText.text = GameManager.GMInstance.SpeedUpPrice + " 마정석";

        GameManager.GMInstance.CoinManagerRef.JsonSave();
        GameManager.GMInstance.UpGradeManagerRef.JsonSave();
    }

    /** 데미지 증가 */
    public void OnClickDamageUp()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        /** 마정석 보유량보다 업그레이드 가격이 높다면? */
        if (GameManager.GMInstance.MagicStone - GameManager.GMInstance.SkillDamageUpPrice < 0)
        {
            return;
        }

        // -------------------새로 추가-----------------------
        GameManager.GMInstance.MagicStone -= GameManager.GMInstance.SkillDamageUpPrice;
        /** 스킬데미지 증가의 가격을 UpPriceRate만큼 높임 */
        GameManager.GMInstance.SkillDamageUpPrice += GameManager.GMInstance.UpPriceRate;
        /** 마정석 텍스트 초기화 */
        MagicStonText.text = GameManager.GMInstance.MagicStone.ToString();
        // -------------------새로 추가-----------------------

        /** 스킬 데미지 증가 레벨 1+ */
        //GameManager.GMInstance.SkillDamageLevel++;
        GameManager.GMInstance.SkillDamageUpLevel++;

        /** 스킬 데미지 증가 계산식 */
        // GameManager.GMInstance.SetSkillDamageUp(GameManager.GMInstance.GetSkillDamageUp() + GameManager.GMInstance.SkillDamageUpRate);
        GameManager.GMInstance.SkillDamageUpSum += GameManager.GMInstance.SkillDamageUpRate;

        /** 능력치 창 스킬 데미지 표시 초기화 */
        // AttackLevelText.text = "Lv" + GameManager.GMInstance.SkillDamageLevel + " 스킬 데미지 증가";
        AttackLevelText.text = "Lv" + GameManager.GMInstance.SkillDamageUpLevel + " 스킬 데미지 증가";

        /** 능력치 창 스킬 데미지 표시 초기화 */
        // AttackText.text = (100 * GameManager.GMInstance.GetSkillDamageUp()).ToString("F0") + "%";
        AttackText.text = (100 * GameManager.GMInstance.SkillDamageUpSum).ToString("F0") + "%";
        /** 마정석 가격 텍스트 초기화 */
        DamageUpPriceText.text = GameManager.GMInstance.SkillDamageUpPrice + " 마정석";

        GameManager.GMInstance.CoinManagerRef.JsonSave();
        GameManager.GMInstance.UpGradeManagerRef.JsonSave();
    }

    #endregion

    /** TODO ## LobbySceneManager.cs 오브젝트 활성화 관련 */
    #region
    public void OnClickAbilityCheckOpen()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        AbilityCheckPanel.gameObject.SetActive(true);
    }

    public void OnClickAbilityCheckClose()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);
        AbilityCheckPanel.gameObject.SetActive(false);
    }

    public void OnClickOpenInGameStore()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);
       
        EquipmentGachaPanel.GetComponent<RectTransform>().localScale = Vector2.zero;
        TicketPanel.GetComponent<RectTransform>().localScale = Vector2.zero;

        InGameMoneyPanel.GetComponent<RectTransform>().localScale = Vector2.one;
    }

    public void OnClickOpenEquipmentGacha()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        InGameMoneyPanel.GetComponent<RectTransform>().localScale = Vector2.zero;
        TicketPanel.GetComponent<RectTransform>().localScale = Vector2.zero;

        EquipmentGachaPanel.GetComponent<RectTransform>().localScale = Vector2.one;


    }
    public void OnClickOpenTicket()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        InGameMoneyPanel.GetComponent<RectTransform>().localScale = Vector2.zero;
        EquipmentGachaPanel.GetComponent<RectTransform>().localScale = Vector2.zero;

        TicketPanel.GetComponent<RectTransform>().localScale = Vector2.one;
    }
    #endregion

    /** TODO ## LobbySceneManager.cs 텍스트 초기화 */
    #region TextInit
    void InitText()
    {
        /** 마정석 텍스트 수량 초기화 */
        MagicStonText.text = GameManager.GMInstance.MagicStone.ToString();
        /** 다이아 텍스트 수량 초기화 */
        DiamondText.text = GameManager.GMInstance.Diamond.ToString();
        /** 입장권 텍스트 수량 초기화 */
        EnterTicketText.text = "x " + GameManager.GMInstance.EnterTicket.ToString();

        /** ---- 스킬 데미지 관련 Text ---- */
        AttackLevelText.text = "Lv" + GameManager.GMInstance.SkillDamageUpLevel + " 스킬 데미지 증가";
        AttackText.text = (100 * GameManager.GMInstance.SkillDamageUpSum).ToString("F0") + "%";
        DamageUpPriceText.text = GameManager.GMInstance.SkillDamageUpPrice + " 마정석";

        /** ---- 최대 체력 관련 Text ---- */
        MaxHpLevelText.text = "Lv" + GameManager.GMInstance.MaxHpLevel + " 최대 체력 증가";
        MaxHPText.text = (GameManager.GMInstance.MaxHealth + GameManager.GMInstance.MaxHpUpSum).ToString("F0") + " HP";
        MaxHpUpPricelText.text = GameManager.GMInstance.MaxHpUpPrice + " 마정석";

        /** ---- 이동 속도 관련 Text ---- */
        CharSpeedLevelText.text = "Lv" + GameManager.GMInstance.SpeedUpLevel + " 이동 속도 증가";
        CharSpeedText.text = (100 * GameManager.GMInstance.PlayerSpeed).ToString("F1") + "%";
        SpeedUpPriceText.text = GameManager.GMInstance.SpeedUpPrice + " 마정석";

        /** ---- 크리티컬 확률 관련 Text ---- */
        CharCriticalPerLevelText.text = "Lv" + GameManager.GMInstance.CriticalUpLevel + " 크리티컬 확률 증가";
        CharCriticalPer.text = (100 * GameManager.GMInstance.GetCriticalPercent()).ToString("F2") + "%";
        CriticalUpPriceText.text = GameManager.GMInstance.CriticalUpPrice + " 마정석";

        /** ---- 크리티컬 데미지 관련 Text ---- */
        CharCriticalDamageLevelText.text = "Lv" + GameManager.GMInstance.CriticalDamageUpLevel + " 크리티컬 데미지 증가";
        CharCriticalDamage.text = (100 * GameManager.GMInstance.GetCriticalDamage()).ToString("F1") + "%";
        CriticalDamageUpPriceText.text = GameManager.GMInstance.CriticalDamageUpPrice + " 마정석";
    }
    public void CharInfoInit()
    {
        // AttackText.text

        MaxHPText.text = GameManager.GMInstance.MaxHealth.ToString("F0") + " HP";
        CharSpeedText.text = (100 * GameManager.GMInstance.PlayerSpeed).ToString("F1") + "%";
        CharCriticalPer.text = (100 * GameManager.GMInstance.GetCriticalPercent()).ToString("F2") + "%";
        CharCriticalDamage.text = (100 * GameManager.GMInstance.GetCriticalDamage()).ToString("F1") + "%";
        AttackText.text = (100 * GameManager.GMInstance.SkillDamageUpRate).ToString("F0") + "%";

        CharCriticalPerLevelText.text = "Lv" + GameManager.GMInstance.CharCriticalPerLevel + " 크리티컬 확률 증가";
        CharCriticalDamageLevelText.text = "Lv" + GameManager.GMInstance.CharCriticalDamageLevel + " 크리티컬 데미지 증가";
        MaxHpLevelText.text = "Lv" + GameManager.GMInstance.MaxHpLevel + " 최대 체력 증가";
        CharSpeedLevelText.text = "Lv" + GameManager.GMInstance.CharSpeedLevel + " 이동 속도 증가";
        AttackLevelText.text = "Lv" + GameManager.GMInstance.SkillDamageLevel + " 스킬 데미지 증가";
    }
    #endregion

    /** TODO ## LobbySceneManager.cs 퀘스트 관련 */
    #region QuestPanel
    public void OnClickDayQuestPanel()
    {
        DayQuestPanel.GetComponent<RectTransform>().localScale = Vector3.one;
        WeekQuestPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void OnClickWeekQuestPanel()
    {
        DayQuestPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
        WeekQuestPanel.GetComponent<RectTransform>().localScale = Vector3.one;
    }
    int GetCurrentDay()
    {
        return (int)(DateTime.Now).DayOfWeek;
    }

    int GetCurrentSecond()
    {
        return (DateTime.Now).Second;
    }

    int GetCurrentMinute()
    {
        return (DateTime.Now).Minute;
    }

    int GetCurrentHour()
    {
        return (DateTime.Now).Hour;
    }

    #endregion

    /** TODO ## LobbySceneManager.cs 상점 구매 관련 */
    #region BuyProduct

    public void OnClick_Buy_3000_Cash_Dia()
    {
        /** 효과음 추가 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

    }

    public void OnClick_Buy_5000_Cash_Dia()
    {
        /** 효과음 추가 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);
    }

    public void OnClick_Buy_10000_Cash_Dia()
    {
        /** 효과음 추가 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);
    }

    public void OnClick_Buy_100_Dia_MagicStone()
    {
        /** 다이아가 100보다 작으면 실행 x */
        if (GameManager.GMInstance.Diamond < 100)
        {
            return;
        }

        /** 효과음 추가 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);
        /** 다이아 100 감소 */
        GameManager.GMInstance.Diamond -= 100;
        /** 마정석 500 추가 */
        GameManager.GMInstance.MagicStone += 500;

        /** 마정석 텍스트 수량 초기화 */
        MagicStonText.text = GameManager.GMInstance.MagicStone.ToString();
        /** 다이아 텍스트 수량 초기화 */
        DiamondText.text = GameManager.GMInstance.Diamond.ToString();

        GameManager.GMInstance.CoinManagerRef.JsonSave();
    }

    public void OnClick_Buy_500_Dia_MagicStone()
    {
        /** 다이아가 100보다 작으면 실행 x */
        if (GameManager.GMInstance.Diamond < 500)
        {
            return;
        }

        /** 효과음 추가 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);
        /** 다이아 100 감소 */
        GameManager.GMInstance.Diamond -= 500;
        /** 마정석 500 추가 */
        GameManager.GMInstance.MagicStone += 3000;

        /** 마정석 텍스트 수량 초기화 */
        MagicStonText.text = GameManager.GMInstance.MagicStone.ToString();
        /** 다이아 텍스트 수량 초기화 */
        DiamondText.text = GameManager.GMInstance.Diamond.ToString();

        GameManager.GMInstance.CoinManagerRef.JsonSave();
    }

    public void OnClick_Buy_1000_Dia_MagicStone()
    {
        /** 다이아가 100보다 작으면 실행 x */
        if (GameManager.GMInstance.Diamond < 1000)
        {
            return;
        }

        /** 효과음 추가 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);
        /** 다이아 100 감소 */
        GameManager.GMInstance.Diamond -= 1000;
        /** 마정석 500 추가 */
        GameManager.GMInstance.MagicStone += 8000;

        /** 마정석 텍스트 수량 초기화 */
        MagicStonText.text = GameManager.GMInstance.MagicStone.ToString();
        /** 다이아 텍스트 수량 초기화 */
        DiamondText.text = GameManager.GMInstance.Diamond.ToString();

        GameManager.GMInstance.CoinManagerRef.JsonSave();
    }

    #endregion
}
