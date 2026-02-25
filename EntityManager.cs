using EminaCraft.shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EminaCraft
{
    internal class EntityManager
    {
        public List<Entity> entities = new List<Entity>();  //Entities in the scene
        private List<Entity> toRemove = new List<Entity>();
        private List<Entity> toAdd = new List<Entity>();
        public void render()
        {
            foreach (Entity entity in entities)
            {
                if (!entity.visible) continue;
                EntityShader shader = entity.shaderProgram;
                entity.Render();
            }
        }
        public Entity addEntity(Entity entity)
        {
            toAdd.Add(entity);
            return entity;
        }
        public Entity removeEntity(Entity entity)
        {
            toRemove.Add(entity);
            return entity;
        }
        public void tickAll()
        {
            foreach (Entity entity in entities)
            {
                entity.Tick();
            }
            foreach (Entity entity in toRemove)
            {
                entities.Remove(entity);
            }foreach (Entity entity in toAdd)
            {
                entities.Add(entity);
                entity.Init(this);
            }
            toAdd.Clear();
            toRemove.Clear();
        }
        //acab even the memory police
        public void cleanUp()
        {
            foreach (Entity entity in entities)
            {
                if(entity.model!= null)
                    entity.model.CleanUp();
            }
        }
    }
}
