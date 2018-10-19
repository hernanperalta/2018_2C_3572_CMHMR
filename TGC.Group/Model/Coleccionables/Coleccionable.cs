using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Collision;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model.Coleccionables
{
    public abstract class Coleccionable
    {
        protected TgcMesh mesh;
        protected GameModel contexto;
        public bool Visible;

        public Coleccionable(GameModel contexto, TgcMesh mesh)
        {
            this.mesh = mesh;
            this.contexto = contexto;
            Visible = true;
        }

        public bool ColisionoContra(Colisionable colisionable)
        {
            return TgcCollisionUtils.testAABBAABB(mesh.BoundingBox, colisionable.BoundingBox()) && Visible;
        }

        public abstract void Juntarme();

        public void Render()
        {
            if(Visible)
                mesh.Render();
        }
    }
}
