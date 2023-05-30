using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My;

public class PassiveSkill : MonoBehaviour
{
    public SkillData.SkillType type;
    public float rate;

    public void Init(SkillData data)
    {
        // Basic Set
        name = "Gear " + data.Skill_Id;
        transform.parent = GameManager.GMInstance.Player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        type = data.skillType;
        rate = data.damages[0];
        ApplyPassiveSkill();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyPassiveSkill();
    }

    void ApplyPassiveSkill()
    {
        switch(type)
        {
            case SkillData.SkillType.Skill_SkillSpeedUp:
                RateUp();
                break;
            case SkillData.SkillType.Skill_CharSpeedUp:
                SpeedUp();
                break;
        }
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(Weapon weapon in weapons)
        {
            switch(weapon.id)
            {
                case 0:
                    weapon.speed = 150 + (150 * rate);
                    break;
                default:
                    weapon.speed = 0.5f * (1f - rate);
                    break;
            }
        }
    }
    
    void SpeedUp()
    {
        
        GameManager.GMInstance.PlayerSpeed += GameManager.GMInstance.PlayerSpeed * rate;
    }

}
