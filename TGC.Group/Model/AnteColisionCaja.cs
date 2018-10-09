using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.Geometry;

namespace TGC.Group.Model
{
    public abstract class AnteColisionCaja : IAnteColision
    {
        public AnteColisionCaja() {

        }

        public abstract void Colisionar(MeshTipoCaja meshTipoCaja, Personaje personaje);

    }
}
