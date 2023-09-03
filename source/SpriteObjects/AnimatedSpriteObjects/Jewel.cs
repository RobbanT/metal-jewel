using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Metal_Jewel.ManagerClasses;
using Metal_Jewel.SpriteObjects.SpriteObjects;
using Metal_Jewel.Effects;

namespace Metal_Jewel.SpriteObjects.AnimatedSpriteObjects
{
    //Enum för alla färger som juvelen kan ha.
    public enum JewelColor { Green, Blue, Yellow, Purple, Red }

    //Detta är klassen som kommer att representera en juvel i spelet.
    public sealed class Jewel : AnimatedSpriteBase
    {
        //Variabel som hållera reda på vilken färg juvelen har.
        public JewelColor JewelColor { get; private set; }
        //Skalningseffekt som kommer att spelas upp precis innan juvelen tas bort.
        public ScaleAnimationEffect ScaleAnimationEffect { get; private set; }
        //Effekt som gör det möjligt för juvelen att röra sig.
        public MoveToPositionEffect MoveToPositionEffect { get; private set; }
        //Bool som kontrolleras om den har juvelen har kontrollerats under en kontroll av närliggande juveler.
        public bool Checked { get; set; }
        //Bool som kontrollerar om spelaren har markerat den här juvelen.
        public bool Selected { get; private set; }
        //Bool som kontrollerar om juvelens skalningseffekt körs.
        public bool Scaling { get; set; }
        //Bool som kontrollerar om juvelen rör sig.
        public bool Moving { get; set; }
        //Bool som kontrollerar om juvelen ska tas bort.
        public bool Remove { get; private set; }

        //Vid anrop måste en contentManager, juvelfärg, startposition, slutposition, fart och flyttstatus anges.
        //(Resten av baskonstruktorns parametrar får ett standardvärde.)
        public Jewel(ContentManager contentManager, JewelColor jewelColor, Vector2 startPosition, Vector2 endPosition,
            Vector2 speed, EffectStatus moveStatus)
            : base(contentManager, "Images/GameScreen/" + jewelColor + "JewelSpriteSheet", startPosition, Color.White,
            1.0f, 20, 0, 100, AnimationState.Paused, true)
        {
            JewelColor = jewelColor;
            ScaleAnimationEffect = new ScaleAnimationEffect(EffectStatus.DecreaseEffect, ref base.scale, 0.05f, 0.25f, 1.0f);
            MoveToPositionEffect = new MoveToPositionEffect(moveStatus, ref base.position, position, endPosition, speed);
            if (moveStatus == EffectStatus.DecreaseEffect || moveStatus == EffectStatus.IncreaseEffect)
                Moving = true;
        }

        //Metod som avgör vad som händer med juvelen när spelaren t.ex. klickar på den.
        public void HandleInput(InputManager inputManager)
        {
            //Rör sig inte juvelen och skalas den inte? Då ska man kunna göra något med juvelen.
            if (!Moving && !Scaling)
            {
                //Har spelaren klickat på juvelen? Då markeras/avmarkeras juvelen.
                if (HasBeenClicked(inputManager))
                {
                    if (Selected)
                        DeselectedJewel();
                    else
                        SelectJewel();
                }
                //Har spelaren inte muspilen på juvelen? Då ska juvelen pausa och start om sin animation.
                else if (!Hovering(inputManager))
                {
                    PausAnimation();
                    ResetAnimation();
                }
                //Har spelaren muspilen på juvelen? Då ska juvelen spela upp sin animation.
                else if (Hovering(inputManager))
                {
                    PlayAnimation();
                }
            }
        }

        //Metoden används för att uppdatera juvelens animation, position och skala.
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            updateJewelPosition();
            updateJewelScale();
            //Skalas juvelen så avmarkeras den och dess animation startas om och pausas.
            if (Scaling)
            {
                DeselectedJewel();
                ResetAnimation();
                PausAnimation();
            }
        }

        //Metod för att uppdatera juvelens position.
        private void updateJewelPosition()
        {
            //Rör juvelen på sig? Då ska MoveToPositionEffect-objektet uppdateras.
            if (Moving)
            {
                MoveToPositionEffect.Update(ref position);
                //Har juvelen nått sin start- eller slutposition? Då ska den inte röra sig längre.
                if (MoveToPositionEffect.EffectStatus == EffectStatus.EffectAtMin ||
                    MoveToPositionEffect.EffectStatus == EffectStatus.EffectAtMax)
                    Moving = false;
            }
        }

        //Metod för att uppdatera juvelens skala.
        private void updateJewelScale()
        {
            //Ska skalning av juvelen ske?
            if (Scaling)
            {
                ScaleAnimationEffect.Update(ref scale);
                //Har skalan nått sitt minimumvärde? Då markerar vi att juvelen ska tas bort.
                if (ScaleAnimationEffect.EffectStatus == EffectStatus.EffectAtMin)
                    Remove = true;
            }
        }

        //Metod som används när en juvel har blivit markerad.
        public void SelectJewel()
        {
            Selected = true;
            color = Color.White * 0.5f;
        }

        //Metod som används när en juvel har blivit omarkerad.
        public void DeselectedJewel()
        {
            Selected = false;
            color = Color.White;
        }

        //Metod som är till för om man vill ändra juvelens flytteffekt.
        public void SetNewMovePosition(EffectStatus moveStatus, Vector2 startPosition, Vector2 endPosition, Vector2 speed)
        {
            MoveToPositionEffect = new MoveToPositionEffect(moveStatus, ref base.position, position, endPosition, speed);
            if (moveStatus == EffectStatus.DecreaseEffect || moveStatus == EffectStatus.IncreaseEffect)
                Moving = true;
        }
    }
}
