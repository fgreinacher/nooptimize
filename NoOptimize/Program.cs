using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CmdLine;

namespace NoOptimize
{
    class Program
    {
        private static readonly string s_IniFileContent = new StringBuilder()
            .AppendLine("[.NET Framework Debugging Control]")
            .AppendLine("AllowOptimize=0")
            .ToString();

        static void Main()
        {
            try
            {
                var arguments = CommandLine.Parse<Arguments>();
                Run(arguments);
            }
            catch (CommandLineException exception)
            {
                Console.WriteLine(exception.ArgumentHelp.Message);
                Console.WriteLine();
                Console.WriteLine(exception.ArgumentHelp.GetHelpText(Console.BufferWidth));
            }
        }

        private static void Run(Arguments arguments)
        {
            var createNoOptimizeFile = arguments.Preview
                ? new Action<string>(PreviewCreateNoOptimizeFile)
                : CreateNoOptimizeFile;

            foreach (var noOptimizeFile in GetMatchingFiles(arguments).Where(IsNetAssembly).Select(GetNoOptimizeFileName))
            {
                createNoOptimizeFile(noOptimizeFile);
            }
        }

        private static string GetNoOptimizeFileName(string file)
        {
            var noOptimizeFile = Path.Combine(Directory.GetParent(file).FullName, Path.GetFileNameWithoutExtension(file) + ".ini");
            return noOptimizeFile;
        }

        private static void CreateNoOptimizeFile(string noOptimizeFile)
        {
            File.WriteAllText(noOptimizeFile, s_IniFileContent);
        }

        private static void PreviewCreateNoOptimizeFile(string noOptimizeFile)
        {
            Console.WriteLine("Create {0}", noOptimizeFile);
        }

        private static IEnumerable<string> GetMatchingFiles(Arguments arguments)
        {
            if (!Directory.Exists(arguments.Directory))
                throw new CommandLineException(
                    new CommandArgumentHelp(typeof(Arguments), "The parameter \"path\" has to be a valid directory."));

            var searchOption = arguments.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var searchPattern = arguments.Filter + ".dll";
            return Directory.EnumerateFiles(arguments.Directory, searchPattern, searchOption);
        }

        public static bool IsNetAssembly(string path)
        {
            var sb = new StringBuilder(256);
            int written;
            var hr = GetFileVersion(path, sb, sb.Capacity, out written);
            return hr == 0;
        }

        [DllImport("mscoree.dll", CharSet = CharSet.Unicode)]
        private static extern int GetFileVersion(string path, StringBuilder buffer, int buflen, out int written);
    }
}
