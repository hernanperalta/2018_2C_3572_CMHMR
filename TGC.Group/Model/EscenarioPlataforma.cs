using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class EscenarioPlataforma : Escenario
    {

        //Escenas
        private TgcScene scene;

        //Plataformas
        private TgcMesh plataforma1Mesh;
        //private TgcMesh plataforma2Mesh;
        private MeshTipoCaja plataforma1;
        //private MeshTipoCaja plataforma2;
        //Transformaciones
        private TGCMatrix transformacionBox;
        //private TGCMatrix transformacionBox2;

        //Constantes para velocidades de movimiento
        private const float MOVEMENT_SPEED = 1f;
        private float orbitaDeRotacion;

        public EscenarioPlataforma(GameModel contexto, Personaje personaje) : base(contexto, personaje)
        {

        }

        protected override void Init()
        {
            var loader = new TgcSceneLoader();
            scene = loader.loadSceneFromFile(GameModel.Media + "\\primer-nivel\\pozo-plataformas\\tgc-scene\\plataformas\\escenarioPlataformas-TgcScene.xml");

            var scene2 = loader.loadSceneFromFile(GameModel.Media + "\\primer-nivel\\pozo-plataformas\\tgc-scene\\plataformas\\plataforma-TgcScene.xml");

            plataforma1Mesh = scene2.Meshes[0];
            //plataforma2Mesh = plataforma1Mesh.createMeshInstance(plataforma1Mesh.Name + "2");

            plataforma1Mesh.AutoTransform = false;
            //plataforma2Mesh.AutoTransform = false;

            plataforma1 = new MeshTipoCaja(new TGCVector3(0,0,0), plataforma1Mesh, false);
            //plataforma2 = new MeshTipoCaja(new TGCVector3(0, 0, 0), plataforma2Mesh);

            planoIzq = loader.loadSceneFromFile(contexto.MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\plataformas\\planoHorizontal-TgcScene.xml").Meshes[0];
            planoIzq.AutoTransform = false;

            planoDer = planoIzq.createMeshInstance("planoDer");
            planoDer.AutoTransform = false;
            planoDer.Transform = TGCMatrix.Translation(-35, -15, -357) * TGCMatrix.Scaling(1, 2f, 1.1f);
            planoDer.BoundingBox.transform(planoDer.Transform);

            planoIzq.Transform = TGCMatrix.Translation(0, -15, -357) * TGCMatrix.Scaling(1, 2f, 1.1f);
            planoIzq.BoundingBox.transform(planoIzq.Transform);

            planoFront = loader.loadSceneFromFile(contexto.MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\plataformas\\planoVertical-TgcScene.xml").Meshes[0];
            planoFront.AutoTransform = false;

            planoBack = planoFront.createMeshInstance("planoBack");
            planoBack.AutoTransform = false;
            planoBack.Transform = TGCMatrix.Translation(50, 0, -350);
            planoBack.BoundingBox.transform(planoBack.Transform);

            planoFront.Transform = TGCMatrix.Translation(50, 0, -535);
            planoFront.BoundingBox.transform(planoFront.Transform);

            planoPiso = loader.loadSceneFromFile(contexto.MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\plataformas\\planoPiso-TgcScene.xml").Meshes[0];
            planoPiso.AutoTransform = false;
            planoPiso.BoundingBox.transform(TGCMatrix.Scaling(1, 1, 2) * TGCMatrix.Translation(-22, -20, -200));

        }

        public override void Update()
        {
            //Muevo las plataformas
            var Mover = TGCMatrix.Translation(0, 0, -10);
            var Mover2 = TGCMatrix.Translation(0, 0, 65);

            //Punto por donde va a rotar
            var Trasladar = TGCMatrix.Translation(0, 0, 10);
            var Trasladar2 = TGCMatrix.Translation(0, 0, -10);

            //Aplico la rotacion
            var Rot = TGCMatrix.RotationX(orbitaDeRotacion);

            //Giro para que la caja quede derecha
            var RotInversa = TGCMatrix.RotationX(-orbitaDeRotacion);

            transformacionBox = Mover * Trasladar * Rot * Trasladar * RotInversa;
            //transformacionBox2 = Mover2 * Trasladar2 * RotInversa * Trasladar2 * Rot;

            plataforma1.Update(transformacionBox);
            //plataforma2.Update(transformacionBox2);
        }

        public override void Colisiones()
        {
            movimiento = personaje.movimiento;

            CalcularColisionesConPlanos();

            CalcularColisionesConMeshes();

            personaje.Movete(movimiento);
        }

        public override void CalcularColisionesConMeshes()
        {
            if (plataforma1.ChocoArriba(personaje))
            {
                if (movimiento.Y < 0)
                {
                    movimiento.Y = 0;
                    personaje.ColisionoEnY();
                }
                personaje.TransformPlataforma = transformacionBox;
            }
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

                if (ChocoConLimite(personaje, planoFront))
                { // HUBO CAMBIO DE ESCENARIO
                  /* Aca deberiamos hacer algo como no testear mas contra las cosas del escenario anterior y testear
                    contra las del escenario actual. 
                  */

                    planoFront.BoundingBox.setRenderColor(Color.AliceBlue);
                }
                else
                {
                    planoFront.BoundingBox.setRenderColor(Color.Yellow);
                }

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

        public override void Render()
        {
            //Dibujamos la escena
            scene.RenderAll();

            //Dibujar la primera plataforma en pantalla
            plataforma1Mesh.Transform = transformacionBox;
            plataforma1Mesh.Render();
            plataforma1Mesh.BoundingBox.transform(plataforma1Mesh.Transform);
            plataforma1Mesh.BoundingBox.Render();

            //Dibujar la segunda plataforma en pantalla
            //plataforma2Mesh.Transform = transformacionBox2;
            //plataforma2Mesh.Render();
            //plataforma2Mesh.BoundingBox.transform(plataforma2Mesh.Transform);
            //plataforma2Mesh.BoundingBox.Render();

            if (contexto.BoundingBox)
            {
                planoBack.BoundingBox.Render();
                planoFront.BoundingBox.Render();
                planoIzq.BoundingBox.Render();
                planoDer.BoundingBox.Render();
                planoPiso.BoundingBox.Render();
                plataforma1.RenderizaRayos();
                //plataforma2.RenderizaRayos();
            }

            //Recalculamos la orbita de rotacion
            orbitaDeRotacion += MOVEMENT_SPEED * contexto.ElapsedTime;
        }

        public override void DisposeAll()
        {
            scene.DisposeAll();
            plataforma1Mesh.Dispose();
        }
    }
}
