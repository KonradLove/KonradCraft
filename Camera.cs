using OpenTK.Mathematics;

namespace KonradCraft
{
    internal class Camera
    {
        public Vector3 position;
        public Vector3 rotation;

        public Matrix4 viewMatrix;

        //=====================================
        //Need to refresh the projection matrix
        //when these variables are changed
        //=====================================
        public float fov {
            get { return _fov; }
            set { _fov = value; dirty = true; }
        }
        public float near {
            get { return _near; }
            set { _near = value; dirty = true; }
        }
        public float far {
            get { return _far; }
            set { _far = value; dirty = true; }
        }
        public float aspectRatio {
            get { return _aspectRatio; }
            set { _aspectRatio = value; dirty = true; }
        }
        private float _fov,_near, _far, _aspectRatio;
        //True if the projection matrix needs to be regenerated
        public bool dirty = true;

        public Camera(Vector3 position, Vector3 rotation)
        {
            this.position = position;
            this.rotation = rotation;
            fov = 90f;
            aspectRatio = 1920f/1080f;
            near = 0.01f;
            far = 1000f;
        }
        Matrix4 proj;
        //Creates the projection matrix.
        public Matrix4 generateProjectionMatrix()
        {
            if (!dirty) return proj;
            proj = Matrix4.CreatePerspectiveFieldOfView(
                (float)Math.PI * (_fov / 180f), _aspectRatio, _near, _far);
            dirty = false;
            return proj;
        }

        //View matrix must be regenerated every frame
        public void update()
        {
            viewMatrix = createViewMatrix();
        }
        public Matrix4 getUntranslatedViewMatrix()
        {
            return Matrix4.CreateFromQuaternion(
                Quaternion.FromEulerAngles(rotation));
        }
        //Creates the view matrix
        private Matrix4 createViewMatrix()
        {
            Matrix4 rotationMatrix = Matrix4.CreateFromQuaternion(
                Quaternion.FromEulerAngles(rotation));

            Matrix4 translationMatrix = Matrix4.CreateTranslation(-position);

            Matrix4 viewMatrix = translationMatrix * rotationMatrix;

            return viewMatrix;
        }

    }
}
