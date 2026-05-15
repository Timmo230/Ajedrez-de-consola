using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chees_console
{
    public class Pieza
    {
        public enum operacion { sumar, restar, Letra, Numero }
        public char InicilPieza { set; get; }
        public string Posicion { get; set; }
        public bool Blanca { get; set; }
        public byte puntosDePieza { get; set; }

        protected virtual string[] mover()
        {
            return null;
        }

        protected int LetraNumero(char letra)
        {
            int numero = letra == 'A' ? 1 : letra == 'B' ? 2 : letra == 'C' ? 3 : letra == 'D' ? 4 : letra == 'E' ? 5 : letra == 'F' ? 6 : letra == 'G' ? 7 : letra == 'H' ? 8 : 0;

            return numero;
        }

        protected char NumeroLetra(int numero)
        {
            char letra = numero == 1 ? 'A' : numero == 2 ? 'B' : numero == 3 ? 'C' : numero == 4 ? 'D' : numero == 5 ? 'E' : numero == 6 ? 'F' : numero == 7 ? 'G' : numero == 8 ? 'H' : '0';

            return letra;
        }

        public virtual List<string> PosicionesValidas(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas, List<string> lugaresAtacados)
        {
            return null;
        }

        public virtual List<string> comer(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas, List<string> lugaresAtacados)
        {
            return null;
        }

    }
}
