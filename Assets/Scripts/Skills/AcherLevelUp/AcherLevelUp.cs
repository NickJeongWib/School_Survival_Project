using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My;
using static Define;

public class AcherLevelUp : MonoBehaviour
{
    RectTransform rect;
    public AcherSkill[] skills;
    List<int> skillIndex;

    void Start()
    {
        /** 만약 현재 캐릭터가 마법사가 아닐 시 */
        if (GameManager.GMInstance.CurrentChar != ECharacterType.AcherChar)
        {
            gameObject.SetActive(false);
            return;
        }

        GameManager.GMInstance.AcherLevelUpRef = this;
        rect = GetComponent<RectTransform>();
        skills = GetComponentsInChildren<AcherSkill>(true);

        /** 화살 발사 선택 함수 호출 */
        BaseAttack(0);

        /** TODO ## LevelUp.cs 기본공격 */
        rect.localScale = Vector3.zero;

        /** TODO ## LevelUp.cs 인게임 시작할 시 스킬 선택 X */
            /** 0.2초있다가 next함수 실행 */
            // StartCoroutine(ShowBtn());


        /** 오브젝트 이름이 WizardLevelUp이고 현재 캐릭터 타입이 마법사가 아닐 때*/
        //else if (gameObject.name == "WizardLevelUp" && GameManager.GMInstance.CurrentChar != ECharacterType.AcherChar)
        //{
        //    /** 현재 캐릭터가 마법사가 아닐 시 OFF */
        //    gameObject.SetActive(false);
        //}
    }

    public void BaseAttack(int index)
    {
        /** 인덱스로 입력받은 값의 스킬을 실행시킨다. */
        skills[index].OnClick();
        // Debug.Log("0");
    }

    IEnumerator ShowBtn()
    {
        yield return 0.2f;

        Next();
    }


    public void Show()
    {
        /** Next함수 호출 */
        Next();
        rect.localScale = Vector3.one;
        /** 화면에 보일 때 마다 텍스트 초기화 */
        GameManager.GMInstance.PlaySceneManagerRef.TextInit();
        /** 플레이 화면 멈춤 */
        GameManager.GMInstance.PlayStop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        /** 플레이 화면 다시하기 함수 호출 */
        GameManager.GMInstance.PlayResume();
    }

    /** TODO ## 스킬 선택시 랜덤으로 생성 함수 핵심!!!!!!!!! */
    public void Next()
    {
        // 1. 모든 아이템 비활성화
        foreach (AcherSkill skill in skills)
        {
            skill.gameObject.SetActive(false);
        }

        // 2. 그 중에서 랜덤 3개 아이템 활성화
        int[] random = new int[3];

        /** 반목문 */
        while (true)
        {
            /** 랜덤 배열에 인덱스로 적용시킬 정수를 집어넣는다. */
            random[0] = Random.Range(0, skills.Length);
            random[1] = Random.Range(0, skills.Length);
            random[2] = Random.Range(0, skills.Length);

            /** 스킬선택 버튼이 중복이 되지 않았다면 반복문을 빠져나감 */
            if (random[0] != random[1] && random[1] != random[2] && random[0] != random[2])
            {
                break;
            }
        }

        /** 랜덤스킬의 길이만큼 반복 */
        for (int index = 0; index < random.Length; index++)
        {
            /** ranSkill은 랜덤으로 뽑힌 스킬버튼 */
            AcherSkill ranSkill = skills[random[index]];

            /** 만약 랜덤으로 뽑은 스킬의 레벨이 최대레벨이라면? */
            if (ranSkill.level == ranSkill.data.damages.Length)
            {
                /** 만렙인 스킬 비활성화 */
                ranSkill.gameObject.SetActive(false);

                /** 무한 반복 */
                while (true)
                {
                    /** 랜덤 값 적용 */
                    int randomIndex = Random.Range(0, skills.Length);
                    /** 만렙인 스킬이 랜덤으로 뽑은 스킬과 같지않고 활성화가 되있지 않다면 */
                    if (ranSkill != skills[randomIndex] && skills[randomIndex].gameObject.activeSelf == false)
                    {
                        /** 랜덤으로 뽑은 버튼 활성화 */
                        skills[randomIndex].gameObject.SetActive(true);
                        /** 반복문 빠져나감 */
                        break;
                    }
                }
            }
            else
            {
                /** 스킬선택 버튼 오브젝트 활성환 */
                ranSkill.gameObject.SetActive(true);
            }

            ///** 스킬 배열에 저장된 값을 검색 */
            //for (int i = 0; i < skills.Length; i++)
            //{
            //    /** 만약 스킬이 비활성화 되있다면 */
            //    if (skills[i].gameObject.activeSelf == false)
            //    {
            //        skillIndex.Add(i);
            //        Debug.Log(skillIndex[0]);
            //    }

            //    skills[skillIndex[Random.Range(0, skillIndex.Count)]].gameObject.SetActive(true);
            //}


            // TODO ## LevelUp.cs 최대레벨 업 시 대체
            ///** 만약 랜덤으로 뽑힌 스킬레벨이 최대레벨이라면 */
            //if (ranSkill.level == ranSkill.data.damages.Length)
            //{
            //    /** 스킬 랜덤 선택 */
            //    skills[(Random.Range(3, 4))].gameObject.SetActive(true);
            //}
            //else
            //{
            //    /** 스킬선택 버튼 오브젝트 활성환 */
            //    ranSkill.gameObject.SetActive(true);
            //}
        }
    }
}
