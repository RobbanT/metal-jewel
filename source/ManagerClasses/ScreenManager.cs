using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Metal_Jewel.ScreenClasses;
using Metal_Jewel.SpriteObjects.SpriteObjects;
using Metal_Jewel.ScreenClasses.ScreenObjects;

namespace Metal_Jewel.ManagerClasses
{
    //Den här klassen kommer att hantera samtliga skärmar(screen-objektet) i spelet.
    //Klassen är för övrigt en så kallad komponent. Detta innebär bland annat att klassens Update- och Draw-metod kallas automatiskt från basklassen).
    public sealed class ScreenManager : DrawableGameComponent
    {
        //I den här listan kommer alla skärmar att lagras.
        private List<Screen> screenList = new List<Screen>();
        //Även i den här listan kommer samtliga skärmar att lagras, men bara temporärt. (detta tillåter mig bland annat att ta bort ett screen-objekt mitt i dess Update-metod.) 
        private List<Screen> tempScreenList = new List<Screen>();
        private SpriteBatch spriteBatch;
        //En ContentManager som samtliga screen-Objekt kommer att kunna använda sig av.
        public ContentManager ContentManager { get { return Game.Content; } }
        //Objektet som kommer att hantera spelarens input.
        private InputManager inputManager;
        //De skrollande bakgrunderna (framgrunderna) som ska röra sig över samtliga skärmar.
        private ScrollingBackground[] scrollingBackgrounds = new ScrollingBackground[5];

        //Vi anrop måste man ange vilket Game-objekt som ska hantera det här objektet.
        public ScreenManager(Game game)
            : base (game)
        {
            inputManager = new InputManager();
        }

        //Metod för sådant som inte kunde göras i konstruktorn (t.ex. tilldelning till vissa variabler).
        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.Initialize();
        }

        //Alla skrollande bakgrunder tilldelas ett objekt samtidigt som dess content läses in
        protected override void LoadContent()
        {
            for (int i = 0; i < scrollingBackgrounds.Length; i++)
                scrollingBackgrounds[i] = new ScrollingBackground(Game.Content, "Images/Common/ScrollBackground");
        }

        //Metod för att uppdatera ScreenManagern och alla dess Screen-objekt (samt de skrollande bakgrunderna).
        public override void Update(GameTime gameTime)
        {
            //Alla skrollande bakgrunder uppdateras först.
            foreach (ScrollingBackground scrollingBackground in scrollingBackgrounds)
                scrollingBackground.Update(gameTime);

            //InputManager-objektet uppdateras.
            inputManager.Update();
            //Listan rensas i förberedelse för nya Screen-objekt som ska in i listan.
            tempScreenList.Clear();

            //Ny Screen-objekt läggs till i listan.
            foreach (Screen screen in screenList)
                tempScreenList.Add(screen);

            foreach (Screen screen in tempScreenList)
            {
                //Är Screen-objektet inte täckt av ett PopUpFönster? Då ska man kunna t.ex. klicka på Screen-objektets knappar. Annars ska man inte kunna göra det.
                if (!screen.CoveredByPopUp)
                    screen.HandleInput(inputManager);

                //Körs Screen-objektet? Då körs Screen-objektets Update-metod. Annars gör den inte det.
                if (screen.ScreenState == ScreenState.Running)
                    screen.Update(gameTime);
            }
        }

        //Metoden målar upp samtliga skärmar (samt de skrollande bakgrunderna).
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            //Alla skärmarna målas upp.
            foreach (Screen screen in screenList)
                screen.Draw(spriteBatch);
            //Alla skrollande bakgrunder (framgrunder) målas upp ovanför samtliga skärmar.
            foreach (ScrollingBackground scrollingBackground in scrollingBackgrounds)
                scrollingBackground.Draw(spriteBatch);
            spriteBatch.End();
        }

        //Metod för att lägga till en skärm. Screen-objektet man vill lägga till måste man ange vid metodanrop.
        public void AddScreen(Screen screen)
        {
            //När en ny skärm ska läggas till körs dess Initialize-metod.
            screen.Initialize();
            //Skärmen läggs till i listan.
            screenList.Add(screen);
        }

        //Metod för att ta bort en skärm. Screen-objektet man tab bort måste man ange vid metodanrop.
        public void RemoveScreen(Screen screen)
        {
            //Objektet tas bort från listan.
            screenList.Remove(screen);
        }

        //Metod för att byta från en skärm till en ny. Vilken skärm man vill lägga till och ta bort måste man ange vid metodanrop.
        public void ChangeScreen(Screen newScreen, Screen oldScreen)
        {
            //Skärmen man ville byta från tas bort.
            RemoveScreen(oldScreen);
            //Skärmen man ville byta till läggs till.
            AddScreen(newScreen);
        }

        //Metod för att lägga till en ny skärm(popUp-fönster). Vilket popUp-fönster som ska läggas till måste man ange vid anrop.
        //Samt skärmen som kommer att befinna sig bakom popUp-fönstret.
        public void AddPopUp(PopUpScreen popUpScreen, Screen screenBehindPopUp)
        {
            //Skärmen man ville byta till läggs till (ett popUp-fönster.
            AddScreen(popUpScreen);
            //Skärmen som är bakom popUpfönstret får sin CoveredByPopUp-variabel satt till true
            //(för att markera att ett popUpfönster är ovanför den skärmen)
            screenBehindPopUp.CoverScreenWithPopUp();
            //Skärmen som befinner sig bakom popUp-fönstret kommer dessutom inte att uppdateras längre.
            screenBehindPopUp.PauseScreen();
        }

        //Metod för att ta bort en skärm(popUp-fönster). Vilket popUp-fönster som ska tas bort måste man ange vid anrop.
        public void RemovePopUp(PopUpScreen popUpScreen)
        {
            //PopUp-fönstret tas bort.
            RemoveScreen(popUpScreen);
            //Listan med alla skärmar loopas igenom för att hitta skärmen som befann sig bakom popUp-skärmen.
            foreach (Screen screen in screenList)
            {
                //Är/Var skärmen täckt av ett pupUp-fönster? Då ska dess Update-metod köras ännu en gång.
                if (screen.CoveredByPopUp)
                {
                    screen.UnCoverScreenWithPopUp();
                    screen.RunScreen();
                }
            }
        }

        //Metod för att ta bort samtliga skärmar.
        public void RemoveAllScreens()
        {
            //Listan med skärmarna rensas.
            screenList.Clear();
        }
    }
}
