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
        private delegate float getCoordenadaDeMovimiento(Personaje personaje);
        private getCoordenadaDeMovimiento getCoordenadaMovimientoPersonaje;

        private delegate float setCoordenadaDeMovimiento(Personaje personaje, float valor);
        private setCoordenadaDeMovimiento setCoordenadaMovimientoPersonaje;

        private delegate TGCVector3 vectorMovimiento(Personaje personaje);
        private vectorMovimiento getVectorMovimiento;

        private delegate bool condicion(Personaje personaje);
        private condicion evaluarCondicionMovimiento;

        private Desplazar(getCoordenadaDeMovimiento getCoordenadaMovimientoPersonaje, setCoordenadaDeMovimiento setCoordenadaMovimientoPersonaje, vectorMovimiento getVectorMovimiento, condicion evaluarCondicionMovimiento) : base()
        {
            this.getCoordenadaMovimientoPersonaje = getCoordenadaMovimientoPersonaje;
            this.setCoordenadaMovimientoPersonaje = setCoordenadaMovimientoPersonaje;
            this.getVectorMovimiento = getVectorMovimiento;
            this.evaluarCondicionMovimiento = evaluarCondicionMovimiento;
        }

        public override void Colisionar(MeshTipoCaja meshTipoCaja, Personaje personaje)
        {
            if (SeEstaMoviendoHaciaMi(personaje))
            {
                var movimientoCaja = TGCMatrix.Translation(Desplazamiento(personaje)); // + distancia minima del rayo
                AgregarRozamiento(personaje);
                meshTipoCaja.Update(movimientoCaja);
            }
        }

        protected void AgregarRozamiento(Personaje personaje)
        {
            setCoordenadaMovimientoPersonaje(personaje, getCoordenadaMovimientoPersonaje(personaje) / 3);
        }

        protected TGCVector3 Desplazamiento(Personaje personaje)
        {
            return getVectorMovimiento(personaje);
        }

        protected bool SeEstaMoviendoHaciaMi(Personaje personaje)
        {
            return evaluarCondicionMovimiento(personaje);
        }

        public static Desplazar HaciaAtras()
        {
            return new Desplazar((personaje) => personaje.movimiento.Z,
                                 (personaje, valor) => personaje.movimiento.Z = valor,
                                 (personaje) => new TGCVector3(0, 0, personaje.movimiento.Z),
                                 (personaje) => personaje.movimiento.Z > 0
                                 );
        }

        public static Desplazar HaciaAdelante()
        {
            return new Desplazar((personaje) => personaje.movimiento.Z,
                                 (personaje, valor) => personaje.movimiento.Z = valor,
                                 (personaje) => new TGCVector3(0, 0, personaje.movimiento.Z),
                                 (personaje) => personaje.movimiento.Z < 0
                                 );
        }

        public static Desplazar HaciaIzquierda()
        {
            return new Desplazar((personaje) => personaje.movimiento.X,
                                 (personaje, valor) => personaje.movimiento.X = valor,
                                 (personaje) => new TGCVector3(personaje.movimiento.X, 0, 0),
                                 (personaje) => personaje.movimiento.X > 0
                                 );
        }

        public static Desplazar HaciaDerecha()
        {
            return new Desplazar((personaje) => personaje.movimiento.X,
                                 (personaje, valor) => personaje.movimiento.X = valor,
                                 (personaje) => new TGCVector3(personaje.movimiento.X, 0, 0),
                                 (personaje) => personaje.movimiento.X < 0
                                 );
        }

    }
}