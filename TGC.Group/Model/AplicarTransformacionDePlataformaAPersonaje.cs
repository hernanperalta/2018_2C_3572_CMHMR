namespace TGC.Group.Model
{
    internal class AplicarTransformacionDePlataformaAPersonaje : IAnteColision
    {
        public void Colisionar(MeshTipoCaja meshTipoCaja, Colisionable colisionable)
        {
            ((Personaje)colisionable).TransformPlataforma = meshTipoCaja.mesh.Transform; // FEO FEO FEO FEO FEO
        }
    }
}