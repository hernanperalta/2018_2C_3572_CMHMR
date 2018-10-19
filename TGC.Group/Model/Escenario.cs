using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TGC.Group.Model.Coleccionables;

namespace TGC.Group.Model
{
    public abstract class Escenario
    {

        public TgcMesh planoPiso;
        public TgcMesh planoIzq;
        public TgcMesh planoDer;
        //public TgcMesh planoFront;
        public TgcMesh planoBack;
        protected TGCVector3 movimiento;
        protected Personaje personaje;
        protected GameModel contexto;
        public List<Caja> cajas; // todos los escenarios deben tenerlas, porque las cajas pueden moverse por todo el nivel
        public List<Coleccionable> coleccionables;

        public Escenario siguiente;
        public Escenario anterior;
        public int nearLimit;
        public int farLimit;
        protected List<TgcBoundingAxisAlignBox> colisionablesConCamara = new List<TgcBoundingAxisAlignBox>();

        protected Escenario(GameModel contexto, Personaje personaje, int nearLimit = 0, int farLimit = 0) {
            this.contexto = contexto;
            this.personaje = personaje;
            this.cajas = new List<Caja>();
            coleccionables = new List<Coleccionable>();
            Init();
        }

        public void AgregarCaja(Caja nuevaCaja) {
            if (!cajas.Contains(nuevaCaja)) {
                this.cajas.Add(nuevaCaja);
                //CalcularEfectoGravedadEnMeshes();
            }
            
        }

        public void QuitarCaja(Caja caja) {
            if (cajas.Contains(caja)) {
                cajas.Remove(caja);
            }
        }

        protected abstract void Init();

        public abstract void Render();

        public virtual void Update() {
            cajas.ForEach((caja) => caja.Update());
        }

        public virtual void Colisiones() {
            CalcularColisionesConPlanos();

            CalcularColisionesConMeshes();

            CalcularColisionesEntreMeshes();

            CalcularEfectoGravedadEnMeshes();

            VerificarSiAlgunMeshSalioDelEscenario();

            cajas.ForEach((caja) => caja.Movete());
            personaje.Movete(personaje.movimiento);
        }

        public void VerificarSiAlgunMeshSalioDelEscenario() {
            foreach (Caja caja in cajas)
            {
               if(siguiente != null)
                    this.siguiente.AgregarCaja(caja); // esto no va, solo esta pa probar 

                //if (caja.Position.Z <= -100/*farLimit*/) {
                //    this.siguiente.AgregarCaja(caja);
                //    this.QuitarCaja(caja);
                //}
            }
        }

        public virtual void CalcularColisionesConMeshes()
        {
            if (personaje.moving)
            {
                foreach (Coleccionable coleccionable in coleccionables)
                    if (coleccionable.ColisionoContra(personaje))
                    {
                        coleccionable.Juntarme();
                        break;
                    }

                foreach (Caja caja in cajas)
                {
                    caja.TestearColisionContra(personaje);
                }
            }
        }

        public virtual void CalcularColisionesEntreMeshes()
        {
            foreach (Caja caja in cajas)
            {
                var cajasFiltradas = cajas.FindAll((caja2) => !caja2.Equals(caja));
                foreach (Caja otraCaja in cajasFiltradas)
                {
                    caja.TestearColisionContra(otraCaja);
                }
            }
        }

        public virtual void CalcularEfectoGravedadEnMeshes()
        {
            foreach (Caja caja in cajas)
            {
                if (caja.EstaEnElPiso(planoPiso))
                    caja.movimiento.Y = 0;
            }
        }

        public abstract void CalcularColisionesConPlanos();

        protected bool ChocoConLimite(Personaje personaje, TgcMesh plano)
        {
            return TgcCollisionUtils.testAABBAABB(plano.BoundingBox, personaje.BoundingBox());
        }

        protected void NoMoverHacia(Key key)
        {
            switch (key)
            {
                case Key.A:
                    if (personaje.movimiento.X > 0) // los ejes estan al reves de como pensaba, o lo entendi mal.
                        personaje.movimiento.X = 0;
                    break;
                case Key.D:
                    if (personaje.movimiento.X < 0)
                        personaje.movimiento.X = 0;
                    break;
                case Key.S:
                    if (personaje.movimiento.Z > 0)
                        personaje.movimiento.Z = 0;
                    break;
                case Key.W:
                    if (personaje.movimiento.Z < 0)
                        personaje.movimiento.Z = 0;
                    break;
            }
        }
        public abstract void DisposeAll();

        public virtual List<TgcBoundingAxisAlignBox> ColisionablesConCamara() {
            return colisionablesConCamara;
        }

        public virtual void RenderHud()
        {
            var d3dDevice = D3DDevice.Instance.Device;
            var viewport = D3DDevice.Instance.Device.Viewport;

            //TgcTexture textura;
            var sprite = new Sprite(d3dDevice);
            sprite.Begin(SpriteFlags.AlphaBlend);
            sprite.Draw2D(contexto.TexturaVidas, Rectangle.Empty, new SizeF(32, 32), new PointF(viewport.Width - 32, 0), Color.White);
            sprite.Draw2D(contexto.TexturaDuraznos, Rectangle.Empty, new SizeF(32, 32), new PointF(viewport.Width - 32, 64), Color.White);
            sprite.End();

            contexto.textoVidas.render();
            contexto.textoDuraznos.render();
        }

        public void JuntarDurazno(Durazno durazno)
        {
            coleccionables.Remove(durazno);
            personaje.JuntarDurazno(durazno);
        }
    }
}