using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHybridMAUIApp
{
    using System.Diagnostics;
    using System.IO;

    public static class DirectoryHelper
    {
        public static void PrintDirectoryStructure(string path, int indentLevel = 0)
        {
            Console.WriteLine("-----------------------------------*****************************----------------------------------");
            Console.WriteLine($"The path we are looking inside is is {path}");
            try
            {
                // Print all files in the current directory
                foreach (var file in Directory.GetFiles(path))
                {
                    Debug.WriteLine($"{new string(' ', indentLevel * 2)}- {Path.GetFileName(file)}");
                }

                // Print all subdirectories and recursively call this function on them
                foreach (var dir in Directory.GetDirectories(path))
                {
                    Debug.WriteLine($"{new string(' ', indentLevel * 2)}+ {Path.GetFileName(dir)}");
                    PrintDirectoryStructure(dir, indentLevel + 1);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error accessing {path}: {ex.Message}");
            }
            Console.WriteLine("-----------------------------------*****************************----------------------------------");
        }

        public static List<string> GetDirectoryStructureStr(string path, int indentLevel = 0)
        {
            List<string> output = new List<string>();
            try
            {
                // Print all files in the current directory
                foreach (var file in Directory.GetFiles(path))
                {
                    output.Add($"{new string(' ', indentLevel * 2)}- {Path.GetFileName(file)}");
                }

                // Print all subdirectories and recursively call this function on them
                foreach (var dir in Directory.GetDirectories(path))
                {
                    output.Add($"{new string(' ', indentLevel * 2)}+ {Path.GetFileName(dir)}");
                    foreach (var s in GetDirectoryStructureStr(dir, indentLevel + 1))
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error accessing {path}: {ex.Message}");
            }
            return output;
        }
    }

}
