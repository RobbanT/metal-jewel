using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Metal_Jewel.Effects;

namespace Metal_Jewel.SpriteFontObjects
{
    //Detta är en en klass som kommer göra det möjligt för en spriteFont att röra sig med en varierande alpha.
    public sealed class FloatingSpriteFont : SpriteFontBase
    {
        //Effekt som gör att objektet rör sig från en plats till en annan.
        private MoveToPositionEffect moveToSpotEffect;
        //Effekt som gör att objektets alpha varierar.
        private AlphaAnimationEffect alphaAnimationEffect;
        //Variabel som håller reda på om objektet ska tas bort.
        public bool Remove { get; private set; }

        //Vid anrop måste en spriteFont, startposition, färg, vad som ska skrivas, slutPosition, 
        //fart och alpha per uppdatering anges.
        public FloatingSpriteFont(SpriteFont spriteFont, Vector2 startPosition, Color color, string text,
            Vector2 endPosition, Vector2 speed, byte alphaPerUpdate)
            : base (spriteFont, startPosition, color, text)
        {
            moveToSpotEffect = new MoveToPositionEffect(EffectStatus.IncreaseEffect, ref base.position,
                startPosition, endPosition, speed);
            alphaAnimationEffect = new AlphaAnimationEffect(EffectStatus.DecreaseEffect, ref color, alphaPerUpdate);
        }

        //Metod som uppdaterar effekterna.
        public void Update()
        {
            moveToSpotEffect.Update(ref position);
            alphaAnimationEffect.Update(ref color);
            //Har moveToSpotEffect eller alphaAnimationEffect kört klart? Då markerar vi att objektet ska tas bort.
            if (moveToSpotEffect.EffectStatus == EffectStatus.EffectAtMax ||
                alphaAnimationEffect.EffectStatus == EffectStatus.EffectAtMin)
                Remove = true;
        }
    }
}
