using System.Collections.Generic;

namespace TGC.Group.Model
{
    internal class CaraZ : Cara
    {
        public CaraZ(MeshTipoCaja meshTipoCaja, List<IAnteColision> accionesAnteColision, List<Rayo> rayos) : base(meshTipoCaja, accionesAnteColision, rayos)
        {
        }
    }
}