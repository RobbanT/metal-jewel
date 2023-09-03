using Microsoft.Xna.Framework;

namespace Metal_Jewel.Effects
{
    //Den här klassen kommer att göra det möjligt för ett objekt att flytta sig från en plats till en annan.
    public sealed class MoveToPositionEffect : EffectBase
    {
        //Variabel håller reda på vilken riktning objektet ska röra sig i.
        private Vector2 direction;
        //Variabel kontrollerar om Effekten ska upprepa sig eller inte.
        private bool looping;
        //Variabler som bestämmer objektets startposition, slutposition och fart.
        private Vector2 startPosition, endPosition, speed;

        //Positionstatus, startPosition, slutPosition och fart måste anges vid anrop (Eventuellt om effekten ska loopa också).
        public MoveToPositionEffect(EffectStatus positionStatus, ref Vector2 objectsCurrentPosition, Vector2 startPosition, Vector2 endPosition, Vector2 speed, bool looping = false)
            : base(positionStatus)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.speed = speed;
            this.looping = looping;
            //Beroende på statuset man angav för effekten kommer objektets position att få ett varierande värde.
            switch (EffectStatus)
            {
                //Vill man att objektet ska röra sig från slutpositionen till startpositionen eller att objektet bara ska stå stilla på målpositionen?
                case EffectStatus.DecreaseEffect:
                case EffectStatus.EffectAtMax:
                    objectsCurrentPosition = endPosition;
                    break;
                //Vill man att objektet ska röra sig från startpositionen till slutpositionen eller att objektet bara ska stå stilla på startpositionen?
                case EffectStatus.IncreaseEffect:
                case EffectStatus.EffectAtMin:
                    objectsCurrentPosition = startPosition;
                    break;
            }
        }

        //Metoden används för att få objektet att röra på sig (beroende på EffectStatus)
        public void Update(ref Vector2 objectsCurrentPosition)
        {
            switch (EffectStatus)
            {
                //Ska objektet röra sig till startPositionen?
                case EffectStatus.DecreaseEffect:
                    direction = startPosition - objectsCurrentPosition;
                    direction.Normalize();
                    objectsCurrentPosition += direction * speed;

                    //Har objektet kommit till eller åkt förbi sin startPosition? Då stannar objektet där eller så loopar effekten igen.
                    if (objectsCurrentPosition.X * direction.X >= startPosition.X * direction.X &&
                        objectsCurrentPosition.Y * direction.Y >= startPosition.Y * direction.Y)
                    {
                        if (!looping)
                        {
                            objectsCurrentPosition = startPosition;
                            EffectStatus = EffectStatus.EffectAtMin;
                        }
                        else if (looping)
                            objectsCurrentPosition = endPosition;
                    }
                    break;

                //Ska objektet röra sig till slutPositionen?
                case EffectStatus.IncreaseEffect:
                    direction = endPosition - objectsCurrentPosition;
                    direction.Normalize();
                    objectsCurrentPosition += direction * speed;

                    //Har objektet kommit till eller åkt förbi sin slutPosition? Då stannar objektet där eller så loopar effekten igen.
                    if (objectsCurrentPosition.X * direction.X >= endPosition.X * direction.X &&
                        objectsCurrentPosition.Y * direction.Y >= endPosition.Y * direction.Y)
                    {
                        if (!looping)
                        {
                            objectsCurrentPosition = endPosition;
                            EffectStatus = EffectStatus.EffectAtMax;
                        }
                        else if (looping)
                            objectsCurrentPosition = startPosition;
                    }
                    break;
            }
        }
    }
}