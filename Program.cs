using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Reflection
{
    class Program
    {
        static void Main()
        {
            string path;
            // цикл пока строка пустая
            while (true)
            {
                try
                {
                    Console.WriteLine("Please enter the dir with dll: \n");
                    while (!String.IsNullOrWhiteSpace(path = Console.ReadLine().Trim().ToLower()))
                    {
                        Get_public_protected_bydll(path); // вывод в консоль public/proteced методов в каждом классе 
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error dir {0}", ex.Message);
                }
            }
        }

        static void Get_public_protected_bydll(string path)
        {
            List<String> Dlls = new List<string>();

            // добавление в стек наименований файлов с расширением "dll"
            Dlls.AddRange(Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".dll") || s.EndsWith(".DLL")));

            foreach (var dll in Dlls) // проход по каждому string в стеке
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(dll); //типа дизассемблера)

                    foreach (var classe in assembly.GetTypes()) // проход по каждому классу
                    {
                        Console.WriteLine(classe.Name); // вывод имени класса
                        foreach (var Method in classe.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        {
                            if (Method.IsFamily || Method.IsPublic) //check for public and prodected methods
                                Console.WriteLine($"- {Method.Name}");
                        }
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Something go wrong with {0}", dll);
                    throw;
                }
            }
        }
    }


}