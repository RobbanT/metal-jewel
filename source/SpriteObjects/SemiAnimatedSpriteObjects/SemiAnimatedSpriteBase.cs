using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Metal_Jewel.SpriteObjects.SpriteObjects;

namespace Metal_Jewel.SpriteObjects.SemiAnimatedSpriteObjects
{
    //Klassen kommer att användas som superklass för samtliga objekt som ska agera som någon form av bild och delvis vara animerad.
    public abstract class SemiAnimatedSpriteBase : SpriteBase
    {
        //Rektangel-array där varje rektangel ska föreställa en hållare för varje bild i en animation.
        protected Rectangle[] sourceRectangles;
        //FrameIndex-variabeln är till för att hålla reda på vilken bild i animationen som används för tillfället.
        //FrameWidth och frameHeight är till för att hålla reda på bildernas höjd och bredd i animationen.
        protected int frameIndex, frameWidth, frameHeight;

        //En rektangel kommer att skapas på objektet när man läsar av den här propertyn.
        public override Rectangle CollisionRectangle
        {
            get { return new Rectangle((int)(position.X - frameWidth / 2),
                (int)(position.Y - frameHeight / 2), frameWidth, frameHeight); }
        }

        //Vid anrop måste en contentManager, sökväg till textur, position, färg, skala, 
        //antalet bilder i animationen och vilken bild i animationen som ska målas upp anges.
        public SemiAnimatedSpriteBase(ContentManager contentManager, string texturePath, 
            Vector2 position, Color color, float scale, int frames, int frameIndex)
            : base(contentManager, texturePath, position, color, scale)
        {
            this.frameIndex = frameIndex;
            frameWidth = texture.Width / frames;
            frameHeight = texture.Height;
            sourceRectangles = new Rectangle[frames];
            for (int i = 0; i < frames; i++)
                sourceRectangles[i] = new Rectangle(i * frameWidth, 0, frameWidth, frameHeight);
        }

        //Metod för att måla upp vald bild i animationen. 
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangles[frameIndex], color, 0.0f,
                new Vector2(frameWidth/2, frameHeight/2), scale, SpriteEffects.None, 0.0f);
        }

        //Metoden är till för om man vill ändra bilden i animationen som ska målas upp 
        //(vilken bild man vill måla upp anger man som argument vid anrop).
        public void ChangeFrameTo(int frameIndex){ this.frameIndex = frameIndex; }
    }
}
