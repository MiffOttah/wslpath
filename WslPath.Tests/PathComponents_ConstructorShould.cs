using NUnit.Framework;
using System;

namespace WslPath.Tests
{
    [TestFixture]
    public class PathComponents_ConstructorShould
    {
        [Test]
        public void Constructor_Parameterless()
        {
            var result = new PathComponents();
            Assert.IsNull(result.DriveLetter);
            CollectionAssert.IsEmpty(result.Components);
        }

        [Test]
        [TestCase(null, null)]
        [TestCase('c', null)]
        [TestCase(null, "c1,c2")]
        [TestCase('d', "c1,c2")]
        [TestCase('E', "")]
        public void Constructor_WithParameters(char? driveLetter, string components)
        {
            string[] componentsArray = components?.Split(',', StringSplitOptions.RemoveEmptyEntries);

            var result = new PathComponents(driveLetter, componentsArray);

            Assert.AreEqual(driveLetter, result.DriveLetter);

            if (components is null)
            {
                CollectionAssert.IsEmpty(result.Components);
            }
            else
            {
                CollectionAssert.AreEqual(componentsArray, result.Components);
            }
        }
    }
}
