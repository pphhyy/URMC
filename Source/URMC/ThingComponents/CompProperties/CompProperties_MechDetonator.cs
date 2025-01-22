using UnityEngine;
using Verse;

namespace URMC
{
    public class CompProperties_MechDetonator : CompProperties
    {
        public float radius = 3f;
        public int damage = -1;
        public string iconPath;
        
        private Texture2D _uiIcon;
        
        public Texture2D GetUiIcon()
        {
            return _uiIcon;
        }
        
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
                    _uiIcon = ContentFinder<Texture2D>.Get(iconPath);
                });
            }
            else
            {
                Log.Error($"[CompProperties_MechDetonator] iconPath is null.");
            }
        }
    }
}
