namespace TGC.Group.Model
{
    public class ChoqueRigido : AnteColisionCaja
    {
        private Eje eje;

        public ChoqueRigido(Eje eje)
        {
            this.eje = eje;
        }

        public override void Colisionar(MeshTipoCaja meshTipoCaja, Colisionable colisionable)
        {
            // ESTO NO ESTA TERMINADO, LO QUE DEBERIA PASAR ES QUE LE RESTRINGE EL MOVIMIENTO EN EL EJE QUE CORRESPONDE A LA CARA
            switch (eje) {
                case Eje.X:
                    if (colisionable.movimiento.X < 0) {
                        colisionable.movimiento.X = 0;
                    }
                    break;

                case Eje.Z:
                    if (colisionable.movimiento.Z < 0)
                    {
                        colisionable.movimiento.Z = 0;
                    }
                    break;

                case Eje.MenosX:
                    if (colisionable.movimiento.X > 0)
                    {
                        colisionable.movimiento.X = 0;
                    }
                    break;

                case Eje.MenosZ:
                    if (colisionable.movimiento.Z > 0)
                    {
                        colisionable.movimiento.Z = 0;
                    }
                    break;
           
            }
        }
    }
}