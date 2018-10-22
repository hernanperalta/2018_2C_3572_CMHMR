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

        public TGCMatrix transformacion;

        public Plataforma(TGCVector3 posicionInicial, TgcMesh mesh, GameModel Context) : base(posicionInicial, mesh, Context)
        {
            this.transformacion = TGCMatrix.Zero;
        }

        public override void Movete()
        {
            mesh.Transform = transformacion;
            mesh.BoundingBox.transform(mesh.Transform);
        }

        protected override void GenerarCaras()
        {
            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(new ChoqueRigido(Eje.Z))
                                 .CaraZ()
                                 .Build());

            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(new ChoqueRigido(Eje.MenosZ))
                                 .CaraMenosZ()
                                 .Build());

            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(new ChoqueRigido(Eje.X))
                                 .CaraX()
                                 .Build());

            caras.Add(CaraBuilder.Instance()
                                 .Mesh(this)
                                 .Accion(new ChoqueRigido(Eje.MenosX))
                                 .CaraMenosX()
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