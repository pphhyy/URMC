using Verse;

namespace URMC
{
    public class HediffCompProperties_BerserkImplant : HediffCompProperties
    {
        public bool forceBerserkMentalState = false;
        public HediffDef berserkHediff;
        public int cooldownTicks = 60000;
        public float minDamageToTrigger = 3f;
        public float severityPerDamage = 0.05f;
        public float minSeverity = 0.1f;
        public float maxSeverity = 1.0f;

        public HediffCompProperties_BerserkImplant()
        {
            compClass = typeof(HediffComp_BerserkImplant);
        }
    }
}