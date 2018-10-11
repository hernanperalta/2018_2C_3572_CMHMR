using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class Caja : MeshTipoCaja 
    {
        public Caja(TGCVector3 posicionInicial, TgcMesh mesh) : base (posicionInicial, mesh){
            
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

        public override void Update(TGCMatrix movimientoCaja)
        {
            mesh.Transform *= movimientoCaja;
            mesh.BoundingBox.transform(mesh.Transform);
            base.Update(movimientoCaja);
        }

        protected override int ModificacionEnY()
        {
            return 2;
        }

        //public override void TestarColisionContra(Personaje personaje)
        //{
        //    if (ChocoConFrente(personaje))
        //    {
        //        //EmpujarSiSeEstaMoviendoEn("z");
        //        var movimientoCaja = TGCMatrix.Translation(0, 0, personaje.movimiento.Z); // + distancia minima del rayo
        //        personaje.movimiento.Z /= 3;
        //        Update(movimientoCaja);
        //        return;
        //    }
        //    else if (ChocoAtras(personaje))
        //    {
        //        //NoMoverHacia(Key.S);
        //        //break;
        //        var movimientoCaja = TGCMatrix.Translation(0, 0, personaje.movimiento.Z); // + distancia minima del rayo
        //        personaje.movimiento.Z /= 3;
        //        Update(movimientoCaja);
        //        return;

        //    }
        //    else if (ChocoALaIzquierda(personaje))
        //    {
        //        //NoMoverHacia(Key.D);
        //        //break;
        //        var movimientoCaja = TGCMatrix.Translation(personaje.movimiento.X, 0, 0); // + distancia minima del rayo
        //        personaje.movimiento.Z /= 3;
        //        Update(movimientoCaja);
        //        return;

        //    }
        //    else if (ChocoALaDerecha(personaje))
        //    {
        //        //NoMoverHacia(Key.A);
        //        //break;
        //        var movimientoCaja = TGCMatrix.Translation(personaje.movimiento.X, 0, 0); // + distancia minima del rayo
        //        personaje.movimiento.Z /= 3;
        //        Update(movimientoCaja);
        //        return;
        //    }
        //    else if (ChocoArriba(personaje))
        //    {
        //        if (personaje.movimiento.Y < 0)
        //        {
        //            personaje.movimiento.Y = 0;
        //            personaje.ColisionoEnY();
        //        }
        //        return;
        //    }
        //}
    }
}
