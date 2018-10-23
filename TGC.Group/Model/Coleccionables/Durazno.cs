using TGC.Core.SceneLoader;

namespace TGC.Group.Model.Coleccionables
{
    public class Durazno : Coleccionable
    {
        public Durazno(GameModel contexto, TgcMesh mesh) : base(contexto, mesh) { }

        public override void Juntarme()
        {
            contexto.escenarioActual.JuntarDurazno();
            Visible = false;
        }
    }
}
