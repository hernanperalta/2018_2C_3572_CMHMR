using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class CaraBuilder
    {
        private List<Rayo> rayos;
        private TgcMesh mesh;

        private CaraBuilder()
        {
            rayos = new List<Rayo>();
        }

        public static CaraBuilder Instance()
        {
            return new CaraBuilder();
        }

        public CaraBuilder Mesh(TgcMesh mesh) {
            this.mesh = mesh;
            return this;
        }

        public CaraBuilder caraMenosX(TgcMesh mesh, int modificacionEnY)
        {
            var rayo1 = new RayoX(new TGCVector3(centroCaraX.X, centroCaraX.Y * modificacionEnY, centroCaraX.Z), new TGCVector3(1, 0, 0));
            var rayo2 = new RayoX(new TGCVector3(centroCaraX.X, centroCaraX.Y * modificacionEnY, mesh.BoundingBox.PMin.Z), new TGCVector3(1, 0, 0));
            var rayo3 = new RayoX(new TGCVector3(centroCaraX.X, centroCaraX.Y * modificacionEnY, mesh.BoundingBox.PMax.Z), new TGCVector3(1, 0, 0));

            rayos.Add(rayo1);
            rayos.Add(rayo2);
            rayos.Add(rayo2);

            return this;
        }
    }
}
