using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class Canion
    {
        private TGCVector3 posicionInicial;
        private TgcMesh mesh;

        public Canion(TGCVector3 posicionInicial)
        {
            this.posicionInicial = posicionInicial;

            var loader = new TgcSceneLoader();
            mesh = loader.loadSceneFromFile(GameModel.Media + "\\objetos\\canion\\canion-TgcScene.xml").Meshes[0];
            mesh.AutoTransform = false;
            mesh.Transform = TGCMatrix.Scaling(0.3f,0.3f,0.3f) * TGCMatrix.Translation(posicionInicial);
            mesh.BoundingBox.transform(mesh.Transform);
        }

        public void Render() {
            mesh.Render();
        }

        public void Dispose() {
            mesh.Dispose();
        }
    }
}