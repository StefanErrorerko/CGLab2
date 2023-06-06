using NUnit.Framework;
using RayCasting.Core.Objects;
using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;

namespace RayCasting.Tests
{
    internal class TriangleIntersectionTest
    {
        Triangle _tr1, _tr2;
        Ray _ray;

        [SetUp]
        public void Setup()
        {
            _tr1 = new Triangle(new(-1, 0, 0), new(-2, 0, 0), new(-1, -2, 0));
            _tr2 = new Triangle(
                new(-0.415473f, 0.0164913f, 0.229491f), 
                new(-0.452258f, 0.003916f, -0.189177f), 
                new(-0.4199f, -3.1819e-05f, 0.218811f));
            _ray = new Ray(origin: new(-1, 0, 0), direction: new Vector3(1, 1, 1));
        }

        [Test]
        [Category("Negative")]
        public void GetIntersectionWith_NotAvailableTrinagle()
        {
            var intersection = _tr1.GetIntersectionWith(_ray);
            Assert.IsNull(intersection.point);
            Assert.IsNull(intersection.t);
        }

        [Test]
        [Category("Positive")]
        public void GetIntersectionWith_AvailableTrinagle()
        {
            var intersection = _tr2.GetIntersectionWith(_ray);
            Assert.IsNotNull(intersection.point);
            Assert.IsNotNull(intersection.t);
        }
    }
}