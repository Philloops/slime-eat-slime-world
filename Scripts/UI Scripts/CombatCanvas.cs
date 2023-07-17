using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatCanvas : MonoBehaviour
{
    [Header("Health/Energy")]
    public Image healthBarMeter;
    public Image energyBarMeter;

    [Header("StatusEffect Data")]
    public StatusEffectUI speedStatusUI;
    public StatusEffectUI stunStatusUI;
    public StatusEffectUI rootStatusUI;
    public StatusEffectUI healthStatusUI;
    public StatusEffectUI poisonStatusUI;
    public StatusEffectUI burnStatusUI;
    public StatusEffectUI powerStatusUI;
    public StatusEffectUI protectionStatusUI;


    [Header("Ability Icons")]
    public List<Image> abilityIcons;
    public List<Image> abilityMeters;
    public List<GameObject> abilityMasks;
    public List<GameObject> silenceMasks;
    public Sprite abilityUIMask;

    public float AbilityTimer01 { get; set; }
    public float AbilityTimer02 { get; set; }
    public float AbilityTimer03 { get; set; }

    public List<Image> portraits;
    public List<Image> portraitMasks;


    public virtual void SetSlime(Slime _slime)
    {
        if(portraitMasks[0].gameObject.activeSelf)
        {
            portraits[0].sprite = _slime.MyPortrait;
            portraits[0].gameObject.SetActive(true);
            portraitMasks[0].gameObject.SetActive(false);
        }
        else
        {
            portraits[0].sprite = _slime.MyPortrait;
        }
    }
    public virtual void RemoveSlimeUI(Slime _slime)
    {
        portraits[0].gameObject.SetActive(false);
        portraitMasks[0].gameObject.SetActive(true);
        ResetStatusUIObjs();
    }
    public void ResetStatusUIObjs()
    {
        speedStatusUI.Reset();
        stunStatusUI.Reset();
        rootStatusUI.Reset();
        healthStatusUI.Reset();
        poisonStatusUI.Reset();
        burnStatusUI.Reset();
        powerStatusUI.Reset();
        protectionStatusUI.Reset();
    }
    public void SetAbilityFillMeter(float _value, float _max, int _index)
    {
        abilityMeters[_index].fillAmount = (_value / _max);
    }
    public void SetAbilityMask(int _index, bool _state)
    {
        abilityMasks[_index].SetActive(_state);
    }
    public void SetAbilityIcon(int _index, Sprite _sprite)
    {
        abilityIcons[_index].sprite = _sprite;
    }
    public void SilenceAbilitySlot(int _index, bool _state)
    {
        silenceMasks[_index].SetActive(_state);
    }
    public void SilenceAll(bool _state)
    {
        for (int i = 0; i < silenceMasks.Count; i++)
            silenceMasks[i].SetActive(_state);
    }
    public void ClearDefeatedSlimeData()
    {
        healthBarMeter.fillAmount = (1 / 1);
        energyBarMeter.fillAmount = (1 / 1);
        SilenceAll(false);
        for (int i = 0; i < abilityIcons.Count; i++)
            abilityIcons[i].sprite = abilityUIMask;
        for (int i = 0; i < abilityMasks.Count; i++)
            abilityMasks[i].SetActive(false);
    }
    //public virtual void SetHealthFillMeter(float _value, float _max, Slime _slime) {}
    //public virtual void SetHealthFillMeter(float _value, float _max, Slime _slime, int _index) { }
    public void SetHealthFillMeter(float _value, float _max)
    {
        healthBarMeter.fillAmount = (_value / _max);
    }

    public void SetEnergyFillMeter(float _value, float _max)
    {
        energyBarMeter.fillAmount = (_value / _max);
    }
}
