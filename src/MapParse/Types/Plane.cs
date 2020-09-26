using System;
using MapParse.Util;

namespace MapParse.Types
{
    /// <summary>
    /// Plane class. Follows N dot P + D = 0 equation.
    /// See "Hessian Normal Form".
    /// </summary>
    public class Plane
    {
        public Vec3 Normal { get; set; }
        public double Distance { get; set; }

        public Plane()
        {
            this.Normal = new Vec3();
            this.Distance = 0F;
        }

        public Plane(Plane p)
        {
            this.Normal = p.Normal;
            this.Distance = p.Distance;
        }

        public Plane(Vec3 n, double d)
        {
            this.Normal = n;
            this.Distance = d;
        }

        public Plane(Vec3 a, Vec3 b, Vec3 c)
        {
            //Console.WriteLine("Calculating plane.");
            this.Normal = (a - b).Cross(a - c);
            //Console.WriteLine("Normal pre normalize: " + this.Normal.ToString());
            //this.Normal.Normalize();
            //Console.WriteLine("Post normalize: " + this.Normal.ToString());
            this.Distance = -this.Normal.Dot(a);
            //Console.WriteLine("A: " + a.ToString());
            //Console.WriteLine("B: " + b.ToString());
            //Console.WriteLine("C: " + c.ToString());
            //Console.WriteLine("D: " + this.Distance);
            //Console.WriteLine("\n\n\n");
        }

        public static bool operator ==(Plane a, Plane b)
        {
            if (a.Normal == b.Normal)
            {
                if (a.Distance == b.Distance)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool operator !=(Plane a, Plane b)
        {
            if (a == b)
            {
                return false;
            }

            return true;
        }

        public override string ToString() { return this.Normal.ToString() + ", D: " + this.Distance.ToString(); }

        public float DistanceToPlane(Vec3 v)
        {
            return (float)(ParseUtils.RoundToSignificantDigits(Vec3.Dot(this.Normal, v), 5) +
                           ParseUtils.RoundToSignificantDigits(this.Distance, 5));
        }

        // calculate whether a point is in front, behind, or on the plane
        public PointClassification ClassifyPoint(Vec3 point)
        {
            float distance = DistanceToPlane(point);
            if (distance > Constants.Epsilon)
            {
                return PointClassification.FRONT;
            }
            else if (distance < -Constants.Epsilon)
            {
                return PointClassification.BACK;
            }

            return PointClassification.ON_PLANE;
        }

        // calculate the point of intersection of this plane and 2 others
        // based on "Intersection of 3 Planes" http://geomalgorithms.com/a05-_intersect-1.html
        public bool GetIntersection(Plane a, Plane b, ref Vec3 intersection)
        {
            float denom = Vec3.Dot(this.Normal, Vec3.Cross(a.Normal, b.Normal));
            if (Math.Abs(denom) < Constants.Epsilon)
            {
                return false;
            }

            intersection = (((Vec3.Cross(a.Normal, b.Normal) * -this.Distance) -
                             (Vec3.Cross(b.Normal, this.Normal) * a.Distance) -
                             (Vec3.Cross(this.Normal, a.Normal) * b.Distance)) / denom);
            return true;
        }

        public bool GetIntersection(Vec3 start, Vec3 end, ref Vertex intersection, ref float percentage)
        {
            Vec3 direction = end - start;
            float num;
            float denom;

            direction.Normalize();

            denom = Vec3.Dot(this.Normal, direction);

            if (Math.Abs(denom) < Constants.Epsilon)
            {
                return false;
            }

            num = -DistanceToPlane(start);
            percentage = num / denom;
            intersection = new Vertex(start + (direction * percentage));
            percentage = percentage / (end - start).Magnitude();
            return true;
        }
    }
}
