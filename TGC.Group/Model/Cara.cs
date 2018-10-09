using System;
using System.Collections.Generic;
using TGC.Core.Mathematica;

namespace TGC.Group.Model
{
    public abstract class Cara
    {
        protected List<Rayo> rayos;
        protected IAnteColision accionAnteColision;
        protected MeshTipoCaja meshTipoCaja;

        public Cara(MeshTipoCaja meshTipoCaja, IAnteColision accionAnteColision, List<Rayo> rayos) {
            this.accionAnteColision = accionAnteColision;
            this.meshTipoCaja = meshTipoCaja;
            this.rayos = rayos;
            GenerarRayos();
        }

        protected abstract void GenerarRayos();

        public void TesteoDeColision(Personaje personaje) {
            if (HuboColision(personaje)) {
                accionAnteColision.Colisionar(meshTipoCaja, personaje);
            }
        }

        private bool HuboColision(Personaje personaje)
        {
            var puntoInterseccion = TGCVector3.Empty;

            foreach (Rayo rayo in rayos)
            {
                rayo.Colisionar(personaje.Mesh);

                if (rayo.HuboColision())
                    return true;
            }

            return false;
        }
    }
}