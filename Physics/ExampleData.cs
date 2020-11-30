using System.Collections.Generic;

namespace Physics
{
    public class ExampleData
    {
        public static readonly Properties example1 =
            new Properties("Example 1", 9.81, 0, 0.1, 2,
                new Vector3(4, 2, 2), new Vector3(0, 0, 10), new Vector3(0, 0, 0),
                new List<Plane>()
                {
                    new Plane(new Vector3(0, 0, 5), new Vector3(10, 10, 1), new Vector3(0, 3, 4),
                        0.8, 0.6)
                });

        public static readonly Properties example2 =
            new Properties("Example 1", 9.81, 0, 0.1, 4,
                new Vector3(4, 2, 2), new Vector3(0, 0, 10), new Vector3(0, 0, 0),
                new List<Plane>()
                {
                    new Plane(new Vector3(0, 0, 5), new Vector3(10, 10, 1), new Vector3(0, 3, 4),
                        0.7, 0.5)
                });

        public static readonly Properties example3 =
            new Properties("Example 1", 9.81, 0, 0.1, 2,
                new Vector3(4, 2, 2), new Vector3(0, 0, 10), new Vector3(0, 0, 0),
                new List<Plane>()
                {
                    new Plane(new Vector3(0, 0, 5), new Vector3(10, 10, 1), new Vector3(0, 0, 1),
                        0.8, 0.6)
                });

        public static readonly Properties custom =
            new Properties("Example 1", 9.81, 0, 0.1, 2,
                new Vector3(4, 2, 2), new Vector3(0, 0, 10), new Vector3(0, 0, 0),
                new List<Plane>()
                {
                    new Plane(new Vector3(0, 0, 5), new Vector3(10, 10, 1), new Vector3(0, 0, 1),
                        0.8, 0.6)
                });
    }
}
