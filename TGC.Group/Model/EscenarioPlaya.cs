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
        private List<TgcBoundingAxisAlignBox> colisionablesConCamara;

        private List<MeshTipoCaja> cajas;
        // Planos de limite

        public EscenarioPlaya(GameModel contexto, Personaje personaje) : base (contexto, personaje){
            
        }

        protected override void Init()
        {
            var MediaDir = contexto.MediaDir;
            var loader = new TgcSceneLoader();
            this.escena = loader.loadSceneFromFile(MediaDir + "primer-nivel\\Playa final\\Playa-TgcScene.xml");

            planoIzq = loader.loadSceneFromFile(MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\plataformas\\planoHorizontal-TgcScene.xml").Meshes[0];
            planoIzq.AutoTransform = false;

            planoDer = planoIzq.createMeshInstance("planoDer");
            planoDer.AutoTransform = false;
            planoDer.Transform = TGCMatrix.Translation(-38, 0, -43) * TGCMatrix.Scaling(1, 1, 3f);
            planoDer.BoundingBox.transform(planoDer.Transform);

            planoIzq.Transform = TGCMatrix.Translation(0, 0, -43) * TGCMatrix.Scaling(1, 1, 3f);
            planoIzq.BoundingBox.transform(planoIzq.Transform);

            //planoFront = loader.loadSceneFromFile(MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\plataformas\\planoVertical-TgcScene.xml").Meshes[0];
            //planoFront.AutoTransform = false;

            planoBack = loader.loadSceneFromFile(MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\plataformas\\planoVertical-TgcScene.xml").Meshes[0];
            planoBack.AutoTransform = false;
            planoBack.Transform = TGCMatrix.Translation(50, 0, 70);
            planoBack.BoundingBox.transform(planoBack.Transform);

            //planoFront.Transform = TGCMatrix.Translation(50, 0, -330);
            //planoFront.BoundingBox.transform(planoFront.Transform);

            planoPiso = loader.loadSceneFromFile(MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\plataformas\\planoPiso-TgcScene.xml").Meshes[0];
            planoPiso.AutoTransform = false;
            planoPiso.BoundingBox.transform(TGCMatrix.Scaling(1, 1, 2.9f) * TGCMatrix.Translation(-25, 0, 250));


            // para probar colision con camara
            colisionablesConCamara = new List<TgcBoundingAxisAlignBox>();
            planoArbol = loader.loadSceneFromFile(MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\plataformas\\planoPiso-TgcScene.xml").Meshes[0];
            planoArbol.AutoTransform = false;
            planoArbol.BoundingBox.transform(TGCMatrix.Scaling(1.5f, 1, 0.5f) * TGCMatrix.Translation(-40, 30, 0));
            colisionablesConCamara.Add(planoArbol.BoundingBox);
            //

            GenerarCajas();
        }

        private void GenerarCajas() {
            cajas = new List<MeshTipoCaja>();

            var loader = new TgcSceneLoader();
            var mesh = loader.loadSceneFromFile(GameModel.Media + "primer-nivel\\Playa final\\caja-TgcScene.xml").Meshes[0];

            cajas.Add(new MeshTipoCaja(new TGCVector3(0,0,-100), mesh, true));
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

        public override void Update()
        {
            
        }

        public override void Colisiones()
        {
            movimiento = personaje.movimiento;

            CalcularColisionesConPlanos();

            CalcularColisionesConMeshes();

            personaje.Movete(movimiento);
        }

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
                    if (movimiento.Y < 0)
                    {
                        movimiento.Y = 0; // Ojo, que pasa si quiero saltar desde arriba de la plataforma?
                        personaje.ColisionoEnY();
                    }
                }
            }
        }

        public override void CalcularColisionesConMeshes()
        {
            if (personaje.moving)
            {
                foreach (MeshTipoCaja caja in cajas)
                {
                    if (caja.ChocoConFrente(personaje))
                    {
                        var movimientoCaja = TGCMatrix.Translation(0, 0, movimiento.Z); // + distancia minima del rayo
                        movimiento.Z /= 3;
                        caja.Update(movimientoCaja);
                        break;
                    }
                    else if (caja.ChocoAtras(personaje))
                    {
                        NoMoverHacia(Key.S);
                        break;
                    }
                    else if (caja.ChocoALaIzquierda(personaje))
                    {
                        NoMoverHacia(Key.D);
                        break;
                    }
                    else if (caja.ChocoALaDerecha(personaje))
                    {
                        NoMoverHacia(Key.A);
                        break;
                    }
                    else if (caja.ChocoArriba(personaje))
                    {
                        if (movimiento.Y < 0)
                        {
                            movimiento.Y = 0; // Ojo, que pasa si quiero saltar desde arriba de la plataforma?
                            personaje.ColisionoEnY();
                        }
                        break;
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