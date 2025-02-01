using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬의 베이스가 되는 클래스
/// </summary>
public abstract class Skill
{
    // 스킬 데이터
    public SkillInfo SkillData { get; protected set; }

    public Skill(int skillId)
    {
        SkillData = GameApplication.Instance.GameModel.PresetData.ReturnData<SkillInfo>(nameof(SkillInfo), skillId);
    }

    // 스킬 사용할 때 써야 하는 메서드
    public abstract void Use(Character caster);
}
