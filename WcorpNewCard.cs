using System;
using LOR_DiceSystem;
using System.Collections;
using UnityEngine;
using JetBrains.Annotations;

namespace stdin_WcorpNewCard
{
    public class DiceCardAbility_WcorpNewCard : DiceCardAbilityBase
    {
        public override string[] Keywords
        {
            get
            {
                return new string[]
                {
                "WarpCharge"
                };
            }
        }
        public override void BeforRollDice()
        {
            System.Random rand = new System.Random();
            BattleUnitBuf_warpCharge battleUnitBuf_warpCharge = base.owner.bufListDetail.GetActivatedBuf(KeywordBuf.WarpCharge) as BattleUnitBuf_warpCharge;
            int temp = RandomUtil.Range(1, battleUnitBuf_warpCharge.stack <= 5 ? battleUnitBuf_warpCharge.stack : 5);
            if(temp == 5) //만약 최대값이라면
            {
                battleUnitBuf_warpCharge.UseStack(temp);
                this.behavior.ApplyDiceStatBonus(new DiceStatBonus
                {
                    face = battleUnitBuf_warpCharge.stack + RandomUtil.Range(5, 10),
                    power = temp
                }) ;
            }
            else if (battleUnitBuf_warpCharge.stack > 0)
            {
                battleUnitBuf_warpCharge.UseStack(temp);
                this.behavior.ApplyDiceStatBonus(new DiceStatBonus
                {
                    power = temp,
                }) ;
            }
            BattleCardTotalResult battleCardResultLog = base.owner.battleCardResultLog;
            if (battleCardResultLog == null)
            {
                return;
            }
            battleCardResultLog.SetSucceedAtkEvent(delegate
            {
                FilterUtil.ShowWarpFilter();
                BattleCamManager instance = SingletonBehavior<BattleCamManager>.Instance;
                CameraFilterPack_FX_EarthQuake cameraFilterPack_FX_EarthQuake = ((instance != null) ? instance.EffectCam.gameObject.AddComponent<CameraFilterPack_FX_EarthQuake>() : null) ?? null;
                if (cameraFilterPack_FX_EarthQuake != null)
                {
                    cameraFilterPack_FX_EarthQuake.StartCoroutine(this.EarthQuakeRoutine_warp(cameraFilterPack_FX_EarthQuake));
                    BattleCamManager instance2 = SingletonBehavior<BattleCamManager>.Instance;
                    AutoScriptDestruct autoScriptDestruct = ((instance2 != null) ? instance2.EffectCam.gameObject.AddComponent<AutoScriptDestruct>() : null) ?? null;
                    if (autoScriptDestruct != null)
                    {
                        autoScriptDestruct.targetScript = cameraFilterPack_FX_EarthQuake;
                        autoScriptDestruct.time = 0.5f;
                    }
                }
            });

        }
        public override void OnSucceedAttack()
        {
            BattleUnitBuf_warpCharge battleUnitBuf_warpCharge = base.owner.bufListDetail.GetActivatedBuf(KeywordBuf.WarpCharge) as BattleUnitBuf_warpCharge;
            if (battleUnitBuf_warpCharge.stack > 0)
            {
                base.ActivateBonusAttackDice();
            }

        }
        private IEnumerator EarthQuakeRoutine_warp(CameraFilterPack_FX_EarthQuake r)
        {
            float e = 0f;
            while (e < 1f)
            {
                e += Time.deltaTime * 2f;
                r.Speed = 30f * (1f - e);
                r.X = 0.02f * (1f - e);
                r.Y = 0.02f * (1f - e);
                yield return null;
            }
            yield break;
        }

    }

}
