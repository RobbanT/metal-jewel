using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Metal_Jewel.SpriteFontObjects
{
    //Detta är superklassen för alla avancerade objekt som kommer att använda sig av en spriteFont;
    public abstract class SpriteFontBase
    {
        //SpriteFonten som objektet använder sig av när det målas upp.
        protected SpriteFont spriteFont;
        //Positionen som objektet kommer att använda sig av när det målas upp.
        protected Vector2 position;
        //Färgen som objektet kommer att använda sig av när det målas upp.
        protected Color color;
        //Vad som ska skrivas ut.
        protected string text;

        //Vid anrop måste en spriteFont, position, färg  och vad som ska skrivas ut anges.
        public SpriteFontBase(SpriteFont spriteFont, Vector2 position, Color color, string text)
        {
            this.spriteFont = spriteFont;
            this.position = position;
            this.color = color;
            this.text = text;
        }

        //Metoden som skriver ut texten.
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, text, position, color, 0.0f, 
                spriteFont.MeasureString(text) / 2, 0.60f, SpriteEffects.None, 0.0f);
        }
    }
}
