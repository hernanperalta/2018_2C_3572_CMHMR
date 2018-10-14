using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.Sound;

namespace TGC.Group.Model.Escenarios
{
    delegate void Accion();

    class Boton
    {
        private TGCQuad quad;
        private Accion accion;
        public TgcBoundingAxisAlignBox AABB;
        private float sizeX = 20, sizeY = 4;

        public Boton(TGCVector3 centro, Accion accion)
        {
            this.accion = accion;
            quad = new TGCQuad();

            quad.Center = centro;
            quad.Size = new TGCVector2(sizeX, sizeY);
            quad.Normal = new TGCVector3(0, 0, 1);
            quad.Color = Color.DarkCyan;
            quad.updateValues();
            var s = quad.Size * 0.5f;
            AABB = new TgcBoundingAxisAlignBox(new TGCVector3(centro.X + sizeX/2, centro.Y - sizeY/2, centro.Z), new TGCVector3(centro.X - sizeX / 2, centro.Y + sizeY / 2, centro.Z));
            //AABB = new TgcBoundingAxisAlignBox(quad.Center - new TGCVector3(s.X, 0, s.Y), quad.Center + new TGCVector3(s.X, 0, s.Y), quad.Center, new TGCVector3(1, 1, 1));
        }

        public void Render(GameModel contexto)
        {
            quad.Render();
            AABB.Render();
        }

        public void Accion()
        {
            accion();
        }
    }

    public class EscenarioMenu : Escenario
    {
        private List<Boton> botones = new List<Boton>();
        private TgcPickingRay pickingRay;
        protected TgcMp3Player cancionPpal = new TgcMp3Player();
        public TGCVector3 lookAt;

        public EscenarioMenu(GameModel contexto, Personaje personaje) : base(contexto, personaje) { }

        protected override void Init()
        {
            pickingRay = new TgcPickingRay(contexto.Input);
            cancionPpal.FileName = contexto.MediaDir + "\\musica\\crash.mp3";

            contexto.Camara = new Core.Camara.TgcCamera();
            lookAt = new TGCVector3(0, 50, 400);
            contexto.Camara.SetCamera(new TGCVector3(lookAt.X, lookAt.Y, lookAt.Z + 30), lookAt);

            botones.Add(
                new Boton(new TGCVector3(lookAt.X, lookAt.Y, lookAt.Z), () => {
                    contexto.CambiarEscenario("playa");
                    contexto.ActualizarCamara();
                })
            );
        }

        public override void Update()
        {
            if (contexto.ElapsedTime > 10000)
                return;

            if (cancionPpal.getStatus() != TgcMp3Player.States.Playing)
            {
                cancionPpal.closeFile();
                cancionPpal.play(true);
            }
        }

        public override void Render()
        {
            if (contexto.Input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                pickingRay.updateRay();

                foreach (var boton in botones)
                {
                    TGCVector3 collisionPoint;
                    var selected = TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, boton.AABB, out collisionPoint);
                    if (selected)
                    {
                        boton.Accion();
                        break;
                    }
                }
            }

            foreach (var boton in botones)
            {
                boton.Render(contexto);
            }
        }

        public override void DisposeAll() { }

        public override void CalcularColisionesConMeshes() { }

        public override void CalcularColisionesConPlanos() { }

        public override void Colisiones() { }
    }
}
