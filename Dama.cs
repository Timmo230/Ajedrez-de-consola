using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chees_console
{
    public class Dama : FormarDeMoverAvanzadas
    {
        public Dama(char inicial, string posicion, bool blanco)
        {
            this.InicilPieza = inicial;
            this.Posicion = posicion;
            this.Blanca = blanco;
            puntosDePieza = 9;
        }

        public override List<string> PosicionesValidas(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas, List<string> lugaresAtacados)
        {
            List<string> posicionesValidas = new List<string>();

            posicionesValidas.AddRange(PosicionesValidasAlfil(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas));
            posicionesValidas.AddRange(PosicionesValidasTorre(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas));

            return posicionesValidas;
        }

        public override List<string> comer(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas, List<string> lugaresAtacados)
        {
            List<string> posicionesValidas = new List<string>();

            List<string> posicionesValidasAlfil = new List<string>();
            List<string> posicionesValidasTorre = new List<string>();

            posicionesValidasAlfil.AddRange(ComerAlfil(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas));
            posicionesValidasTorre.AddRange(ComerTorre(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas));

            posicionesValidasAlfil.ForEach(item => posicionesValidas.Add(item));

            posicionesValidasTorre.ForEach(item => posicionesValidas.Add(item));

            return posicionesValidas;
        }
    }
}
