using System;

namespace Physics
{
    public class Rk4
    {
        #region Fields

        private Properties prop;
        private Data data;
        private Vector3 acceleration;
        private bool forcesDone;

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

        //F̅g = -mgk̂
        public Vector3 ForceGravity()
        {
            return Vector3.Multiply(Vector3.Up(), (-prop.Mass * prop.Gravity));
        }

        //F̅gn = [F̅g.N̂][N̂]
        public Vector3 ForceGravityNormal(Vector3 _Fg)
        {
            Vector3 normalHat = Vector3.Normalise(prop.CurrentNormal);
            return Vector3.Dot(_Fg, normalHat) * normalHat;
        }

        public Vector3 CalculateAcceleration(Vector3 velocty)
        {
            Vector3 _Fg = ForceGravity();
            Vector3 _Fgn = ForceGravityNormal(_Fg);
            Vector3 _Fgp = _Fg - _Fgn;
            Vector3 _Fn = _Fgn * -1;
            double friction = prop.CurrentMuStatic * _Fn.Length;

            if (!forcesDone)
            {
                data.AddForces(_Fg, _Fgn, _Fgp, _Fn, friction);
            }

            //F̅net = 0
            if (friction >= _Fgp.Length && velocty.IsZero())
            {
                return Vector3.Zero();
            }

            friction = prop.CurrentMuKinetic * _Fn.Length;
            Vector3 _FfUnit = Vector3.Normalise(_Fgp) * -1;
            Vector3 _Ff = friction * _FfUnit;

            Vector3 _Fnet = _Fgn + _Fgp + _Fn + _Ff;
            Vector3 acceleration = (1d / prop.Mass) * _Fnet;

            if(!forcesDone)
            {
                data.AddKinetic(friction, _FfUnit, _Ff, _Fnet, acceleration);
            }

            return acceleration;
        }

        public void Eulers(Vector3 acceleration)
        {
            double t1 = prop.Time + prop.Steps;
            Vector3 p1 = prop.Position + (prop.Velocity * prop.Steps);
            Vector3 v1 = prop.Velocity + (acceleration * prop.Steps);
            Vector3 a1 = CalculateAcceleration(prop.Velocity);
            Console.WriteLine("\nEulers: ");
            Console.WriteLine("p1: " + p1);
            Console.WriteLine("v1: " + v1);
            Console.WriteLine("a1: " + a1);
        }

        public Vector2 CalculateRk4()
        {
            forcesDone = false;
            Vector2 pv0 = new Vector2(prop.Position, prop.Velocity);
            acceleration = prop.Steps * CalculateAcceleration(pv0.Y);
            forcesDone = true;

            data.AddPV(pv0, prop.Time);
            //Acceleration static, not moving
            if (acceleration.IsZero())
                return pv0;

            Vector2 k1 = F(pv0) * prop.Steps;
            Vector2 k2 = F(pv0 + (k1 / 2)) * prop.Steps;
            Vector2 k3 = F(pv0 + (k2 / 2)) * prop.Steps;
            Vector2 k4 = F(pv0 + k3) * prop.Steps;
            Vector2 k = (k1 + (k2 * 2) + (k3 * 2) + k4) * (1d / 6d);
            Vector2 pv1 = pv0 + k;

            data.AddRK4(k1, k2, k3, k4, k);

            return pv1;
        }

        public Vector2 F(Vector2 pv)
        {
            return new Vector2(pv.Y, acceleration);
        }

        public void UpdatePV(Vector2 pv)
        {
            prop.Position = pv.X;
            prop.Velocity = pv.Y;
        }
    }
}
