using System;
using System.Collections.Generic;
using System.Drawing;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.Sound;
using TGC.Core.Text;

namespace TGC.Group.Model.Escenarios
{
    delegate void Accion();

    class Boton
    {
        private TGCQuad quad;
        private Accion accion;
        public TgcBoundingAxisAlignBox AABB;
        private float sizeX = 15, sizeY = 3;
        int offsetTexto;
        private TgcText2D texto;

        public Boton(TGCVector3 centro, string texto, Color color, int offsetTexto, Accion accion)
        {
            this.offsetTexto = offsetTexto;
            CrearQuad(centro, color);
            CrearTexto(texto);
            this.accion = accion;
        }

        private void CrearQuad(TGCVector3 centro, Color color)
        {
            quad = new TGCQuad();
            quad.Center = centro;
            quad.Size = new TGCVector2(sizeX, sizeY);
            quad.Normal = new TGCVector3(0, 0, 1);
            quad.Color = color;
            quad.updateValues();
            var s = quad.Size * 0.5f;
            AABB = new TgcBoundingAxisAlignBox(new TGCVector3(centro.X - sizeX / 2, centro.Y - sizeY / 2, centro.Z), new TGCVector3(centro.X + sizeX / 2, centro.Y + sizeY / 2, centro.Z));
        }

        private void CrearTexto(string text)
        {
            var viewport = D3DDevice.Instance.Device.Viewport;

            texto = new TgcText2D();
            texto.Position = new Point(viewport.Width/2 - 100, viewport.Height/2 - offsetTexto);
            texto.Size = new Size(200, 50);
            texto.changeFont(new Font("TimesNewRoman", 50, FontStyle.Regular));
            texto.Color = Color.Yellow;
            texto.Align = TgcText2D.TextAlign.CENTER;
            texto.Text = text;
        }

        public void Render(GameModel contexto)
        {
            if(contexto.escenarioActual is EscenarioMenu)
                texto.render();
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
        public TGCVector3 lookAt;

        public EscenarioMenu(GameModel contexto, Personaje personaje) : base(contexto, personaje) { }

        protected override void Init()
        {
            pickingRay = new TgcPickingRay(contexto.Input);

            contexto.Camara = new Core.Camara.TgcCamera();
            lookAt = new TGCVector3(0, 50, 400);
            contexto.Camara.SetCamera(new TGCVector3(lookAt.X, lookAt.Y, lookAt.Z + 30), lookAt);

            botones.Add(
                new Boton(new TGCVector3(lookAt.X, lookAt.Y + 6, lookAt.Z), "Jugar", Color.Green, 300, () =>
                {
                    contexto.Empezar();
                })
            );

            botones.Add(
                new Boton(new TGCVector3(lookAt.X, lookAt.Y, lookAt.Z), "Salir", Color.Red, 25, () =>
                {
                    Environment.Exit(0);
                })
            );

            contexto.cancionPpal.play(true);
        }

        public override void Update()
        {
            if (contexto.ElapsedTime > 10000)
                return;
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

        public override void RenderHud() { }

        public override void DisposeAll() { }

        public override void CalcularColisionesConMeshes() { }

        public override void CalcularColisionesConPlanos() { }

        public override void Colisiones() { }
    }
}
