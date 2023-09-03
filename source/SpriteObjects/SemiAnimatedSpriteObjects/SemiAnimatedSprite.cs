using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Metal_Jewel.SpriteObjects.SemiAnimatedSpriteObjects
{
    //Klassen gör det möjligt att använda SemiAnimatedSpriteBase-klassen för att skapa objekt med hjälp av dess variabler.
    public sealed class SemiAnimatedSprite : SemiAnimatedSpriteBase
    {
        //Konstruktor för simpla objekt (endast contentManager, texturePath, position och frames behövs anges. 
        //Resten av baskonstruktorns parametrar får ett standardvärde.
        public SemiAnimatedSprite (ContentManager contentManager, string texturePath, Vector2 position, int frames)
            : this(contentManager, texturePath, position, Color.White, 1.0f, frames, 0)
        {
        }

        //Konstruktor för avancerade objekt (ett värde till alla parametrar i baskonstruktorn behövs anges).
        public SemiAnimatedSprite (ContentManager contentManager, string texturePath, Vector2 position, 
            Color color, float scale, int frames, int frameIndex)
            : base(contentManager, texturePath, position, color, scale, frames, frameIndex)
        {
        }
    }
}
