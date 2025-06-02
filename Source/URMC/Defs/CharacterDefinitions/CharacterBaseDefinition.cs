//  Copyright (c) 2024 RickGrymes
// Licensed under the MIT License
// See LICENSE.txt for the full text of the license

using Verse;

namespace URMC
{
    public abstract class CharacterBaseDefinition
    {
        public abstract bool AppliesPreGeneration { get; }
        public abstract bool AppliesPostGeneration { get; }

        public virtual void ApplyToPawn(Pawn pawn)
        {
        }

        public virtual void ApplyToRequest(ref PawnGenerationRequest request)
        {
        }
    }
}
