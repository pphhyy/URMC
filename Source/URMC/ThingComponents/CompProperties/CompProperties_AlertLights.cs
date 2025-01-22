using UnityEngine;
using Verse;

namespace URMC
{
    public class CompProperties_AlertLights : CompProperties
    {
        public string alertLightsGraphicLeft = null;
        public string alertLightsGraphicRight = null;
        public Color colorOne = Color.white;
        public Color colorTwo = Color.white;

        public CompProperties_AlertLights()
        {
            compClass = typeof(Comp_AlertLights);
        }
    }
}