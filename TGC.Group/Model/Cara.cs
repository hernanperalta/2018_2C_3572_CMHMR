using System;
using System.Collections.Generic;
using TGC.Core.Mathematica;

namespace TGC.Group.Model
{
    public abstract class Cara
    {
        protected List<Rayo> rayos;
        protected List<IAnteColision> accionesAnteColision;
        protected MeshTipoCaja meshTipoCaja;

        public Cara(MeshTipoCaja meshTipoCaja, List<IAnteColision> accionesAnteColision, List<Rayo> rayos) {
            this.accionesAnteColision = accionesAnteColision;
            this.meshTipoCaja = meshTipoCaja;
            this.rayos = rayos;
        }

        public void TesteoDeColision(Colisionable colisionable) {
            if (HuboColision(colisionable)) {
                accionesAnteColision.ForEach((accion) => accion.Colisionar(meshTipoCaja, colisionable));
            }
        }

        private bool HuboColision(Colisionable colisionable)
        {
            var puntoInterseccion = TGCVector3.Empty;

            foreach (Rayo rayo in rayos)
            {
                rayo.Colisionar(colisionable.BoundingBox());

                if (rayo.HuboColision())
                    return true;
            }

            return false;
        }

        internal void RenderizaRayos()
        {
            rayos.ForEach((rayo) => rayo.Render());
        }
    }
}