using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class Caja : MeshTipoCaja
    {
        public Caja(TGCVector3 posicionInicial, TgcMesh mesh) : base (posicionInicial, mesh){

        }

        public override void Update(TGCMatrix movimientoCaja)
        {
            base.Update(movimientoCaja);
            mesh.Transform *= movimientoCaja;
            mesh.BoundingBox.transform(mesh.Transform);
            GenerarRayos();
        }

        protected override int ModificacionEnY()
        {
            return 2;
        }


    }
}
