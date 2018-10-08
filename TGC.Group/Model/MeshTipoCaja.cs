using TGC.Core.SceneLoader;
using TGC.Core.SkeletalAnimation;
using TGC.Core.Mathematica;
using TGC.Core.Collision;
using System.Collections;
using System.Collections.Generic;
using System;
//using

namespace TGC.Group.Model
{
    public abstract class MeshTipoCaja
    {
        public TgcMesh mesh;
        protected List<Rayo> rayosZ;
        protected List<Rayo> rayosMenosZ;
        protected List<Rayo> rayosX;
        protected List<Rayo> rayosMenosX;
        protected List<Rayo> rayosY;
        protected List<Rayo> rayosMenosY;
        protected TGCVector3 posicionInicial;
        //private bool soyUnaCaja;

        protected MeshTipoCaja(TGCVector3 posicionInicial, TgcMesh mesh)
        {
            this.mesh = mesh;
            //this.soyUnaCaja = soyUnaCaja;
            this.rayosX = new List<Rayo>();
            this.rayosMenosX = new List<Rayo>();
            this.rayosY = new List<Rayo>();
            this.rayosMenosY = new List<Rayo>();
            this.rayosZ = new List<Rayo>();
            this.rayosMenosZ = new List<Rayo>();
            this.posicionInicial = posicionInicial;

            Init();
        }

        private void Init() {

            mesh.AutoTransform = false;
            mesh.Transform = TGCMatrix.Translation(posicionInicial);
            mesh.BoundingBox.transform(mesh.Transform);

            //if (soyUnaCaja)
            //{
            //    GenerarRayosCaja();
            //}
            //else {
            //    GenerarRayosPlataformas();
            //}

            GenerarRayos();

        }

        protected abstract void GenerarRayos();

        
        

        internal void Render()
        {
            mesh.Render();
        }

        public virtual void Update(TGCMatrix movimientoCaja){
            ClearRayos();
        }

        public void ClearRayos() {
            rayosX.Clear();
            rayosMenosX.Clear();
            rayosY.Clear();
            rayosMenosY.Clear();
            rayosZ.Clear();
            rayosMenosZ.Clear();
        }

        public bool ChocoConFrente(Personaje personaje) {
            return this.TesteoDeRayos(personaje, rayosZ);   
        }

        public bool ChocoALaIzquierda(Personaje personaje) {
            return this.TesteoDeRayos(personaje, rayosX);
        }

        public bool ChocoArriba(Personaje personaje)
        {
            return this.TesteoDeRayos(personaje, rayosY);
        }

        public bool ChocoALaDerecha(Personaje personaje) {
            return this.TesteoDeRayos(personaje, rayosMenosX);
        }

        public bool ChocoAtras(Personaje personaje)
        {
            return this.TesteoDeRayos(personaje, rayosMenosZ);
        }

        public bool ChocoAbajo(Personaje personaje)
        {
            return this.TesteoDeRayos(personaje, rayosMenosY);
        }

        protected bool TesteoDeRayos(Personaje personaje, List<Rayo> rayos) {
            var puntoInterseccion = TGCVector3.Empty;

            foreach (Rayo rayo in rayos)
            {
                rayo.Colisionar(personaje.Mesh);

                if (rayo.HuboColision())
                    return true;
            }

            return false;
        }

        protected TGCVector3 HallarCentroDeCara(String dirCara)
        { 
            var PMin = mesh.BoundingBox.PMin;
            var PMax = mesh.BoundingBox.PMax;

            var centro = mesh.BoundingBox.calculateBoxCenter();

            switch (dirCara)
            {
                case "x":
                    return new TGCVector3(centro.X + (FastMath.Abs(PMax.X - PMin.X) / 2), centro.Y, centro.Z);
                case "-x":
                    return new TGCVector3(centro.X - (FastMath.Abs(PMax.X - PMin.X) / 2), centro.Y, centro.Z);
                case "y":
                    return new TGCVector3(centro.X, centro.Y + (FastMath.Abs(PMax.Y - PMin.Y) / 2), centro.Z);
                case "-y":
                    return new TGCVector3(centro.X, centro.Y - (FastMath.Abs(PMax.Y - PMin.Y) / 2), centro.Z);
                case "z":
                    return new TGCVector3(centro.X, centro.Y, centro.Z + (FastMath.Abs(PMax.Z - PMin.Z) / 2));
                case "-z":
                    return new TGCVector3(centro.X, centro.Y, centro.Z - (FastMath.Abs(PMax.Z - PMin.Z) / 2));
                default:
                    throw new Exception("direccion invalida");
            }
        }

        internal void RenderizaRayos()
        {
            mesh.BoundingBox.Render();

            rayosX.ForEach(rayo => { rayo.Render(); });

            rayosMenosX.ForEach(rayo => { rayo.Render(); });

            rayosY.ForEach(rayo => { rayo.Render(); });

            rayosMenosY.ForEach(rayo => { rayo.Render(); });

            rayosZ.ForEach(rayo => { rayo.Render(); });

            rayosMenosZ.ForEach(rayo => { rayo.Render(); });
        }

        // TODO dispose
    }
}