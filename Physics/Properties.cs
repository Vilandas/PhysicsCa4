using System;
using System.Collections.Generic;

namespace Physics
{
    public class Properties
    {
        #region Fields

        private string name;
        private double gravity;
        private double time, originalTime;
        private double steps, originalSteps;
        private double mass;
        private Vector3 dimensions;
        private Vector3 position, originalPosition;
        private Vector3 velocity, originalVelocity;
        private Vector3 currentNormal;
        private double currentMuStatic;
        private double currentMuKinetic;
        private List<Plane> planes;

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public double Gravity
        {
            get { return gravity; }
            set { gravity = value; }
        }
        public double Time
        {
            get { return time; }
            set { time = value; }
        }
        public double OriginalTime
        {
            get { return originalTime; }
            set { originalTime = value; }
        }
        public double Steps
        {
            get { return steps; }
            set { steps = value; }
        }
        public double OriginalSteps
        {
            get { return originalSteps; }
            set { originalSteps = value; }
        }
        public double Mass
        {
            get { return mass; }
            set { mass = value; }
        }
        public Vector3 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector3 OriginalPosition
        {
            get { return originalPosition; }
            set { originalPosition = value; }
        }
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        public Vector3 OriginalVelocity
        {
            get { return originalVelocity; }
            set { originalVelocity = value; }
        }
        public Vector3 CurrentNormal
        {
            get { return currentNormal; }
            set { currentNormal = value; }
        }
        public double CurrentMuStatic
        {
            get { return currentMuStatic; }
            set { currentMuStatic = value; }
        }
        public double CurrentMuKinetic
        {
            get { return currentMuKinetic; }
            set { currentMuKinetic = value; }
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

        public Properties(string name, double gravity,
            double time, double steps, double mass,
            Vector3 dimensions, Vector3 position, Vector3 velocity, List<Plane> planes)
        {
            this.name = name;
            this.gravity = gravity;
            this.time = this.originalTime = time;
            this.steps = this.originalSteps = steps;
            this.mass = mass;

            this.dimensions = dimensions;
            this.position = this.originalPosition = position;
            this.velocity = this.originalVelocity = velocity;
            this.planes = planes;
            SetPlane(0);
        }

        #endregion

        public void SetPlane(int index)
        {
            currentMuStatic = planes[index].MuStatic;
            currentMuKinetic = planes[index].MuKinetic;
            currentNormal = planes[index].Normal;
        }

        public void DisplayDetails()
        {
            Console.WriteLine("\nGravity: " + gravity);
            Console.WriteLine("Time: " + time);
            Console.WriteLine("Time-Steps: " + steps);

            Console.WriteLine("Mass: " + mass);
            Console.WriteLine("Dimensions: " + dimensions);
            Console.WriteLine("Position: " + position);
            Console.WriteLine("Velocity: " + velocity);
        }
    }
}
