using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Metal_Jewel.ManagerClasses;

namespace Metal_Jewel.SpriteObjects.SpriteObjects
{
    //Klassen kommer att användas som superklass för samtliga objekt som ska agera som någon form av bild.
    public abstract class SpriteBase
    {
        //Texturen som objektet använder sig av när det målas upp.
        protected Texture2D texture;
        //Positionen som objektet kommer att använda sig av när det målas upp.
        protected Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; } }
        //Färgen som objektet kommer att använda sig av när det målas upp.
        protected Color color;
        public Color Color { get { return color; } set { color = value; } }
        //Skalan som objektet ska använda sig av när det målas upp.
        protected float scale;
        public float Scale { get { return scale; } set { scale = value; } }
        //En rektangel kommer att skapas på objektet när man läsar av den här propertyn.
        public virtual Rectangle CollisionRectangle
        {
            get { return new Rectangle((int)(position.X - texture.Width / 2),
                (int)(position.Y - texture.Height / 2), texture.Width, texture.Height); }
        }

        //Vid anrop måste en contentManager, sökväg till textur, position, färg och skala anges.
        public SpriteBase(ContentManager contentManager, string texturePath, Vector2 position, Color color, float scale)
        {
            texture = contentManager.Load<Texture2D>(texturePath);
            this.position = position;
            this.color = color;
            this.scale = scale;
        }

        //Metod för att måla upp objektet.
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color, 0.0f, 
                new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0.0f);
        }

        //Metod för att kolla om muspilen befinner sig på objektet.
        public bool Hovering(InputManager inputManager){ return inputManager.MousePositionOnObject(CollisionRectangle); }

        //Metod för att kolla om vänster musknapp är nedtryckt på objektet.
        public bool Pressed(InputManager inputManager){ return inputManager.LeftMouseButtonPressedOnObject(CollisionRectangle); }

        //Metod för att kolla om man har klickat på objektet med vänster musknapp.
        public bool HasBeenClicked(InputManager inputManager) { return inputManager.LeftMouseButtonClickedOnObject(CollisionRectangle); }
    }
}
