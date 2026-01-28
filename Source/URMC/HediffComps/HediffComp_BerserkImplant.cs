using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace URMC
{
    public class HediffComp_BerserkImplant : HediffComp
    {
        private int lastTriggerTick = -99999;

        public HediffCompProperties_BerserkImplant Props => (HediffCompProperties_BerserkImplant)props;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref lastTriggerTick, "lastTriggerTick");
        }

        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            if (Pawn.Dead || Pawn.Downed) return;
            if (!AddToExistingRage(totalDamageDealt)) return;
            if (totalDamageDealt < Props.minDamageToTrigger) return;
            int ticksGame = Find.TickManager.TicksGame;
            if (ticksGame - lastTriggerTick < Props.cooldownTicks) return;
            lastTriggerTick = ticksGame;
            React(totalDamageDealt);
        }

        private bool AddToExistingRage(float totalDamageDealt)
        {
            Hediff existing = Pawn.health.hediffSet.GetFirstHediffOfDef(Props.berserkHediff);
            if (existing != null)
            {
                float addedSeverity = totalDamageDealt * Props.severityPerDamage;
                existing.Severity = Mathf.Clamp(existing.Severity + addedSeverity, Props.minSeverity, Props.maxSeverity);
                return false;
            }
            return true;
        }

        private void React(float totalDamageDealt)
        {
            Hediff berserkerHediff = HediffMaker.MakeHediff(Props.berserkHediff, Pawn);
            berserkerHediff.Severity = Mathf.Clamp(totalDamageDealt * Props.severityPerDamage, Props.minSeverity, Props.maxSeverity);
            Pawn.health.AddHediff(berserkerHediff);
        }
    }
}