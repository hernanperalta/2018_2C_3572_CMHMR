using Microsoft.DirectX.DirectInput;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class EscenarioPlataforma : Escenario
    {
        //Escenas
        private TgcScene planos;
        //Plataformas
        private TgcMesh plataforma1Mesh;
        private TgcMesh plataforma2Mesh;

        private Plataforma plataforma1;
        private Plataforma plataforma2;
        
        //Transformaciones
        private TGCMatrix transformacionBox;
        private TGCMatrix transformacionBox2;

        //Constantes para velocidades de movimiento
        private const float MOVEMENT_SPEED = 1f;
        private float orbitaDeRotacion;

        public EscenarioPlataforma(GameModel contexto, Personaje personaje) : base(contexto, personaje, -355, -500)
        {

        }

        protected override void Init()
        {
            var loader = new TgcSceneLoader();
            scene = loader.loadSceneFromFile(GameModel.Media + "\\escenarios\\plataformas\\plataformas-TgcScene.xml");

            var scene2 = loader.loadSceneFromFile(GameModel.Media + "\\objetos\\plataforma\\plataforma-TgcScene.xml");

            plataforma1Mesh = scene2.Meshes[0];
            plataforma2Mesh = scene2.Meshes[0];

            plataforma1Mesh.AutoTransform = false;
            plataforma2Mesh.AutoTransform = false;

            plataforma1 = new Plataforma(new TGCVector3(0,0,0), plataforma1Mesh, contexto);
            plataforma2 = new Plataforma(new TGCVector3(0, 0, 0), plataforma1Mesh, contexto);

            this.planos = loader.loadSceneFromFile(GameModel.Media + "planos\\plataforma-TgcScene.xml");

            planoIzq = this.planos.getMeshByName("planoIzq");
            planoIzq.AutoTransform = false;

            planoDer = this.planos.getMeshByName("planoDer");
            planoDer.AutoTransform = false;

            planoPiso = this.planos.getMeshByName("planoPiso");
            planoPiso.AutoTransform = false;

            planoFront = this.planos.getMeshByName("planoFin");
            planoFront.AutoTransform = false;

            planoBack = this.planos.getMeshByName("planoInicio");
            planoBack.AutoTransform = false;
        }

        public override void Update()
        {
            //Muevo las plataformas
            var Mover = TGCMatrix.Translation(0, 0, -15);
            var Mover2 = TGCMatrix.Translation(0, 0, 65);

            //Punto por donde va a rotar
            var Trasladar = TGCMatrix.Translation(0, 0, 10);
            var Trasladar2 = TGCMatrix.Translation(0, 0, -10);

            //Aplico la rotacion
            var Rot = TGCMatrix.RotationX(orbitaDeRotacion);

            //Giro para que la caja quede derecha
            var RotInversa = TGCMatrix.RotationX(-orbitaDeRotacion);

            transformacionBox = Mover * Trasladar * Rot * Trasladar * RotInversa;
            transformacionBox2 = Mover2 * Trasladar2 * RotInversa * Trasladar2 * Rot;

            plataforma1.Update();
            plataforma1.transformacion = transformacionBox;
            plataforma1.Movete();


            plataforma2.Update();
            plataforma2.transformacion = transformacionBox2;
            plataforma2.Movete();


            base.Update();
            //foreach (Caja caja in cajas)
            //{

            //    Console.WriteLine(String.Format("CAJAS : ", caja.ToString()));
            //}

        }

        //public override void Colisiones()
        //{
        //    movimiento = personaje.movimiento;

        //    CalcularColisionesConPlanos();

        //    CalcularColisionesConMeshes();

        //    personaje.Movete(personaje.movimiento);
        //}

        public override void CalcularColisionesConMeshes()
        {
            plataforma1.TestearColisionContra(personaje);
            //base.CalcularColisionesConMeshes(); // 

            // NO BORRAR TODAVIA
            //if (plataforma1.TestearColisionContra(personaje))
            //{
            //    if (movimiento.Y < 0)
            //    {
            //        movimiento.Y = 0;
            //        personaje.ColisionoEnY();
            //    }
            //    personaje.TransformPlataforma = transformacionBox;
            //}
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
                    //if (personaje.movimiento.Y < 0)
                    //{
                    //    personaje.movimiento.Y = 0; // Ojo, que pasa si quiero saltar desde arriba de la plataforma?
                    //    personaje.ColisionoEnY();
                    //}
                    personaje.Restaurar();
                }

                //if()
            }
        }

        public override void Renderizar()
        {
            //Dibujamos la escena
            scene.RenderAll();
            
            //Dibujar la primera plataforma en pantalla
            plataforma1Mesh.Transform = transformacionBox;
            plataforma1Mesh.Render();
            plataforma1Mesh.BoundingBox.transform(plataforma1Mesh.Transform);
            plataforma1Mesh.BoundingBox.Render();

            //Dibujar la segunda plataforma en pantalla
            plataforma2Mesh.Transform = transformacionBox2;
            plataforma2Mesh.Render();
            plataforma2Mesh.BoundingBox.transform(plataforma2Mesh.Transform);
            plataforma2Mesh.BoundingBox.Render();

            if (contexto.BoundingBox)
            {
                planoBack.BoundingBox.Render();
                //planoFront.BoundingBox.Render();
                planoIzq.BoundingBox.Render();
                planoDer.BoundingBox.Render();
                planoPiso.BoundingBox.Render();
                plataforma1.RenderizaRayos();
                //plataforma2.RenderizaRayos();
            }

            //Recalculamos la orbita de rotacion
            orbitaDeRotacion += MOVEMENT_SPEED * contexto.ElapsedTime;
        }

        public override List<TgcBoundingAxisAlignBox> ColisionablesConCamara()
        {
            return new List<TgcBoundingAxisAlignBox>();
        }

        public override void DisposeAll()
        {
            scene.DisposeAll();
            plataforma1Mesh.Dispose();
        }

        public override void CalcularColisionesEntreMeshes()
        {
           
        }
    }
}
