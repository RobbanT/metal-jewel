using Microsoft.Xna.Framework;

namespace Metal_Jewel.Effects
{
    //Den här klassen kommer att användas på samma sätt som sin basklass förutom att alphan ökar och sedan minskar automatiskt (det ska simulera solljus).
    public sealed class SunlightEffect : AlphaAnimationEffect
    {
        //Variabler som kontrollerar hur länge sedan den senaste uppdateringen gjordes, samt hur ofta koden i Update-metoden ska köras.
        float timeSinceLastUpdate, intervalBetweenUpdates;

        //Animationens status, själva färgen, hur mycket färgens alpha ska öka/minska per update och hur ofta alphan ska uppdateras måste anges vid konstruktoranrop.
        //(Eventuellt minAlpha och maxAlpha också.)
        public SunlightEffect(EffectStatus alphaStatus, ref Color objectsColor, byte alphaPerUpdate, float intervalBetweenUpdates, byte minAlpha = 0, byte maxAlpha = 255)
            : base(alphaStatus, ref objectsColor, alphaPerUpdate, minAlpha, maxAlpha)
        {
            this.intervalBetweenUpdates = intervalBetweenUpdates;
        }

        //Metod som används för att uppdatera alphan hos en färg.
        public void Update(GameTime gameTime, ref Color objectsColor)
        {
            timeSinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;
            //Har timeSinceLastUpdate blivit större än intervalBetweenUpdates?
            if (timeSinceLastUpdate >= intervalBetweenUpdates)
            {
                base.Update(ref objectsColor);

                switch (EffectStatus)
                {
                    //Har alphan nått sitt maxvärde? Då ska den minskas.
                    case EffectStatus.EffectAtMax:
                        EffectStatus = EffectStatus.DecreaseEffect;
                        timeSinceLastUpdate = 0;
                        break;
                    //Har alphan nått sitt minvärde? Då ska den ökas.
                    case EffectStatus.EffectAtMin:
                        EffectStatus = EffectStatus.IncreaseEffect;
                        timeSinceLastUpdate = 0;
                        break;
                }
            }
        }
    }
}
