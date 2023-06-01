using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using My;
using static Define;

public class PlaySceneManager : MonoBehaviour
{
    public float PlayTime;
    public Text SkillSelectPaneltext;
    public Image GameOverImage;
    // float fadeImage = 1;
    bool bIsFirstStart;
    float BasePlayerSpeed;

    public float HpUpTime;
    public float MonsterHpUpRate = 0.1f;
    public bool bIsMonsterHpUp;
    public float NextMonsterHp;
    public float BaseMonsterHp;
    int HpLevel;
    int GainMagicStone;

    // public GameObject GameClearPanel;
    // public GameObject GameOverPanel;
    public GameObject ConfigPanel;
    public LevelUp WizardLevelUp;

    /** 패시브 스킬 증가 비율 저장 */
    [Header("-----PassiveSkillUpRate-----")]
    public float PassiveCriticalUpRate;
    public float PassiveCriticalDamageUpRate;
    public float PassiveSkillDamageUpRate;

    [Header("---GainMagicStoneText---")]
    public Text GameClearMagicStoneTxt;
    public Text GameOverMagicStoneTxt;

    [Header("---Damage---")]
    public Transform Damage_Canvas;
    public GameObject m_DamageRoot;
    GameObject a_DmgClone;
    DamageText DmgTxt;
    Vector3 StartCacPos;

    [Header("---Volum---")]
    public Slider BGMVolum;
    public Slider SFXVolum;

    [Header("---DungeonTile---")]
    public GameObject GrassLandTile;
    public GameObject RockLandTile;
    public GameObject DeathLandTile;

    [Header("---Check---")]
    public Image PlayScene_BGM_Off_Check;
    public Image PlayScene_BGM_On_Check;
    public Image PlayScene_EffectSound_On_Check;
    public Image PlayScene_EffectSound_Off_Check;

    public int GetGameEndMagicStone()
    {
        return GainMagicStone;
    }
    public void SetGameEndMagicStone(int magicStone)
    {
        GainMagicStone = magicStone;
    }

    void Start()
    {
        /** 만약 선택된 모드가 초원지대라면 */
        if (GameManager.GMInstance.SelectDungeonMode == ESelectDungeon.GrassLand)
        {
            /** 초원지대 배경음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlayBGM(SoundManager.BGM.Dungeon_Grassland);

            GrassLandTile.SetActive(true);
            RockLandTile.SetActive(false);
            DeathLandTile.SetActive(false);
        }
        /** 만약 선택된 모드가 암석지대라면 */
        else if (GameManager.GMInstance.SelectDungeonMode == ESelectDungeon.RockLand)
        {
            /** 암석지대 배경음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlayBGM(SoundManager.BGM.Dungeon_Rockland);

            GrassLandTile.SetActive(false);
            RockLandTile.SetActive(true);
            DeathLandTile.SetActive(false);
        }
        /** 만약 선택된 모드가 망자의 숲이라면 */
        else if (GameManager.GMInstance.SelectDungeonMode == ESelectDungeon.DeathLand)
        {
            /** 망자의 숲 배경음 재생 */
            GameManager.GMInstance.SoundManagerRef.PlayBGM(SoundManager.BGM.Dungeon_Deathland);

            GrassLandTile.SetActive(false);
            RockLandTile.SetActive(false);
            DeathLandTile.SetActive(true);
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


        /** 스킬 선택 텍스트UI 함수 호출 */
        TextInit();
        GameManager.GMInstance.CurrentScene = Define.ESceneType.PlayScene;
        GameManager.GMInstance.PlaySceneManagerRef = this;

        /** 게임 플레이 변수 초기화 */
        GameManager.GMInstance.PlaySceneInit(false, 0, GameManager.GMInstance.MaxHealth);

        /** 게임 생명 활성화 */
        GameManager.GMInstance.bIsLive = true;

        GameManager.GMInstance.InfoInit();

        /** 입장 시 플레이어 이동속도 저장 */
        BasePlayerSpeed = GameManager.GMInstance.PlayerSpeed;

        /** 초기화 플레이화면 진입시 BaseHp 최대체력으로 초기화 */
        GameManager.GMInstance.BaseHp = GameManager.GMInstance.MaxHealth;
        /** 초기화 플레이화면 진입시 PassiveSkillDamageTotal 0으로 초기화 */
        PassiveSkillDamageUpRate = 0;

        /** 처음 시작하는지 확인하기 위한 변수 */
        bIsFirstStart = true;

        GameManager.GMInstance.CoinManagerRef.JsonSave();
    }

    void Update()
    {
        if (GameManager.GMInstance.bIsLive == false)
        {
            return;
        }

        PlayTime += Time.deltaTime;
        HpUpTime += Time.deltaTime;
        GameManager.GMInstance.PlayTime = PlayTime;

        MonsterHpUp();

        GameClear();
    }


    void MonsterHpUp()
    {
        if (NextMonsterHp == 0)
        {
            NextMonsterHp = BaseMonsterHp;
        }

        /** 20초마다 재생 */
        if (HpUpTime / 20 >= 1)
        {
            bIsMonsterHpUp = true;

            /** 시간 초기화 */
            HpUpTime = 0.0f;

            HpLevel++;
            MonsterHpUpRate += 0.1f;

            NextMonsterHp = BaseMonsterHp + BaseMonsterHp * MonsterHpUpRate;

            Debug.Log("BaseMonsterHp * MonsterHpUpRate " + BaseMonsterHp * MonsterHpUpRate);
            Debug.Log("NextMonsterHp " + NextMonsterHp);

            /** 몬스터 체력 증가 */
            // GameManager.GMInstance.MoveRef.MaxHp += GameManager.GMInstance.MoveRef.MaxHp * HpUpRate;

            if (HpLevel == 9)
            {
                /** 다음 몬스터를 위해 초기화 */
                MonsterHpUpRate = 0;
                /** 다음 몬스터를 위해 초기화 */
                HpLevel = 0;
            }       
        }
    }

    public void OnClickReSkill()
    {
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);
    }

    public void OnClickTicketSkillRe()
    {
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        /** 만약 현재 마법사 캐릭터라면 */
        if (GameManager.GMInstance.CurrentChar == ECharacterType.WizardChar)
        {
            /** 마법사 캐릭터 스킬 재설정 */
            GameManager.GMInstance.UiLevelUp.Next();
        }
        /** 만약 현재 궁수 캐릭터라면 */
        else if (GameManager.GMInstance.CurrentChar == ECharacterType.AcherChar)
        {
            /** 궁수 스킬 재설정 */
            GameManager.GMInstance.AcherLevelUpRef.Next();
        }
    }

    /** 환경설정 클릭 */
    public void OnClickConfig()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        ConfigPanel.SetActive(true);

        /** 화면이 멈췄기 때문에 다음 입장을 위해 false로 해준다. */
        GameManager.GMInstance.bIsLive = false;

        Time.timeScale = 0.0f;
    }

    /** 환경설정 닫기 버튼 */
    public void OnClickConfig_Resume()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        Time.timeScale = 1.0f;

        /** 화면이 멈췄기 때문에 다음 입장을 위해 false로 해준다. */
        GameManager.GMInstance.bIsLive = true;

        ConfigPanel.SetActive(false);
    }

