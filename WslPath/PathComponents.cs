using System;
using System.Collections.Generic;
using System.Text;

namespace WslPath
{
    public class PathComponents
    {
        public char? DriveLetter { get; set; }
        public List<string> Components { get; }

        public PathComponents(char? driveLetter, IEnumerable<string> components)
        {
            DriveLetter = driveLetter;
            Components = new List<string>();
            if (components != null) Components.AddRange(components);
        }

        public PathComponents()
        {
            DriveLetter = null;
            Components = new List<string>();
        }

        public static bool TryParse(string path, out PathComponents result)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            result = new PathComponents();

            if (path == string.Empty)
            {
                return true;
            }
            else if (path[0] == '/')
            {
                return result.ParseAbsoluteUnixPath(path);
            }
            else if (path.Length >= 2 && path[1] == ':')
            {
                return result.ParseAbsoluteWindowsPath(path);
            }
            else if (path[0] == '\\') // windows path relative to drive root (not supported)
            {
                return false;
            }
            else
            {
                return result.ParseRelativePath(path.Replace('\\', '/'));
            }
        }

        private bool ParseAbsoluteUnixPath(string path)
        {
            if (path.Length < 6) return false;
            if (path.Substring(0, 5) != "/mnt/") return false;
            if (path.Length > 6 && path[6] != '/') return false;

            DriveLetter = path[5];
            ParseRelativePath(path.Substring(6));
            return true;
        }

        private bool ParseAbsoluteWindowsPath(string path)
        {
            DriveLetter = path[0];
            if (path.Length >= 3)
            {
                path = path.Replace('\\', '/');
                if (path[2] != '/') return false;
                ParseRelativePath(path.Substring(3));
            }
            return true;
        }

        private bool ParseRelativePath(string path)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            Components.AddRange(parts);
            return true;
        }

        public string ToString(PathFormat format)
        {
            var pathBuilder = new StringBuilder();
            char pathSeparator = format == PathFormat.Unix ? '/' : '\\';

            if (DriveLetter.HasValue)
            {
                if (format == PathFormat.Unix)
                {
                    pathBuilder.Append("/mnt/");
                    pathBuilder.Append(char.ToLowerInvariant(DriveLetter.Value));

                    if (Components.Count > 0)
                    {
                        pathBuilder.Append(pathSeparator);
                    }
                }
                else
                {
                    pathBuilder.Append(DriveLetter.Value);
                    pathBuilder.Append(":\\");
                }
            }

            bool first = true;
            foreach (string component in Components)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    pathBuilder.Append(pathSeparator);
                }

                pathBuilder.Append(component);
            }

            return pathBuilder.ToString();
        }
    }

    public enum PathFormat
    {
        Unix = 0,
        Windows = 1
    }
}
