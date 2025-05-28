using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace URMC
{
    [StaticConstructorOnStartup]
    public static class Patches
    {
        static Patches()
        {
            Harmony harmony = new (id: "rimworld.urmc.harmonyPatches");
            
            harmony.Patch(original: AccessTools.Method(typeof(PawnGroupMakerUtility), "GeneratePawns"),
                postfix: new HarmonyMethod(typeof(Patches), nameof(PawnGroupMakerUtilityGeneratePawns_Postfix)));
        }
        
        public static void PawnGroupMakerUtilityGeneratePawns_Postfix(PawnGroupMakerParms parms,
            bool warnOnZeroResults, ref IEnumerable<Pawn> __result)
        {
            if (!IsCalledFromRaidIncident())
                return;
            
            var pawnList = __result.ToList();
            
            if (parms?.faction?.def == URMCDefOf.pphhyy_Faction_URMC)
            {
                var ext = parms?.faction?.def?.GetModExtension<ModExt_FactionRaidControl>();
                if (ext?.mandatoryRaidPawnKinds != null)
                {
                    foreach (var entry in ext.mandatoryRaidPawnKinds)
                    {
                        for (int i = 0; i < entry.count; i++)
                        {
                            var request = new PawnGenerationRequest(
                                kind: entry.pawnKind,
                                faction: parms.faction,
                                context: PawnGenerationContext.NonPlayer,
                                tile: parms.tile,
                                forceGenerateNewPawn: true,
                                allowDead: false,
                                allowDowned: false,
                                mustBeCapableOfViolence: true,
                                allowAddictions: true,
                                developmentalStages: DevelopmentalStage.Adult
                            );
                            
                            Pawn pawn = PawnGenerator.GeneratePawn(request);
                            pawnList.Add(pawn);
                        }
                    }
                }
            }
            __result = pawnList;
        }
        
        /// <summary>
        /// DO NOT DO THIS SHIT IF CALLED EVERY TICK OR EVEN CLOSE TO EVERY TICK.
        /// THIS WILL NOT MESH WELL WITH MONO'S GC. YOU'VE BEEN WARNED.
        /// </summary>
        /// <returns></returns>
        private static bool IsCalledFromRaidIncident()
        {
            var stackTrace = new StackTrace();
            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                var method = stackTrace.GetFrame(i)?.GetMethod();
                if (method?.DeclaringType == null) continue;

                if (typeof(IncidentWorker_Raid).IsAssignableFrom(method.DeclaringType))
                {
                    return true;
                }
            }
            return false;
        }
    }
}