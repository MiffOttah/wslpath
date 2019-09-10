using System;
using System.Collections.Generic;
using System.Text;

namespace WslPath
{
    public interface IPathFormatter
    {
        string Format(string path, ref bool error);
    }

    public class WindowsPathFormatter : IPathFormatter
    {
        public string Format(string path, ref bool error)
        {
            throw new NotImplementedException();
        }
    }

    public class LinuxPathFormatter : IPathFormatter
    {
        public string Format(string path, ref bool error)
        {
            throw new NotImplementedException();
        }
    }
}
