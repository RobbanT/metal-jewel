using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Metal_Jewel.ManagerClasses;

namespace Metal_Jewel.SpriteObjects.SpriteObjects
{
    //Det här objektet kommer att användas för att agera som en knapp. Klickar man t.ex. på den så ska något hända.
    public sealed class Button : SpriteBase
    {
        //Knappens bakgrundsskugga (bara för utseendets skull).
        private Sprite buttonShade;
        //Knappens font.
        private SpriteFont spriteFont;
        //Texten som ska stå på knappen.
        public string ButtonText { get; private set; }
        //Skala för knappens text.
        private float buttonTextScale;

        //Konstruktor för simpla objekt 
        //(endast contentManager, texturePath, position, buttonShadePath, spriteFontPath och buttonText behövs anges.
        //Resten av baskonstruktorns parametrar får ett standardvärde.)
        public Button (ContentManager contentManager, string buttonPath, Vector2 position, string buttonShadePath,
            string spriteFontPath, string buttonText, float buttonTextScale = 1.0f)
            : this(contentManager, buttonPath, position, Color.White, 1.0f, buttonShadePath, spriteFontPath, buttonText)
        {
            this.buttonTextScale = buttonTextScale;
        }

        //Konstruktor för avancerade objekt (ett värde till alla parametrar i baskonstruktorn och den här konstruktorn behövs anges).
        public Button (ContentManager contentManager, string buttonPath, Vector2 position, Color color, float scale,
            string buttonShadePath, string spriteFontPath, string buttonText)
            : base(contentManager, buttonPath, position, Color.White, scale)
        {
            buttonShade = new Sprite(contentManager, buttonShadePath, new Vector2(position.X, position.Y + 2), Color.White, scale);
            spriteFont = contentManager.Load<SpriteFont>(spriteFontPath);
            this.ButtonText = buttonText;
        }

        //Metoden ser till att knappens skugga alltid har samma skala som button-objektet.
        public void Update(GameTime gameTime)
        {
            buttonShade.Scale = scale;
        }

        //Metoden målar upp knappen.
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Knappens skugga målas först upp.
            buttonShade.Draw(spriteBatch);
            //Själva knappobjektet målas sedan upp.
            base.Draw(spriteBatch);
            //Till sist målas knappens text upp.
            spriteBatch.DrawString(spriteFont, ButtonText, position, Color.White, 0f,
                spriteFont.MeasureString(ButtonText) / 2, buttonTextScale, SpriteEffects.None, 0.0f);
        }

        //Metod för att kolla input på knappen. Har spelaren t.ex. muspilen på knappen?
        public void HandleInput(InputManager inputManager, ref string selected)
        {
            //Har spelare klickat på en knapp? Då tilldelas selected-variabeln dess text.
            if(HasBeenClicked(inputManager))
            {
                selected = ButtonText;            
                DefaultState();
                return;
            }

            //Har spelare inte muspilen på knappen?
            if (!Hovering(inputManager))
            {
                DefaultState();
                return;
            }

            //Har spelaren muspilen på knappen samtidigt som vänster musknapp är nedtryckt?
            if (Pressed(inputManager))
            {
                PressedState();
                return;
            }

            //Har spelaren muspilen på knappen?
            if (Hovering(inputManager))
                HoveringState();
        }

        //Metod som kallas när spelaren inte har muspilen över knappen.
        public void DefaultState()
        {
            buttonShade.Color = Color.White;
            color = Color.White;
        }
        //Metod som kallas när spelaren har musen över knappen samtidigt som en musknapp är nedtryckt.
        public void PressedState()
        {
            buttonShade.Color = Color.Transparent;
            color = Color.White;
        }
        //Metod som kallas när spelaren har musen över knappen.
        public void HoveringState()
        {
            buttonShade.Color = Color.White;
            color = Color.LightBlue;
        }
    }
}
