using System;

namespace Metal_Jewel
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        //Exekveringen kommer att b�rja h�r n�r programmet startar. 
        //Kortfattat s� initierar metoden ett nytt objekt av typen Game1. Sedan k�rs objektets Run-metod.
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

