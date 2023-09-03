using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Metal_Jewel.SpriteObjects.SemiAnimatedSpriteObjects;

namespace Metal_Jewel.SpriteObjects.AnimatedSpriteObjects
{
    //Enum för att hålla reda på vilket status som animationen har. Körs animationen eller är den pausad?
    public enum AnimationState { Playing, Paused }

    //Klassen kommer att användas som superklass för samtliga objekt som ska agera som någon form av bild och vara helt animerade.
    public abstract class AnimatedSpriteBase : SemiAnimatedSpriteBase
    {
        //Variabeln kommer att användas för att kontrollera när en bild i animationen ska bytas.
        private int timeSinceLastFrame;
        //Variabeln bestämmer hur länge varje bild i animationen ska visas.
        private int millisecondsPerFrame;
        //Variabel för att hålla reda på animationens status.
        private AnimationState animationState;
        //Variabel som bestämmer om animationen ska loopas.
        //Är variabeln false körs animationen bara en gång, annars körs den oändligt många gånger.
        private bool looping;

        //Vid anrop måste en contentManager, sökväg till textur, position, färg, skala, antalet bilder i animationen, 
        //vilken bild i animationen som ska målas, millisecondsPerFrame, animationens status och om animation ska loopas anges.
        public AnimatedSpriteBase(ContentManager contentManager, string texturePath, Vector2 position,
            Color color, float scale, int frames, int frameIndex, int millisecondsPerFrame, 
            AnimationState animationState, bool looping)
            : base(contentManager, texturePath, position, color, scale, frames, frameIndex)
        {
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.animationState = animationState;
            this.looping = looping;
        }

        //Metoden uppdaterar animationen.
        public virtual void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            //Spelas animationen upp och är timeSinceLastFrame större eller lika med millisecondsPerFrame? Då körs koden nedan.
            if (animationState == AnimationState.Playing && timeSinceLastFrame >= millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                //Är animationens bildindex lika med antalet bilder som finns i animationen? Då ska animationen börja på bild 1 igen.
                if (frameIndex == sourceRectangles.Length - 1)
                {
                    //Loopas inte animationen? Då pausar vi den.
                    if (!looping)
                        PausAnimation();
                    ResetAnimation();
                    return;
                }
                frameIndex++;
            }
        }

        //Metod för att få en animation att spelas upp.
        public void PlayAnimation(){ animationState = AnimationState.Playing; }

        //Metod för att pausa en animation.
        public void PausAnimation(){ animationState = AnimationState.Paused; }

        //Metod för att få animationen att börja om på sin första bild.
        public void ResetAnimation(){ frameIndex = 0; }
    }
}
