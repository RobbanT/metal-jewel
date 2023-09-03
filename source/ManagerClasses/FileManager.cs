using System;
using System.IO;
using System.Collections.Generic;

namespace Metal_Jewel.ManagerClasses
{
    //Klassen är nu serialiserad och går därmed att skicka i en ström.
    [Serializable]

    //Den här klassen används för att hantera filer(skriva och läsa).
    public sealed class FileManager
    {
        //Metod för att skriva till en fil. Vid metodanrop måste man ange vad man vill skriva till filen, samt filens sökväg.
        public void WriteToFile(string stringToSave, string filePath)
        {
            //Objekt som hanterar filen man vill skriva till. 
            StreamWriter steamWriter = new StreamWriter(filePath, true);
            //En ny rad skrivs i filen som består av strängen som angavs vid metodanrop.
            steamWriter.WriteLine(stringToSave);
            //Strömmen stängs(data skickas).
            steamWriter.Close();
        }

        //Metod för att läsa en fil. Vid metodanrop måste man ange filens sökväg.
        public List<string> ReadFromFile(string fileName)
        {
            //Detta är listan som alla textrader i filen kommer att sparas i. Varje element är en textrad.
            List<string> list = new List<string>();
            //Objekt som hanterar filen man vill läsa av. 
            StreamReader streamReader = new StreamReader(fileName);
            //Första raden läses av.
            string line = streamReader.ReadLine();
            //Så länge en rad innehåller någon text körs koden i kodblocket nedan.
            while (line != null)
            {
                //Texten på raden i filen läggs till i listan.
                list.Add(line);
                //Nästa rad läses av.
                line = streamReader.ReadLine();
            }
            //Strömmen stängs(data skickas).
            streamReader.Close();
            //Listan med alla strängar (textrader) returneras.
            return list;
        }
    }
}
