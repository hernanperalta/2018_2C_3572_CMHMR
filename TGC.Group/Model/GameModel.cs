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
using TGC.Core.Shaders;
using Microsoft.DirectX;
using TGC.Core.SceneLoader;

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
        public TgcMp3Player cancionPpal = new TgcMp3Player();
        public TgcMp3Player woah = new TgcMp3Player();

        private Microsoft.DirectX.Direct3D.Effect sombra;
        private Texture g_pShadowMap; // Texture to which the shadow map is rendered
        private TGCMatrix g_mShadowProj; // Projection matrix for shadow map
        private Surface g_pDSShadow; // Depth-stencil buffer for rendering to shadow map
        private readonly int SHADOWMAP_SIZE = 1024;
        private readonly float far_plane = 1500f;
        private readonly float near_plane = 2f;
        private TGCVector3 g_LightDir; // direccion de la luz actual
        private TGCVector3 g_LightPos; // posicion de la luz actual (la que estoy analizando)
        private TGCMatrix g_LightView; // matriz de view del light

        TgcMesh mesh;

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

            mesh = new TgcSceneLoader().loadSceneFromFile(GameModel.Media + "objetos\\caja\\caja-TgcScene.xml").Meshes[0];
            mesh.AutoTransform = false;
            cargarEscenarios();

            CargarHud();

            CargarShaderSombra();

            BoundingBox = false;
        }

        private void CargarShaderSombra()
        {
            sombra = TgcShaders.loadEffect(MediaDir + "shaders//ShadowMap.fx");

            personaje.Mesh.Effect = sombra;
            personaje.Mesh.Technique = "RenderShadow";

            g_pShadowMap = new Texture(D3DDevice.Instance.Device, SHADOWMAP_SIZE, SHADOWMAP_SIZE, 1, Usage.RenderTarget, Format.R32F, Pool.Default);

            g_pDSShadow = D3DDevice.Instance.Device.CreateDepthStencilSurface(SHADOWMAP_SIZE, SHADOWMAP_SIZE, DepthFormat.D24S8, MultiSampleType.None, 0, true);

            var aspectRatio = D3DDevice.Instance.AspectRatio;
            g_mShadowProj = TGCMatrix.PerspectiveFovLH(Geometry.DegreeToRadian(80), aspectRatio, 50, 5000);
            D3DDevice.Instance.Device.Transform.Projection = TGCMatrix.PerspectiveFovLH(Geometry.DegreeToRadian(45.0f), aspectRatio, near_plane, far_plane).ToMatrix();

            g_LightPos = TGCVector3.Empty;
            g_LightDir = new TGCVector3(0, 0, -1);
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

            if (between(posicionMeshEjeZ, -464f, -330f))
                escenarioActual = escenarios["plataforma"];

            if (between(posicionMeshEjeZ, -598f, -464f))
                escenarioActual = escenarios["hielo"];

            if (between(posicionMeshEjeZ, -750f, -598f))
                escenarioActual = escenarios["pozo"];

            if (between(posicionMeshEjeZ, -850f, -750f))
                escenarioActual = escenarios["piramide"];
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
            

            RenderShaderSombra();

            D3DDevice.Instance.Device.BeginScene();

            D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);

            personaje.Render();

            foreach (Escenario escenario in escenarios.Values)
            {
                escenario.Render();
            }

            escenarioActual.RenderHud();

            //Finaliza el render y presenta en pantalla, al igual que el preRender se debe para casos puntuales es mejor utilizar a mano las operaciones de EndScene y PresentScene
            D3DDevice.Instance.Device.EndScene();
            D3DDevice.Instance.Device.Present();
        }

        private void RenderShaderSombra()
        {
            ClearTextures();
            //D3DDevice.Instance.Device.EndScene();

            //D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);

            RenderShadowMap();

            D3DDevice.Instance.Device.BeginScene();

            D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            //mesh.Technique = "RenderScene";
            mesh.Render();

            D3DDevice.Instance.Device.EndScene();
            //D3DDevice.Instance.Device.Present();
        }

        public void RenderShadowMap()
        {
            // Calculo la matriz de view de la luz
            sombra.SetValue("g_vLightPos", new Vector4(g_LightPos.X, g_LightPos.Y, g_LightPos.Z, 1));
            sombra.SetValue("g_vLightDir", new Vector4(g_LightDir.X, g_LightDir.Y, g_LightDir.Z, 1));
            g_LightView = TGCMatrix.LookAtLH(g_LightPos, g_LightPos + g_LightDir, new TGCVector3(0, 0, 1));

            // inicializacion standard:
            sombra.SetValue("g_mProjLight", g_mShadowProj.ToMatrix());
            sombra.SetValue("g_mViewLightProj", (g_LightView * g_mShadowProj).ToMatrix());

            // Primero genero el shadow map, para ello dibujo desde el pto de vista de luz
            // a una textura, con el VS y PS que generan un mapa de profundidades.
            var pOldRT = D3DDevice.Instance.Device.GetRenderTarget(0);
            var pShadowSurf = g_pShadowMap.GetSurfaceLevel(0);
            D3DDevice.Instance.Device.SetRenderTarget(0, pShadowSurf);
            var pOldDS = D3DDevice.Instance.Device.DepthStencilSurface;
            D3DDevice.Instance.Device.DepthStencilSurface = g_pDSShadow;
            D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            D3DDevice.Instance.Device.BeginScene();

            // Hago el render de la escena pp dicha
            sombra.SetValue("g_txShadow", g_pShadowMap);

            //mesh.Technique = "RenderShadow";
            //mesh.Render();

            // Termino
            D3DDevice.Instance.Device.EndScene();

            //TextureLoader.Save("shadowmap.bmp", ImageFileFormat.Bmp, g_pShadowMap);

            // restuaro el render target y el stencil
            D3DDevice.Instance.Device.DepthStencilSurface = pOldDS;
            D3DDevice.Instance.Device.SetRenderTarget(0, pOldRT);
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