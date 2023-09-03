using System;

namespace Metal_Jewel
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        //Exekveringen kommer att börja här när programmet startar. 
        //Kortfattat så initierar metoden ett nytt objekt av typen Game1. Sedan körs objektets Run-metod.
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

