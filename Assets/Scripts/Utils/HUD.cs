using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using My;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health }
    public InfoType type;
    public Text GameOverRemainTime;

    Text myText;
    public Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void Update()
    {
  

        switch (type)
        {
            /** TODO ## HUD.cs 최대 레벨 도달 시 게임 끊김 수정 요함 - 배열 값 추가 */
            case InfoType.Exp:
                float curExp = GameManager.GMInstance.exp;
                /** 10레벨 이후의 경험치는 똑같음 */
                float maxExp = GameManager.GMInstance.nextExp[Mathf.Min(GameManager.GMInstance.level, GameManager.GMInstance.nextExp.Length - 1)];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.GMInstance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.GMInstance.killcount);
                break;
            case InfoType.Time:

                if (!GameManager.GMInstance.bIsLive)
                {
                    return;
                }

                float remainTime = GameManager.GMInstance.maxGameTime - GameManager.GMInstance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                GameOverRemainTime.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:

                if (!GameManager.GMInstance.bIsLive)
                {
                    return;
                }

                float curHealth = GameManager.GMInstance.Health;
                float maxHealth = GameManager.GMInstance.MaxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}
