using System;
using System.Collections.Generic;

namespace WslPath
{
    internal class Program
    {
        internal static int Main(string[] args)
        {
            string option = "-u";

            var argsQueue = new Queue<string>(args);
            if (argsQueue.Count > 0 && argsQueue.Peek().StartsWith('-'))
            {
                option = argsQueue.Dequeue();
            }

            if (argsQueue.Count == 0)
            {
                argsQueue.Enqueue(Environment.CurrentDirectory);
            }

            switch (option)
            {
                case "--help":
                    Console.Out.WriteLine(Properties.Resources.HelpText);
                    return 0;

                case "--version":
                    Console.Out.WriteLine(
                        "wslpath ver {0}",
                        System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
                    );
                    return 0;

                case "-u":
                case "--unix":
                    return FormatPaths(PathFormat.Unix, argsQueue);

                case "-w":
                case "--windows":
                    return FormatPaths(PathFormat.Windows, argsQueue);

                default:
                    Console.Error.WriteLine("Unrecognized argument: " + option);
                    return 2;
            }
        }

        internal static int FormatPaths(PathFormat format, Queue<string> paths)
        {
            bool error = false;

            while (paths.Count > 0)
            {
                if (PathComponents.TryParse(paths.Dequeue(), out var components))
                {
                    Console.Out.WriteLine(components.ToString(format));
                }
                else
                {
                    error = true;
                }
            }

            return error ? 1 : 0;
        }
    }
}
