using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace TGC.Group.Model
{
    public class DesplazarEnZ : AnteColisionCaja
    {
        public DesplazarEnZ() : base(){
        }

        protected override void AgregarRozamiento(Personaje personaje)
        {
            personaje.movimiento.Z /= 3;
        }

        protected override TGCVector3 Desplazamiento(Personaje personaje)
        {
            return new TGCVector3(0, 0, personaje.movimiento.Z);
        }

        protected override bool SeEstaMoviendoHaciaMi(Personaje personaje)
        {
            return personaje.movimiento.Z < 0;
        }
    }
}
