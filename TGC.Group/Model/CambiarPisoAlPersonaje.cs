using TGC.Core.Mathematica;

namespace TGC.Group.Model
{
    public class CambiarPisoAlPersonaje : AnteColisionCaja
    {
        public override void Colisionar(MeshTipoCaja mesh, Colisionable colisionable) {
            if (colisionable.movimiento.Y <= 0)
            {
                //var centroCaraY = CaraBuilder.Instance().Mesh(mesh).HallarCentroDeCara("y");
                //personaje.Mesh.Transform *= TGCMatrix.Translation(personaje.Position.X, centroCaraY.Y, personaje.Position.Z); ;
                colisionable.movimiento.Y = 0;
                colisionable.ColisionoEnY(); 
            }
        }
        
    }
}