using KonradCraft.gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonradCraft.scenes
{
    internal abstract class Scene
    {
        protected EntityManager entityManager;
        protected GUIManager guiManager;
        protected SceneManager sceneManager;
        public Scene(EntityManager entityManager, GUIManager guiManager, SceneManager sceneManager)
        {
            this.entityManager = entityManager;
            this.guiManager = guiManager;
            this.sceneManager = sceneManager;
        }
        public virtual void Start() { }
        public virtual void Update()
        {
            entityManager.tickAll();
            guiManager.Update();
        }
        public virtual void Render()
        {
            entityManager.render();
            guiManager.Render();
        }
        public virtual void OnClose() { }
    }
}
