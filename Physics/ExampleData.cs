using System.Collections.Generic;

namespace Physics
{
    public class ExampleData
    {
        //Friction data from https://www.tribology-abc.com/abc/cof.htm

        //Steel on wood
        public static readonly Properties example1 =
            new Properties("Example 1", 9.81, 0, 0.1, 2,
                new Vector3(0, 0, 10), new Vector3(4, 2, 2), new Vector3(0, 0, 0),
                new List<Plane>()
                {
                    new Plane(new Vector3(0, 0, 20), new Vector3(10, 1, 50), new Vector3(0, 2, 6),
                        0.6, 0.4)
                });

        //Plastic on metal
        public static readonly Properties example2 =
            new Properties("Example 2", 9.81, 0, 0.1, 2,
                new Vector3(0, 0, 10), new Vector3(4, 2, 2), new Vector3(0, 0, 0),
                new List<Plane>()
                {
                    new Plane(new Vector3(0, 0, 20), new Vector3(10, 1, 50), new Vector3(0, 2, 6),
                        0.25, 0.1)
                });

        //Steel on ice
        public static readonly Properties example3 =
            new Properties("Example 3", 9.81, 0, 0.1, 2,
                new Vector3(0, 0, 4), new Vector3(4, 2, 2), new Vector3(0, 0, 0),
                new List<Plane>()
                {
                    new Plane(new Vector3(0, 0, 0), new Vector3(10, 1, 120), new Vector3(1, 1, 30),
                        0.03, 0.015)
                });

        public static readonly Properties custom =
            new Properties("Custom", 9.81, 0, 0.1, 2,
                new Vector3(0, 0, 10), new Vector3(4, 2, 2), new Vector3(0, 0, 0),
                new List<Plane>()
                {
                    new Plane(new Vector3(0, 0, 20), new Vector3(10, 1, 50), new Vector3(0, 2, 6),
                        0.6, 0.4)
                });

    }
}
