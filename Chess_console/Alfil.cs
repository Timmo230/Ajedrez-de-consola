using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chees_console
{
    public class Alfil : FormarDeMoverAvanzadas
    {
        public Alfil(char inicial, string posicion, bool blanco)
        {
            this.InicilPieza = inicial;
            this.Posicion = posicion;
            this.Blanca = blanco;
            puntosDePieza = 3;
        }

        public override List<string> PosicionesValidas(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas, List<string> lugaresAtacados)
        {
            return PosicionesValidasAlfil(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
        }

        public override List<string> comer(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas, List<string> lugaresAtacados)
        {
            return ComerAlfil(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
        }

    }
}
