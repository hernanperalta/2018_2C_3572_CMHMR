using Microsoft.DirectX.DirectInput;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Group.Camera;
using System;
using System.Collections.Generic;
using TGC.Core.BoundingVolumes;
using TGC.Group.Model.Escenarios;
using Microsoft.DirectX.Direct3D;
using TGC.Core.Text;
using System.Drawing;
using TGC.Core.Sound;
using TGC.Core.Mathematica;
using TGC.Group.Model.Coleccionables;

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
        private bool muteado = true;
        private const float VELOCIDAD_DESPLAZAMIENTO = 50f;
        private Personaje personaje;
        public GameCamera camara;
        
        private Dictionary<string, Escenario> escenarios;
        public Escenario escenarioActual;


        //Constantes para velocidades de movimiento de plataforma
        private const float MOVEMENT_SPEED = 1f;
        public Texture TexturaVidas;
        public Texture TexturaDuraznos;
        public TgcText2D textoVidas;
        public TgcText2D textoDuraznos;
        public TgcText2D ayuda;
        public TgcMp3Player cancionPpal = new TgcMp3Player();
        public TgcMp3Player woah = new TgcMp3Player();

        public List<TgcBoundingAxisAlignBox> ColisionablesConCamara()
        {
            return escenarioActual.ColisionablesConCamara();
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: cargar modelos, texturas, estructuras de optimización, todo
        ///     procesamiento que podemos pre calcular para nuestro juego.
        ///     Borrar el codigo ejemplo no utilizado.
        /// </summary>
        public override void Init()
        {
            personaje = new Personaje(this);

            woah.FileName = MediaDir + "musica\\woah.mp3";
            cancionPpal.FileName = MediaDir + "musica\\crash.mp3";

            cargarEscenarios();

            CargarHud();

            CargarAyuda();

            BoundingBox = false;
        }

        public void CargarHud()
        {
            //Device de DirectX para crear primitivas.
            var d3dDevice = D3DDevice.Instance.Device;
            var viewport = D3DDevice.Instance.Device.Viewport;

            TexturaVidas = TextureLoader.FromFile(d3dDevice, MediaDir + "\\sprites\\vida.png");

            TexturaDuraznos = TextureLoader.FromFile(d3dDevice, MediaDir + "\\sprites\\durazno.png");
            
            textoVidas = new TgcText2D();
            textoVidas.Position = new Point(viewport.Width - 96, 0);
            textoVidas.Size = new Size(64, 32);
            textoVidas.changeFont(new System.Drawing.Font("TimesNewRoman", 23, FontStyle.Bold));
            textoVidas.Color = Color.Yellow;
            textoVidas.Align = TgcText2D.TextAlign.RIGHT;
            textoVidas.Text = personaje.Vidas.ToString();
            
            textoDuraznos = new TgcText2D();
            textoDuraznos.Position = new Point(viewport.Width - 96, 64);
            textoDuraznos.Size = new Size(64, 32);
            textoDuraznos.changeFont(new System.Drawing.Font("TimesNewRoman", 23, FontStyle.Bold));
            textoDuraznos.Color = Color.Yellow;
            textoDuraznos.Align = TgcText2D.TextAlign.RIGHT;
            textoDuraznos.Text = personaje.Duraznos.ToString();
        }

        private void CargarAyuda()
        {
            ayuda = new TgcText2D();
            ayuda.Position = new Point(10, 10);
            ayuda.Size = new Size(500, 500);
            ayuda.changeFont(new System.Drawing.Font("Arial", 23, FontStyle.Regular));
            ayuda.Color = Color.Black;
            ayuda.Align = TgcText2D.TextAlign.LEFT;
            ayuda.Text = "Ayuda:\n" +
                "Q: mostrar/ocultar bounding box\n" +
                "M: mutear/desmutear\n" +
                "G: god mode\n";
        }

        public void cargarEscenarios()
        {
            escenarios = new Dictionary<string, Escenario>();

            escenarios["playa"] = new EscenarioPlaya(this, personaje);

            escenarios["plataforma"] = new EscenarioPlataforma(this, personaje);

            escenarios["playa"].siguiente = escenarios["plataforma"];

          //escenarios["camino"] = new EscenarioCamino(this, personaje);

            escenarios["pozo"] = new EscenarioPozo(this, personaje);

            escenarios["piramide"] = new EscenarioPiramide(this, personaje);

            escenarios["hielo"] = new EscenarioHielo(this, personaje);

            escenarios["menu"] = new EscenarioMenu(this, personaje);
            
            escenarioActual = escenarios["menu"];
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        /// 

        public bool between(float num, float lower, float upper)
        {
            return (lower <= num && num < upper);
        }

        public void actualizarEscenario()
        {
            if (escenarioActual is EscenarioMenu)
                return;

            float posicionMeshEjeZ = personaje.Mesh.Transform.Origin.Z;

            if (between(posicionMeshEjeZ, -330f, 0f))
                escenarioActual = escenarios["playa"];

            if (between(posicionMeshEjeZ, -465f, -330f))
                escenarioActual = escenarios["plataforma"];

            if (between(posicionMeshEjeZ, -600 , -465f))
                escenarioActual = escenarios["hielo"];

            //if (between(posicionMeshEjeZ, ???f, -465f))
            //    escenarioActual = escenarios["plataforma"];
        }

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
            actualizarEscenario();

            escenarioActual.AplicarGravedad();

            foreach (Escenario escenario in escenarios.Values)
            {
                escenario.Update();
            }

            
            escenarioActual.Colisiones();

            if (Input.keyPressed(Key.Q))
            {
                BoundingBox = !BoundingBox;
            }

            if (Input.keyPressed(Key.M))
                muteado = !muteado;

            if (muteado)
            {
                cancionPpal.pause();
            }
            else
            {
                cancionPpal.resume();
            }

            if (cancionPpal.getStatus() != TgcMp3Player.States.Playing && !muteado)
            {
                cancionPpal.closeFile();
                cancionPpal.play(true);
            }

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

            escenarioActual.RenderHud();

            ayuda.render();

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

        public void CambiarEscenario(string nombre)
        {
            escenarioActual = escenarios[nombre];
        }

        public void Empezar()
        {
            CambiarEscenario("playa");

            camara = new GameCamera(personaje.Position, 60, 200, this);
            Camara = camara;

            personaje.SetUp();

            textoVidas.Text = personaje.Vidas.ToString();
            textoDuraznos.Text = personaje.Duraznos.ToString();
        }

        public void ReproducirWoah()
        {
            cancionPpal.pause();
            woah.closeFile();
            woah.play(false);
            cancionPpal.resume();
        }

        public void VolverAMenu()
        {
            CambiarEscenario("menu");
            Camara = new Core.Camara.TgcCamera();
            var lookAt = new TGCVector3(0, 50, 400);
            Camara.SetCamera(new TGCVector3(lookAt.X, lookAt.Y, lookAt.Z + 30), lookAt);
        }

        public void ResetearColisionables()
        {
            foreach (Escenario escenario in escenarios.Values)
                escenario.ResetearColisionables();
        }
    }
}