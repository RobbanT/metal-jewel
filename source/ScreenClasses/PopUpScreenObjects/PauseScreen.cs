using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Metal_Jewel.ManagerClasses;
using Metal_Jewel.SpriteObjects.SpriteObjects;
using Metal_Jewel.ScreenClasses.MenuScreenObjects;
using Metal_Jewel.Effects;

namespace Metal_Jewel.ScreenClasses
{
    //Detta är pausskärmen som kommer att visas i själva spelet (när det pausas).
    public class PauseScreen : PopUpScreen
    {
        //Vid anrop måste man ange vilken screenManager som ska hantera screen-objektet.
        public PauseScreen(ScreenManager screenManager)
            : base(screenManager)
        {
        }

        //Alla variabler som behöver läsa in någon form av content tilldelas ett objekt samtidigt som dess content läses in.
        protected override void LoadContent()
        {
            background = new Sprite(screenManager.ContentManager, "Images/PauseScreen/PauseScreenBackground", new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2));
            backgroundShade = new Sprite(screenManager.ContentManager, "Images/Common/backgroundShade", new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2));
            buttonList.Add(new Button(screenManager.ContentManager, "Images/Common/SmallButton", new Vector2(164, Game1.WindowHeight / 2 + 48),
                "Images/Common/SmallButtonShade", "Fonts/Font", "New Game", 0.5f));
            buttonList.Add(new Button(screenManager.ContentManager, "Images/Common/SmallButton", new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2 + 48),
                "Images/Common/SmallButtonShade", "Fonts/Font", "Resume", 0.5f));
            buttonList.Add(new Button(screenManager.ContentManager, "Images/Common/SmallButton", new Vector2(400, Game1.WindowHeight / 2 + 48),
                "Images/Common/SmallButtonShade", "Fonts/Font", "Menu", 0.5f));  
        }

        //Vad som händer när spelaren klickar på en knapp avgörs hos den här metoden med hjälp av värdet på selected-variabeln.
        public override void HandleInput(InputManager inputManager)
        {
            base.HandleInput(inputManager);
            switch (selected)
            {
                //Klickade spelaren på New Game-knappen?
                case "New Game":
                    //Har spelaren tryckt på New Game-knappen så ska inget hända innan bakgrundsskuggan har blivit helt genomskinlig.
                    if (alphaAnimationEffect.EffectStatus == EffectStatus.EffectAtMin)
                    {
                        screenManager.RemoveAllScreens();
                        screenManager.AddScreen(new GameScreen(screenManager));
                    }
                    break;
                //Klickade spelaren på Resume-knappen?
                case "Resume":
                    //Har spelaren tryckt på Resume-knappen så ska inget hända innan bakgrundsskuggan har blivit helt genomskinlig.
                    if (alphaAnimationEffect.EffectStatus == EffectStatus.EffectAtMin)
                        screenManager.RemovePopUp(this);
                    break;
                //Klickade spelaren på Menu-knappen?
                case "Menu":
                    screenManager.RemoveAllScreens();
                    screenManager.AddScreen(new MainMenuScreen(screenManager));
                    break;
            }
        }
    }
}
