using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class EscenarioPozo : Escenario
    {
        private TgcScene planos;

        public EscenarioPozo(GameModel contexto, Personaje personaje) : base(contexto, personaje) { }
    
        protected override void Init()
        {
            var loader = new TgcSceneLoader();
            scene = loader.loadSceneFromFile(GameModel.Media + "\\escenarios\\pozos\\pozos-TgcScene.xml");
            this.planos = loader.loadSceneFromFile(GameModel.Media + "planos\\pozos-TgcScene.xml");

            planoIzq = planos.getMeshByName("planoIzq");
            planoIzq.AutoTransform = false;

            planoDer = planos.getMeshByName("planoDer");
            planoDer.AutoTransform = false;

            planoPiso = planos.getMeshByName("planoPiso");
            planoPiso.AutoTransform = false;
        }

        public override void Update(){}

        public override void Colisiones()
        {
            CalcularColisionesConPlanos();

            CalcularColisionesConMeshes();

            personaje.Movete(personaje.movimiento);
        }

        public override void CalcularColisionesConMeshes(){}

        public override void CalcularColisionesConPlanos()
        {
            if (personaje.moving)
            {
                if (ChocoConLimite(personaje, planoIzq))
                    NoMoverHacia(Key.A);

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
