using Microsoft.DirectX.DirectInput;
using System.Collections.Generic;
using System.Drawing;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class EscenarioPiramide : Escenario
    {
        //Escenas
        private TgcScene planos;
        private List<Escalon> escalones;
        
        public EscenarioPiramide(GameModel contexto, Personaje personaje) : base(contexto, personaje) { }

        protected override void Init()
        {
            var loader = new TgcSceneLoader();
            scene = loader.loadSceneFromFile(GameModel.Media + "\\escenarios\\piramide\\piramide2-TgcScene.xml");
            this.planos = loader.loadSceneFromFile(GameModel.Media + "planos\\piramide-TgcScene.xml");

            planoIzq = planos.getMeshByName("planoIzq");
            planoIzq.AutoTransform = false;

            planoDer = planos.getMeshByName("planoDer");
            planoDer.AutoTransform = false;
            
            planoFront = planos.getMeshByName("planoFront");
            planoFront.AutoTransform = false;

            planoPiso = planos.getMeshByName("planoPiso");
            planoPiso.AutoTransform = false;

            ArmarEscalones();
        }

        public override void Update() { }

        private void ArmarEscalones() {
            escalones = new List<Escalon>();

            var frente = scene.Meshes.Find((m) => m.Name.Contains("frente"));
            frente.AutoTransform = false;
            var arriba = scene.Meshes.Find((m) => m.Name.Contains("arriba"));
            arriba.AutoTransform = false;

            escalones.Add(new Escalon(frente, arriba));

            // Segundo escalon
            var frente2 = frente.createMeshInstance("frente2");
            
            var arriba2 = arriba.createMeshInstance("arriba2");

            frente2.Transform = TGCMatrix.Translation(new TGCVector3(0, 21.27f, -21f));
            frente2.BoundingBox.transform(frente2.Transform);
            arriba2.Transform = TGCMatrix.Translation(new TGCVector3(0, 21.7f, -21f));
            arriba2.BoundingBox.transform(arriba2.Transform);

            escalones.Add(new Escalon(frente2, arriba2));

            // Tercer escalon
            var frente3 = frente.createMeshInstance("frente3");

            var arriba3 = arriba.createMeshInstance("arriba3");

            frente3.Transform = TGCMatrix.Translation(new TGCVector3(0, 43.64f, -40f));
            frente3.BoundingBox.transform(frente3.Transform);
            arriba3.Transform = TGCMatrix.Translation(new TGCVector3(0, 43.76f, -40f));
            arriba3.BoundingBox.transform(arriba3.Transform);

            escalones.Add(new Escalon(frente3, arriba3));

            // Cuarto escalon
            var frente4 = frente.createMeshInstance("frente4");

            var arriba4 = arriba.createMeshInstance("arriba4");

            frente4.Transform = TGCMatrix.Translation(new TGCVector3(0, 66.62f, -58f));
            frente4.BoundingBox.transform(frente4.Transform);
            arriba4.Transform = TGCMatrix.Translation(new TGCVector3(0, 66.28f, -58f));
            arriba4.BoundingBox.transform(arriba4.Transform);

            escalones.Add(new Escalon(frente4, arriba4));

            // Cuarto escalon
            var frente5 = frente.createMeshInstance("frente5");

            var arriba5 = scene.Meshes.Find(mesh => mesh.Name.Contains("ultimo"));

            frente5.Transform = TGCMatrix.Translation(new TGCVector3(0, 88.18f, -77f));
            frente5.BoundingBox.transform(frente5.Transform);

            escalones.Add(new Escalon(frente5, arriba5));
        }

        public override void Colisiones()
        {
            CalcularColisionesConPlanos();

            CalcularColisionesConEscalones();

            CalcularColisionesConMeshes();

            personaje.Movete(personaje.movimiento);
        }

        public void CalcularColisionesConEscalones() {
            //if (ChocoConLimite(personaje, planos.getMeshByName("planoTecho")))
            //    personaje.movimiento.Y = 0;

            escalones.ForEach((escalon) => escalon.ColisionarContra(personaje));
        }

        public override void CalcularColisionesConMeshes() { }

        public override void CalcularColisionesConPlanos()
        {
            if (personaje.moving)
            {
                if (ChocoConLimite(personaje, planoIzq))
                    NoMoverHacia(Key.A);

                if (ChocoConLimite(personaje, planoDer))
                    NoMoverHacia(Key.D);

                if (ChocoConLimite(personaje, planoFront))
                    NoMoverHacia(Key.W);
                    

                if (ChocoConLimite(personaje, planoPiso))
                {
                    if (personaje.movimiento.Y < 0)
                    {
                        personaje.movimiento.Y = 0; 
                        personaje.ColisionoEnY();
                    }
                }
            }
        }

        public override void Renderizar()
        {
            //Dibujamos la escena
            scene.Meshes.FindAll(unMesh => !unMesh.Name.Contains("Escalon"))
                        .ForEach(unMesh => unMesh.Render());

            if (contexto.BoundingBox)
            {
                escalones.ForEach(unEscalon => unEscalon.Render());
                //planoBack.BoundingBox.Render();
                //planoFront.BoundingBox.Render();
                //escalones.ForEach((e) => e.);
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
