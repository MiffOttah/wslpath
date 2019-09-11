using NUnit.Framework;
using System;

namespace WslPath.Tests
{
    [TestFixture]
    public class PathComponents_TryParseShould
    {
        [Test]
        [TestCase(@"C:\Folder\Filename.ext", 'C', "Folder,Filename.ext")]
        [TestCase(@"d:\example", 'd', "example")]
        [TestCase(@"relative\path", null, "relative,path")]
        [TestCase(@"E:/Forward/Slashes", 'E', "Forward,Slashes")]
        [TestCase(@"c:\Trailing\Backslash\", 'c', "Trailing,Backslash")]
        [TestCase(@"c:\Multiple\\Backslashes", 'c', "Multiple,Backslashes")]
        [TestCase(@"C:\", 'C', "")]
        public void TryParse_WindowsPath_Success(string path, char? drive, string components)
        {
            bool success = PathComponents.TryParse(path, out var result);

            Assert.IsTrue(success);
            Assert.IsNotNull(result);

            Assert.AreEqual(drive, result.DriveLetter);
            CollectionAssert.AreEqual(components.Split(',', StringSplitOptions.RemoveEmptyEntries), result.Components);
        }

        [Test]
        [TestCase(@"\\UNC\Path")]
        [TestCase(@"\Root\Without\Drive")]
        [TestCase(@"C:Drive\Without\Root")]
        public void TryParse_WindowsPath_Failure(string path)
        {
            bool success = PathComponents.TryParse(path, out var result);
            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [Test]
        [TestCase(@"/mnt/c/Folder/Filename.ext", 'c', "Folder,Filename.ext")]
        [TestCase(@"/mnt/d/example", 'd', "example")]
        [TestCase(@"relative/path", null, "relative,path")]
        [TestCase(@"/mnt/c/Trailing/Backslash/", 'c', "Trailing,Backslash")]
        [TestCase(@"/mnt/c/Multiple//Backslashes", 'c', "Multiple,Backslashes")]
        [TestCase(@"/mnt/c", 'c', "")]
        public void TryParse_UnixPath_Success(string path, char? drive, string components)
        {
            bool success = PathComponents.TryParse(path, out var result);

            Assert.IsTrue(success);
            Assert.IsNotNull(result);

            Assert.AreEqual(drive, result.DriveLetter);
            CollectionAssert.AreEqual(components.Split(',', StringSplitOptions.RemoveEmptyEntries), result.Components);
        }

        [Test]
        [TestCase(@"/tmp/Outside/WSL")]
        public void TryParse_UnixPath_Failure(string path)
        {
            bool success = PathComponents.TryParse(path, out var result);
            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [Test]
        public void TryParse_NullPath()
        {
            Assert.Throws<ArgumentNullException>(() => PathComponents.TryParse(null, out _));
        }
    }
}
