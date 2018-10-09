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

        public void TesteoDeColision(Personaje personaje) {
            if (HuboColision(personaje)) {
                accionesAnteColision.ForEach((accion) => accion.Colisionar(meshTipoCaja, personaje));
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

        internal void RenderizaRayos()
        {
            rayos.ForEach((rayo) => rayo.Render());
        }
    }
}