using Microsoft.Xna.Framework;

namespace Metal_Jewel.Effects
{
    //Den här klassen kommer att användas för att skapa en skalningsanimation.
    public sealed class ScaleAnimationEffect : EffectBase
    {
        //Variabler som bestämmer objektets minsta och högsta värde skalnings-värde, samt hur mycket skalan ska öka/minska per update.
        private float minScale, maxScale, scalePerUpdate;

        //Skalans status och hur mycket skalan ska öka/minska per update måste anges vid konstruktoranrop. (Eventuellt minScale och maxScale också.)
        public ScaleAnimationEffect(EffectStatus scaleStatus, ref float objectsScale, float scalePerUpdate, float minScale = 0.0f, float maxScale = 1.0f)
            : base(scaleStatus)
        {
            this.minScale = minScale;
            this.maxScale = maxScale;
            this.scalePerUpdate = scalePerUpdate;
            switch (EffectStatus)
            {
                //Vill man att alphan ska minska eller att den ska ha sitt högsta värde? Då får alphan sitt högsta värde.
                case EffectStatus.DecreaseEffect:
                case EffectStatus.EffectAtMax:
                    objectsScale = maxScale;
                    break;
                //Vill man att alphan ska öka eller att den ska ha sitt minsta värde? Då får alphan sitt minsta värde.
                case EffectStatus.IncreaseEffect:
                case EffectStatus.EffectAtMin:
                    objectsScale = minScale;
                    break;
            }
        }

        //Metod som används för att uppdatera ett objekts skala beorende på EffectStatus
        public void Update(ref float objectsScale)
        {
            switch (EffectStatus)
            {
                //Ska objektets skala minska?
                case EffectStatus.DecreaseEffect:
                    //Är objektets skala fortfarande mer än minScale? Då minskas den, annars gör den inte det och EffectStatus markerar att objektets skala är vid sin minScale.
                    if (objectsScale > minScale)
                        objectsScale -= scalePerUpdate;
                    else
                        EffectStatus = EffectStatus.EffectAtMin;
                    break;

                //Ska färgens alpha öka?
                case EffectStatus.IncreaseEffect:
                    //Är objektets skala fortfarande mindre än maxScale? Då ökas den, annars gör den inte det och EffectStatus markerar att objektets skala är vid sin maxScale.
                    if (objectsScale < maxScale)
                        objectsScale += scalePerUpdate;
                    else
                        EffectStatus = EffectStatus.EffectAtMax;
                    break;
            }
        }
    }
}
