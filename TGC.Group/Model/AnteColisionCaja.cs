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

        public void Colisionar(MeshTipoCaja meshTipoCaja, Personaje personaje)
        {
            if (SeEstaMoviendoHaciaMi())
            {
                var movimientoCaja = TGCMatrix.Translation(Desplazamiento(personaje)); // + distancia minima del rayo
                AgregarRozamiento(personaje);
                meshTipoCaja.Update(movimientoCaja);
            }
        }

        protected abstract bool SeEstaMoviendoHaciaMi();

        protected abstract TGCVector3 Desplazamiento(Personaje personaje);

        protected abstract void AgregarRozamiento(Personaje personaje);
    }
}
