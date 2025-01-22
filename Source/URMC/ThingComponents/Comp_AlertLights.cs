using UnityEngine;
using Verse;

namespace URMC
{
    public class Comp_AlertLights : ThingComp
    {
        private Graphic _alertLightsGraphicLeft;
        private Graphic _alertLightsGraphicRight;
        private float _lerpProgress;
        private bool _lerpDirection = true;
        private Vector2 _drawSize;
        private readonly MaterialPropertyBlock _mPB = new ();
        
        private const float LERP_SPEED = 0.01f;
        
        public CompProperties_AlertLights Props => (CompProperties_AlertLights)props;
        
        public override void CompTick()
        {
            base.CompTick();
            if (parent is not Pawn parentPawn)
                return;

            if (!ShouldDraw(parentPawn)) 
                return;
            
            if (_lerpDirection)
            {
                _lerpProgress += LERP_SPEED * Props.lerpSpeed;
                if (!(_lerpProgress >= 1f)) 
                    return;
                    
                _lerpProgress = 1f;
                _lerpDirection = false;
            }
            else
            {
                _lerpProgress -= LERP_SPEED * Props.lerpSpeed;
                if (!(_lerpProgress <= 0f)) 
                    return;
                    
                _lerpProgress = 0f;
                _lerpDirection = true;
            }
        }
        
        public override void PostDraw()
        {
            base.PostDraw();
            if (parent is not Pawn parentPawn || !ShouldDraw(parentPawn)) 
                return;
            
            if (_alertLightsGraphicLeft == null || _alertLightsGraphicRight == null)
            {
                CacheAlertLightsGraphics(parentPawn);
            }
                
            DrawAlertLight(parentPawn, _alertLightsGraphicLeft, 
                parentPawn.Rotation, _drawSize, _lerpProgress);
                
            DrawAlertLight(parentPawn, _alertLightsGraphicRight, 
                parentPawn.Rotation, _drawSize, 1f - _lerpProgress);
        }
        
        private static bool ShouldDraw(Pawn pawn)
        {
            return pawn.Drafted;
        }
        
        private void DrawAlertLight(Pawn pawn, Graphic alertLightGraphic,
            Rot4 rotation, Vector2 drawSize, float lerpProgress)
        {
            Vector3 drawPos = pawn.Drawer.DrawPos;
            drawPos.y += 0.0075f;

            Color blendedColor = Color.Lerp(Props.colorOne, 
                Props.colorTwo, lerpProgress);
            
            Material fadedMaterial = FadedMaterialPool.FadedVersionOf(
                alertLightGraphic.MatAt(rotation), 1f);
            
            Matrix4x4 matrix4X4 = Matrix4x4.TRS(
                drawPos, Quaternion.identity, 
                new Vector3(drawSize.x, 1f, drawSize.y));
            
            _mPB.Clear();
            _mPB.SetColor(ShaderPropertyIDs.Color, blendedColor);
            Graphics.DrawMesh(MeshPool.plane10, matrix4X4, fadedMaterial, 
                0, null, 0, _mPB);
        }
        
        private void CacheAlertLightsGraphics(Pawn pawn)
        {
            PawnKindDef pawnKind = pawn.kindDef;
            PawnKindLifeStage curLSI = pawnKind.lifeStages[pawn.ageTracker.CurLifeStageIndex];
            
            _drawSize = curLSI.bodyGraphicData.Graphic.drawSize;
            
            string alertLightsGraphicLeftPath = Props.alertLightsGraphicLeft;
            string alertLightsGraphicRightPath = Props.alertLightsGraphicRight;
            
            _alertLightsGraphicLeft = GraphicDatabase.Get<Graphic_Multi>(
                alertLightsGraphicLeftPath,
                ShaderDatabase.MoteGlow,
                new Vector2(_drawSize.x, _drawSize.y),
                Color.white);

            _alertLightsGraphicRight = GraphicDatabase.Get<Graphic_Multi>(
                alertLightsGraphicRightPath,
                ShaderDatabase.MoteGlow,
                new Vector2(_drawSize.x, _drawSize.y),
                Color.white);
        }
        
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref _lerpProgress, "_lerpProgress", 0f);
            Scribe_Values.Look(ref _lerpDirection, "_lerpDirection", true);
        }
    }
}