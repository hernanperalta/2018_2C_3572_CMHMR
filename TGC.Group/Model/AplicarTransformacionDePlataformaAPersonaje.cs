namespace TGC.Group.Model
{
    internal class AplicarTransformacionDePlataformaAPersonaje : IAnteColision
    {
        public void Colisionar(MeshTipoCaja meshTipoCaja, Personaje personaje)
        {
            personaje.TransformPlataforma = meshTipoCaja.mesh.Transform;
        }
    }
}