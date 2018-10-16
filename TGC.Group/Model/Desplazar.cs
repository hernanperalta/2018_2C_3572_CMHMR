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
        private getCoordenadaDeMovimiento getCoordenadaMovimientoPersonaje;

        private delegate float setCoordenadaDeMovimiento(Colisionable colisionable, float valor);
        private setCoordenadaDeMovimiento setCoordenadaMovimientoPersonaje;

        private delegate TGCVector3 vectorMovimiento(Colisionable colisionable);
        private vectorMovimiento getVectorMovimiento;

        private delegate bool condicion(Colisionable colisionable);
        private condicion evaluarCondicionMovimiento;

        private Desplazar(getCoordenadaDeMovimiento getCoordenadaMovimientoPersonaje, setCoordenadaDeMovimiento setCoordenadaMovimientoPersonaje, vectorMovimiento getVectorMovimiento, condicion evaluarCondicionMovimiento) : base()
        {
            this.getCoordenadaMovimientoPersonaje = getCoordenadaMovimientoPersonaje;
            this.setCoordenadaMovimientoPersonaje = setCoordenadaMovimientoPersonaje;
            this.getVectorMovimiento = getVectorMovimiento;
            this.evaluarCondicionMovimiento = evaluarCondicionMovimiento;
        }

        public override void Colisionar(MeshTipoCaja meshTipoCaja, Colisionable colisionable)
        {
            if (SeEstaMoviendoHaciaMi(colisionable))
            {
                //var movimientoCaja = TGCMatrix.Translation(Desplazamiento(colisionable)); // + distancia minima del rayo
                AgregarRozamiento(colisionable);
                //meshTipoCaja.Update(movimientoCaja);
                meshTipoCaja.movimiento += Desplazamiento(colisionable);
            }
        }

        protected void AgregarRozamiento(Colisionable colisionable)
        {
            setCoordenadaMovimientoPersonaje(colisionable, getCoordenadaMovimientoPersonaje(colisionable) / 3);
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