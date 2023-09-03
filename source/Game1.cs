using Microsoft.Xna.Framework;
using Metal_Jewel.ManagerClasses;
using Metal_Jewel.ScreenClasses.MenuScreenObjects;

namespace Metal_Jewel
{
    //Detta är klassen som håller spelet vid liv kan man säga. Den läser in content, uppdaterar spelets logik, 
    //och till sist målas grafiken upp. Sedan körs spelloopen hela tiden(Update-metoden och Draw-metoden).
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        //Det här objektet kommer att hantera samtliga skärmar i spelet.
        private ScreenManager screenManager;
        //Variabel som håller värdet på hur brett spelfönstret är i pixlar.
        public const int WindowWidth = 564;
        //Variabel som håller värdet på hur högt spelfönstret är i pixlar.
        public const int WindowHeight = 406;
        
        public Game1()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphicsDeviceManager.PreferredBackBufferWidth = WindowWidth;
            graphicsDeviceManager.PreferredBackBufferHeight = WindowHeight;
            IsMouseVisible = true;
            screenManager = new ScreenManager(this);
            //En ny komponent läggs till.
            Components.Add(screenManager);
        }

        //Metod för sådant som inte kunde göras i konstruktorn (t.ex. tilldelning till vissa variabler).
        protected override void Initialize()
        {
            //En ny skärm läggs till som screenManager-objektet ska hantera.
            screenManager.AddScreen(new MainMenuScreen(screenManager));
            //screenManager-objektets Initialize-metod körs.
            base.Initialize();
        }
    }
}
