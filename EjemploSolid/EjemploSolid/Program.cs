using System;
using System.Collections.Generic;

namespace EjemploSolid
{
    public static class Program
    {
        static void Main()
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\jose.ek\Desktop\tarea.txt");
            Dictionary<string, string> fechas = SplitDates(lines);
            List<string> mensajes = CreateMessages(fechas);
            PrintMessages(mensajes);
        }

        private static void PrintMessages(List<string> messages)
        {
            Console.WriteLine("Lista de eventos: ");

            foreach (string item in messages)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static Dictionary<string, string> SplitDates(string[] lines)
        {
            Dictionary<string, string> listFormatted = new Dictionary<string, string>();

            foreach (string line in lines)
            {
                string[] datos = line.Split(Convert.ToChar(","));
                listFormatted.Add(datos[0], datos[1]);
            }
            return listFormatted;
        }

        private static List<string> CreateMessages(Dictionary<string, string> fechas)
        {
            List<string> lstMensajes = new List<string>();

            foreach (KeyValuePair<string, string> item in fechas)
            {
                DateTime eventDateTime = DateTime.Parse(item.Value);
                bool isMinor = DateMinorCurrentDate(eventDateTime);
                string cadena = DictionaryToMessage(item, isMinor);
                lstMensajes.Add(cadena);
            }

            return lstMensajes;
        }

        private static string DictionaryToMessage(KeyValuePair<string, string> item, bool isCurrentDateMinor)
        {
            DateTime eventDateTime = DateTime.Parse(item.Value);
            string predicate = " ocurrió hace ";
            string timeElapsed = GetElapsedTime(DateTime.Now, eventDateTime);

            if (isCurrentDateMinor)
            {
                predicate = " ocurrirá dentro de ";
                timeElapsed = GetElapsedTime(eventDateTime, DateTime.Now);
            }

            string cadena = "\t" + $"{item.Key},{item.Value}".PadRight(40, '.');
            return cadena + $"{item.Key} {predicate} {timeElapsed}";
        }

        private static string GetElapsedTime(DateTime majorTime, DateTime minorTime)
        {
            TimeSpan compareTimeSpan = majorTime.Subtract(minorTime);
            string result;

            if (compareTimeSpan.Days > 30)
            {
                result = compareTimeSpan.Days / 30 + " mes(es)";
            }
            else if (compareTimeSpan.Days > 0)
            {
                result = compareTimeSpan.Days + " día(s)";
            }
            else if (compareTimeSpan.Hours > 0)
            {
                result = compareTimeSpan.Hours + " hora(s)";
            }
            else
            {
                result = compareTimeSpan.Minutes + " minuto(s)";
            }

            return result;
        }

        private static bool DateMinorCurrentDate(DateTime eventDate)
        {
            int compareDate = DateTime.Now.CompareTo(eventDate);
            return compareDate < 0;
        }
    }
}
