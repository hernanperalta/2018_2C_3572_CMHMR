using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace TGC.Group.Model
{
    public class Desplazar : AnteColisionCaja
    {
        private delegate float getCoordenadaDeMovimiento(Colisionable colisionable);
        private getCoordenadaDeMovimiento getCoordenadaMovimientoColisionable;

        private delegate float setCoordenadaDeMovimiento(Colisionable colisionable, float valor);
        private setCoordenadaDeMovimiento setCoordenadaMovimientoColisionable;

        private delegate TGCVector3 vectorMovimiento(Colisionable colisionable);
        private vectorMovimiento getVectorMovimiento;

        private delegate bool condicion(Colisionable colisionable);
        private condicion evaluarCondicionMovimiento;

        private Desplazar(getCoordenadaDeMovimiento getCoordenadaMovimientoColisionable, setCoordenadaDeMovimiento setCoordenadaMovimientoColisionable, vectorMovimiento getVectorMovimiento, condicion evaluarCondicionMovimiento) : base()
        {
            this.getCoordenadaMovimientoColisionable = getCoordenadaMovimientoColisionable;
            this.setCoordenadaMovimientoColisionable = setCoordenadaMovimientoColisionable;
            this.getVectorMovimiento = getVectorMovimiento;
            this.evaluarCondicionMovimiento = evaluarCondicionMovimiento;
        }

        public override void Colisionar(MeshTipoCaja meshTipoCaja, Colisionable colisionable)
        {
            if (SeEstaMoviendoHaciaMi(colisionable))
            {
                if(colisionable is Personaje) {
                    AgregarRozamiento(colisionable);
                }
                // Es feo, pero es la unica que se me ocurre, porque solo tenes que aplicarle rozamiento al personaje, 
                // si empuja las dos cajas y aplicas rozamiento a la segunda caja, el efecto es que se termina atrasando la segunda 
                // por el rozamiento y se mete adentro de la que esta mas cerca del personaje, o eso me entendi...

                meshTipoCaja.movimiento += Desplazamiento(colisionable);
            }
        }

        protected void AgregarRozamiento(Colisionable colisionable)
        {
            setCoordenadaMovimientoColisionable(colisionable, getCoordenadaMovimientoColisionable(colisionable) / 3);
        }

        protected TGCVector3 Desplazamiento(Colisionable colisionable)
        {
            return getVectorMovimiento(colisionable);
        }

        protected bool SeEstaMoviendoHaciaMi(Colisionable colisionable)
        {
            return evaluarCondicionMovimiento(colisionable);
        }

        public static Desplazar HaciaAtras()
        {
            return new Desplazar((colisionable) => colisionable.movimiento.Z,
                                 (colisionable, valor) => colisionable.movimiento.Z = valor,
                                 (colisionable) => new TGCVector3(0, 0, colisionable.movimiento.Z),
                                 (colisionable) => colisionable.movimiento.Z > 0
                                 );
        }

        public static Desplazar HaciaAdelante()
        {
            return new Desplazar((colisionable) => colisionable.movimiento.Z,
                                 (colisionable, valor) => colisionable.movimiento.Z = valor,
                                 (colisionable) => new TGCVector3(0, 0, colisionable.movimiento.Z),
                                 (colisionable) => colisionable.movimiento.Z < 0
                                 );
        }

        public static Desplazar HaciaIzquierda()
        {
            return new Desplazar((colisionable) => colisionable.movimiento.X,
                                 (colisionable, valor) => colisionable.movimiento.X = valor,
                                 (colisionable) => new TGCVector3(colisionable.movimiento.X, 0, 0),
                                 (colisionable) => colisionable.movimiento.X > 0
                                 );
        }

        public static Desplazar HaciaDerecha()
        {
            return new Desplazar((colisionable) => colisionable.movimiento.X,
                                 (colisionable, valor) => colisionable.movimiento.X = valor,
                                 (colisionable) => new TGCVector3(colisionable.movimiento.X, 0, 0),
                                 (colisionable) => colisionable.movimiento.X < 0
                                 );
        }

    }
}