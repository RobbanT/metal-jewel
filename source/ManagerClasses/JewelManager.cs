using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Metal_Jewel.SpriteObjects.AnimatedSpriteObjects;
using Metal_Jewel.SpriteFontObjects;
using Metal_Jewel.Effects;

namespace Metal_Jewel.ManagerClasses
{
    //Detta är objektet som kommer att hantera alla jewel-objekt.
    public class JewelManager
    {
        //Bool som kontrollerar om jewelManagern har kört sin LoadContent-metod.
        private bool jewelManagerLoaded;
        //Objektet kommer hela tiden att ha en contentManager så att nya juveler kan läsas in vid behov.
        private ContentManager contentManager;
        //I den här arrayen kommer alla jewel-objekt att lagras.
        private Jewel[,] jewels;
        //SpriteFont för de tillfälliga poängen som ska målas upp.
        SpriteFont spriteFont;
        //Variabel som håller reda på poängen som spelaren får för sammanhängande juveler.
        public int TempScore { get; private set; }
        //Variabel som håller reda på tiden som spelaren får för sammanhängande juveler.
        public int TempTime { get; private set; }
        //Lista för de tillfälliga poängen som målas upp när en juvel tas bort.
        private List<FloatingSpriteFont> floatingScores = new List<FloatingSpriteFont>();
        //Bool som håller reda på om spelaren kan flytta någon juvel.
        public bool PossibleMoves { get; private set; }

        //Vid anrop måste man ange en contentmanager och hur många juveler man vill att klassen ska hantera.
        public JewelManager(ContentManager contentManager, int numberOfJewelsHorizontal, int numberOfJewelsVertical)
        {
            this.contentManager = contentManager;
            jewels = new Jewel[numberOfJewelsHorizontal, numberOfJewelsVertical];
        }

        //Metoden kallar först och främst på LoadContent-metoden men metoden kommer även att användas 
        //för att initiera variabler som inte kunde initieras i konstruktorn.
        public void Initialize()
        {
            LoadContent();
            jewelManagerLoaded = true;
        }

        //Alla variabler som behöver läsa in någon form av content tilldelas ett objekt samtidigt som dess content läses in.
        public void LoadContent()
        {
            //Är jewelManagern redan laddad? Då laddas bara nya juveler fram som ska falla ned.
            if (!jewelManagerLoaded)
            {
                spriteFont = contentManager.Load<SpriteFont>("Fonts/Font");
                for (int x = 0; x < jewels.GetLength(0); x++)
                    for (int y = 0; y < jewels.GetLength(1); y++)
                    {
                        jewels[x, y] = new Jewel(contentManager, (JewelColor)EffectBase.Random.Next(5), new Vector2(213 + 42 * x, (56 + 42 * y) - Game1.WindowHeight),
                             new Vector2(213 + 42 * x, 56 + 42 * y), new Vector2(0, 10.0f + 1.0f * x + y), EffectStatus.EffectAtMin);
                        //En kontroll sker så att det inte ligger 3 sammanhängande juveler någonstans. Gör det det så slumpas en ny juvel fram.
                        if (checkConnectedJewels(x, y, jewels[x, y].JewelColor) >= 4)
                            y--;
                        //Alla juveler avmarkeras så att de kan kontrolleras igen.
                        uncheckJewels();
                    }
                //Alla juveler faller ner till sin plats.
                moveAllJewelsForward();
            }
            else//Juveler som har blivit borttagna ersätts med en ny.
                for (int x = 0; x < jewels.GetLength(0); x++)
                    for (int y = 0; y < jewels.GetLength(1); y++)
                        if (jewels[x, y] == null)
                            jewels[x, y] = new Jewel(contentManager, (JewelColor)EffectBase.Random.Next(5), new Vector2(213 + 42 * x, (56 + 42 * y) - Game1.WindowHeight),
                                new Vector2(213 + 42 * x, 56 + 42 * y), new Vector2(0, 5), EffectStatus.IncreaseEffect);


        }

