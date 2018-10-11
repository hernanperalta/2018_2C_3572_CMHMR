using TGC.Core.Mathematica;

namespace TGC.Group.Model
{
    public class CambiarPisoAlPersonaje : AnteColisionCaja
    {
        public override void Colisionar(MeshTipoCaja mesh, Personaje personaje) {
            if (personaje.movimiento.Y <= 0)
            {
                //var centroCaraY = CaraBuilder.Instance().Mesh(mesh).HallarCentroDeCara("y");
                //personaje.Mesh.Transform *= TGCMatrix.Translation(personaje.Position.X, centroCaraY.Y, personaje.Position.Z); ;
                personaje.movimiento.Y = 0;
                personaje.ColisionoEnY();
            }
        }
        
    }
}