using System;
using System.Linq;
using Verse;
using RimWorld;

namespace URMC
{

	public class IncidentWorker_AlienTeam : IncidentWorker
	{
		#region Fields

		private const float RelationWithColonistWeight = 20f;

		private ModExt_AlienTeam modExtension = null;

		#endregion

		#region Methods
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			modExtension = this.def.GetModExtension<ModExt_AlienTeam>();
			if (modExtension == null) return false;
			Map map = (Map)parms.target;
			return this.CanSpawnJoiner(map);
		}

		public virtual Pawn GeneratePawn(CharacterDef characterDef)
		{
			PawnGenerationRequest request = new PawnGenerationRequest(characterDef.pawnKind);
			request.KindDef = characterDef.pawnKind;
			request.Faction = Faction.OfPlayer;
			request.ForceGenerateNewPawn = true;

			// Generate the pawn.
			CharacterDefinitionUtils.ApplyRequestDefinitions(ref request, characterDef.definitions);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			CharacterDefinitionUtils.ApplyPawnDefinitions(pawn, characterDef.definitions);
			return pawn;
		}

		public virtual bool CanSpawnJoiner(Map map)
		{
			IntVec3 intVec;
			return this.TryFindEntryCell(map, out intVec);
		}

		public virtual void SpawnJoiner(Map map, Pawn pawn)
		{
			IntVec3 loc;
			this.TryFindEntryCell(map, out loc);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!this.CanSpawnJoiner(map))
			{
				return false;
			}
			Pawn firstPawn = null;
			foreach (CharacterDef characterDef in modExtension.characterToSpawn)
			{
				Pawn pawn = this.GeneratePawn(characterDef);
				if (firstPawn == null)
				{
					firstPawn = pawn;
				}
				this.SpawnJoiner(map, pawn);
			}
			// TaggedString baseLetterText = (this.def.pawnHediff != null) ? this.def.letterText.Formatted(pawn.Named("PAWN"), this.def.pawnHediff.Named("HEDIFF")).AdjustedFor(pawn, "PAWN", true) : this.def.letterText.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			// TaggedString baseLetterLabel = this.def.letterLabel.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			// PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref baseLetterText, ref baseLetterLabel, pawn);
			//base.SendStandardLetter(baseLetterLabel, baseLetterText, LetterDefOf.PositiveEvent, parms, pawn, Array.Empty<NamedArgument>());
			base.SendStandardLetter(this.def.letterLabel, this.def.letterText, LetterDefOf.PositiveEvent, parms, firstPawn, Array.Empty<NamedArgument>());
			return true;
		}

		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c) && !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out cell);
		}

		#endregion


	}
}
