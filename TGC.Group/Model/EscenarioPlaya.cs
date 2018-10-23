using Microsoft.DirectX.DirectInput;
using System.Collections.Generic;
using System.Drawing;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class EscenarioPlaya : Escenario
    {
        private TgcScene escena;
        private TgcScene planos;
        public TgcMesh planoArbol;

        // Planos de limite

        public EscenarioPlaya(GameModel contexto, Personaje personaje) : base (contexto, personaje, 0, (335*(-1)))
        {
            
        }

        protected override void Init()
        {
            var MediaDir = contexto.MediaDir;
            var loader = new TgcSceneLoader();
            this.escena = loader.loadSceneFromFile(GameModel.Media + "escenarios\\playa\\playa-TgcScene.xml");
            this.planos = loader.loadSceneFromFile(GameModel.Media + "planos\\playa-TgcScene.xml");

            planoIzq = this.planos.getMeshByName("planoIzq");
            planoIzq.AutoTransform = false;

            planoDer = this.planos.getMeshByName("planoDer");
            planoDer.AutoTransform = false;

            planoBack = this.planos.getMeshByName("planoInicio");
            planoBack.AutoTransform = false;

            planoPiso = this.planos.getMeshByName("planoPiso");
            planoPiso.AutoTransform = false;

            planoFront = this.planos.getMeshByName("planoFin");
            planoFront.AutoTransform = false;

            // para probar colision con camara
            colisionablesConCamara = new List<TgcBoundingAxisAlignBox>();
            planoArbol = loader.loadSceneFromFile(MediaDir + "planos\\planoPiso-TgcScene.xml").Meshes[0];
            planoArbol.AutoTransform = false;
            planoArbol.BoundingBox.transform(TGCMatrix.Scaling(1.5f, 1, 0.5f) * TGCMatrix.Translation(-40, 30, 0));
            colisionablesConCamara.Add(planoArbol.BoundingBox);
            //

            GenerarCajas();
        }

        private void GenerarCajas() {
            var loader = new TgcSceneLoader();
            var mesh = loader.loadSceneFromFile(GameModel.Media + "objetos\\caja\\caja-TgcScene.xml").Meshes[0];
            var mesh2 = mesh.createMeshInstance("mesh2");
            cajas.Add(new Caja(new TGCVector3(0,0,-100), mesh, contexto));
            cajas.Add(new Caja(new TGCVector3(0, 20, -150), mesh2, contexto));
        }

        public override void Render() {
            escena.RenderAll();
            cajas.ForEach((caja) => { caja.Render(); });

            if (contexto.BoundingBox) {
                cajas.ForEach((caja) => {caja.RenderizaRayos(); }) ;
                planoBack.BoundingBox.Render();
                //planoFront.BoundingBox.Render();
                planoIzq.BoundingBox.Render();
                planoDer.BoundingBox.Render();
                planoPiso.BoundingBox.Render();
                planoArbol.BoundingBox.Render();
            }
        }


        //public override void Colisiones()
        //{
        //    CalcularColisionesConPlanos();

        //    CalcularColisionesConMeshes();

        //    CalcularColisionesEntreMeshes();

        //    CalcularEfectoGravedadEnMeshes();

        //    personaje.Movete(personaje.movimiento);
        //}

        public override void CalcularColisionesConPlanos()
        {
            if (personaje.moving)
            {
                //personaje.playAnimation("Caminando", true); // esto creo que esta mal, si colisiono no deberia caminar.

                if (ChocoConLimite(personaje, planoIzq))
                    NoMoverHacia(Key.A);

                if (ChocoConLimite(personaje, planoBack))
                {
                    NoMoverHacia(Key.S);
                    planoBack.BoundingBox.setRenderColor(Color.AliceBlue);
                }
                else
                { // esto no hace falta despues
                    planoBack.BoundingBox.setRenderColor(Color.Yellow);
                }

                if (ChocoConLimite(personaje, planoDer))
                    NoMoverHacia(Key.D);

                if (ChocoConLimite(personaje, planoPiso))
                {
                    if (personaje.movimiento.Y < 0)
                    {
                        personaje.movimiento.Y = 0; // Ojo, que pasa si quiero saltar desde arriba de la plataforma?
                        personaje.ColisionoEnY();
                    }
                }
            }
        }

        

        public override List<TgcBoundingAxisAlignBox> ColisionablesConCamara()
        {
            return colisionablesConCamara;
        }

        public override void DisposeAll()
        {
            planoIzq.Dispose();
            //planoFront.Dispose();
            planoPiso.Dispose();
            escena.DisposeAll();
        }
    }
}