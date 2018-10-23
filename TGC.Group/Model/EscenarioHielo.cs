using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class EscenarioHielo : Escenario
    {
        //Escenas
        private TgcScene planos;
        public EscenarioHielo(GameModel contexto, Personaje personaje) : base(contexto, personaje, 0 ,0) { }

        protected override void Init()
        {
            var loader = new TgcSceneLoader();
            scene = loader.loadSceneFromFile(GameModel.Media + "\\escenarios\\hielo\\hielo-TgcScene.xml");
            this.planos = loader.loadSceneFromFile(GameModel.Media + "planos\\hielo-TgcScene.xml");

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

        }


        //public override void CalcularColisionesConMeshes() { }

        public override void CalcularColisionesConPlanos()
        {
            if (personaje.moving)
            {
                //personaje.playAnimation("Caminando", true); // esto creo que esta mal, si colisiono no deberia caminar.

                if (ChocoConLimite(personaje, planoIzq))
                    NoMoverHacia(Key.A);

                //if (ChocoConLimite(personaje, planoBack))
                //{
                //    planoBack.BoundingBox.setRenderColor(Color.AliceBlue);
                //}
                //else
                //{ // esto no hace falta despues
                //    planoBack.BoundingBox.setRenderColor(Color.Yellow);
                //}

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
                    
                    personaje.DesplazarConInercia();
                    if (personaje.movimiento.Y < 0)
                    {
                        personaje.movimiento.Y = 0; // Ojo, que pasa si quiero saltar desde arriba de la plataforma?
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
                //planoBack.BoundingBox.Render();
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
