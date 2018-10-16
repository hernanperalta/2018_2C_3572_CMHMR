using System.Collections.Generic;

namespace TGC.Group.Model
{
    internal class CaraY : Cara
    {
        public CaraY(MeshTipoCaja meshTipoCaja, List<IAnteColision> accionesAnteColision, List<Rayo> rayos) : base(meshTipoCaja, accionesAnteColision, rayos)
        {
        }
    }
}