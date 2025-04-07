using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chees_console
{
    public class Rey : Pieza
    {
        public bool permiteEnroque { get; set; }
        public bool jaque { get; set; }
        public Rey(char inicial, string posicion, bool blanco)
        {
            this.InicilPieza = inicial;
            this.Posicion = posicion;
            this.Blanca = blanco;
            permiteEnroque = true;
            jaque = false;
        }

        public List<string> bucle(string[] posicionesIncaccesibles, List<string> posicionesIncaccesibles2, List<string> posivilidades)
        {
            int counter = 0;

            if (posicionesIncaccesibles != null)
            {
                foreach (string posicion in posicionesIncaccesibles)
                {
                    counter = 0;
                    foreach (string posicionValida in posivilidades)
                    {
                        if (posicion == posicionValida)
                        {
                            posivilidades[counter] = null;
                            break;
                        }
                        counter++;
                    }
                }
            }
            if (posicionesIncaccesibles2 != null)
            {

                foreach (string posicion in posicionesIncaccesibles2)
                {
                    counter = 0;
                    foreach (string posicionValida in posivilidades)
                    {
                        if (posicion == posicionValida)
                        {
                            posivilidades[counter] = null;
                            break;
                        }
                        counter++;
                    }
                }
            }
            return posivilidades;
        }

        private List<string> posicoines(int letraNumero, int numeroNumero)
        {
            List<string> PosicionesValidas = new List<string>();

            PosicionesValidas.Add(Posicion[0] != 'A' ? NumeroLetra(letraNumero - 1) + numeroNumero.ToString() : null);
            PosicionesValidas.Add(Posicion[0] != 'H' ? NumeroLetra(letraNumero + 1) + numeroNumero.ToString() : null);
            PosicionesValidas.Add(Posicion[1] != '1' ? NumeroLetra(letraNumero) + (numeroNumero - 1).ToString() : null);
            PosicionesValidas.Add(Posicion[1] != '8' ? NumeroLetra(letraNumero) + (numeroNumero + 1).ToString() : null);
            PosicionesValidas.Add(Posicion[0] != 'A' && Posicion[1] != '1' ? NumeroLetra(letraNumero - 1) + (numeroNumero - 1).ToString() : null);
            PosicionesValidas.Add(Posicion[0] != 'A' && Posicion[1] != '8' ? NumeroLetra(letraNumero - 1) + (numeroNumero + 1).ToString() : null);
            PosicionesValidas.Add(Posicion[0] != 'H' && Posicion[1] != '1' ? NumeroLetra(letraNumero + 1) + (numeroNumero - 1).ToString() : null);
            PosicionesValidas.Add(Posicion[0] != 'H' && Posicion[1] != '8' ? NumeroLetra(letraNumero + 1) + (numeroNumero + 1).ToString() : null);

            return PosicionesValidas;
        }

        public override List<string> PosicionesValidas(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas, List<string> lugaresAtacados)
        {
            char letra = (char)Posicion[0];
            char numero = (char)Posicion[1];

            int letraNumero = LetraNumero(letra);
            int numeroNumero = Int32.Parse(numero.ToString());

            List<string> PosicionesValidas = new List<string>();

            PosicionesValidas = posicoines(letraNumero, numeroNumero);

            PosicionesValidas = bucle(posicionesOcupadas, lugaresAtacados, PosicionesValidas);

            return PosicionesValidas;
        }

        public override List<string> comer(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas, List<string> lugaresAtacados)
        {
            char letra = (char)Posicion[0];
            char numero = (char)Posicion[1];

            int letraNumero = LetraNumero(letra);
            int numeroNumero = Int32.Parse(numero.ToString());

            List<string> PosicionesValidas = posicoines(letraNumero, numeroNumero);

            int indice = 0;
            if (Blanca)
            {
                PosicionesValidas = bucle(posicionesOcupadasBlancas, lugaresAtacados, PosicionesValidas);
                List<string> copia = new List<string>(PosicionesValidas);

                foreach (string posicionValidas in copia)
                {
                    bool borrar = true;
                    foreach (string posicion in posicionesOcupadasNegras)
                    {
                        if (posicion == posicionValidas)
                        {
                            borrar = false;
                            break;
                        }
                    }
                    PosicionesValidas[indice] = borrar ? null : PosicionesValidas[indice];
                    indice++;
                }
            }
            else
            {
                PosicionesValidas = bucle(posicionesOcupadasNegras, lugaresAtacados, PosicionesValidas);
                List<string> copia = new List<string>(PosicionesValidas);

                foreach (string posicionValidas in copia)
                {
                    bool borrar = true;
                    foreach (string posicion in posicionesOcupadasBlancas)
                    {
                        if (posicion == posicionValidas)
                        {
                            borrar = false;
                            break;
                        }
                    }
                    PosicionesValidas[indice] = borrar ? null : PosicionesValidas[indice];
                    indice++;
                }
            }

            return PosicionesValidas;
        }

        public List<string> lugaresAtacados()
        {
            return posicoines(LetraNumero(Posicion[0]), Int32.Parse(Posicion[1].ToString()));
        }

        public bool comprovarJaque(List<string> lugaresAtacadosOtroColor)
        {
            foreach (string lugar in lugaresAtacadosOtroColor)
            {
                if (lugar == Posicion)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
