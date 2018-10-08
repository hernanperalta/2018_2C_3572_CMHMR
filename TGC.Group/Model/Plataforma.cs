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
            GenerarRayos();
        }

        protected override void GenerarRayos()
        {
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
            var rayoCentroCaraX = new RayoX(new TGCVector3(centroCaraX.X, centroCaraX.Y, centroCaraX.Z), new TGCVector3(1, 0, 0));
            var rayoIzqCaraX = new RayoX(new TGCVector3(centroCaraX.X, centroCaraX.Y, mesh.BoundingBox.PMin.Z), new TGCVector3(1, 0, 0));
            var rayoDerCaraX = new RayoX(new TGCVector3(centroCaraX.X, centroCaraX.Y, mesh.BoundingBox.PMax.Z), new TGCVector3(1, 0, 0));

            //cara derecha
            var rayoCaraMenosX = new RayoX(new TGCVector3(centroCaraMenosX.X, centroCaraMenosX.Y, centroCaraMenosX.Z), new TGCVector3(-1, 0, 0));
            var rayoIzqCaraMenosX = new RayoX(new TGCVector3(centroCaraMenosX.X, centroCaraMenosX.Y, mesh.BoundingBox.PMin.Z), new TGCVector3(-1, 0, 0));
            var rayoDerCaraMenosX = new RayoX(new TGCVector3(centroCaraMenosX.X, centroCaraMenosX.Y, mesh.BoundingBox.PMax.Z), new TGCVector3(-1, 0, 0));

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
            var rayoCaraZ = new RayoZ(new TGCVector3(centroCaraZ.X, centroCaraZ.Y, centroCaraZ.Z), new TGCVector3(0, 0, 1));
            var rayoIzqCaraZ = new RayoZ(new TGCVector3(mesh.BoundingBox.PMax.X, centroCaraZ.Y, centroCaraZ.Z), new TGCVector3(0, 0, 1));
            var rayoDerCaraZ = new RayoZ(new TGCVector3(mesh.BoundingBox.PMin.X, centroCaraZ.Y, centroCaraZ.Z), new TGCVector3(0, 0, 1));

            //cara del fondo
            var rayoCaraMenosZ = new RayoZ(new TGCVector3(centroCaraMenosZ.X, centroCaraMenosZ.Y, centroCaraMenosZ.Z), new TGCVector3(0, 0, -1));
            var rayoIzqCaraMenosZ = new RayoZ(new TGCVector3(mesh.BoundingBox.PMax.X, centroCaraMenosZ.Y, centroCaraMenosZ.Z), new TGCVector3(0, 0, -1));
            var rayoDerCaraMenosZ = new RayoZ(new TGCVector3(mesh.BoundingBox.PMin.X, centroCaraMenosZ.Y, centroCaraMenosZ.Z), new TGCVector3(0, 0, -1));

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

    }
}
