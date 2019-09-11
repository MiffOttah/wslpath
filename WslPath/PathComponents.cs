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
            throw new NotImplementedException();
        }

        public string ToString(PathFormat format)
        {
            throw new NotImplementedException();
        }
    }

    public enum PathFormat
    {
        Unix = 0,
        Windows = 1
    }
}
