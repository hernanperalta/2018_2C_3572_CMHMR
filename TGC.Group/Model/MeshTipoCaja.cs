using TGC.Core.SceneLoader;
using TGC.Core.SkeletalAnimation;
using TGC.Core.Mathematica;
using TGC.Core.Collision;
using System.Collections;
using System.Collections.Generic;
using TGC.Core.Geometry;
using System;
using TGC.Core.BoundingVolumes;
//using

namespace TGC.Group.Model
{
    public abstract class MeshTipoCaja
    {
        public TgcMesh mesh;
        public TgcBoundingAxisAlignBox BoundingBox {
            get => this.mesh.BoundingBox;
        }
        
        protected TGCVector3 posicionInicial;
        protected List<Cara> caras;

        protected MeshTipoCaja(TGCVector3 posicionInicial, TgcMesh mesh)
        {
            this.mesh = mesh;
          
            this.posicionInicial = posicionInicial;

            mesh.AutoTransform = false;
            mesh.Transform = TGCMatrix.Translation(posicionInicial);
            mesh.BoundingBox.transform(mesh.Transform);

            GenerarCaras();
        }

        protected abstract void GenerarCaras();

        protected void ClearCaras() {
            this.caras.Clear();
        }

        protected abstract int ModificacionEnY();

        internal void Render()
        {
            mesh.Render();
        }

        public virtual void Update(TGCMatrix movimientoCaja) {
            ClearCaras();
            GenerarCaras();
        }

        public void TestearColisionContra(Personaje personaje) {
            foreach (Cara cara in caras)
            {
                cara.TesteoDeColision(personaje);
            }
        }

        internal void RenderizaRayos()
        {
            mesh.BoundingBox.Render();

            caras.ForEach((cara) => cara.RenderizaRayos());
        }

        // TODO dispose
    }
}