        //Metod som avgör vad som händer om någon t.ex. klickar på en juvel.
        public void HandleInput(InputManager inputManager)
        {
            int selectedJewel = 0;
            foreach (Jewel jewel in jewels)
            {
                jewel.HandleInput(inputManager);
                //Har den här juvelen markerats av spelaren?
                if (jewel.Selected)
                    selectedJewel++;
            }
            //Har två juveler markerats av spelaren? Då ska dessa juveler eventuellt byta plats.
            if (selectedJewel == 2)
                swapJewels(inputManager);
        }

        //Metoden uppdaterar samtliga juveler, och temporär poäng och tid.
        public void Update(GameTime gameTime)
        {
            TempTime = 0;
            TempScore = 0;
            PossibleMoves = false;

            //Alla juveler uppdateras.
            foreach (Jewel jewel in jewels)
                jewel.Update(gameTime);
                //Juveler som har fått juvelen nedanför sig själv borttagen faller ner (eventuellt).
                fall();
                //Nya juveler som eventuellt behövs får sin content laddad och börjar sedan falla till sin plats.
                LoadContent();
            
            //Kontroll av juveler.
            for (int x = 0; x < jewels.GetLength(0); x++)
                for (int y = 0; y < jewels.GetLength(1); y++)
                {
                    int connectedJewels = checkConnectedJewels(x, y, jewels[x, y].JewelColor);
                    //Har den här juvelen tre eller fler juveler i samma färg anslutna till sig? Då ska dessa juveler tas bort.
                    //Poäng och tid ska dessutom ändras också.
                    if (connectedJewels >= 4)
                    {
                        prepareRemovalOfConnectedJewels(connectedJewels);
                        TempScore += connectedJewels * 10 * connectedJewels;
                        TempTime += connectedJewels;
                    }
                    uncheckJewels();

                    //Här sker en flyttningskontroll på juvelen (om den går att flytta till vänster, höger, upp eller ner).
                    if (x < jewels.GetLength(0) - 1) checkPossibleMoves(x, y, x + 1, y);
                    if (x > 0) checkPossibleMoves(x, y, x - 1, y);
                    if (y < jewels.GetLength(1) - 1) checkPossibleMoves(x, y, x, y + 1);
                    if (y > 0) checkPossibleMoves(x, y, x, y - 1);
                }

            //Den temporära poängen uppdateras.
            for (int i = 0; i < floatingScores.Count; i++)
            {
                floatingScores[i].Update();
                if (floatingScores[i].Remove)
                {
                    floatingScores.RemoveAt(i);
                    i--;
                }
            }
        }

        //Alla juveler målas upp via den här metoden.
        public void Draw(SpriteBatch spriteBatch)
        {
            //Juvelerna målas upp.
            foreach (Jewel jewel in jewels)
                jewel.Draw(spriteBatch);
            //Den temporära poängen målas upp.
            foreach (FloatingSpriteFont floatingSpriteFont in floatingScores)
                floatingSpriteFont.Draw(spriteBatch);
        }

        //Metod som gör det möjligt för juveler att falla ned.
        private void fall()
        {
            for (int x = 0; x < jewels.GetLength(0); x++)
            {
                //Hur många steg juvelerna ska röra sig nedåt.
                int moveDistance = 0;
                for (int y = jewels.GetLength(1) - 1; y >= 0; y--)
                {
                    //Är juvelen redo för borttagning så ska juvelen(juvelerna) ovanför hoppa ned och ta dess plats.
                    if (jewels[x, y].Remove)
                        moveDistance++;
                    else if(moveDistance > 0)
                    {
                        //Juvelen nedanför ersätts med den ovanför.
                        jewels[x, y].SetNewMovePosition(EffectStatus.IncreaseEffect, jewels[x, y].Position, new Vector2(213 + 42 * x, 56 + 42 * (y + moveDistance)), new Vector2(0, 6));
                        jewels[x, y + moveDistance] = jewels[x,y];
                    }
                }
                //Juveler som inte har någon juvel ovanför sig tas bort.
                for (int y = 0; y < moveDistance; y++)
                    jewels[x, y] = null;
            }
        }

