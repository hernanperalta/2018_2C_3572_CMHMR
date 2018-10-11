namespace TGC.Group.Model
{
    public class ChoqueRigido : AnteColisionCaja
    {
        private Eje eje;

        public ChoqueRigido(Eje eje)
        {
            this.eje = eje;
        }

        public override void Colisionar(MeshTipoCaja meshTipoCaja, Personaje personaje)
        {
            // ESTO NO ESTA TERMINADO, LO QUE DEBERIA PASAR ES QUE LE RESTRINGE EL MOVIMIENTO EN EL EJE QUE CORRESPONDE A LA CARA
            switch (eje) {
                case Eje.X:
                    if (personaje.movimiento.X < 0) {
                        personaje.movimiento.X = 0;
                    }
                    break;

                case Eje.Z:
                    if (personaje.movimiento.Z < 0)
                    {
                        personaje.movimiento.Z = 0;
                    }
                    break;

                case Eje.MenosX:
                    if (personaje.movimiento.X > 0)
                    {
                        personaje.movimiento.X = 0;
                    }
                    break;

                case Eje.MenosZ:
                    if (personaje.movimiento.Z > 0)
                    {
                        personaje.movimiento.Z = 0;
                    }
                    break;
           
            }
        }
    }
}