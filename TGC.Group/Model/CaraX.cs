using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    public class CaraX : Cara
    {
        public CaraX(MeshTipoCaja meshTipoCaja, IAnteColision accionAnteColision) : base(meshTipoCaja, accionAnteColision) {

        }

        protected override void GenerarRayos()
        {
            var rayoCentroCaraX = new RayoX(new TGCVector3(centroCaraX.X, centroCaraX.Y * modificacionEnY, centroCaraX.Z), new TGCVector3(1, 0, 0));
            var rayoIzqCaraX = new RayoX(new TGCVector3(centroCaraX.X, centroCaraX.Y * modificacionEnY, mesh.BoundingBox.PMin.Z), new TGCVector3(1, 0, 0));
            var rayoDerCaraX = new RayoX(new TGCVector3(centroCaraX.X, centroCaraX.Y * modificacionEnY, mesh.BoundingBox.PMax.Z), new TGCVector3(1, 0, 0));

        }
    }

    

    //CaraBuilder.caraMenosX(centro)
    //            .accionAnteColision(ac)
    //            .wild() // ahi puse a cargar :(
}
