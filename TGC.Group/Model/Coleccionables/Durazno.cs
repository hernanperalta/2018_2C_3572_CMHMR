using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model.Coleccionables
{
    public class Durazno : Coleccionable
    {
        public Durazno(GameModel contexto, TgcMesh mesh) : base(contexto, mesh) { }

        protected override void Juntarme()
        {
            contexto.escenarioActual.JuntarDurazno(this);
        }
    }
}
