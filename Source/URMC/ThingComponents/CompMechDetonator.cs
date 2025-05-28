using Verse;
using RimWorld;
using System.Collections.Generic;

namespace URMC
{
    public class CompMechDetonator : ThingComp
    {
        public CompProperties_MechDetonator Props => (CompProperties_MechDetonator)props;
        
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            // invert this to reduce nesting and make shit more readable if desired :)
            if (parent.Faction == Faction.OfPlayer)
            {
                Command_Action command_Action = new Command_Action
                {
                    icon = Props.GetUiIcon(),
                    defaultLabel = "URMC_MechDetonate".Translate(),
                    action = delegate
                    {
                        parent.Kill();
                    }
                };
                yield return command_Action;
            }
        }
    }
}
