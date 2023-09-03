using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Metal_Jewel.ManagerClasses;
using Metal_Jewel.SpriteObjects.SpriteObjects;
using Metal_Jewel.ScreenClasses.MenuScreenObjects;

namespace Metal_Jewel.ScreenClasses.MenuScreenObjects
{
    //Detta är skärmen där spelaren spelar själva spelet.
    public class GameScreen : MenuScreen
    {
        //Bool som håller reda på om skärmen är initierad.
        private bool gameScreenInitialized;
        //SpriteFont för att skriva ut poäng och tid.
        private SpriteFont spriteFont;
        //Timer som kommer att räkna ned (den börjar på 1 minut).
        private TimeSpan countDownTimer = new TimeSpan(0, 1, 0);
        //Detta är objektet som kommer att hantera alla juveler som ska användas.
        private JewelManager jewelManager;
        //Spelarens poäng
        private int score = 0;
        //Filhanterare som kommer att användas när spelaren poäng ska sparas.
        private FileManager fileManager = new FileManager();

        //Vid anrop måste man ange vilken screenManager som ska hantera screen-objektet.
        public GameScreen(ScreenManager screenManager)
            : base(screenManager) 
        {
            jewelManager = new JewelManager(screenManager.ContentManager, 8, 8);
        }

        //Metoden kallar först och främst på LoadContent-metoden men metoden kommer även att användas 
        //för att initiera variabler som inte kunde initieras i konstruktorn.
        public override void Initialize()
        {
            //Är skärmen redan initierad? Då initieras bara en ny jewelManager.
            if (!gameScreenInitialized)
            {
                base.Initialize();
                gameScreenInitialized = true;
            }
            jewelManager.Initialize();
        }

        //Alla variabler som behöver läsa in någon form av content tilldelas ett objekt samtidigt som dess content läses in.
        protected override void LoadContent()
        {
                background = new Sprite(screenManager.ContentManager, "Images/GameScreen/GameScreenBackground", 
                    new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2));
                buttonList.Add(new Button(screenManager.ContentManager, "Images/Common/SmallButton", 
                    new Vector2(96, Game1.WindowHeight / 2 + 149), "Images/Common/SmallButtonShade", "Fonts/Font", "Menu", 0.65f));
                spriteFont = screenManager.ContentManager.Load<SpriteFont>("Fonts/Font");
        }

        //Vad som händer när spelaren klickar på en knapp/juvel avgörs hos den här metoden.
        public override void HandleInput(InputManager inputManager)
        {
            base.HandleInput(inputManager);
            //Klickade spelaren på Menu-knappen?
            if (selected == "Menu")
            {
                screenManager.AddPopUp(new PauseScreen(screenManager), this);
                //Efter att spelaren har tryckt på en knapp så ska selected alltid bli null
                //(Detta är så att knappens funktion inte upprepar sig).
                selected = null;
                return;
            }
            //Input hos alla juveler kontrolleras.
            jewelManager.HandleInput(inputManager);
        }

        //Metoden uppdatera poängen, timern och juvelerna.
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Poängen uppdateras.
            score += jewelManager.TempScore;
            //Timern uppdateras.
            countDownTimer += TimeSpan.FromSeconds(jewelManager.TempTime);
            countDownTimer -= gameTime.ElapsedGameTime;
            //Har timern blivit 0 eller mindre? Då blir det Game Over.
            if (countDownTimer.TotalSeconds <= 0)
            {
                fileManager.WriteToFile(score.ToString(), "highScores.txt");
                screenManager.AddPopUp(new GameOverScreen(screenManager), this);
                return;
            }
            //Juvelhanteraren uppdateras.
            jewelManager.Update(gameTime);

            //Kan spelaren inte flytta någon mer juvel? Då ska nya juveler slumpas fram.
            if (!jewelManager.PossibleMoves)
            {
                jewelManager = new JewelManager(screenManager.ContentManager, 8, 8);
                Initialize();
            }
        }

        //Metoden målar upp det som är unikt för det här screen-objektet men även dess knappar och bakgrund.
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Bakgrund och knappar målas först upp.
            base.Draw(spriteBatch);
            //Poäng målas upp.
            spriteBatch.DrawString(spriteFont, score.ToString(), new Vector2(50, 49), Color.White, 0.0f, 
                Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
            //Timern målas upp.
            spriteBatch.DrawString(spriteFont, countDownTimer.ToString(@"hh\:mm\:ss"), new Vector2(51, 108), Color.White, 0.0f, 
                Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
            //Juvelerna målas upp.
            jewelManager.Draw(spriteBatch);
        }
    }
}
