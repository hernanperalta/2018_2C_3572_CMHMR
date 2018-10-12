using Microsoft.DirectX.DirectInput;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Geometry;
using TGC.Group.Camera;
using System;
using System.Collections.Generic;

namespace TGC.Group.Model
{
    /// <summary>
    ///     Ejemplo para implementar el TP.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar el modelo que instancia GameForm <see cref="Form.GameForm.InitGraphics()" />
    ///     line 97.
    /// </summary>
    public class GameModel : TgcExample
    {
        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        /// <param name="mediaDir">Ruta donde esta la carpeta con los assets</param>
        /// <param name="shadersDir">Ruta donde esta la carpeta con los shaders</param>
        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir)
        {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }

        //Boleano para ver si dibujamos el boundingbox
        public static String Media
        {
            get => "../../Media/";
        }
        public bool BoundingBox { get; set; }
        private const float VELOCIDAD_DESPLAZAMIENTO = 50f;
        private Personaje personaje;
        private GameCamera camara;
        
        private Dictionary<string, Escenario> escenarios;
        private Escenario escenarioActual;


        //Constantes para velocidades de movimiento de plataforma
        private const float MOVEMENT_SPEED = 1f;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: cargar modelos, texturas, estructuras de optimización, todo
        ///     procesamiento que podemos pre calcular para nuestro juego.
        ///     Borrar el codigo ejemplo no utilizado.
        /// </summary>
        public override void Init()
        {
            //Device de DirectX para crear primitivas.
            var d3dDevice = D3DDevice.Instance.Device;

            personaje = new Personaje(this);

            escenarios = new Dictionary<string, Escenario>();

           
            //escenarios["plataforma"] = new EscenarioPlataforma(this, personaje);
            var escenarioPlataforma = new EscenarioPlataforma(this, personaje);

            var escenarioPlaya = new EscenarioPlaya(this, personaje);
            escenarioPlaya.siguiente = escenarioPlataforma;

            //escenarios["playa"] = new EscenarioPlaya(this, personaje);

            escenarios["playa"] = escenarioPlaya;
            escenarios["plataforma"] = escenarioPlataforma;

            escenarioActual = escenarios["playa"];

            //(escenarios["playa"]).siguiente = (Escenario)escenarios["plataforma"];
            //(escenarios["plataforma"]).anterior = (Escenario)escenarios["playa"];

            BoundingBox = true;

            camara = new GameCamera(personaje.Position, 60, 200);
            Camara = camara;
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        public override void Update()
        {
            PreUpdate();

            //movimientoCaja = TGCMatrix.Identity;

            //// Agrego a la lista de meshes colisionables tipo caja, todas las cosas del pedazo de escenario donde estoy contra las que puedo colisionar.
            //caja1Mesh = new MeshTipoCaja(caja1);

            //meshesColisionables = new ArrayList();

            //meshesColisionables.Add(caja1Mesh);
            //// 

            personaje.Update();

            //escenarioActual.Update();
            foreach(Escenario escenario in escenarios.Values)
            {
                escenario.Update();
            }

            escenarioActual.Colisiones();
            //escenarioActual.Update();

            if (personaje.Mesh.Transform.Origin.Z < -335 /*escenarioActual.farLimit*/)
            { // HUBO CAMBIO DE ESCENARIO
              /* Aca deberiamos hacer algo como no testear mas contra las cosas del escenario anterior y testear
                contra las del escenario actual. 
              */

                //planoFront.BoundingBox.setRenderColor(Color.AliceBlue);
                escenarioActual = escenarios["plataforma"]; /*escenarioActual.siguiente*/
            }
            else
            {
                //planoFront.BoundingBox.setRenderColor(Color.Yellow);
                escenarioActual = escenarios["playa"]; // nada
            }

            if (Input.keyPressed(Key.Q))
            {
                BoundingBox = !BoundingBox;
            }

            camara.Target = personaje.Position;

            PostUpdate();
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        ///     Borrar todo lo que no haga falta.
        /// </summary>
        public override void Render()
        {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
            PreRender();

            personaje.Render();

            foreach (Escenario escenario in escenarios.Values)
            {
                escenario.Render();
            }

            //escenarioActual.Render();

            //Finaliza el render y presenta en pantalla, al igual que el preRender se debe para casos puntuales es mejor utilizar a mano las operaciones de EndScene y PresentScene
            PostRender();
        }


        /// <summary>
        ///     Se llama cuando termina la ejecución del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gráficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            //Dispose del mesh.
            escenarioActual.DisposeAll();
            personaje.Dispose();
            //escenarioActual.planoIzq.Dispose(); // solo se borran los originales
            //escenarioActual.planoFront.Dispose(); // solo se borran los originales
            //escenarioActual.planoPiso.Dispose();

            //foreach (TgcMesh mesh in meshesColisionables) {
            //    mesh.Dispose(); // mmm, no se que pasaria con las instancias...
            //} // recontra TODO
        }
    }
}