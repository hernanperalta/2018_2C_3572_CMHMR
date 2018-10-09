using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class Plataforma : MeshTipoCaja
    {

        public Plataforma(TGCVector3 posicionInicial, TgcMesh mesh) : base(posicionInicial, mesh)
        {

        }

        public override void Update(TGCMatrix movimientoCaja)
        {
            base.Update(movimientoCaja);
            mesh.Transform = movimientoCaja;
            mesh.BoundingBox.transform(mesh.Transform);
            GenerarCaras();
        }

        protected override void GenerarCaras()
        {
            caras.Add(CaraBuilder.Instance().Mesh(this).Accion(Desplazar.HaciaNingunLado()).CaraZ(ModificacionEnY()).Build());
            caras.Add(CaraBuilder.Instance().Mesh(this).Accion(Desplazar.HaciaNingunLado()).CaraMenosZ(ModificacionEnY()).Build());

            caras.Add(CaraBuilder.Instance().Mesh(this).Accion(Desplazar.HaciaNingunLado()).CaraX(ModificacionEnY()).Build());
            caras.Add(CaraBuilder.Instance().Mesh(this).Accion(Desplazar.HaciaNingunLado()).CaraMenosX(ModificacionEnY()).Build());

            caras.Add(CaraBuilder.Instance().Mesh(this).Accion(new CambiarPisoAlPersonaje()).CaraY().Build());
            caras.Add(CaraBuilder.Instance().Mesh(this).Accion(Desplazar.HaciaNingunLado()).CaraMenosY().Build());
        }

        protected override int ModificacionEnY()
        {
            return 1;
        }

    }
}
