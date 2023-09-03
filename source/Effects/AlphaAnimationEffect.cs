using Microsoft.Xna.Framework;

namespace Metal_Jewel.Effects
{
    //Den här klassen kommer att användas för att skapa en alphaAnimation med hjälp av ett Alpha-värde.
    public class AlphaAnimationEffect : EffectBase
    {
        //Variabler som bestämmer objektets minsta och högsta alpha-värde, samt hur mycket alphan ska öka/minska per update.
        protected byte minAlpha, maxAlpha, alphaPerUpdate;

        //Animationens status, själva färgen, och hur mycket färgens alpha ska öka/minska per update måste anges vid konstruktoranrop. (Eventuellt minAlpha och maxAlpha också.)
        public AlphaAnimationEffect(EffectStatus alphaStatus, ref Color objectsColor, byte alphaPerUpdate, byte minAlpha = 0, byte maxAlpha = 255)
            : base(alphaStatus)
        {
            this.minAlpha = minAlpha;
            this.maxAlpha = maxAlpha;
            this.alphaPerUpdate = alphaPerUpdate;
            //Beroende på statuset man angav för animationen kommer alphan att få ett varierande värde.
            switch (EffectStatus)
            {
                //Vill man att alphan ska minska eller att den ska ha sitt högsta värde? Då får alphan sitt högsta värde.
                case EffectStatus.DecreaseEffect:
                case EffectStatus.EffectAtMax:
                    objectsColor.R = maxAlpha;
                    objectsColor.G = maxAlpha;
                    objectsColor.B = maxAlpha;
                    objectsColor.A = maxAlpha;
                    break;
                //Vill man att alphan ska öka eller att den ska ha sitt minsta värde? Då får alphan sitt minsta värde.
                case EffectStatus.IncreaseEffect:
                case EffectStatus.EffectAtMin:
                    objectsColor.R = minAlpha;
                    objectsColor.G = minAlpha;
                    objectsColor.B = minAlpha;
                    objectsColor.A = minAlpha;
                    break;
            }
        }

        //Metod som används för att uppdatera alphan hos en färg.
        public void Update(ref Color objectsColor)
        {
            switch (EffectStatus)
            {
                //Ska färgens alpha minska?
                case EffectStatus.DecreaseEffect:
                    //Är färgens alpha fortfarande mer än minAlpha? Då minskas den, annars gör den inte det och EffectStatus markerar att färgens alpha är vid sin minAlpha.
                    if (objectsColor.A > minAlpha)
                    {
                    objectsColor.R -= alphaPerUpdate;
                    objectsColor.G -= alphaPerUpdate;
                    objectsColor.B -= alphaPerUpdate;
                    objectsColor.A -= alphaPerUpdate;
                    }
                    else
                        EffectStatus = EffectStatus.EffectAtMin;
                    break;

                //Ska färgens alpha öka?
                case EffectStatus.IncreaseEffect:
                    //Är färgens alpha fortfarande mindre än maxAlpha? Då ökas den, annars gör den inte det och EffectStatus markerar att färgens alpha är vid sin maxAlpha.
                    if (objectsColor.A < maxAlpha)
                    {
                        objectsColor.R += alphaPerUpdate;
                        objectsColor.G += alphaPerUpdate;
                        objectsColor.B += alphaPerUpdate;
                        objectsColor.A += alphaPerUpdate;
                    }
                    else
                        EffectStatus = EffectStatus.EffectAtMax;
                    break;
            }
        }
    }
}
