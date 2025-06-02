//  Copyright (c) 2024 RickGrymes
// Licensed under the MIT License
// See LICENSE.txt for the full text of the license

using Verse;
using System.Collections.Generic;

namespace URMC
{
    public static class CharacterDefinitionUtils
    {
        public static Pawn GenerateWithDefinitions(PawnGenerationRequest request, List<CharacterBaseDefinition> definitions)
        {
            ApplyRequestDefinitions(ref request, definitions);
            Pawn pawn = PawnGenerator.GeneratePawn(request);
            ApplyPawnDefinitions(pawn, definitions);

            return pawn;
        }

        public static void ApplyRequestDefinitions(ref PawnGenerationRequest request, List<CharacterBaseDefinition> definitions)
        {
            foreach (CharacterBaseDefinition definition in definitions)
            {
                if (!definition.AppliesPreGeneration) return;
                definition.ApplyToRequest(ref request);
            }
            request.ValidateAndFix();
        }

        public static void ApplyPawnDefinitions(Pawn pawn, List<CharacterBaseDefinition> definitions)
        {
            foreach (CharacterBaseDefinition definition in definitions)
            {
                if (!definition.AppliesPostGeneration) return;
                definition.ApplyToPawn(pawn);
            }
        }
    }
}
