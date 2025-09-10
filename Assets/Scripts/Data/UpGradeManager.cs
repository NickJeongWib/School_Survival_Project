using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using My;

[System.Serializable]
public struct UpGradeData
{
    /** 스킬 데미지 : 증가량, 레벨, 가격 */
    public float SkillDamageUpSum;
    public int SkillDamageUpLevel;
    public int SkillDamageUpPrice;

    /** 최대 체력 : 증가량, 레벨, 가격 */
    public float MaxHpUpSum;
    public int MaxHpUpLevel;
    public int MaxHpUpPrice;

    /** 이동속도  : 증가량, 레벨, 가격 */
    public float SpeedUpSum;
    public int SpeedUpLevel;
    public int SpeedUpPrice;

    /** 크리티컬 확률 : 증가량, 레벨, 가격 */
    public float CriticalUpSum;
    public int CriticalUpLevel;
    public int CriticalUpPrice;

    /** 크리티컬 데미지 : 증가량, 레벨, 가격 */
    public float CriticalDamageUpSum;
    public int CriticalDamageUpLevel;
    public int CriticalDamageUpPrice;
}

public class UpGradeManager : MonoBehaviour
{
    string UpGradepath;

    void Start()
    {
        GameManager.GMInstance.UpGradeManagerRef = this;

        /** 파일 이름과 경로 저장 */
        UpGradepath = Path.Combine(Application.dataPath, "UpGrade.json");
        /** 데이터 불러오기 */
        JsonLoad();
    }

    /** 데이터 불러오기 */
    public void JsonLoad()
    {
        UpGradeData saveUpGrade = new UpGradeData();

        /** path가 존재하지 않는다면 */
        if (!File.Exists(UpGradepath))
        {
            Debug.Log(0);

            /** --- 스킬 데미지 업그레이드 관련 초기화 --- */
            GameManager.GMInstance.SkillDamageUpLevel = 1;
            GameManager.GMInstance.SkillDamageUpSum = 0.0f;
            GameManager.GMInstance.SkillDamageUpPrice = 100;

            /** --- 최대 체력 업그레이드 관련 초기화 --- */
            GameManager.GMInstance.MaxHpUpLevel = 1;
            GameManager.GMInstance.MaxHpUpSum = 0.0f;
            GameManager.GMInstance.MaxHpUpPrice = 100;

            /** --- 이동속도 업그레이드 관련 초기화 --- */
            GameManager.GMInstance.SpeedUpLevel = 1;
            GameManager.GMInstance.SpeedUpSum = 0.0f;
            GameManager.GMInstance.SpeedUpPrice = 100;

            /** --- 크리티컬 확률 업그레이드 관련 초기화 --- */
            GameManager.GMInstance.CriticalUpLevel = 1;
            GameManager.GMInstance.CriticalUpSum = 0.0f;
            GameManager.GMInstance.CriticalUpPrice = 100;

            /** --- 스킬 데미지 업그레이드 관련 초기화 --- */
            GameManager.GMInstance.CriticalDamageUpLevel = 1;
            GameManager.GMInstance.CriticalDamageUpSum = 0.0f;
            GameManager.GMInstance.CriticalDamageUpPrice = 100;

            JsonSave();
        }
        /** path가 존재한다면 */
        else if (File.Exists(UpGradepath))
        {
            /** path의 모든 text를 읽음 */
            string loadJson = File.ReadAllText(UpGradepath);
            /** saveEconomy는 loadJson에 저장된 json으로부터 SaveEconomy의 데이터를 불러온다 */
            saveUpGrade = JsonUtility.FromJson<UpGradeData>(loadJson);

            /** 스킬 데미지 관련 초기화 */
            GameManager.GMInstance.SkillDamageUpSum = saveUpGrade.SkillDamageUpSum;
            GameManager.GMInstance.SkillDamageUpLevel = saveUpGrade.SkillDamageUpLevel;
            GameManager.GMInstance.SkillDamageUpPrice = saveUpGrade.SkillDamageUpPrice;

            /** 최대 체력 증가 관련 초기화 */
            GameManager.GMInstance.MaxHpUpSum = saveUpGrade.MaxHpUpSum;
            GameManager.GMInstance.MaxHpLevel = saveUpGrade.MaxHpUpLevel;
            GameManager.GMInstance.MaxHpUpPrice = saveUpGrade.MaxHpUpPrice;

            /** 이동속도 증가 관련 초기화 */
            GameManager.GMInstance.SpeedUpSum = saveUpGrade.SpeedUpSum;
            GameManager.GMInstance.SpeedUpLevel = saveUpGrade.SpeedUpLevel;
            GameManager.GMInstance.SpeedUpPrice = saveUpGrade.SpeedUpPrice;

            /** 크리티컬 확률 증가 관련 초기화*/
            GameManager.GMInstance.CriticalUpSum = saveUpGrade.CriticalUpSum;
            GameManager.GMInstance.CriticalUpLevel = saveUpGrade.CriticalUpLevel;
            GameManager.GMInstance.CriticalUpPrice = saveUpGrade.CriticalUpPrice;

            /** 크리티컬 데미지 증가 관련 초기화 */
            GameManager.GMInstance.CriticalDamageUpSum = saveUpGrade.CriticalDamageUpSum;
            GameManager.GMInstance.CriticalDamageUpLevel = saveUpGrade.CriticalDamageUpLevel;
            GameManager.GMInstance.CriticalDamageUpPrice = saveUpGrade.CriticalDamageUpPrice;
        }
    }

