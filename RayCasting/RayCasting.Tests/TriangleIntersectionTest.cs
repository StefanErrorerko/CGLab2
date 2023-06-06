using NUnit.Framework;
using RayCasting.Core.Objects;
using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;

namespace RayCasting.Tests
{
    internal class TriangleIntersectionTest
    {
        [Test]
        [Category("Negative")]
        public void GetIntersectionWith_NotAvailableTrinagles()
        {
            var tr = new Triangle(new(-1, 0, 0), new(-2, 0, 0), new(-1, -2, 0));
            var ray = new Ray(origin: new(0, 0, 0), direction: new Vector3(1, 0, 0));

            Assert.IsFalse(tr.GetIntersectionWith(ray) != (null, null));
        }

        [Test]
        [Category("Positive")]
        public void GetIntersectionWith_AvailableTrinagles()
        {
            var tr = new Triangle(new(-1, 0, 0), new(-2, 0, 0), new(-1, -2, 0));
            var ray = new Ray(origin: new(0, 0, 0), direction: new Vector3(-1, -1, 0));

            Assert.IsTrue(tr.GetIntersectionWith(ray) != (null, null));
        }
    }
}