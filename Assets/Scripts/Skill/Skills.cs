using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 스킬 관련 컨테이너 클래스
/// </summary>
public class Skills
{
    public Character Caster { get; private set; }  // 시전자
    public Dictionary<int, Skill> SkillDatas { get; private set; }  // 스킬 데이터들
    
    public Skills(Character caster)
    {
        Caster = caster;
        SkillDatas = new Dictionary<int, Skill>();
    }

    public void AddSkill(Skill skill)
    {
        var skillId = skill.SkillData.Id;
        if (SkillDatas.ContainsKey(skillId))
        {
            Debug.LogWarning($"{skillId}의 스킬은 이미 존재합니다.");
            return;
        }
        
        SkillDatas.Add(skillId, skill);
    }

    // 스킬 사용
    public void Use(int skillId)
    {
        if (!SkillDatas.ContainsKey(skillId))
        {
            Debug.LogWarning($"{skillId}는 스킬 컨테이너에 존재하지 않습니다.");
            return;
        }

        SkillDatas[skillId].Use(Caster);
    }
    public void Use<T>() where T : Skill
    {
        var skillData = SkillDatas.Values.Where(x => x is T).FirstOrDefault();
        if (skillData == null)
        {
            Debug.LogWarning($"{skillData}는 스킬 컨테이너에 존재하지 않습니다.");
            return;
        }

        skillData.Use(Caster);
    }
}
