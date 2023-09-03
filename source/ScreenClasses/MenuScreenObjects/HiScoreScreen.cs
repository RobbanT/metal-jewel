using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Metal_Jewel.ManagerClasses;
using Metal_Jewel.SpriteObjects.SpriteObjects;
using Metal_Jewel.ScreenClasses.MenuScreenObjects;
using System;

namespace Metal_Jewel.ScreenClasses.MenuScreenObjects
{
    //Detta är skärmen där spelaren kommer att kunna se High Score-listan.
    public sealed class HiScoreScreen : MenuScreen
    {
        //Detta objektet kommer att användas för att läsa in poängen från en fil.
        private FileManager fileManager = new FileManager();
        //Listan kommer att användas som hållare för poängen som finns i filen.
        private List<int>highScores = new List<int>();
        //SpriteFont för att skriva ut poängen.
        private SpriteFont spriteFont;

        //Vid anrop måste man ange vilken screenManager som ska hantera screen-objektet.
        public HiScoreScreen(ScreenManager screenManager)
            : base (screenManager)
        {
            //Filen läses in och highScores-listan tilldelas alla poäng.
            highScores = fileManager.ReadFromFile("highScores.txt").ConvertAll<int>(Convert.ToInt32);
            //Listan sorteras(minsta värdet först).
            highScores.Sort();
            //Elementen i listan byter plats (Största elementen först och minsta elementen sist).
            highScores.Reverse();
        }

        //Alla variabler som behöver läsa in någon form av content tilldelas ett objekt samtidigt som dess content läses in.
        protected override void LoadContent()
        {
            background = new Sprite(screenManager.ContentManager, "Images/Hi-ScoreScreen/Hi-ScoreScreenBackground", new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2));
            buttonList.Add(new Button(screenManager.ContentManager, "Images/Common/SmallButton", new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2 + 149),
                "Images/Common/SmallButtonShade", "Fonts/Font", "Back", 0.65f));
            spriteFont = screenManager.ContentManager.Load<SpriteFont>("Fonts/Font");
        }

        //Vad som händer när spelaren klickar på en knapp avgörs hos den här metoden med hjälp av värdet på selected-variabeln.
        public override void HandleInput(InputManager inputManager)
        {
            base.HandleInput(inputManager);
            //Klickade spelaren på Back-knappen?
            if(selected == "Back")
                screenManager.ChangeScreen(new MainMenuScreen(screenManager), this);
        }

        //Metoden målar upp det som är unikt för det här screen-objektet men även dess knappar och bakgrund.
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Bakgrund och knappar målas först upp.
            base.Draw(spriteBatch);
            //Texten med de fyra högsta poängen målas sedan upp.
            spriteBatch.DrawString(spriteFont, "1ST: " + highScores[0], new Vector2(70, 107), Color.White);
            spriteBatch.DrawString(spriteFont, "2ND: " + highScores[1], new Vector2(70, 161), Color.White);
            spriteBatch.DrawString(spriteFont, "3RD: " + highScores[2],  new Vector2(70, 215), Color.White);
            spriteBatch.DrawString(spriteFont, "4TH: " + highScores[3], new Vector2(70, 269), Color.White);
        }
    }
}
