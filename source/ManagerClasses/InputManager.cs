using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Metal_Jewel.ManagerClasses
{
    //Klassen kommer att användas för att läsa av tangentbord och mus. 
    public sealed class InputManager
    {
        //Dessa variabler kommer att användas för att kolla statuset hos musen.
        private MouseState oldMouseState, newMouseState;
        //Dessa variabler kommer att användas för att hålla reda på muspilens position.
        private Point oldMousePosition, newMousePosition;
        //Dessa variabler kommer att användas för att kolla statuset hos tangetbordet.
        private KeyboardState oldKeyboardState, newKeyboardState;

        //Klassens konstruktor. En tilldelning till klassens alla variabler sker här.
        public InputManager()
        {
            oldMouseState = newMouseState;
            newMouseState = Mouse.GetState();
            oldMousePosition = newMousePosition;
            newMousePosition = new Point(newMouseState.X, newMouseState.Y);
            oldKeyboardState = newKeyboardState;
            newKeyboardState = Keyboard.GetState();
        }

        //Metod för att kolla att vänster musknapp är nedtryck på ett objekt 
        //(collisionRectangle används för att kontrollera om muspilens position är på objektet).
        public bool LeftMouseButtonPressedOnObject(Rectangle collisionRectangle)
        {
            //Är vänster musknapp nedtryckt och är muspilens position på objektet?
            return newMouseState.LeftButton == ButtonState.Pressed && collisionRectangle.Contains(newMousePosition);
        }

        //Metod för att kolla att höger musknapp är nedtryck på ett objekt 
        //(collisionRectangle används för att kontrollera om muspilens position är på objektet).
        public bool RightMouseButtonPressed(Rectangle collisionRectangle)
        {
            //Är höger musknapp nedtryckt och är muspilens position på objektet?
            return newMouseState.RightButton == ButtonState.Pressed && collisionRectangle.Contains(newMousePosition);
        }

        //Metod för att kolla om det sker ett musklick, med vänster musknapp, på ett objekt 
        //(collisionRectangle används för att kontrollera om muspilens position är på objektet).
        public bool LeftMouseButtonClickedOnObject(Rectangle collisionRectangle)
        {
            //Har ett musklick skett med vänster musknapp och är muspilens position på objektet? 
            return oldMouseState.LeftButton == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Released
                && collisionRectangle.Contains(oldMousePosition) && collisionRectangle.Contains(newMousePosition);
        }

        //Metod för att kolla om det sker ett musklick, med höger musknapp, på ett objekt 
        //(collisionRectangle används för att kontrollera om muspilens position är på objektet).
        public bool RightMouseButtonClickedOnObject(Rectangle collisionRectangle)
        {
            //Har ett musklick skett med höger musknapp och är muspilens position på objektet? 
            return oldMouseState.RightButton == ButtonState.Pressed && newMouseState.RightButton == ButtonState.Released
                && collisionRectangle.Contains(oldMousePosition) && collisionRectangle.Contains(newMousePosition);
        }

        //Metod för att kolla om muspilen är på ett objekt 
        //(collisionRectangle används för att kontrollera om muspilens position är på objektet).
        public bool MousePositionOnObject(Rectangle collisionRectangle)
        {
            //Är muspilens position på objektet? 
            return collisionRectangle.Contains(newMousePosition);
        }

        //Metod för att kolla om en viss tangentbordsknapp har blivit klickad.
        //(Vilken knapp man vill kontrollera anger man vid metodanrop).
        public bool KeyboardButtonClicked(Keys key)
        {
            //Har ett knappklick skett med vald knapp?
            return oldKeyboardState.IsKeyDown(key) && newKeyboardState.IsKeyUp(key);
        }

        //Metod för att uppdatera samtliga variabler i klassen.
        public void Update()
        {
            oldMouseState = newMouseState;
            newMouseState = Mouse.GetState();
            oldMousePosition = newMousePosition;
            newMousePosition = new Point(newMouseState.X, newMouseState.Y);
            oldKeyboardState = newKeyboardState;
            newKeyboardState = Keyboard.GetState();
        }
    }
}
