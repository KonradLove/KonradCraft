

using OpenTK.Mathematics;

namespace KonradCraft
{
    internal struct Triangle
    {
        public Vector3 v1;
        public Vector3 v2;
        public Vector3 v3;
        public Vector3 normal;
        public int texture;
        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 normal, int texture)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.normal = normal;
            this.texture = texture;
        }
    }
}
