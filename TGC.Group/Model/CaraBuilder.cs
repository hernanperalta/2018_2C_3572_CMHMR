﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class CaraBuilder
    {
        private List<Rayo> rayos;
        private MeshTipoCaja mesh;
        private List<IAnteColision> accionesAnteColision;
        private delegate Cara constructor(MeshTipoCaja mesh, List<IAnteColision> acciones, List<Rayo> rayos);
        private constructor caraConstructor;

        private CaraBuilder()
        {
            rayos = new List<Rayo>();
            accionesAnteColision = new List<IAnteColision>();
        }

        public static CaraBuilder Instance()
        {
            return new CaraBuilder();
        }

        public CaraBuilder Mesh(MeshTipoCaja mesh)
        {
            this.mesh = mesh;
            return this;
        }

        public CaraBuilder CaraX()
        {
            var centroCaraX = HallarCentroDeCara("x");
            var centroCaraY = HallarCentroDeCara("y");

            var rayoCentroCaraX = new RayoX(new TGCVector3(centroCaraX.X, centroCaraY.Y, centroCaraX.Z), new TGCVector3(1, 0, 0));
            var rayoIzqCaraX = new RayoX(new TGCVector3(centroCaraX.X, centroCaraY.Y, mesh.BoundingBox().PMin.Z), new TGCVector3(1, 0, 0));
            var rayoDerCaraX = new RayoX(new TGCVector3(centroCaraX.X, centroCaraY.Y, mesh.BoundingBox().PMax.Z), new TGCVector3(1, 0, 0));

            rayos.Add(rayoCentroCaraX);
            rayos.Add(rayoIzqCaraX);
            rayos.Add(rayoDerCaraX);

            caraConstructor = (mesh, accionesAnteColision, rayos) => new CaraX(mesh, accionesAnteColision, rayos);

            return this;
        }

        public CaraBuilder CaraMenosX()
        {
            var centroCaraMenosX = HallarCentroDeCara("-x");
            var centroCaraY = HallarCentroDeCara("y");

            var rayoCaraMenosX = new RayoX(new TGCVector3(centroCaraMenosX.X, centroCaraY.Y, centroCaraMenosX.Z), new TGCVector3(-1, 0, 0));
            var rayoIzqCaraMenosX = new RayoX(new TGCVector3(centroCaraMenosX.X, centroCaraY.Y, mesh.BoundingBox().PMin.Z), new TGCVector3(-1, 0, 0));
            var rayoDerCaraMenosX = new RayoX(new TGCVector3(centroCaraMenosX.X, centroCaraY.Y, mesh.BoundingBox().PMax.Z), new TGCVector3(-1, 0, 0));

            rayos.Add(rayoCaraMenosX);
            rayos.Add(rayoIzqCaraMenosX);
            rayos.Add(rayoDerCaraMenosX);

            caraConstructor = (mesh, accionesAnteColision, rayos) => new CaraX(mesh, accionesAnteColision, rayos);

            return this;
        }

        public CaraBuilder CaraY()
        {
            var centroCaraY = HallarCentroDeCara("y");
            var centroCaraMenosZ = HallarCentroDeCara("-z");
            var centroCaraZ = HallarCentroDeCara("z");
            var centroCaraX = HallarCentroDeCara("x");
            var centroCaraMenosX = HallarCentroDeCara("-x");

            // RAYOS MAS ALEJADOS, MENOS Z
            var rayoCaraY = new RayoY(new TGCVector3(centroCaraY.X, centroCaraY.Y, centroCaraMenosZ.Z), new TGCVector3(0, 1, 0));
            var rayoIzqCaraY = new RayoY(new TGCVector3(mesh.BoundingBox().PMax.X, centroCaraY.Y, centroCaraMenosZ.Z), new TGCVector3(0, 1, 0));
            var rayoDerCaraY = new RayoY(new TGCVector3(mesh.BoundingBox().PMin.X, centroCaraY.Y, centroCaraMenosZ.Z), new TGCVector3(0, 1, 0));

            rayos.Add(rayoCaraY);
            rayos.Add(rayoIzqCaraY);
            rayos.Add(rayoDerCaraY);

            // RAYOS MAS CERCANOS, Z
            var rayoCaraYMasAdelante = new RayoY(new TGCVector3(centroCaraY.X, centroCaraY.Y, centroCaraZ.Z), new TGCVector3(0, 1, 0));
            var rayoIzqCaraYMasAdelante = new RayoY(new TGCVector3(mesh.BoundingBox().PMax.X, centroCaraY.Y, centroCaraZ.Z), new TGCVector3(0, 1, 0));
            var rayoDerCaraYMasAdelante = new RayoY(new TGCVector3(mesh.BoundingBox().PMin.X, centroCaraY.Y, centroCaraZ.Z), new TGCVector3(0, 1, 0));

            rayos.Add(rayoCaraYMasAdelante);
            rayos.Add(rayoIzqCaraYMasAdelante);
            rayos.Add(rayoDerCaraYMasAdelante);

            // RAYOS DEL MEDIO
            var rayoCaraYMasAtras = new RayoY(new TGCVector3(centroCaraY.X, centroCaraY.Y, centroCaraX.Z), new TGCVector3(0, 1, 0));
            var rayoIzqCaraYMasAtras = new RayoY(new TGCVector3(mesh.BoundingBox().PMax.X, centroCaraY.Y, centroCaraX.Z), new TGCVector3(0, 1, 0));
            var rayoDerCaraYMasAtras = new RayoY(new TGCVector3(mesh.BoundingBox().PMin.X, centroCaraY.Y, centroCaraX.Z), new TGCVector3(0, 1, 0));

            rayos.Add(rayoCaraYMasAtras);
            rayos.Add(rayoIzqCaraYMasAtras);
            rayos.Add(rayoDerCaraYMasAtras);

            // RAYOS DIAGONALES:
            var rayoCaraYAbajoIzq = new RayoY(new TGCVector3((centroCaraX.X + centroCaraY.X) / 2, centroCaraY.Y, (centroCaraZ.Z + centroCaraY.Z) / 2), new TGCVector3(0, 1, 0));
            var rayoIzqCaraYAbajoDer = new RayoY(new TGCVector3((centroCaraY.X + centroCaraMenosX.X) / 2, centroCaraY.Y, (centroCaraZ.Z + centroCaraY.Z) / 2), new TGCVector3(0, 1, 0));
            var rayoDerCaraYArribaIzq = new RayoY(new TGCVector3((centroCaraX.X + centroCaraY.X) / 2, centroCaraY.Y, (centroCaraY.Z + centroCaraMenosZ.Z) / 2), new TGCVector3(0, 1, 0));
            var rayoDerCaraYArribaDer = new RayoY(new TGCVector3((centroCaraY.X + centroCaraMenosX.X) / 2, centroCaraY.Y, (centroCaraY.Z + centroCaraMenosZ.Z) / 2), new TGCVector3(0, 1, 0));

            rayos.Add(rayoCaraYAbajoIzq);
            rayos.Add(rayoIzqCaraYAbajoDer);
            rayos.Add(rayoDerCaraYArribaIzq);
            rayos.Add(rayoDerCaraYArribaDer);

            caraConstructor = (mesh, accionesAnteColision, rayos) => new CaraY(mesh, accionesAnteColision, rayos);

            return this;
        }

        public CaraBuilder CaraMenosY()
        {
            var centroCaraMenosY = HallarCentroDeCara("-y");

            var rayoCaraMenosY = new RayoY(centroCaraMenosY, new TGCVector3(0, -1, 0));
            var rayoIzqCaraMenosY = new RayoY(new TGCVector3(mesh.BoundingBox().PMax.X, centroCaraMenosY.Y, centroCaraMenosY.Z), new TGCVector3(0, -1, 0));
            var rayoDerCaraMenosY = new RayoY(new TGCVector3(mesh.BoundingBox().PMin.X, centroCaraMenosY.Y, centroCaraMenosY.Z), new TGCVector3(0, -1, 0));

            rayos.Add(rayoCaraMenosY);
            rayos.Add(rayoIzqCaraMenosY);
            rayos.Add(rayoDerCaraMenosY);

            caraConstructor = (mesh, accionesAnteColision, rayos) => new CaraY(mesh, accionesAnteColision, rayos);

            return this;
        }


        public CaraBuilder CaraZ()
        {
            var centroCaraZ = HallarCentroDeCara("z");
            var centroCaraY = HallarCentroDeCara("y");

            var rayoCaraZ = new RayoZ(new TGCVector3(centroCaraZ.X, centroCaraY.Y, centroCaraZ.Z), new TGCVector3(0, 0, 1));
            var rayoIzqCaraZ = new RayoZ(new TGCVector3(mesh.BoundingBox().PMax.X, centroCaraY.Y, centroCaraZ.Z), new TGCVector3(0, 0, 1));
            var rayoDerCaraZ = new RayoZ(new TGCVector3(mesh.BoundingBox().PMin.X, centroCaraY.Y, centroCaraZ.Z), new TGCVector3(0, 0, 1));

            rayos.Add(rayoCaraZ);
            rayos.Add(rayoIzqCaraZ);
            rayos.Add(rayoDerCaraZ);

            caraConstructor = (mesh, accionesAnteColision, rayos) => new CaraZ(mesh, accionesAnteColision, rayos);

            return this;
        }

        public CaraBuilder CaraMenosZ()
        {
            var centroCaraMenosZ = HallarCentroDeCara("-z");
            var centroCaraY = HallarCentroDeCara("y");

            var rayoCaraMenosZ = new RayoZ(new TGCVector3(centroCaraMenosZ.X, centroCaraY.Y, centroCaraMenosZ.Z), new TGCVector3(0, 0, -1));
            var rayoIzqCaraMenosZ = new RayoZ(new TGCVector3(mesh.BoundingBox().PMax.X, centroCaraY.Y, centroCaraMenosZ.Z), new TGCVector3(0, 0, -1));
            var rayoDerCaraMenosZ = new RayoZ(new TGCVector3(mesh.BoundingBox().PMin.X, centroCaraY.Y, centroCaraMenosZ.Z), new TGCVector3(0, 0, -1));

            rayos.Add(rayoCaraMenosZ);
            rayos.Add(rayoIzqCaraMenosZ);
            rayos.Add(rayoDerCaraMenosZ);

            caraConstructor = (mesh, accionesAnteColision, rayos) => new CaraZ(mesh, accionesAnteColision, rayos);

            return this;
        }

        public CaraBuilder Accion(IAnteColision accion)
        {
            this.accionesAnteColision.Add(accion);
            return this;
        }

        public Cara Build()
        {
            if (!rayos.Any())
                throw new ArgumentException("No hay rayos agregados a la cara!");

            if (!accionesAnteColision.Any())
                throw new ArgumentException("Debe haber al menos una accion ante colision seteada!");

            if (mesh == null)
                throw new ArgumentException("No hay un mesh tipo caja seteado!");

            return caraConstructor(mesh, accionesAnteColision, rayos);
        }


        public TGCVector3 HallarCentroDeCara(String dirCara)
        {
            var PMin = mesh.BoundingBox().PMin;
            var PMax = mesh.BoundingBox().PMax;

            var centro = mesh.BoundingBox().calculateBoxCenter();

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
    }
}

 