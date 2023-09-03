using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Metal_Jewel.ManagerClasses;
using Metal_Jewel.SpriteObjects.SpriteObjects;
using Metal_Jewel.ScreenClasses.MenuScreenObjects;
using Metal_Jewel.Effects;

namespace Metal_Jewel.ScreenClasses
{
    //Detta är GameOverskärmen som kommer att visas i spelet när spelarens tid har tagit slut.
    public class GameOverScreen : PopUpScreen
    {
        //Vid anrop måste man ange vilken screenManager som ska hantera screen-objektet.
        public GameOverScreen(ScreenManager screenManager)
            : base(screenManager)
        {
        }

        //Alla variabler som behöver läsa in någon form av content tilldelas ett objekt samtidigt som dess content läses in.
        protected override void LoadContent()
        {
            background = new Sprite(screenManager.ContentManager, "Images/GameOverScreen/GameOverScreenBackground", new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2));
            backgroundShade = new Sprite(screenManager.ContentManager, "Images/Common/backgroundShade", new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2));
            buttonList.Add(new Button(screenManager.ContentManager, "Images/Common/SmallButton", new Vector2(212, Game1.WindowHeight / 2 + 48),
                "Images/Common/SmallButtonShade", "Fonts/Font", "Yes", 0.65f));
            buttonList.Add(new Button(screenManager.ContentManager, "Images/Common/SmallButton", new Vector2(352, Game1.WindowHeight / 2 + 48),
                "Images/Common/SmallButtonShade", "Fonts/Font", "No", 0.65f));
        }

        //Vad som händer när spelaren klickar på en knapp avgörs hos den här metoden med hjälp av värdet på selected-variabeln.
        public override void HandleInput(InputManager inputManager)
        {
            base.HandleInput(inputManager);
            switch (selected)
            {
                //Klickade spelaren på Yes-knappen?
                case "Yes":
                    //Har spelaren tryckt på Yes-knappen så ska inget hända innan bakgrundsskuggan har blivit helt genomskinlig.
                    if (alphaAnimationEffect.EffectStatus == EffectStatus.EffectAtMin)
                    {
                        screenManager.RemoveAllScreens();
                        screenManager.AddScreen(new GameScreen(screenManager));
                    }
                    break;
                //Klickade spelaren på No-knappen?
                case "No":
                    screenManager.RemoveAllScreens();
                    screenManager.AddScreen(new MainMenuScreen(screenManager));
                    break;
            }

        }
    }
}
