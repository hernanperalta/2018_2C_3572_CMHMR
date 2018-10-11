﻿using Microsoft.DirectX.DirectInput;
using System;
using TGC.Core.Collision;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

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
        

        protected Escenario(GameModel contexto, Personaje personaje) {
            this.contexto = contexto;
            this.personaje = personaje;
            Init();
        }

        protected abstract void Init();

        public abstract void Render();

        public abstract void Update();

        public abstract void Colisiones();

        public abstract void CalcularColisionesConPlanos();

        public abstract void CalcularColisionesConMeshes();

        protected bool ChocoConLimite(Personaje personaje, TgcMesh plano)
        {
            return TgcCollisionUtils.testAABBAABB(plano.BoundingBox, personaje.BoundingBox);
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
    }
}