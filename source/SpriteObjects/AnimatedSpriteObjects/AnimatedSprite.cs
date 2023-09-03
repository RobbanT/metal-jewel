using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Metal_Jewel.SpriteObjects.AnimatedSpriteObjects
{
    //Klassen gör det möjligt att använda AnimatedSpriteBase-klassen för att skapa objekt med hjälp av dess variabler.
    public sealed class AnimatedSprite : AnimatedSpriteBase
    {
        //Konstruktor för simpla objekt (endast contentManager, texturePath, position och frames behövs anges. 
        //Resten av baskonstruktorns parametrar får ett standardvärde).
        public AnimatedSprite (ContentManager contentManager, string texturePath, Vector2 position, int frames)
            : this(contentManager, texturePath, position, Color.White, 1.0f, frames, 0, 100, AnimationState.Playing, true)
        {
        }

        //Konstruktor för avancerade objekt (ett värde till alla parametrar i baskonstruktorn behövs anges).
        public AnimatedSprite(ContentManager contentManager, string texturePath, Vector2 position, Color color, 
            float scale, int frames, int frameIndex, int millisecondsPerFrame, AnimationState animationState, bool looping)
            : base(contentManager, texturePath, position, color, scale, frames, frameIndex, millisecondsPerFrame, animationState, looping)
        {
        }
    }
}
