namespace Physics
{
    public class Plane
    {
        #region Fields

        private Vector3 position;
        private Vector3 dimensions;
        private Vector3 normal;
        private double muStatic;
        private double muKinetic;

        #endregion

        #region Properties

        public Vector3 Position
        {
            get { return position; } 
            set { position = value; }
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

        public Vector3 Centre
        {
            get
            {
                return new Vector3(
                    (Position.X + Dimensions.X) / 2,
                    (Position.Y + Dimensions.Y) / 2,
                    (Position.Z + Dimensions.Z) / 2);
            }
        }

        #endregion

        #region Constructor

        public Plane(Vector3 position, Vector3 dimensions, Vector3 normal,
            double muStatic, double muKinetic)
        {
            this.position = position;
            this.dimensions = dimensions;
            this.normal = normal;
            this.muStatic = muStatic;
            this.muKinetic = muKinetic;
        }

        #endregion
    }
}
