using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Bat : LitMonster
    {
        private new void Start()
        {
            base.Start();

            Health = Data.batHealth;
            Attack = Data.batAttack;
            Defense = Data.batDefense;
            exp = Data.batExp;
        }
        
        protected override void CheckAttack()
        {
            var info = _animator.GetCurrentAnimatorStateInfo(0);
            
            if (info.IsName("Attack") && info.normalizedTime > 0.2f && info.normalizedTime < 0.4f)
            {
                isAttack = true;
            }
            else
            {
                isAttack = false;
            }
        }

    }


