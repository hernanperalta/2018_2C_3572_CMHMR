﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    public class CaraX : Cara
    {
        public CaraX(MeshTipoCaja meshTipoCaja, List<IAnteColision> accionesAnteColision, List<Rayo> rayos) : base(meshTipoCaja, accionesAnteColision, rayos) {

        }

       
    }

    //CaraBuilder.caraMenosX(centro)
    //            .accionAnteColision(ac)
    //            .wild() // ahi puse a cargar :(
}
