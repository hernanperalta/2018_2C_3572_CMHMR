﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SkeletalAnimation;
using TGC.Core.Mathematica;
using Microsoft.DirectX.DirectInput;
using TGC.Core.BoundingVolumes;

namespace TGC.Group.Model
{
    public class Personaje
    {
        public TgcSkeletalMesh Mesh { get; set; }
        public bool moving;
        public bool colisionaEnY;
        private TGCMatrix ultimaPosicion;
        public TGCVector3 movimiento;
        private const float VelocidadDesplazamiento = 50f;
        public TGCVector3 Position
        {
            get => Mesh.Transform.Origin;
        }
        public TgcBoundingAxisAlignBox BoundingBox
        {
            get => Mesh.BoundingBox;
        }
        public TGCMatrix TransformPlataforma;

        private GameModel contexto;
        //
        public float VelocidadY = 0f;
        float VelocidadSalto = 90f;
        float Gravedad = -60f;
        float VelocidadTerminal = -50f;
        float DesplazamientoMaximoY = 5f;
        private bool PuedeSaltar;
        //

        public void Init(GameModel contexto)
        {
            this.contexto = contexto;
            string mediaDir = contexto.MediaDir;
            PuedeSaltar = true;

            var skeletalLoader = new TgcSkeletalLoader();
            Mesh = skeletalLoader.loadMeshAndAnimationsFromFile(
                mediaDir + "objetos\\robot\\Robot-TgcSkeletalMesh.xml",
                mediaDir + "objetos\\robot\\",
                new[]
                {
                    mediaDir + "objetos\\robot\\Caminando-TgcSkeletalAnim.xml",
                    mediaDir + "objetos\\robot\\Parado-TgcSkeletalAnim.xml"
                });


            Mesh.AutoTransform = false;
            Mesh.Transform = TGCMatrix.Identity;

            //Configurar animacion inicial
            Mesh.playAnimation("Parado", true);
            //Escalarlo porque es muy grande
            Mesh.Position = new TGCVector3(0, 0, 50);
            Mesh.Scale = new TGCVector3(0.1f, 0.1f, 0.1f);
            ultimaPosicion = TGCMatrix.Translation(Mesh.Position);
        }

        public void Update()
        {
            var elapsedTime = contexto.ElapsedTime;
            var input = contexto.Input;

            var velocidadCaminar = VelocidadDesplazamiento * elapsedTime;

            //Calcular proxima posicion de personaje segun Input
            moving = false;
            colisionaEnY = false;
            movimiento = TGCVector3.Empty;
            TransformPlataforma = TGCMatrix.Identity;
            //transformacionPersonaje = TGCMatrix.Identity;

            if (input.keyDown(Key.W))
            {
                movimiento += new TGCVector3(0, 0, -velocidadCaminar);
                moving = true;
            }

            if (input.keyDown(Key.S))
            {
                movimiento += new TGCVector3(0, 0, velocidadCaminar);
                moving = true;
            }

            if (input.keyDown(Key.D))
            {
                movimiento += new TGCVector3(-velocidadCaminar, 0, 0);
                moving = true;
            }

            if (input.keyDown(Key.A))
            {
                movimiento += new TGCVector3(velocidadCaminar, 0, 0);
                moving = true;
            }
            //
            if (input.keyPressed(Key.Space) && PuedeSaltar)
            {
                VelocidadY = VelocidadSalto;
            }

            if (!colisionaEnY)
            {
                VelocidadY = FastMath.Clamp(VelocidadY + Gravedad * elapsedTime, VelocidadTerminal, -VelocidadTerminal);

                movimiento += new TGCVector3(0, FastMath.Clamp(VelocidadY * elapsedTime, -DesplazamientoMaximoY, DesplazamientoMaximoY), 0);
                moving = true;
            }
        }

        public void Movete(TGCVector3 movimiento)
        {
            PuedeSaltar = movimiento.Y == 0;

            if (moving)
            {
                Mesh.playAnimation("Caminando", true);

                //personaje.Position += movimiento;
                ultimaPosicion *= TGCMatrix.Translation(movimiento);
            }
            else
            {
                Mesh.playAnimation("Parado", true);
            }

            contexto.camara.Target = Position;
        }

        public void Render()
        {
            Mesh.Transform =
                TGCMatrix.Scaling(Mesh.Scale)
                * TGCMatrix.RotationYawPitchRoll(Mesh.Rotation.Y, Mesh.Rotation.X, Mesh.Rotation.Z)
                * TransformPlataforma
                * ultimaPosicion;

            Mesh.BoundingBox.transform(Mesh.Transform);

            Mesh.animateAndRender(contexto.ElapsedTime);

            if (contexto.BoundingBox)
            {
                Mesh.BoundingBox.Render();
            }
        }

        public void Dispose()
        {
            Mesh.Dispose();
        }

        internal void ColisionoEnY()
        {
            this.colisionaEnY = true;
            if (movimiento.X == 0 && movimiento.Z == 0)
                this.moving = false;
        }
    }
}