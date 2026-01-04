using System;
using FixMath;
using LogicLayer;
using SkillSystem.Buff.BuffCfg;
using SkillSystem.Config;

namespace Game
{
    public class DamageCalculateCenter
    {
        /// <summary>
        /// 获取物理攻击力
        /// </summary>
        /// <param name=" attacker"></param>
        /// <returns></returns>
        public static FixInt GetADAttack(LogicActor attacker)
        {
            return  attacker.AD * (1 +  attacker.STR / 250);
        }

        /// <summary>
        /// 获取魔法攻击力
        /// </summary>
        /// <param name=" attacker"></param>
        /// <returns></returns>
        public static FixInt GetAPAttack(LogicActor attacker)
        {
            return   attacker.AP * (1 +  attacker.INT / 250);
        }

        /// <summary>
        /// 物理伤害减免
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="attackTarget"></param>
        /// <returns></returns>
        public static FixInt GetADReduction(LogicActor attacker,LogicActor attackTarget)
        {
            FixInt damageReductionRate = attackTarget.AdDef/(attacker.Level*200+attackTarget.AdDef);
            return damageReductionRate > 0.75f? 0.75f:damageReductionRate;
        }
        
        /// <summary>
        /// 魔法伤害减免
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="attackTarget"></param>
        /// <returns></returns>
        public static FixInt GetAPReduction(LogicActor attacker,LogicActor attackTarget)
        {
            FixInt damageReductionRate = attackTarget.ApDef/(attacker.Level*200+attackTarget.ApDef);
            return damageReductionRate > 0.75f? 0.75f:damageReductionRate;
        }

        public static FixInt GetADPCTDamage(FixInt totalDamage, LogicActor  attacker)
        {
            return totalDamage * (1 +  attacker.PCT);
        }
        
        public static FixInt GetAPMCTDamage(FixInt totalDamage, LogicActor  attacker)
        {
            return totalDamage * (1 +  attacker.MCT);
        }

        public static FixInt CalculateDamage(SkillDamageConfig damageConfig, LogicActor attacker, LogicActor attackTarget)
        {
            FixInt finalDamage = FixInt.Zero;
            switch (damageConfig.damageType)
            {
                case DamageType.None:
                    break;
                case DamageType.ADDamage:
                    finalDamage = GetADReduction(attacker, attackTarget)*GetADAttack(attacker);
                    break;
                case DamageType.APDamage:
                    finalDamage = GetAPReduction(attacker, attackTarget)*GetAPAttack(attacker);
                    break;
            }

            return finalDamage * (new FixInt(damageConfig.damageRate) / new FixInt(100));
        }
        
        public static FixInt CalculateDamage(BuffConfig buffConfig, LogicActor attacker, LogicActor attackTarget)
        {
            FixInt finalDamage = FixInt.Zero;
            DamageType damageType = buffConfig.targetConfig.isOpen?buffConfig.targetConfig.damageCfg.damageType :  buffConfig.damageType;
            switch (damageType)
            {
                case DamageType.None:
                    break;
                case DamageType.ADDamage:
                    finalDamage = GetADReduction(attacker, attackTarget)*GetADAttack(attacker);
                    break;
                case DamageType.APDamage:
                    finalDamage = GetAPReduction(attacker, attackTarget)*GetAPAttack(attacker);
                    break;
            }
            return finalDamage * 
                   (buffConfig.targetConfig.isOpen
                       ? new FixInt(buffConfig.targetConfig.damageCfg.damageRate) / new FixInt(100) 
                       : new FixInt(buffConfig.damageRate) / new FixInt(100));
        }
    }
}