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
        public bool colisionaEnY = true;
        public float VelocidadY = 0f;
        public float VelocidadSalto = 90f;
        public float Gravedad = -60f;
        public float VelocidadTerminal = -50f;
        public float DesplazamientoMaximoY = 5f;
        protected GameModel Context;
        public bool moving;

        //public abstract TgcMesh Mesh();

        protected Colisionable(GameModel Context) {
            this.Context = Context;
        }

        public virtual void ColisionoEnY() {
            this.colisionaEnY = true;
        }

        public virtual void Update() {
            if (!colisionaEnY)
            {
                VelocidadY = FastMath.Clamp(VelocidadY + Gravedad * Context.ElapsedTime, VelocidadTerminal, -VelocidadTerminal);

                movimiento += new TGCVector3(0, FastMath.Clamp(VelocidadY * Context.ElapsedTime, -DesplazamientoMaximoY, DesplazamientoMaximoY), 0);
                moving = true;
            }

            //this.colisionaEnY = false;
        }

    }
}
