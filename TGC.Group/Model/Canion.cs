using Microsoft.DirectX.Direct3D;
using System;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;

namespace TGC.Group.Model
{
    public class Canion
    {
        private TGCVector3 posicionInicial;
        private TgcMesh mesh;
        private Effect efectoDisparos;
        private TgcMesh planoDisparos;
        private float time;
        private GameModel contexto;

        public Canion(GameModel contexto, TGCVector3 posicionInicial)
        {
            this.contexto = contexto;
            this.posicionInicial = posicionInicial;

            var loader = new TgcSceneLoader();
            mesh = loader.loadSceneFromFile(GameModel.Media + "\\objetos\\canion\\canion-TgcScene.xml").Meshes[0];
            mesh.AutoTransform = false;
            mesh.Transform = TGCMatrix.Scaling(0.3f,0.3f,0.3f) * TGCMatrix.Translation(posicionInicial);
            mesh.BoundingBox.transform(mesh.Transform);

            planoDisparos = loader.loadSceneFromFile(GameModel.Media + "\\objetos\\disparos-canion\\disparos-TgcScene.xml").Meshes[0];
            planoDisparos.AutoTransform = false;
            planoDisparos.Transform = TGCMatrix.Scaling(0.3f, 0.08f, 0.5f) * TGCMatrix.Translation(new TGCVector3(posicionInicial.X, posicionInicial.Y - 1, posicionInicial.Z));
            planoDisparos.BoundingBox.transform(mesh.Transform);

            //Cargar Shader personalizado
            efectoDisparos = TgcShaders.loadEffect(GameModel.Media + "\\Shaders\\CanionShader.fx");

            // le asigno el efecto a la malla
            //planoDisparos.Effect = efectoDisparos;

            //// indico que tecnica voy a usar
            //// Hay effectos que estan organizados con mas de una tecnica.
            //planoDisparos.Technique = "DIFFUSE_MAP";

            time = 0;
        }

        public void Render() {
            time += contexto.ElapsedTime;
            //efectoDisparos.SetValue("time", time + contexto.ElapsedTime);
            Console.WriteLine(String.Format("time : {0}", time));

            mesh.Render();
            planoDisparos.Render();
        }

        public void Dispose() {
            mesh.Dispose();
            planoDisparos.Dispose();
        }
    }
}