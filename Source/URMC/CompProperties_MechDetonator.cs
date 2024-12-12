﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace URMC
{
    public class CompProperties_MechDetonator : CompProperties
    {

        public float radius = 3f;
        public int damage = -1;
        public string iconPath;
        Texture2D uiIcon;
        public Texture2D GetUiIcon() { return uiIcon; }
        public CompProperties_MechDetonator()
        {
            compClass = typeof(CompMechDetonator);
        }

        public override void ResolveReferences(ThingDef parentDef)
        {
            if (!string.IsNullOrEmpty(iconPath))
            {
                LongEventHandler.ExecuteWhenFinished(delegate
                {
                    uiIcon = ContentFinder<Texture2D>.Get(iconPath);
                });
            }
        }
    }
}
