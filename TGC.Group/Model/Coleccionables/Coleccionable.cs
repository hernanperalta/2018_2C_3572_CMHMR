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

        public Coleccionable(GameModel contexto, TgcMesh mesh)
        {
            this.mesh = mesh.createMeshInstance("durazno");
            this.contexto = contexto;
        }

        public void TestearColisionContra(Colisionable colisionable)
        {
            bool colisiono = TgcCollisionUtils.testAABBAABB(mesh.BoundingBox, colisionable.BoundingBox());
            if (colisiono)
            {
                Juntarme();
            }
        }

        protected abstract void Juntarme();
    }
}
