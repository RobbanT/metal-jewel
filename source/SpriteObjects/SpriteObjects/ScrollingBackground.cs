using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Metal_Jewel.Effects;

namespace Metal_Jewel.SpriteObjects.SpriteObjects
{
    //Det här objektet ska agera som en skrollande bakgrund med en del effekter.
    public sealed class ScrollingBackground : SpriteBase
    {
        //Effekt som gör att objektet rör sig från en plats till en annan.
        MoveToPositionEffect moveToSpotEffect;
        //Effekt som gör att objektets alpha varierar.
        SunlightEffect sunlightEffect;

        //Konstruktor för simpla objekt (endast contentManager och texturePath behövs anges.
        //Resten av baskonstruktorns parametrar får ett standardvärde).
        public ScrollingBackground(ContentManager contentManager, string texturePath)
            : this(contentManager, texturePath, new Vector2(Game1.WindowWidth / 2 - Game1.WindowWidth, Game1.WindowHeight / 2), 
            Color.White, 1.0f, new Vector2(Game1.WindowWidth / 2 + Game1.WindowWidth, Game1.WindowHeight / 2))
        {
        }

        //Konstruktor för avancerade objekt (ett värde till alla parametrar behövs anges).
        public ScrollingBackground(ContentManager contentManager, string texturePath, Vector2 startPosition, 
            Color color, float scale, Vector2 endPosition)
            : base(contentManager, texturePath, startPosition, color, scale)
        {
            moveToSpotEffect = new MoveToPositionEffect(EffectStatus.IncreaseEffect, ref position, startPosition, endPosition, 
                new Vector2((float)EffectBase.Random.NextDouble(), 0), true);
            sunlightEffect = new SunlightEffect(EffectStatus.IncreaseEffect, ref base.color, 1, 1000.0f, 
                (byte)EffectBase.Random.Next(64), (byte)EffectBase.Random.Next(65, 255));
        }

        //Metoden uppdaterar objektets position och alpha (med hjälp av effekterna).
        public void Update(GameTime gameTime)
        {
            moveToSpotEffect.Update(ref position);
            sunlightEffect.Update(gameTime, ref color);
        }
    }
}
