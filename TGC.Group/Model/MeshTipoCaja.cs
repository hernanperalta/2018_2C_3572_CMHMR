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
    public class MeshTipoCaja
    {
        public TgcMesh mesh;
        private List<Rayo> rayosZ;
        private List<Rayo> rayosMenosZ;
        private List<Rayo> rayosX;
        private List<Rayo> rayosMenosX;
        private List<Rayo> rayosY;
        private List<Rayo> rayosMenosY;
        private TGCVector3 posicionInicial;

        public MeshTipoCaja(TGCVector3 posicionInicial)
        {
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
            var loader = new TgcSceneLoader();
            mesh = loader.loadSceneFromFile(GameModel.Media + "primer-nivel\\Playa final\\caja-TgcScene.xml").Meshes[0];
            mesh.AutoTransform = false;
            mesh.Transform = TGCMatrix.Translation(posicionInicial);
            mesh.BoundingBox.transform(mesh.Transform);

            GenerarRayos();
        }

        private void GenerarRayos() {
            // el orden es el mismo que retorna el metodo computeFaces de un BB, visto de frente (hacia -z) => Up, Down, Front, Back, Right, Left
            var rayos = new List<Rayo>();

            // Solucion casera del centro...
            //var PMax = meshTipoCaja.BoundingBox.PMax;
            //var PMin = meshTipoCaja.BoundingBox.PMin;
            //var centro = new TGCVector3((PMax.X + PMin.X) / 2, (PMax.Y + PMin.Y) / 2, (PMax.Z + PMin.Z) / 2);
            //

            var centro = mesh.BoundingBox.calculateBoxCenter();

            var centroCaraX = HallarCentroDeCara("x");
            var centroCaraMenosX = HallarCentroDeCara("-x");
            var centroCaraY = HallarCentroDeCara("y");
            var centroCaraMenosY = HallarCentroDeCara("-y");
            var centroCaraZ = HallarCentroDeCara("z");
            var centroCaraMenosZ = HallarCentroDeCara("-z");

            //cara izquierda
            var rayoCentroCaraX = new RayoX(new TGCVector3(centroCaraX.X, centroCaraX.Y * 2, centroCaraX.Z), new TGCVector3(1,0,0));
            var rayoIzqCaraX = new RayoX(new TGCVector3(centroCaraX.X, centroCaraX.Y * 2, mesh.BoundingBox.PMin.Z), new TGCVector3(1, 0, 0));
            var rayoDerCaraX = new RayoX(new TGCVector3(centroCaraX.X, centroCaraX.Y * 2, mesh.BoundingBox.PMax.Z), new TGCVector3(1, 0, 0));

            //cara derecha
            var rayoCaraMenosX = new RayoX(new TGCVector3(centroCaraMenosX.X, centroCaraMenosX.Y * 2, centroCaraMenosX.Z), new TGCVector3(-1, 0, 0));
            var rayoIzqCaraMenosX = new RayoX(new TGCVector3(centroCaraMenosX.X, centroCaraMenosX.Y * 2, mesh.BoundingBox.PMin.Z), new TGCVector3(-1, 0, 0));
            var rayoDerCaraMenosX = new RayoX(new TGCVector3(centroCaraMenosX.X, centroCaraMenosX.Y * 2, mesh.BoundingBox.PMax.Z), new TGCVector3(-1, 0, 0));

            //cara de arriba
            var rayoCaraY = new RayoY(new TGCVector3(centroCaraY.X, centroCaraY.Y, centroCaraMenosZ.Z), new TGCVector3(0, 1, 0));
            var rayoIzqCaraY = new RayoY(new TGCVector3(mesh.BoundingBox.PMax.X, centroCaraY.Y, centroCaraMenosZ.Z), new TGCVector3(0, 1, 0));
            var rayoDerCaraY = new RayoY(new TGCVector3(mesh.BoundingBox.PMin.X, centroCaraY.Y, centroCaraMenosZ.Z), new TGCVector3(0, 1, 0));

            var rayoCaraYMasAdelante = new RayoY(new TGCVector3(centroCaraY.X, centroCaraY.Y, centroCaraZ.Z), new TGCVector3(0, 1, 0));
            var rayoIzqCaraYMasAdelante = new RayoY(new TGCVector3(mesh.BoundingBox.PMax.X, centroCaraY.Y, centroCaraZ.Z), new TGCVector3(0, 1, 0));
            var rayoDerCaraYMasAdelante = new RayoY(new TGCVector3(mesh.BoundingBox.PMin.X, centroCaraY.Y, centroCaraZ.Z), new TGCVector3(0, 1, 0));

            var rayoCaraYMasAtras = new RayoY(new TGCVector3(centroCaraY.X, centroCaraY.Y, centroCaraX.Z), new TGCVector3(0, 1, 0));
            var rayoIzqCaraYMasAtras = new RayoY(new TGCVector3(mesh.BoundingBox.PMax.X, centroCaraY.Y, centroCaraX.Z), new TGCVector3(0, 1, 0));
            var rayoDerCaraYMasAtras = new RayoY(new TGCVector3(mesh.BoundingBox.PMin.X, centroCaraY.Y, centroCaraX.Z), new TGCVector3(0, 1, 0));

            //cara de abajo??!!
            var rayoCaraMenosY = new RayoY(centroCaraMenosY, new TGCVector3(0, -1, 0));
            var rayoIzqCaraMenosY = new RayoY(new TGCVector3(mesh.BoundingBox.PMax.X, centroCaraMenosY.Y, centroCaraMenosY.Z), new TGCVector3(0, -1, 0));
            var rayoDerCaraMenosY = new RayoY(new TGCVector3(mesh.BoundingBox.PMin.X, centroCaraMenosY.Y, centroCaraMenosY.Z), new TGCVector3(0, -1, 0));

            //cara del frente
            var rayoCaraZ = new RayoZ(new TGCVector3(centroCaraZ.X, centroCaraZ.Y * 2, centroCaraZ.Z), new TGCVector3(0, 0, 1));
            var rayoIzqCaraZ = new RayoZ(new TGCVector3(mesh.BoundingBox.PMax.X, centroCaraZ.Y * 2, centroCaraZ.Z), new TGCVector3(0, 0, 1));
            var rayoDerCaraZ = new RayoZ(new TGCVector3(mesh.BoundingBox.PMin.X, centroCaraZ.Y * 2, centroCaraZ.Z), new TGCVector3(0, 0, 1));

            //cara del fondo
            var rayoCaraMenosZ = new RayoZ(new TGCVector3(centroCaraMenosZ.X, centroCaraMenosZ.Y * 2, centroCaraMenosZ.Z), new TGCVector3(0, 0, -1));
            var rayoIzqCaraMenosZ = new RayoZ(new TGCVector3(mesh.BoundingBox.PMax.X, centroCaraMenosZ.Y * 2, centroCaraMenosZ.Z), new TGCVector3(0, 0, -1));
            var rayoDerCaraMenosZ = new RayoZ(new TGCVector3(mesh.BoundingBox.PMin.X, centroCaraMenosZ.Y * 2, centroCaraMenosZ.Z), new TGCVector3(0, 0, -1));

            rayosY.Add(rayoCaraY); // UpCentro
            rayosY.Add(rayoIzqCaraY); // UpIzq
            rayosY.Add(rayoDerCaraY); // UpDer
            rayosY.Add(rayoCaraYMasAdelante); // UpCentro
            rayosY.Add(rayoIzqCaraYMasAdelante); // UpIzq
            rayosY.Add(rayoDerCaraYMasAdelante); // UpDer
            rayosY.Add(rayoCaraYMasAtras); // UpCentro
            rayosY.Add(rayoIzqCaraYMasAtras); // UpIzq
            rayosY.Add(rayoDerCaraYMasAtras); // UpDer

            rayosMenosY.Add(rayoCaraMenosY); // Down
            rayosMenosY.Add(rayoIzqCaraMenosY); // DownIzq
            rayosMenosY.Add(rayoDerCaraMenosY); // DownDer

            rayosZ.Add(rayoCaraZ); // Front
            rayosZ.Add(rayoIzqCaraZ); // FrontIzq
            rayosZ.Add(rayoDerCaraZ); // FronDer

            rayosMenosZ.Add(rayoCaraMenosZ); // Back
            rayosMenosZ.Add(rayoIzqCaraMenosZ); // BackIzq
            rayosMenosZ.Add(rayoDerCaraMenosZ); // BackDer

            rayosMenosX.Add(rayoCaraMenosX); // Right
            rayosMenosX.Add(rayoIzqCaraMenosX); // RightIzq
            rayosMenosX.Add(rayoDerCaraMenosX); // RightDer

            rayosX.Add(rayoCentroCaraX); // Left
            rayosX.Add(rayoIzqCaraX); // LeftIzq
            rayosX.Add(rayoDerCaraX);
        }

        internal void Render()
        {
            mesh.Render();
        }

        public void Update(TGCMatrix movimientoCaja)
        {
            ClearRayos();
            GenerarRayos();
            mesh.Transform *= movimientoCaja;
            mesh.BoundingBox.transform(mesh.Transform);
        }

        private void ClearRayos() {
            rayosX.Clear();
            rayosMenosX.Clear();
            rayosY.Clear();
            rayosMenosY.Clear();
            rayosZ.Clear();
            rayosMenosZ.Clear();
        }

        public bool ChocoConFrente(Personaje personaje) {
            //this.GenerarRayos();
            return this.TesteoDeRayos(personaje, rayosZ);   
        }

        public bool ChocoALaIzquierda(Personaje personaje) {
            //this.GenerarRayos();
            return this.TesteoDeRayos(personaje, rayosX);
        }

        public bool ChocoArriba(Personaje personaje)
        {
            //this.GenerarRayos();
            return this.TesteoDeRayos(personaje, rayosY);
        }

        public bool ChocoALaDerecha(Personaje personaje) {
            //this.GenerarRayos();
            return this.TesteoDeRayos(personaje, rayosMenosX);
        }

        public bool ChocoAtras(Personaje personaje)
        {
            //this.GenerarRayos();
            return this.TesteoDeRayos(personaje, rayosMenosZ);
        }

        public bool ChocoAbajo(Personaje personaje)
        {
            //this.GenerarRayos();
            return this.TesteoDeRayos(personaje, rayosMenosY);
        }

        private bool TesteoDeRayos(Personaje personaje, List<Rayo> rayos) {
            var puntoInterseccion = TGCVector3.Empty;

            foreach (Rayo rayo in rayos)
            {
                rayo.Colisionar(personaje.Mesh);

                if (rayo.HuboColision())
                    return true;
            }

            return false;
        }

        private TGCVector3 HallarCentroDeCara(String dirCara)
        { // le pasas el centro para no tener que calcularlo cada vez que entras aca. en dirCara quise no pasarle un string, pero no anduvo con TGCVector3
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
    }
}