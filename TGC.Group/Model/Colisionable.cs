using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.BoundingVolumes;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public abstract class Colisionable
    {
        public TGCVector3 movimiento;
        public abstract TgcBoundingAxisAlignBox BoundingBox();

        //public abstract TgcMesh Mesh();

        protected Colisionable() {

        }

        public virtual void ColisionoEnY() {
        }

    }
}