        //Detta är metoden som kontrollerar om den valda juvelen har några anslutna juveler kring sig (rekursiv metod)
        private int checkConnectedJewels(int x, int y, JewelColor jewelColor)
        {
            //Variabel som hållera reda på hur många juveler som är anslutna till den valda juvelen.
            int connectedJewels = 0;
            //För att en juvel ska kunna kontrolleras får den inte vara null, kontrollerad, röra sig eller skala sig.
            //Dessutom måste den ha samma färg som den första juvelen som kontrolleras.
            if (jewels[x, y] != null && !jewels[x, y].Checked && !jewels[x, y].Moving &&
                !jewels[x, y].Scaling && jewels[x, y].JewelColor == jewelColor)
            {
                connectedJewels++;
                jewels[x, y].Checked = true;
                //Juvelerna kring den här juvelen kontrolleras
                if (x < jewels.GetLength(0) - 1) connectedJewels += checkConnectedJewels(x + 1, y, jewelColor);
                if (x > 0) connectedJewels += checkConnectedJewels(x - 1, y, jewelColor);
                if (y < jewels.GetLength(1) - 1) connectedJewels += checkConnectedJewels(x, y + 1, jewelColor);
                if (y > 0) connectedJewels += checkConnectedJewels(x, y - 1, jewelColor);
            }
            //När metoden har kört klart returneras värdet på hur många juveler som är anslutna till juvelen som kontrollerades först.
            return connectedJewels;
        }

        //Metod som är till för att kontrollera om det är möjligt att byta plats på någon juvel.
        private void checkPossibleMoves(int x, int y, int x2, int y2)
        {
            //En kontroll ska endast ske om ingen juvel rör sig och om ingen juvel skalar sig. Är PossibleMoves redan true
            //så behövs det inte heller kontrolleras om det är möjligt att byta plats på någon juvel. PossibleMoves blir då true som standard.
            if (!anyJewelMoving() && !anyJewelScaling() && !PossibleMoves)
            {
                //Tillfälliga juveler så att vi kan återanvända dem.
                Jewel firstSelectedJewel = jewels[x, y], secondSelectedJewel = jewels[x2, y2];

                //De två juvelerna byter plats i arrayen.
                jewels[x, y] = secondSelectedJewel;
                jewels[x2, y2] = firstSelectedJewel;

                //Resulterade flyttningen i att det nu finns fyra sammanhängande juveler någonstans? Då betyder det att det åtminstone finns en giltlig flyttning.
                if (checkConnectedJewels(x, y, secondSelectedJewel.JewelColor) >= 4 || checkConnectedJewels(x2, y2, firstSelectedJewel.JewelColor) >= 4)
                    PossibleMoves = true;
                //Alla juveler avmarkeras
                uncheckJewels();

                //De två juvelerna byter tillbaka till sina vanliga platser i arrayen.
                jewels[x, y] = firstSelectedJewel;
                jewels[x2, y2] = secondSelectedJewel;
            }
            else
                PossibleMoves = true;
        }

