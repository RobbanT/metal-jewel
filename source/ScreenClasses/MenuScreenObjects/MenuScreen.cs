using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Metal_Jewel.SpriteObjects.SpriteObjects;
using Metal_Jewel.ManagerClasses;
using Metal_Jewel.ScreenClasses.ScreenObjects;

namespace Metal_Jewel.ScreenClasses.MenuScreenObjects
{
    //Detta är superklassen för samtliga screen-objekt som kommer att behöva någon form av meny (knappar).
    public abstract class MenuScreen : Screen
    {
        //En lista som kommer att innehålla alla button-objekt. 
        protected List<Button> buttonList = new List<Button>();
        //Den här variabeln håller reda på vilken knapp som spelaren har tryckt på.
        protected string selected;

        //Vid anrop måste man ange vilken screenManager som ska hantera screen-objektet.
        public MenuScreen(ScreenManager screenManager)
            : base(screenManager)
        {
        }

        //Metod som läsar av input på knapparna. Har spelaren t.ex. tryckt på en knapp?
        public override void HandleInput(InputManager inputManager)
        {
            foreach (Button button in buttonList)
                button.HandleInput(inputManager, ref selected);
        }

        //Metoden kör alla Button-objektens Update-metod.
        public override void Update(GameTime gameTime)
        {
            foreach (Button button in buttonList)
                button.Update(gameTime);
        }

        //Metoden målar upp knapparna och kallar även på basklassen Draw-metod.
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Bakgrunden målas upp först.
            base.Draw(spriteBatch);
            //Knapparna målas sedan upp.
            foreach (Button button in buttonList)
                button.Draw(spriteBatch);
        }
    }
}
