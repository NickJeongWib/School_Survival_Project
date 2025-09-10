using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using My;

[System.Serializable]
public struct SaveEconomy
{
    public int MagicStone;
    public int Diamond;
    public int SkillTicket;
    public int EnterTicket;
}

public class CoinManager : MonoBehaviour
{
    string path;

    void Start()
    {
        GameManager.GMInstance.CoinManagerRef = this;

        /** 파일 이름과 경로 저장 */
        path = Path.Combine(Application.persistentDataPath, "Economy.json");
        /** 데이터 불러오기 */
        JsonLoad();
    }

    /** 데이터 불러오기 */
    public void JsonLoad()
    {
        SaveEconomy saveEconomy = new SaveEconomy();

        /** path가 존재하지 않는다면 */
        if (!File.Exists(path))
        {
            /** 마정석 초기화 */
            GameManager.GMInstance.MagicStone = 10000;
            /** 다이아몬드 초기화 */
            GameManager.GMInstance.Diamond = 10000;
            /** 스킬 재설정 티켓 초기화 */
            GameManager.GMInstance.SkillTicket = 5;
            /** 입장 티켓 초기화 */
            GameManager.GMInstance.EnterTicket = 20;

            JsonSave();
        }
        /** path가 존재한다면 */
        else if (File.Exists(path))
        {
            /** path의 모든 text를 읽음 */
            string loadJson = File.ReadAllText(path);
            /** saveEconomy는 loadJson에 저장된 json으로부터 SaveEconomy의 데이터를 불러온다 */
            saveEconomy = JsonUtility.FromJson<SaveEconomy>(loadJson);

            /** 저장되어 있는 마정석을 가져온다. */
            GameManager.GMInstance.MagicStone = saveEconomy.MagicStone;
            /** 저장되어 있는 다이아몬드를 불러온다. */
            GameManager.GMInstance.Diamond = saveEconomy.Diamond;
            /** 저장되어 있는 스킬 재설정 티켓을 불러온다. */
            GameManager.GMInstance.SkillTicket = saveEconomy.SkillTicket;
            /** 저장되어 있는 입장 티켓을 불러온다. */
            GameManager.GMInstance.EnterTicket = saveEconomy.EnterTicket;
        }
    }

    public void JsonSave()
    {
        SaveEconomy saveEconomy = new SaveEconomy();

        /** 저장할 마정석은 GameManager에 저장된 마정석으로 초기화한다. */
        saveEconomy.MagicStone = GameManager.GMInstance.MagicStone;
        /** 저장한 다이아는 GameManager에 저장된 다이아몬드 수량으로 초기화한다. */
        saveEconomy.Diamond = GameManager.GMInstance.Diamond;
        /** 저장한 스킬 재설정권은 GameManager에 저장된 티켓 수량으로 초기화한다. */
        saveEconomy.SkillTicket = GameManager.GMInstance.SkillTicket;
        /** 저장한 입장권 GameManager에 저장된 티켓 수량으로 초기화한다. */
        saveEconomy.EnterTicket = GameManager.GMInstance.EnterTicket;

        /** json 문자열로 saveEconomy에 저장된 마정석과 다이아몬드를 저장한다. */
        string json = JsonUtility.ToJson(saveEconomy, true);

        /** path에 저장된 json데이터를 읽는다. */
        File.WriteAllText(path, json);
    }
}
