using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Metal_Jewel.SpriteObjects.SpriteObjects;
using Metal_Jewel.SpriteObjects.AnimatedSpriteObjects;
using Metal_Jewel.Effects;
using Metal_Jewel.ManagerClasses;

namespace Metal_Jewel.ScreenClasses.MenuScreenObjects
{
    //Detta är huvumenyskärmen i spelet.
    public sealed class MainMenuScreen : MenuScreen
    {
        //Två juveler ska målas upp (bara för utseendets skull).
        private Sprite leftJewel, rightJewel;

        //Vid anrop måste man ange vilken screenManager som ska hantera screen-objektet.
        public MainMenuScreen(ScreenManager screenManager)
            : base (screenManager) 
        {
        }

        //Alla variabler som behöver läsa in någon form av content tilldelas ett objekt samtidigt som dess content läses in.
        protected override void LoadContent()
        {
            background = new Sprite(screenManager.ContentManager, "Images/MainMenuScreen/MainMenuBackground", new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2));
            buttonList.Add(new Button(screenManager.ContentManager, "Images/Common/BigButton", new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2 - 22),
                "Images/Common/BigButtonShade", "Fonts/Font", "Play"));
            buttonList.Add(new Button(screenManager.ContentManager, "Images/Common/BigButton", new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2 + 58),
                "Images/Common/BigButtonShade", "Fonts/Font", "Hi-Score"));
            buttonList.Add(new Button(screenManager.ContentManager, "Images/Common/BigButton", new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2 + 138),
                "Images/Common/BigButtonShade", "Fonts/Font", "Quit"));
            leftJewel = new Sprite(screenManager.ContentManager, "Images/MainMenuScreen/" + (JewelColor)EffectBase.Random.Next(5) + "JewelConcave", new Vector2(75, 75));
            rightJewel = new Sprite(screenManager.ContentManager, "Images/MainMenuScreen/" + (JewelColor)EffectBase.Random.Next(5) + "JewelConcave", new Vector2(489, 75));
        }

        //Vad som händer när spelaren klickar på en knapp avgörs hos den här metoden med hjälp av värdet på selected-variabeln.
        public override void HandleInput(InputManager inputManager)
        {
            base.HandleInput(inputManager);
            switch (selected)
            {
                //Klickade spelaren på Play-knappen?
                case "Play":
                    screenManager.ChangeScreen(new GameScreen(screenManager), this);
                    break;
                //Klickade spelaren på Hi-Score-knappen
                case "Hi-Score":
                    screenManager.ChangeScreen(new HiScoreScreen(screenManager), this);
                    break;
                //Klickade spelaren på Quit-knappen
                case "Quit":
                    screenManager.Game.Exit();
                    break;
            }
        }

        //Metoden målar upp det som är unikt för det här screen-objektet men även dess knappar och bakgrund.
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Bakgrund och knappar målas först upp.
            base.Draw(spriteBatch);
            //De två juvelerna målas sedan upp.
            leftJewel.Draw(spriteBatch);
            rightJewel.Draw(spriteBatch);
        }
    }
}
