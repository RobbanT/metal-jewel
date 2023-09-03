using System;

namespace Metal_Jewel.Effects
{
    //Enum som beskriver statuset som ett Effect-objekt kan ha.
    public enum EffectStatus { DecreaseEffect, IncreaseEffect, EffectAtMin, EffectAtMax }

    //Den här klassen kommer att agera som en superklass för objekt som ska fungera som någon form av effekt.
    public abstract class EffectBase
    {
        //Variabel som kommer att användas för att slumpa fram tal.
        public static readonly Random Random = new Random();
        //Variabeln/Propertyn används för att hålla reda på objektets status.
        public EffectStatus EffectStatus { get; protected set; }

        //Vid anrop måste effektens status anges.
        public EffectBase(EffectStatus effectStatus)
        {
            EffectStatus = effectStatus;
        }

        //Metod som används för att börja minska effekten.
        public void startDecreaseEffect(){ EffectStatus = EffectStatus.DecreaseEffect; }

        //Metod som används för att börja öka effekten.
        public void startIncreaseEffect(){ EffectStatus = EffectStatus.IncreaseEffect; }
    }
}
