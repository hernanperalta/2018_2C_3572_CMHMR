using System;
using TGC.Core.Collision;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class Escalon 
    {
        //private IAnteColision accionFrente;
        //private IAnteColision accionArriba;
        private TgcMesh frente;
        private TgcMesh arriba;

        public Escalon(TgcMesh frente, TgcMesh arriba)
        {
            this.frente = frente;
            this.arriba = arriba;
            //this.accionFrente = new ChoqueRigido(Eje.Z);
            //this.accionArriba = new CambiarPisoAlPersonaje();
        }

        public void Render()
        {
            frente.BoundingBox.Render();
            arriba.BoundingBox.Render();
        }

        public void ColisionarContra(Personaje personaje) {
            if (TgcCollisionUtils.testAABBAABB(frente.BoundingBox, personaje.BoundingBox()))
            {
                //accionFrente.Colisionar(, personaje);
                if (personaje.movimiento.Z < 0)
                {
                    personaje.movimiento.Z = 0;
                }
            }
            else if (TgcCollisionUtils.testAABBAABB(arriba.BoundingBox, personaje.BoundingBox())) {
                if (personaje.movimiento.Y <= 0)
                {
                    personaje.movimiento.Y = 0;
                    personaje.ColisionoEnY();
                }
            }
                
        }
    }
}

