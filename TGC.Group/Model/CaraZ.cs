using System.Collections.Generic;

namespace TGC.Group.Model
{
    internal class CaraZ : Cara
    {
        public CaraZ(MeshTipoCaja meshTipoCaja, IAnteColision accionAnteColision, List<Rayo> rayos) : base(meshTipoCaja, accionAnteColision, rayos)
        {
        }
    }
}