using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Metal_Jewel.ManagerClasses;
using Metal_Jewel.SpriteObjects.SpriteObjects;

namespace Metal_Jewel.ScreenClasses.ScreenObjects
{
    //Enum för alla status som en skärm kan ha. Körs objektet eller inte?
    public enum ScreenState { Running, Paused }
    
    //Detta är superklassen för samtliga skärm-objekt.
    public abstract class Screen
    {
        //Variabel som håller reda på statuset som screen-objektet har.
        public ScreenState ScreenState { get; private set; }
        //Variabel som gör det möjligt för alla screen-objekt att använda sig av sitt screenManager-objekt.
        protected ScreenManager screenManager { get; private set; }
        //Variabel som hållera reda på om screen-objektet är täckt av ett popUp-fönster.
        public bool CoveredByPopUp { get; private set; }
        //Alla screen-objekt kommer att behöva ha någon form av bakgrund.
        protected Sprite background;

        //Vid anrop måste man ange vilken screenManager som ska hantera screen-objektet.
        public Screen(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
        }

        //Metoden kallar först och främst på LoadContent-metoden men metoden kommer även att användas 
        //för att initiera variabler som inte kunde initieras i konstruktorn.
        public virtual void Initialize(){ LoadContent(); }

        //Alla subklassen kommer att behöva implementera en LoadContent-metod.
        protected abstract void LoadContent();

        //Alla subklassen kommer att behöva implementera en HandleImput-metod.
        public abstract void HandleInput(InputManager inputManager);

        //Alla subklassen kommer att behöva implementera en Update-metod.
        public abstract void Update(GameTime gameTime);

        //Metoden målar upp bakgrunden för screen-objektet.
        public virtual void Draw(SpriteBatch spriteBatch){ background.Draw(spriteBatch); }

        //Metod som används till att markera att skärmen är täckt av ett PopUp-fönster.
        public void CoverScreenWithPopUp(){ CoveredByPopUp = true; }

        //Metod som används till att markera att skärmen inte längre är täckt av ett PopUp-fönster.
        public void UnCoverScreenWithPopUp(){ CoveredByPopUp = false; }

        //Metod för om man vill pausa screen-objektet.
        public void PauseScreen(){ ScreenState = ScreenState.Paused; }

        //Metod för om man vill köra screen-objektet.
        public void RunScreen(){ ScreenState = ScreenState.Running; }
    }
}
