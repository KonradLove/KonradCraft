using KonradCraft.gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonradCraft.scenes
{
    internal class SceneManager
    {
        public Scene? scene;
        private EntityManager entities;
        private GUIManager gui;
        private Scene? nextScene = null;
        public SceneManager(EntityManager entities, GUIManager gui)
        {
            this.entities = entities;
            this.gui = gui;
        }
        public void Update()
        {
            if(scene!=null)
                scene.Update();
            if (nextScene != null)
            {
                LoadScene(nextScene);
                nextScene = null;
            }
        }
        public void Render()
        {
            if (scene != null)
                scene.Render();
        }
        public void Unload()
        {
            if(scene!=null)
                scene.OnClose();
        }
        public void LoadScene(Scene newScene)
        {
            if (scene != null) scene.OnClose();
            entities.entities.Clear();
            gui.objects.Clear();
            scene = newScene;
            scene.Start();
        }

        internal void SwitchScene(Scene scene)
        {
            nextScene = scene;
        }
    }
}
