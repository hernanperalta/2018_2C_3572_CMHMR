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

        public override void Update(TGCMatrix movimiento)
        {
            mesh.Transform = movimiento;
            mesh.BoundingBox.transform(mesh.Transform);
            base.Update(movimiento);
        }

        protected override void GenerarCaras()
        {
            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(new ChoqueRigido(Eje.Z))
                                 .CaraZ(ModificacionEnY())
                                 .Build());

            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(new ChoqueRigido(Eje.MenosZ))
                                 .CaraMenosZ(ModificacionEnY())
                                 .Build());

            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(new ChoqueRigido(Eje.X))
                                 .CaraX(ModificacionEnY())
                                 .Build());

            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(new ChoqueRigido(Eje.MenosX))
                                 .CaraMenosX(ModificacionEnY())
                                 .Build());

            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(new CambiarPisoAlPersonaje())
                                 .Accion(new AplicarTransformacionDePlataformaAPersonaje())
                                 .CaraY()
                                 .Build());

            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(new ChoqueRigido(Eje.MenosY))
                                 .CaraMenosY()
                                 .Build());
        }

        protected override int ModificacionEnY()
        {
            return 1;
        }

    }
}
