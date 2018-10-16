using Microsoft.DirectX.DirectInput;
using System;
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
        public TgcMesh planoArbol;
        private List<Caja> cajas;

        // Planos de limite

        public EscenarioPlaya(GameModel contexto, Personaje personaje) : base (contexto, personaje, 0, (335*(-1)))
        {
            
        }

        protected override void Init()
        {
            var MediaDir = contexto.MediaDir;
            var loader = new TgcSceneLoader();
            this.escena = loader.loadSceneFromFile(MediaDir + "escenarios\\playa\\playa-TgcScene.xml");

            planoIzq = loader.loadSceneFromFile(MediaDir + "planos\\planoHorizontal-TgcScene.xml").Meshes[0];
            planoIzq.AutoTransform = false;

            planoDer = planoIzq.createMeshInstance("planoDer");
            planoDer.AutoTransform = false;
            planoDer.Transform = TGCMatrix.Translation(-38, 0, -43) * TGCMatrix.Scaling(1, 1, 3f);
            planoDer.BoundingBox.transform(planoDer.Transform);

            planoIzq.Transform = TGCMatrix.Translation(0, 0, -43) * TGCMatrix.Scaling(1, 1, 3f);
            planoIzq.BoundingBox.transform(planoIzq.Transform);

            //planoFront = loader.loadSceneFromFile(MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\plataformas\\planoVertical-TgcScene.xml").Meshes[0];
            //planoFront.AutoTransform = false;

            planoBack = loader.loadSceneFromFile(MediaDir + "planos\\planoVertical-TgcScene.xml").Meshes[0];
            planoBack.AutoTransform = false;
            planoBack.Transform = TGCMatrix.Translation(50, 0, 70);
            planoBack.BoundingBox.transform(planoBack.Transform);

            //planoFront.Transform = TGCMatrix.Translation(50, 0, -330);
            //planoFront.BoundingBox.transform(planoFront.Transform);

            planoPiso = loader.loadSceneFromFile(MediaDir + "planos\\planoPiso-TgcScene.xml").Meshes[0];
            planoPiso.AutoTransform = false;
            planoPiso.BoundingBox.transform(TGCMatrix.Scaling(1, 1, 2.9f) * TGCMatrix.Translation(-25, 0, 250));


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