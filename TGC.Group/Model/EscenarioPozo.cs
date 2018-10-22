using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class EscenarioPozo : Escenario
    {
        public EscenarioPozo(GameModel contexto, Personaje personaje) : base(contexto, personaje) { }
    
        protected override void Init()
        {
            var loader = new TgcSceneLoader();
            scene = loader.loadSceneFromFile(GameModel.Media + "\\escenarios\\pozos\\pozos-TgcScene.xml");

            planoIzq = loader.loadSceneFromFile(contexto.MediaDir + "planos\\planoHorizontal-TgcScene.xml").Meshes[0];
            planoIzq.AutoTransform = false;

            planoDer = planoIzq.createMeshInstance("planoDer");
            planoDer.AutoTransform = false;
            planoDer.Transform = TGCMatrix.Translation(-35, -15, -357) * TGCMatrix.Scaling(1, 2f, 1.1f);
            planoDer.BoundingBox.transform(planoDer.Transform);

            planoIzq.Transform = TGCMatrix.Translation(0, -15, -357) * TGCMatrix.Scaling(1, 2f, 1.1f);
            planoIzq.BoundingBox.transform(planoIzq.Transform);

            //planoFront = loader.loadSceneFromFile(contexto.MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\plataformas\\planoVertical-TgcScene.xml").Meshes[0];
            //planoFront.AutoTransform = false;

            planoBack = loader.loadSceneFromFile(contexto.MediaDir + "planos\\planoVertical-TgcScene.xml").Meshes[0]; //planoFront.createMeshInstance("planoBack");
            planoBack.AutoTransform = false;
            planoBack.Transform = TGCMatrix.Translation(50, 0, -350);
            planoBack.BoundingBox.transform(planoBack.Transform);

            //planoFront.Transform = TGCMatrix.Translation(50, 0, -535);
            //planoFront.BoundingBox.transform(planoFront.Transform);

            planoPiso = loader.loadSceneFromFile(contexto.MediaDir + "planos\\planoPiso-TgcScene.xml").Meshes[0];
            planoPiso.AutoTransform = false;
            planoPiso.BoundingBox.transform(TGCMatrix.Scaling(1, 1, 2) * TGCMatrix.Translation(-22, -20, -200));

        }

        public override void Update(){}

        public override void Colisiones()
        {
            movimiento = personaje.movimiento;

            CalcularColisionesConPlanos();

            CalcularColisionesConMeshes();

            personaje.Movete(movimiento);
        }

        public override void CalcularColisionesConMeshes(){}

        public override void CalcularColisionesConPlanos()
        {
            if (personaje.moving)
            {
                //personaje.playAnimation("Caminando", true); // esto creo que esta mal, si colisiono no deberia caminar.

                if (ChocoConLimite(personaje, planoIzq))
                    NoMoverHacia(Key.A);

                if (ChocoConLimite(personaje, planoBack))
                {
                    planoBack.BoundingBox.setRenderColor(Color.AliceBlue);
                }
                else
                { // esto no hace falta despues
                    planoBack.BoundingBox.setRenderColor(Color.Yellow);
                }

                if (ChocoConLimite(personaje, planoDer))
                    NoMoverHacia(Key.D);

                //if (ChocoConLimite(personaje, planoFront))
                //{ // HUBO CAMBIO DE ESCENARIO
                //  /* Aca deberiamos hacer algo como no testear mas contra las cosas del escenario anterior y testear
                //    contra las del escenario actual. 
                //  */

                //    planoFront.BoundingBox.setRenderColor(Color.AliceBlue);
                //}
                //else
                //{
                //    planoFront.BoundingBox.setRenderColor(Color.Yellow);
                //}

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

        public override void Renderizar()
        {
            //Dibujamos la escena
            scene.RenderAll();

            if (contexto.BoundingBox)
            {
                planoBack.BoundingBox.Render();
                //planoFront.BoundingBox.Render();
                planoIzq.BoundingBox.Render();
                planoDer.BoundingBox.Render();
                planoPiso.BoundingBox.Render();
                //plataforma1.RenderizaRayos();
                //plataforma2.RenderizaRayos();
            }
        }

        public override void DisposeAll()
        {
            scene.DisposeAll();
        }
    }
}
