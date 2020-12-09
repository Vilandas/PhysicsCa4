namespace Physics
{
    public class Plane
    {
        #region Fields

        private Vector3 offset;
        private Vector3 dimensions;
        private Vector3 normal;
        private double muStatic;
        private double muKinetic;

        #endregion

        #region Properties

        public Vector3 Offset
        {
            get { return offset; } 
            set { offset = value; }
        }

        public Vector3 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        public Vector3 Normal
        {
            get { return normal; }
            set { normal = value; }
        }

        public double MuStatic
        {
            get { return muStatic; }
            set { muStatic = value; }
        }

        public double MuKinetic
        {
            get { return muKinetic; }
            set { muKinetic = value; }
        }

        #endregion

        #region Constructor

        public Plane(Vector3 offset, Vector3 dimensions, Vector3 normal,
            double muStatic, double muKinetic)
        {
            this.offset = offset;
            this.dimensions = dimensions;
            this.normal = normal;
            this.muStatic = muStatic;
            this.muKinetic = muKinetic;
        }

        #endregion
    }
}
