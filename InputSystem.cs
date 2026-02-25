using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace EminaCraft
{
    internal class InputAxis
    {
        public Keys[] inputKeys;  //All the keys in the input axis
        public bool[] keysDown;

        //Can have many different configurations of keys.
        public InputAxis(Keys[] keys) {
            inputKeys = keys;
            keysDown = new bool[keys.Length];
        }
        public InputAxis(Keys key1)
        {
            inputKeys = new Keys[] { key1 };
            keysDown = new bool[inputKeys.Length];
        }
        public InputAxis(Keys key1, Keys key2)
        {
            inputKeys = new Keys[] { key1, key2 };
            keysDown = new bool[inputKeys.Length];
        }
        public InputAxis(Keys key1, Keys key2, Keys key3)
        {
            inputKeys = new Keys[] { key1, key2, key3 };
            keysDown = new bool[inputKeys.Length];
        }
        public InputAxis(Keys key1, Keys key2, Keys key3, Keys key4)
        {
            inputKeys = new Keys[] { key1, key2, key3, key4 };
            keysDown = new bool[inputKeys.Length];
        }
        public InputAxis(Keys key1, Keys key2, Keys key3, Keys key4, Keys key5, Keys key6)
        {
            inputKeys = new Keys[] { key1, key2, key3, key4, key5, key6 };
            keysDown = new bool[inputKeys.Length];
        }

        //Get 1 key as a scalar
        public bool getDown()
        {
            return keysDown[0];
        }
        //Get 4 keys as a 2D vector
        public Vector2 getVec2()
        {
            return new Vector2((keysDown[0] ? -1f : 0f) + (keysDown[1] ? 1f : 0f), (keysDown[2] ? -1f : 0f) + (keysDown[3] ? 1f : 0f));
        }
        //Get 6 keys as a 3D vector
        public Vector3 getVec3()
        {
            return new Vector3((keysDown[0] ? -1f : 0f) + (keysDown[1] ? 1f : 0f), (keysDown[4] ? -1f : 0f) + (keysDown[5] ? 1f : 0f), (keysDown[2] ? -1f : 0f) + (keysDown[3] ? 1f : 0f));
        }
    }
    internal class InputSystem
    {
        //Registered axes
        public List<InputAxis> axes = new List<InputAxis>();
        //Should be called every frame to update the registered axes
        public void updateInputs()
        {
            foreach (InputAxis axis in axes)
            {
                //Update all the keys
                for(int i = 0; i < axis.inputKeys.Length; i++)
                {
                    axis.keysDown[i] = Keyboard.getKey(axis.inputKeys[i]);
                }
            }
        }
    }
}
