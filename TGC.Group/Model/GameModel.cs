using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.SkeletalAnimation;
using TGC.Group.Camera;

namespace TGC.Group.Model
{
    /// <summary>
    ///     Ejemplo para implementar el TP.
    ///     Inicialmente puede ser renombrado o copiado para hacer m�s ejemplos chicos, en el caso de copiar para que se
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
        private bool BoundingBox { get; set; }

        private TgcScene scene;
        private TgcSkeletalMesh personaje;
        private GameCamera camara;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqu� todo el c�digo de inicializaci�n: cargar modelos, texturas, estructuras de optimizaci�n, todo
        ///     procesamiento que podemos pre calcular para nuestro juego.
        ///     Borrar el codigo ejemplo no utilizado.
        /// </summary>
        public override void Init()
        {
            //Device de DirectX para crear primitivas.
            var d3dDevice = D3DDevice.Instance.Device;

            var loader = new TgcSceneLoader();
            scene = loader.loadSceneFromFile(MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\plataformas\\plataformas-TgcScene.xml");

            var skeletalLoader = new TgcSkeletalLoader();
            personaje = skeletalLoader.loadMeshAndAnimationsFromFile(
                MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\Robot\\Robot-TgcSkeletalMesh.xml",
                MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\Robot\\",
                new[]
                {
                    MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\Robot\\Caminando-TgcSkeletalAnim.xml",
                    MediaDir + "primer-nivel\\pozo-plataformas\\tgc-scene\\Robot\\Parado-TgcSkeletalAnim.xml"
                });

            //Configurar animacion inicial
            personaje.playAnimation("Parado", true);
            //Escalarlo porque es muy grande
            personaje.Position = new TGCVector3(100, 0, 100);
            personaje.Scale = new TGCVector3(0.75f, 0.75f, 0.75f);


            camara = new GameCamera(personaje.Position, 200, -300);
            var cameraPosition = new TGCVector3(0, 0, 200);
            //Quiero que la camara mire hacia el origen (0,0,0).
            var lookAt = TGCVector3.Empty;
            //Configuro donde esta la posicion de la camara y hacia donde mira.
            Camara.SetCamera(cameraPosition, lookAt);

        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la l�gica de computo del modelo, as� como tambi�n verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        public override void Update()
        {
            PreUpdate();

            //Capturar Input teclado
            if (Input.keyPressed(Key.F))
            {
                BoundingBox = !BoundingBox;
            }

            camara.Target = personaje.Position;

            PostUpdate();
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqu� todo el c�digo referido al renderizado.
        ///     Borrar todo lo que no haga falta.
        /// </summary>
        public override void Render()
        {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones seg�n nuestra conveniencia.
            PreRender();

            personaje.animateAndRender(ElapsedTime);

            scene.RenderAll();

            //Siempre antes de renderizar el modelo necesitamos actualizar la matriz de transformacion.
            //Debemos recordar el orden en cual debemos multiplicar las matrices, en caso de tener modelos jer�rquicos, tenemos control total.
            //Box.Transform = TGCMatrix.Scaling(Box.Scale) * TGCMatrix.RotationYawPitchRoll(Box.Rotation.Y, Box.Rotation.X, Box.Rotation.Z) * TGCMatrix.Translation(Box.Position);


            //A modo ejemplo realizamos toda las multiplicaciones, pero aqu� solo nos hacia falta la traslaci�n.
            //Finalmente invocamos al render de la caja
            //Box.Render();

            //Cuando tenemos modelos mesh podemos utilizar un m�todo que hace la matriz de transformaci�n est�ndar.
            //Es �til cuando tenemos transformaciones simples, pero OJO cuando tenemos transformaciones jer�rquicas o complicadas.
            //Mesh.UpdateMeshTransform();
            //Render del mesh
            //Mesh.Render();

            //Render de BoundingBox, muy �til para debug de colisiones.
            if (BoundingBox)
            {
                //Box.BoundingBox.Render();
                //Mesh.BoundingBox.Render();
            }

            //Finaliza el render y presenta en pantalla, al igual que el preRender se debe para casos puntuales es mejor utilizar a mano las operaciones de EndScene y PresentScene
            PostRender();
        }

        /// <summary>
        ///     Se llama cuando termina la ejecuci�n del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gr�ficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            //Dispose del mesh.
            scene.DisposeAll();
            personaje.Dispose();
        }
    }
}