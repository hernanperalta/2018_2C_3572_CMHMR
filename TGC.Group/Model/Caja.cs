using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Collision;

namespace TGC.Group.Model
{
    public class Caja : MeshTipoCaja 
    {
        public Caja(TGCVector3 posicionInicial, TgcMesh mesh, GameModel Context) : base (posicionInicial, mesh, Context){
            
        }

        protected override void GenerarCaras() {
            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(Desplazar.HaciaAdelante())
                                 .CaraZ(ModificacionEnY())
                                 .Build());

            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(Desplazar.HaciaAtras())
                                 .CaraMenosZ(ModificacionEnY())
                                 .Build());

            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(Desplazar.HaciaDerecha())
                                 .CaraX(ModificacionEnY())
                                 .Build());

            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(Desplazar.HaciaIzquierda())
                                 .CaraMenosX(ModificacionEnY())
                                 .Build());

            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(new CambiarPisoAlPersonaje())
                                 .CaraY()
                                 .Build());

            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(new ChoqueRigido(Eje.MenosY))
                                 .CaraMenosY()
                                 .Build());
        }

        public override void Movete()
        {
            mesh.Transform *= TGCMatrix.Translation(movimiento);
            mesh.BoundingBox.transform(mesh.Transform);
        }

        public bool EstaEnElPiso(TgcMesh planoPiso)
        {
            return TgcCollisionUtils.testAABBAABB(mesh.BoundingBox, planoPiso.BoundingBox);//this.caras.Any((cara) => cara.rayos.Any((rayo) => rayo.Colisionar(planoPiso.BoundingBox) && rayo.HuboColision()));
        } // esto rompe todo el encapsulamiento, habria que hacer una clase Plano que extienda de TieneBoundingBox (y que Colisionable tambien extienda de eso), entonces el testeo de colision de los rayos se hace con algo de tipo "TieneBoundingBox"
    }
}