        //Metod som byter plats på två juveler (eventuellt).
        private void swapJewels(InputManager inputManager)
        {
            //Index till de två juvelerna som ska byta plats.
            int x = findSelectedJewelsIndex(inputManager)[0].X, y = findSelectedJewelsIndex(inputManager)[0].Y;
            int x2 = findSelectedJewelsIndex(inputManager)[1].X, y2 = findSelectedJewelsIndex(inputManager)[1].Y;

            //Tillfälliga juveler så att vi kan återanvända dem om flytningen blir ogiltlig.
            Jewel firstSelectedJewel = jewels[x, y], secondSelectedJewel = jewels[x2, y2];
            int deltaX = Math.Abs(x - x2), deltaY = Math.Abs(y - y2);

            //Befinner sig juvelerna en plats från varandra i arrayen?
            if ((deltaX == 0 && deltaY == 1) || (deltaX == 1 && deltaY == 0))
            {
                //De två juvelerna byter plats i arrayen.
                jewels[x, y] = secondSelectedJewel;
                jewels[x2, y2] = firstSelectedJewel;

                //Resulterade flyttningen i att det nu finns fyra sammanhängande juveler någonstans? Då byter juvelerna plats (synligt)
                //Annars byter juvelerna tillbaka till sina platser i arrayen.
                if (checkConnectedJewels(x, y, secondSelectedJewel.JewelColor) >= 4 || checkConnectedJewels(x2, y2, firstSelectedJewel.JewelColor) >= 4)
                {
                    jewels[x, y].SetNewMovePosition(EffectStatus.IncreaseEffect, secondSelectedJewel.Position, firstSelectedJewel.Position, new Vector2(10, 10));
                    jewels[x2, y2].SetNewMovePosition(EffectStatus.IncreaseEffect, firstSelectedJewel.Position, secondSelectedJewel.Position, new Vector2(10, 10));
                }
                else
                {
                    //De två juvelerna byter tillbaka till sina platser i arrayen.
                    jewels[x, y] = firstSelectedJewel;
                    jewels[x2, y2] = secondSelectedJewel;
                }
            }
            //Alla juveler avmarkeras
            deselectJewels();
        }

        //Metod som hittar indexet till de juvelerna som har blivit markerade.
        private Point[] findSelectedJewelsIndex(InputManager inputManager)
        {
            Point[] selectedJewelsIndex = new Point[2];
            for (int x = 0; x < jewels.GetLength(0); x++)
                for (int y = 0; y < jewels.GetLength(1); y++)
                {
                    //Är den här juvelen markerad och har spelaren inte klickat på den precis? Då är det den första juvelen som spelaren klickade på.
                    if (jewels[x, y].Selected && !jewels[x, y].HasBeenClicked(inputManager))
                        selectedJewelsIndex[0] = new Point(x, y);
                    //Har någon precis klickat på den här juvelen? Då är det den andra juvelen som markerades av spelaren.
                    else if (jewels[x, y].HasBeenClicked(inputManager))
                        selectedJewelsIndex[1] = new Point(x, y);
                }
            //En array med indexet till juvelerna returneras.
            return selectedJewelsIndex;
        }

        //Metod som avmarkerar alla juveler.
        private void deselectJewels()
        {
            foreach (Jewel jewel in jewels)
                    jewel.DeselectedJewel();
        }

        //Metod som avkontrollerar alla juveler.
        private void uncheckJewels()
        {
            foreach (Jewel jewel in jewels)
                if (jewel != null)
                    jewel.Checked = false;
        }

        //Metod som förbereder juveler för borttagning (skalning börjar och tillfälliga poäng målas upp).
        private void prepareRemovalOfConnectedJewels(int connectedJewels)
        {
            foreach (Jewel jewel in jewels)
                if (jewel.Checked)
                {
                    jewel.Scaling = true;
                    floatingScores.Add(new FloatingSpriteFont(spriteFont, jewel.Position, Color.SteelBlue, (10 * connectedJewels).ToString(),
                        new Vector2(jewel.Position.X, jewel.Position.Y - 42), new Vector2(0, 0.5f), 3));
                }
        }

        //Metoden får alla juveler att röra sig framåt (till sin slutposition).
        private void moveAllJewelsForward()
        {
            foreach (Jewel jewel in jewels)
                {
                    jewel.MoveToPositionEffect.startIncreaseEffect();
                    jewel.Moving = true;
                }
        }

        //Metod som kontrollerar om någon juvel rör sig.
        private bool anyJewelMoving()
        {
            foreach (Jewel jewel in jewels)
                if (jewel.Moving)
                    return true;
            return false;
        }

        //Metod som kontrollerar om någon juvel skalar sig.
        private bool anyJewelScaling()
        {
            foreach (Jewel jewel in jewels)
                if (jewel.Scaling)
                    return true;
            return false;
        }
    }
}
