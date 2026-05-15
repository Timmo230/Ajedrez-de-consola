using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chees_console
{
    public class Torre : FormarDeMoverAvanzadas
    {
        public bool permiteEnroque { get; set; }
        public Torre(char inicial, string posicion, bool blanco)
        {
            this.InicilPieza = inicial;
            this.Posicion = posicion;
            this.Blanca = blanco;
            permiteEnroque = true;
            puntosDePieza = 5;
        }

        public override List<string> PosicionesValidas(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas, List<string> lugaresAtacados)
        {
            return PosicionesValidasTorre(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
        }

        public override List<string> comer(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas, List<string> lugaresAtacados)
        {
            int Counter = 0;
            int indiceRetorno = 0;

            List<string> posicionesValidas = PosicionesValidas(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas, null);

            posicionesValidas.ForEach(n => {
                bool counter = true;
                if (int.TryParse(n, out int number)) counter = false;

                if (counter) Counter++;
            });

            List<string> posicoinesComibles = new List<string>();

            for (int i = Counter; i < posicionesValidas.Count; i++)
            {
                if (Blanca)
                {
                    posicoinesComibles.Add(PlantillaComer(posicionesOcupadasNegras, Int32.Parse(posicionesValidas[i]), Int32.Parse(posicionesValidas[i + 1])));
                    indiceRetorno++;
                }
                else
                {
                    posicoinesComibles.Add(PlantillaComer(posicionesOcupadasBlancas, Int32.Parse(posicionesValidas[i]), Int32.Parse(posicionesValidas[i + 1])));
                    indiceRetorno++;
                }
                i++;
            }

            return posicoinesComibles;
        }

        public string PlantillaComer(string[] posicionesOcupadasOtroColor, int comprobarLetra, int comprobarNumero)
        {
            foreach (string a in posicionesOcupadasOtroColor)
            {
                if (a != null && NumeroLetra(comprobarLetra).ToString().ToLower() + comprobarNumero == a.ToLower())
                {
                    return NumeroLetra(comprobarLetra).ToString().ToUpper() + comprobarNumero;
                }
            }

            return "";
        }

    }
}
