using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;

namespace TGC.Group.Model.Escenarios
{
    delegate void Accion();

    class Boton
    {
        TGCQuad quad;
        Accion accion;

        public Boton(Accion accion)
        {
            quad = new TGCQuad();
            this.accion = accion;
        }
    }

    public class EscenarioMenu : Escenario
    {
        private List<Boton> botones = new List<Boton>();

        public EscenarioMenu(GameModel contexto, Personaje personaje) : base(contexto, personaje) { }

        protected override void Init()
        {
            botones.Add(
                new Boton(() => { })
            );
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Render()
        {
            throw new NotImplementedException();
        }

        public override void DisposeAll()
        {
            throw new NotImplementedException();
        }

        public override void CalcularColisionesConMeshes() { }

        public override void CalcularColisionesConPlanos() { }

        public override void Colisiones() { }
    }
}
