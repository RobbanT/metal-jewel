using Microsoft.Xna.Framework;
using Metal_Jewel.ManagerClasses;
using Metal_Jewel.ScreenClasses.MenuScreenObjects;

namespace Metal_Jewel
{
    //Detta �r klassen som h�ller spelet vid liv kan man s�ga. Den l�ser in content, uppdaterar spelets logik, 
    //och till sist m�las grafiken upp. Sedan k�rs spelloopen hela tiden(Update-metoden och Draw-metoden).
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        //Det h�r objektet kommer att hantera samtliga sk�rmar i spelet.
        private ScreenManager screenManager;
        //Variabel som h�ller v�rdet p� hur brett spelf�nstret �r i pixlar.
        public const int WindowWidth = 564;
        //Variabel som h�ller v�rdet p� hur h�gt spelf�nstret �r i pixlar.
        public const int WindowHeight = 406;
        
        public Game1()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphicsDeviceManager.PreferredBackBufferWidth = WindowWidth;
            graphicsDeviceManager.PreferredBackBufferHeight = WindowHeight;
            IsMouseVisible = true;
            screenManager = new ScreenManager(this);
            //En ny komponent l�ggs till.
            Components.Add(screenManager);
        }

        //Metod f�r s�dant som inte kunde g�ras i konstruktorn (t.ex. tilldelning till vissa variabler).
        protected override void Initialize()
        {
            //En ny sk�rm l�ggs till som screenManager-objektet ska hantera.
            screenManager.AddScreen(new MainMenuScreen(screenManager));
            //screenManager-objektets Initialize-metod k�rs.
            base.Initialize();
        }
    }
}
