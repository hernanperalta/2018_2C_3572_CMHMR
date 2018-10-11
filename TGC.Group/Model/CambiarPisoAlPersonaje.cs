using TGC.Core.Mathematica;

namespace TGC.Group.Model
{
    public class CambiarPisoAlPersonaje : AnteColisionCaja
    {
        public override void Colisionar(MeshTipoCaja mesh, Personaje personaje) {
            //if (personaje.movimiento.Y <= 0)
            //{
                personaje.movimiento.Y = 0;
                personaje.ColisionoEnY();
            //}
        }
        
    }
}