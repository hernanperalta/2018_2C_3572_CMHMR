using System;

namespace TGC.Group.Model
{
    public interface IAnteColision
    {
        void Colisionar(MeshTipoCaja meshTipoCaja, Colisionable colisionable);
    }
}