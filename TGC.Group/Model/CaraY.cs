using System.Collections.Generic;

namespace TGC.Group.Model
{
    internal class CaraY : Cara
    {
        public CaraY(MeshTipoCaja meshTipoCaja, IAnteColision accionAnteColision, List<Rayo> rayos) : base(meshTipoCaja, accionAnteColision, rayos)
        {
        }
    }
}