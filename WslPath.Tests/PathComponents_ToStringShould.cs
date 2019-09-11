using NUnit.Framework;
using System;

namespace WslPath.Tests
{
    [TestFixture]
    public class PathComponents_ToStringShould
    {
        [Test]
        [TestCase(PathFormat.Windows, 'c', "Folder,Filename.ext", @"c:\Folder\Filename.ext")]
        [TestCase(PathFormat.Unix, 'c', "Folder,Filename.ext", @"/mnt/c/Folder/Filename.ext")]
        [TestCase(PathFormat.Windows, 'D', "Folder,Filename.ext", @"D:\Folder\Filename.ext")]
        [TestCase(PathFormat.Unix, 'D', "Folder,Filename.ext", @"/mnt/d/Folder/Filename.ext")]
        [TestCase(PathFormat.Windows, 'E', "", @"E:\")]
        [TestCase(PathFormat.Unix, 'E', "", @"/mnt/e")]
        public void ToString_Rooted(PathFormat format, char drive, string components, string expected)
        {
            var componentsObject = new PathComponents(drive, components.Split(',', StringSplitOptions.RemoveEmptyEntries));

            string result = componentsObject.ToString(format);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(PathFormat.Windows, "Relative,Path,Test", @"Relative\Path\Test")]
        [TestCase(PathFormat.Unix, "Relative,Path,Test", @"Relative/Path/Test")]
        [TestCase(PathFormat.Windows, "", "")]
        [TestCase(PathFormat.Unix, "", "")]
        public void ToString_Relative(PathFormat format, string components, string expected)
        {
            var componentsObject = new PathComponents(null, components.Split(',', StringSplitOptions.RemoveEmptyEntries));

            string result = componentsObject.ToString(format);
            Assert.AreEqual(expected, result);
        }
    }
}