    /** Lobby Scene으로 이동 */
    public void OnClickConfig_GoLobby()
    {
        /** 효과음 재생 */
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.Select);

        /** 입장해서 증가하는 이동속도 반영 x */
        GameManager.GMInstance.PlayerSpeed = BasePlayerSpeed;

        /** 시간흐름 정상화 */
        Time.timeScale = 1.0f;

        SceneManager.LoadScene("Lobby");
    }

    public void TextInit()
    {
        if (GameManager.GMInstance.level == 1)
        {
            SkillSelectPaneltext.text = "스킬을 선택해 주세요!";
        }
        else if (GameManager.GMInstance.level != 1)
        {
            SkillSelectPaneltext.text = "축하 합니다!\n레벨이 상승 했습니다.";
        }
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        /** 플레이어 생존 함수 off */
        GameManager.GMInstance.bIsLive = false;

        /** GameOverImage 적용 */
        GameOverImage.gameObject.SetActive(true);

        /** 처치한 몬스터의 10% 만큼 마정석 획득 */
        GainMagicStone = Mathf.RoundToInt(GameManager.GMInstance.killcount * 0.1f);

        /** 획득한 마정석을 더해준다. */
        GameManager.GMInstance.MagicStone += GainMagicStone;

        GameOverMagicStoneTxt.text = GainMagicStone + " 마정석";

        /** 1초 후 */
        yield return new WaitForSeconds(1.0f);

        /** GameOverPanel On */
        GameManager.GMInstance.GameFailedAdsPanelRef.EndGamePanelrect.localScale = Vector3.one;

        StartCoroutine(GameOverTimeRoutine());
        
    }

    IEnumerator GameOverTimeRoutine()
    {
        /** 0.5초 후 */
        yield return new WaitForSeconds(1.0f);

        /** PlayStop함수 호출 */
        GameManager.GMInstance.PlayStop();

        /** 재화 저장 */
        GameManager.GMInstance.CoinManagerRef.JsonSave();
    }

    /** 게임 클리어 함수 */
    public void GameClear()
    {
        /** 시간이 다 되었다면 */
        if (GameManager.GMInstance.gameTime == GameManager.GMInstance.maxGameTime || GameManager.GMInstance.SpawnerRef.GetbIsBossClear() == true) 
        {
            GameManager.GMInstance.EndGameAdsPanelRef.EndGamePanelrect.localScale = Vector3.one;

            /** 처치한 몬스터의 10% 만큼 마정석 획득 */
            GainMagicStone = Mathf.RoundToInt(GameManager.GMInstance.killcount * 0.1f);

            /** 획득한 마정석을 더해준다. */
            GameManager.GMInstance.MagicStone += GainMagicStone;

            GameClearMagicStoneTxt.text = GainMagicStone + " 마정석";

            /** 게임 시간 멈춤 */
            Time.timeScale = 0;

            /** 마정석 저장 */
            GameManager.GMInstance.CoinManagerRef.JsonSave();
        }
    }

    /** 로비화면 이동 함수 */
    public void OnClickLobby()
    {
        /** 입장해서 증가하는 이동속도 반영 x */
        GameManager.GMInstance.PlayerSpeed = BasePlayerSpeed;

        /** 게임 시간을 원상복귀 */
        Time.timeScale = 1.0f;
        /** 로비 씬으로 전환되기 때문에 다음 입장을 위해 false로 해준다. */
        GameManager.GMInstance.bIsLive = false;
        /** 로비화면 이동 */
        SceneManager.LoadScene("Lobby");
    }

    /** 로비화면 이동 함수 */
    public void OnClickReStart()
    {
        /** 게임 시간을 원상복귀 */
        Time.timeScale = 1.0f;

        /** 플레이 시간 초기화 */
        GameManager.GMInstance.PlayTime = 0.0f;

        /** 플레이어 스피드는 기존의 베이스 이동속도를 가져온다. */
        GameManager.GMInstance.PlayerSpeed = GameManager.GMInstance.GetPlayerBaseSpeed();

        /** 로비 씬으로 전환되기 때문에 다음 입장을 위해 false로 해준다. */
        GameManager.GMInstance.bIsLive = false;

        /** 입장해서 증가하는 이동속도 반영 x */
        GameManager.GMInstance.PlayerSpeed = BasePlayerSpeed;

        /** 로비화면 이동 */
        SceneManager.LoadScene("PlayScene");
    }

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

        PlayScene_EffectSound_On_Check.gameObject.SetActive(false);
        PlayScene_EffectSound_Off_Check.gameObject.SetActive(true);
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

        PlayScene_EffectSound_On_Check.gameObject.SetActive(true);
        PlayScene_EffectSound_Off_Check.gameObject.SetActive(false);
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

        PlayScene_BGM_On_Check.gameObject.SetActive(true);
        PlayScene_BGM_Off_Check.gameObject.SetActive(false);
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

        PlayScene_BGM_On_Check.gameObject.SetActive(false);
        PlayScene_BGM_Off_Check.gameObject.SetActive(true);
    }

    /** TODO ## PlaySceneManager.cs 배경음 볼륨 조절 */
    public void SetBGMVolum(float volum)
    {
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.BGMPlayers.Length; i++)
        {
            GameManager.GMInstance.SoundManagerRef.BGMPlayers[i].volume = volum;
        }
    }

    /** TODO ## PlaySceneManager.cs 효과음 볼륨 조절 */
    public void SetSFXVolum(float volum)
    {
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.SFXPlayers.Length; i++)
        {
            GameManager.GMInstance.SoundManagerRef.SFXPlayers[i].volume = volum;
        }
    }

    /** TODO ## PlaySceneManager 데미지 Test */
    public void DamageTxt(float a_Value, Vector3 a_Pos, Color a_Color)
    {
        if (m_DamageRoot == null || Damage_Canvas == null)
            return;

        a_DmgClone = (GameObject)Instantiate(m_DamageRoot);
        a_DmgClone.transform.SetParent(Damage_Canvas);
        DmgTxt = a_DmgClone.GetComponent<DamageText>();

        if (DmgTxt != null)
            DmgTxt.initDamge(a_Value, a_Color);
        StartCacPos = new Vector3(a_Pos.x, a_Pos.y, 0.0f);
        a_DmgClone.transform.position = StartCacPos;
    }

    // public float PassiveCriticalUpRate;
    // public float PassiveCriticalDamageUpRate;

    /** 패시브 스킬 반환 */
    #region PassiveSkillSetting
    
    public float GetPassiveCriticalUpRate()
    {
        return PassiveCriticalUpRate;
    }

    public void SetPassiveCriticalUpRate(float value)
    {
        /** value값 만큼 증가 */
        PassiveCriticalUpRate += value;
    }

    public float GetPassiveCriticalDamageUpRate()
    {
        return PassiveCriticalDamageUpRate;
    }

    public void SetPassiveCriticalDamageUpRate(float value)
    {
        /** value값 만큼 증가 */
        PassiveCriticalDamageUpRate += value;
    }

    #endregion
}
