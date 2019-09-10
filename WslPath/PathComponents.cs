using System;
using System.Collections.Generic;
using System.Text;

namespace WslPath
{
    public class PathComponents
    {
        public char DriveLetter { get; set; }
        public List<string> Components { get; set; } = new List<string>();

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
