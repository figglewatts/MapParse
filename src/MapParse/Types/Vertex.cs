namespace MapParse.Types
{
    public class Vertex
    {
        public Vec3 P { get; set; }
        public float[] Tex { get; set; }

        public Vertex(Vec3 v)
        {
            P = v;
            Tex = new float[2];
        }

        public Vertex(Vertex v)
        {
            P = v.P;
            Tex = v.Tex;
        }

        public Vertex()
        {
            P = new Vec3();
            Tex = new float[2];
        }
    }
}
