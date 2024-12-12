using Verse;
using RimWorld;
using Verse.Sound;
using System.Collections.Generic;
using Verse.AI.Group;
using System;
using UnityEngine;

namespace URMC
{
    public class CompMechDetonator : ThingComp
    {
        public CompProperties_MechDetonator Props => (CompProperties_MechDetonator)props;

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {

            if (parent.Faction == Faction.OfPlayer)
            {
                Command_Action command_Action = new Command_Action();
                command_Action.icon = Props.GetUiIcon();
                command_Action.defaultLabel = "URMC_MechDetonate".Translate();
                command_Action.action = delegate
                {
                    //Log.Message("BOOM");
                    parent.Kill();
                };
                yield return command_Action;
            }

        }
    }
}
