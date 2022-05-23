using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniName : MonoBehaviour
{
    public static int attackIdle;
    
    public static int defense;
    public static int defCombo;
    public static int Roll;
    
    public static int equip_bool;
    public static int unequip;
    public static int combo_04_1;
    public static int combo_04_2;
    public static int combo_04_3;
    public static int combo_04_4;
    public static int combo_04_5;
    public static int Dizzy;
    public static int GetHit;
    public static int Die;
    public static int speed;
    public static int Taunting;

    private void Awake()
    {
        attackIdle = Animator.StringToHash("idle");
        defense = Animator.StringToHash("defense");
        defCombo = Animator.StringToHash("defCombo");
        Roll = Animator.StringToHash("Roll");
        equip_bool = Animator.StringToHash("Equiped");
        combo_04_1 = Animator.StringToHash("combo0401");
        combo_04_2 = Animator.StringToHash("combo0402");
        combo_04_3 = Animator.StringToHash("combo0403");
        combo_04_4 = Animator.StringToHash("combo0404");
        combo_04_5 = Animator.StringToHash("combo0405");
        combo_04_5 = Animator.StringToHash("combo0405");
        
        Dizzy = Animator.StringToHash("Dizzy");
        GetHit = Animator.StringToHash("GetHit");
        Die = Animator.StringToHash("Die");
        speed = Animator.StringToHash("Speed");
        Taunting = Animator.StringToHash("Taunting");
    }
}
