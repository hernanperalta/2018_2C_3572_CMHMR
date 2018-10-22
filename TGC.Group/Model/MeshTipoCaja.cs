using System.Collections.Generic;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
//using

namespace TGC.Group.Model
{
    public abstract class MeshTipoCaja : Colisionable
    {
        public TgcMesh mesh;

        protected TGCVector3 posicionInicial;
        public TGCVector3 Position {
            get => this.mesh.Position;
        }
        protected List<Cara> caras;

        public override TgcBoundingAxisAlignBox BoundingBox() {
            return this.mesh.BoundingBox;
        }

        protected MeshTipoCaja(TGCVector3 posicionInicial, TgcMesh mesh, GameModel Context) : base(Context)
        {
            this.mesh = mesh;
            this.caras = new List<Cara>();
          
            this.posicionInicial = posicionInicial;
            this.movimiento = TGCVector3.Empty;

            mesh.AutoTransform = false;
            mesh.Transform = TGCMatrix.Translation(posicionInicial);
            mesh.BoundingBox.transform(mesh.Transform);

            GenerarCaras();
        }

        public TGCVector3 Movimiento()
        {
            return this.movimiento;
        }

        protected abstract void GenerarCaras();

        protected void ClearCaras() {
            this.caras = new List<Cara>();
        }

        public void Render()
        {
            mesh.Render();
        }

        public override void Update() {
            this.movimiento = TGCVector3.Empty; 
            base.Update();
            ClearCaras();
            GenerarCaras();
        }

        public abstract void Movete();

        public void TestearColisionContra(Colisionable colisionable) {
            foreach (Cara cara in caras)
            {
                cara.TesteoDeColision(colisionable);
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