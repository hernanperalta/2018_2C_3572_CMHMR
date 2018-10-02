using System;
using System.Collections.Generic;
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
        private TgcMesh plataforma1;
        private TgcMesh plataforma2;

        //Transformaciones
        private TGCMatrix transformacionBox;
        private TGCMatrix transformacionBox2;

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

            plataforma1 = scene2.Meshes[0];
            plataforma2 = plataforma1.createMeshInstance(plataforma1.Name + "2");

            plataforma1.AutoTransform = false;
            plataforma2.AutoTransform = false;
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
            transformacionBox2 = Mover2 * Trasladar2 * RotInversa * Trasladar2 * Rot;

            movimiento = personaje.movimiento;

            //CalcularColisionesConPlanos();

            //CalcularColisionesConMeshes();

            personaje.Movete(movimiento);
        }

        public override void CalcularColisionesConMeshes()
        {
            throw new NotImplementedException();
        }

        public override void CalcularColisionesConPlanos()
        {
            throw new NotImplementedException();
        }

        public override void Render()
        {
            //Dibujamos la escena
            scene.RenderAll();

            //Dibujar la primera plataforma en pantalla
            plataforma1.Transform = transformacionBox;
            plataforma1.Render();
            plataforma1.BoundingBox.transform(plataforma1.Transform);
            plataforma1.BoundingBox.Render();

            //Dibujar la segunda plataforma en pantalla
            plataforma2.Transform = transformacionBox2;
            plataforma2.Render();
            plataforma2.BoundingBox.transform(plataforma2.Transform);
            plataforma2.BoundingBox.Render();

            //Recalculamos la orbita de rotacion
            orbitaDeRotacion += MOVEMENT_SPEED * contexto.ElapsedTime;
        }

        public override void DisposeAll()
        {
            scene.DisposeAll();
            plataforma1.Dispose();
        }
    }
}
