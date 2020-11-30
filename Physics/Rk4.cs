using System;

namespace Physics
{
    public class Rk4
    {
        #region Fields

        private Properties prop;
        private Data data;

        #endregion

        public Data Data
        { 
            get { return data; } 
        }

        #region Constructor

        public Rk4(Properties prop)
        {
            this.prop = prop;
            this.data = new Data(this.prop.Name);
        }

        #endregion

        public void Start()
        {
            CalculateRk4();
            Eulers(CalculateAcceleration(prop.Velocity));

        }

        public Vector3 CalculateAcceleration(Vector3 velocity)
        {
            //Vector3 va = Vector3.Subtract(velocity, prop.FlowRate);

            //Vector3 fGravity = ForceGravity();
            //Vector3 fDrag = ForceDrag(va);
            //Vector3 fMagnus = ForceMagnus(va);

            //Vector3 fNet = fGravity + fDrag + fMagnus;

            //Vector3 acceleration = Vector3.Multiply(fNet, 1d / prop.Mass);

            //data.AddForces(fGravity, fDrag, fMagnus, fNet);

            //return acceleration;
            return Vector3.Zero();
        }

        public void Eulers(Vector3 acceleration)
        {
            double t1 = prop.Time + prop.Steps;
            Vector3 p1 = prop.Position + (prop.Velocity * prop.Steps);
            Vector3 v1 = prop.Velocity + (acceleration * prop.Steps);
            Vector3 a1 = CalculateAcceleration(v1);
            Console.WriteLine("\nEulers: ");
            Console.WriteLine("p1: " + p1);
            Console.WriteLine("v1: " + v1);
            Console.WriteLine("a1: " + a1);
        }

        public Vector2 CalculateRk4()
        {
            Vector2 pv0 = new Vector2(prop.Position, prop.Velocity);
            Vector2 k1 = F(pv0) * prop.Steps;
            Vector2 k2 = F(pv0 + (k1 / 2)) * prop.Steps;
            Vector2 k3 = F(pv0 + (k2 / 2)) * prop.Steps;
            Vector2 k4 = F(pv0 + k3) * prop.Steps;
            Vector2 k = (k1 + (k2 * 2) + (k3 * 2) + k4) * (1d / 6d);
            Vector2 pv1 = pv0 + k;

            data.AddRK4(pv0, k1, k2, k3, k4, k, prop.Time);

            return pv1;
        }

        public Vector2 F(Vector2 pv)
        {
            Vector3 acceleration = CalculateAcceleration(pv.Y);
            return new Vector2(pv.Y, acceleration);
        }

        public void UpdatePV(Vector2 pv)
        {
            prop.Position = pv.X;
            prop.Velocity = pv.Y;
        }
    }
}
