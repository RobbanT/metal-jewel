using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Metal_Jewel.SpriteObjects.SpriteObjects
{
    //Klassen gör det möjligt att använda SpriteBase-klassen för att skapa objekt med hjälp av dess variabler.
    public sealed class Sprite : SpriteBase
    {
        //Konstruktor för simpla objekt (endast contentManager, texturePath och position behövs anges.
        //Resten av baskonstruktorns parametrar får ett standardvärde.)
        public Sprite(ContentManager contentManager, string texturePath, Vector2 position)
            : this(contentManager, texturePath, position, Color.White, 1.0f)
        {
        }

        //Konstruktor för avancerade objekt (ett värde till alla parametrar i baskonstruktorn behövs anges).
        public Sprite(ContentManager contentManager, string texturePath, Vector2 position, Color color, float scale)
            : base(contentManager, texturePath, position, color, scale)
        {
        }
    }
}
