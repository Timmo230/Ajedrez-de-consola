using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chees_console.Program;

namespace Chees_console
{
    public class Peon : Pieza
    {
        public string comerAlPaso { get; set; }
        public Peon(char inicial, string posicion, bool blanco)
        {
            this.InicilPieza = inicial;
            this.Posicion = posicion;
            this.Blanca = blanco;
            puntosDePieza = 1;
        }

        public override List<string> PosicionesValidas(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas, List<string> lugaresAtacados)
        {
            char letra = (char)Posicion[0];
            char numero = (char)Posicion[1];
            string calculoDePosicion = "";

            int letraNumero = LetraNumero(letra);
            int numeroNumero = Int32.Parse(numero.ToString());

            bool dosPasos = false;

            List<string> PosicionesValidas = new List<string>();

            if (Blanca)
            {
                //Mover 1
                calculoDePosicion = NumeroLetra(letraNumero).ToString() + (numeroNumero + 1).ToString();
                PosicionesValidas.Add(calculoDePosicion);
                //Mover 2 si es el primer movimiento
                if (Posicion[1] == '2')
                {
                    calculoDePosicion = NumeroLetra(letraNumero).ToString() + (numeroNumero + 2).ToString();
                    PosicionesValidas.Add(calculoDePosicion);
                    dosPasos = true;
                }
                else PosicionesValidas.Add(null);

            }

            else if (!Blanca)
            {
                //Mover 1
                calculoDePosicion = NumeroLetra(letraNumero).ToString() + (numeroNumero - 1).ToString();
                PosicionesValidas.Add(calculoDePosicion);
                //Mover 2 si es el primer movimiento
                if (Posicion[1] == '7')
                {
                    calculoDePosicion = NumeroLetra(letraNumero).ToString() + (numeroNumero - 2).ToString();
                    PosicionesValidas.Add(calculoDePosicion);
                    dosPasos = true;
                }
                else PosicionesValidas.Add(null);
            }

            foreach (string b in posicionesOcupadas)
            {
                if (PosicionesValidas[0] == b)
                {
                    PosicionesValidas[0] = null;
                    if (dosPasos) PosicionesValidas[1] = null;

                    break;
                }

                if (dosPasos && PosicionesValidas[1] == b)
                {
                    PosicionesValidas[1] = null;
                }
            }

            return PosicionesValidas;
        }

        public override List<string> comer(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas, List<string> lugaresAtacados)
        {
            char letra = (char)Posicion[0];
            char numero = (char)Posicion[1];
            string calculoDePosicion = "";

            int letraNumero = LetraNumero(letra);
            int numeroNumero = Int32.Parse(numero.ToString());

            bool comerIzquierda = false;
            bool comerDerecha = false;

            List<string> PosicionesValidas = new List<string>();
            if (Blanca)
            {
                //Comer
                if ((char)Posicion[0] != 'H')
                {
                    calculoDePosicion = NumeroLetra(letraNumero + 1).ToString() + (numeroNumero + 1).ToString();
                    PosicionesValidas.Add(calculoDePosicion);
                }
                else PosicionesValidas.Add(null);

                if ((char)Posicion[0] != 'A')
                {
                    calculoDePosicion = NumeroLetra(letraNumero - 1).ToString() + (numeroNumero + 1).ToString();
                    PosicionesValidas.Add(calculoDePosicion);
                }
                else PosicionesValidas.Add(null);


                foreach (string b in posicionesOcupadasNegras)
                {
                    if (PosicionesValidas[0] == null && PosicionesValidas[1] == null)
                    {
                        return null;
                    }
                    else if (PosicionesValidas[0] == b) comerDerecha = true;
                    else if (PosicionesValidas[1] == b) comerIzquierda = true;
                }
            }
            else
            {
                //Comer
                if ((char)Posicion[0] != 'H')
                {
                    calculoDePosicion = NumeroLetra(letraNumero + 1).ToString() + (numeroNumero - 1).ToString();
                    PosicionesValidas.Add(calculoDePosicion);
                }
                else PosicionesValidas.Add(null);

                if ((char)Posicion[0] != 'A')
                {
                    calculoDePosicion = NumeroLetra(letraNumero - 1).ToString() + (numeroNumero - 1).ToString();
                    PosicionesValidas.Add(calculoDePosicion);
                }
                else PosicionesValidas.Add(null);

                foreach (string b in posicionesOcupadasBlancas)
                {
                    if (PosicionesValidas[0] == null && PosicionesValidas[1] == null)
                    {
                        return null;
                    }
                    else if (PosicionesValidas[0] == b) comerDerecha = true;
                    else if (PosicionesValidas[1] == b) comerIzquierda = true;
                }
            }

            if (!comerDerecha) PosicionesValidas[0] = null;
            if (!comerIzquierda) PosicionesValidas[1] = null;

            return PosicionesValidas;
        }

        public bool llegarAlFilnal()
        {
            if (Blanca && Posicion[1] == '8') return true;
            else if (!Blanca && Posicion[1] == '1') return true;
            else return false;
        }
    }
}
