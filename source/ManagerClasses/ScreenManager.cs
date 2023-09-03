using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Metal_Jewel.ScreenClasses;
using Metal_Jewel.SpriteObjects.SpriteObjects;
using Metal_Jewel.ScreenClasses.ScreenObjects;

namespace Metal_Jewel.ManagerClasses
{
    //Den h�r klassen kommer att hantera samtliga sk�rmar(screen-objektet) i spelet.
    //Klassen �r f�r �vrigt en s� kallad komponent. Detta inneb�r bland annat att klassens Update- och Draw-metod kallas automatiskt fr�n basklassen).
    public sealed class ScreenManager : DrawableGameComponent
    {
        //I den h�r listan kommer alla sk�rmar att lagras.
        private List<Screen> screenList = new List<Screen>();
        //�ven i den h�r listan kommer samtliga sk�rmar att lagras, men bara tempor�rt. (detta till�ter mig bland annat att ta bort ett screen-objekt mitt i dess Update-metod.) 
        private List<Screen> tempScreenList = new List<Screen>();
        private SpriteBatch spriteBatch;
        //En ContentManager som samtliga screen-Objekt kommer att kunna anv�nda sig av.
        public ContentManager ContentManager { get { return Game.Content; } }
        //Objektet som kommer att hantera spelarens input.
        private InputManager inputManager;
        //De skrollande bakgrunderna (framgrunderna) som ska r�ra sig �ver samtliga sk�rmar.
        private ScrollingBackground[] scrollingBackgrounds = new ScrollingBackground[5];

        //Vi anrop m�ste man ange vilket Game-objekt som ska hantera det h�r objektet.
        public ScreenManager(Game game)
            : base (game)
        {
            inputManager = new InputManager();
        }

        //Metod f�r s�dant som inte kunde g�ras i konstruktorn (t.ex. tilldelning till vissa variabler).
        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.Initialize();
        }

        //Alla skrollande bakgrunder tilldelas ett objekt samtidigt som dess content l�ses in
        protected override void LoadContent()
        {
            for (int i = 0; i < scrollingBackgrounds.Length; i++)
                scrollingBackgrounds[i] = new ScrollingBackground(Game.Content, "Images/Common/ScrollBackground");
        }

        //Metod f�r att uppdatera ScreenManagern och alla dess Screen-objekt (samt de skrollande bakgrunderna).
        public override void Update(GameTime gameTime)
        {
            //Alla skrollande bakgrunder uppdateras f�rst.
            foreach (ScrollingBackground scrollingBackground in scrollingBackgrounds)
                scrollingBackground.Update(gameTime);

            //InputManager-objektet uppdateras.
            inputManager.Update();
            //Listan rensas i f�rberedelse f�r nya Screen-objekt som ska in i listan.
            tempScreenList.Clear();

            //Ny Screen-objekt l�ggs till i listan.
            foreach (Screen screen in screenList)
                tempScreenList.Add(screen);

            foreach (Screen screen in tempScreenList)
            {
                //�r Screen-objektet inte t�ckt av ett PopUpF�nster? D� ska man kunna t.ex. klicka p� Screen-objektets knappar. Annars ska man inte kunna g�ra det.
                if (!screen.CoveredByPopUp)
                    screen.HandleInput(inputManager);

                //K�rs Screen-objektet? D� k�rs Screen-objektets Update-metod. Annars g�r den inte det.
                if (screen.ScreenState == ScreenState.Running)
                    screen.Update(gameTime);
            }
        }

        //Metoden m�lar upp samtliga sk�rmar (samt de skrollande bakgrunderna).
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            //Alla sk�rmarna m�las upp.
            foreach (Screen screen in screenList)
                screen.Draw(spriteBatch);
            //Alla skrollande bakgrunder (framgrunder) m�las upp ovanf�r samtliga sk�rmar.
            foreach (ScrollingBackground scrollingBackground in scrollingBackgrounds)
                scrollingBackground.Draw(spriteBatch);
            spriteBatch.End();
        }

        //Metod f�r att l�gga till en sk�rm. Screen-objektet man vill l�gga till m�ste man ange vid metodanrop.
        public void AddScreen(Screen screen)
        {
            //N�r en ny sk�rm ska l�ggas till k�rs dess Initialize-metod.
            screen.Initialize();
            //Sk�rmen l�ggs till i listan.
            screenList.Add(screen);
        }

        //Metod f�r att ta bort en sk�rm. Screen-objektet man tab bort m�ste man ange vid metodanrop.
        public void RemoveScreen(Screen screen)
        {
            //Objektet tas bort fr�n listan.
            screenList.Remove(screen);
        }

        //Metod f�r att byta fr�n en sk�rm till en ny. Vilken sk�rm man vill l�gga till och ta bort m�ste man ange vid metodanrop.
        public void ChangeScreen(Screen newScreen, Screen oldScreen)
        {
            //Sk�rmen man ville byta fr�n tas bort.
            RemoveScreen(oldScreen);
            //Sk�rmen man ville byta till l�ggs till.
            AddScreen(newScreen);
        }

        //Metod f�r att l�gga till en ny sk�rm(popUp-f�nster). Vilket popUp-f�nster som ska l�ggas till m�ste man ange vid anrop.
        //Samt sk�rmen som kommer att befinna sig bakom popUp-f�nstret.
        public void AddPopUp(PopUpScreen popUpScreen, Screen screenBehindPopUp)
        {
            //Sk�rmen man ville byta till l�ggs till (ett popUp-f�nster.
            AddScreen(popUpScreen);
            //Sk�rmen som �r bakom popUpf�nstret f�r sin CoveredByPopUp-variabel satt till true
            //(f�r att markera att ett popUpf�nster �r ovanf�r den sk�rmen)
            screenBehindPopUp.CoverScreenWithPopUp();
            //Sk�rmen som befinner sig bakom popUp-f�nstret kommer dessutom inte att uppdateras l�ngre.
            screenBehindPopUp.PauseScreen();
        }

        //Metod f�r att ta bort en sk�rm(popUp-f�nster). Vilket popUp-f�nster som ska tas bort m�ste man ange vid anrop.
        public void RemovePopUp(PopUpScreen popUpScreen)
        {
            //PopUp-f�nstret tas bort.
            RemoveScreen(popUpScreen);
            //Listan med alla sk�rmar loopas igenom f�r att hitta sk�rmen som befann sig bakom popUp-sk�rmen.
            foreach (Screen screen in screenList)
            {
                //�r/Var sk�rmen t�ckt av ett pupUp-f�nster? D� ska dess Update-metod k�ras �nnu en g�ng.
                if (screen.CoveredByPopUp)
                {
                    screen.UnCoverScreenWithPopUp();
                    screen.RunScreen();
                }
            }
        }

        //Metod f�r att ta bort samtliga sk�rmar.
        public void RemoveAllScreens()
        {
            //Listan med sk�rmarna rensas.
            screenList.Clear();
        }
    }
}
