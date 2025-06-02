//  Copyright (c) 2024 RickGrymes
// Licensed under the MIT License
// See LICENSE.txt for the full text of the license

using Verse;
using RimWorld;
using JetBrains.Annotations;
using System.Collections.Generic;


namespace URMC
{
    [UsedImplicitly]
    public class CharacterDef : Def
    {
        public PawnKindDef pawnKind;
        public XenotypeDef xenotype;
        [UsedImplicitly] public List<CharacterBaseDefinition> definitions = [];

    }
}