    public void JsonSave()
    {
        UpGradeData saveUpGrade = new UpGradeData();

        /** 스킬 데미지 관련 초기화 */
        saveUpGrade.SkillDamageUpSum = GameManager.GMInstance.SkillDamageUpSum;
        saveUpGrade.SkillDamageUpLevel = GameManager.GMInstance.SkillDamageUpLevel;
        saveUpGrade.SkillDamageUpPrice = GameManager.GMInstance.SkillDamageUpPrice;

        /** 최대 체력 증가 관련 초기화 */
        saveUpGrade.MaxHpUpSum = GameManager.GMInstance.MaxHpUpSum;
        saveUpGrade.MaxHpUpLevel = GameManager.GMInstance.MaxHpLevel;
        saveUpGrade.MaxHpUpPrice = GameManager.GMInstance.MaxHpUpPrice;

        /** 이동속도 증가 관련 초기화 */
        saveUpGrade.SpeedUpSum = GameManager.GMInstance.SpeedUpSum;
        saveUpGrade.SpeedUpLevel = GameManager.GMInstance.SpeedUpLevel;
        saveUpGrade.SpeedUpPrice = GameManager.GMInstance.SpeedUpPrice;

        /** 크리티컬 확률 증가 관련 초기화*/
        saveUpGrade.CriticalUpSum = GameManager.GMInstance.CriticalUpSum;
        saveUpGrade.CriticalUpLevel = GameManager.GMInstance.CriticalUpLevel;
        saveUpGrade.CriticalUpPrice = GameManager.GMInstance.CriticalUpPrice;

        /** 크리티컬 데미지 증가 관련 초기화 */
        saveUpGrade.CriticalDamageUpSum = GameManager.GMInstance.CriticalDamageUpSum;
        saveUpGrade.CriticalDamageUpLevel = GameManager.GMInstance.CriticalDamageUpLevel;
        saveUpGrade.CriticalDamageUpPrice = GameManager.GMInstance.CriticalDamageUpPrice;

        /** json 문자열로 saveEconomy에 저장된 마정석과 다이아몬드를 저장한다. */
        string json = JsonUtility.ToJson(saveUpGrade, true);

        /** path에 저장된 json데이터를 읽는다. */
        File.WriteAllText(UpGradepath, json);
    }
}
