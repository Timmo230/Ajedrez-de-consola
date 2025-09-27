using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chees_console
{
    public class Caballo : Pieza
    {
        public Caballo(char inicial, string posicion, bool blanco)
        {
            this.InicilPieza = inicial;
            this.Posicion = posicion;
            this.Blanca = blanco;
            puntosDePieza = 3;
        }

        public List<string> posicoines(int letraNumero, int numeroNumero)
        {
            List<string> PosicionesValidas = new List<string>();

            PosicionesValidas.Add(Posicion[1] != '1' && Posicion[1] != '2' && Posicion[0] != 'H' ? NumeroLetra(letraNumero + 1) + (numeroNumero - 2).ToString() : null);
            PosicionesValidas.Add(Posicion[1] != '1' && Posicion[1] != '2' && Posicion[0] != 'A' ? NumeroLetra(letraNumero - 1) + (numeroNumero - 2).ToString() : null);
            PosicionesValidas.Add(Posicion[0] != 'G' && Posicion[0] != 'H' && Posicion[1] != '1' ? NumeroLetra(letraNumero + 2) + (numeroNumero - 1).ToString() : null);
            PosicionesValidas.Add(Posicion[0] != 'A' && Posicion[0] != 'B' && Posicion[1] != '1' ? NumeroLetra(letraNumero - 2) + (numeroNumero - 1).ToString() : null);
            PosicionesValidas.Add(Posicion[0] != 'H' && Posicion[1] != '8' && Posicion[1] != '7' ? NumeroLetra(letraNumero + 1) + (numeroNumero + 2).ToString() : null);
            PosicionesValidas.Add(Posicion[0] != 'A' && Posicion[1] != '8' && Posicion[1] != '7' ? NumeroLetra(letraNumero - 1) + (numeroNumero + 2).ToString() : null);
            PosicionesValidas.Add(Posicion[0] != 'A' && Posicion[0] != 'B' && Posicion[1] != '8' ? NumeroLetra(letraNumero - 2) + (numeroNumero + 1).ToString() : null);
            PosicionesValidas.Add(Posicion[0] != 'H' && Posicion[0] != 'G' && Posicion[1] != '8' ? NumeroLetra(letraNumero + 2) + (numeroNumero + 1).ToString() : null);

            return PosicionesValidas;
        }

        public List<string> bucle(string[] posicionesIncaccesibles, List<string> posivilidades)
        {
            int counter = 0;

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
            return posivilidades;
        }

        public override List<string> PosicionesValidas(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas, List<string> lugaresAtacados)
        {
            char letra = (char)Posicion[0];
            char numero = (char)Posicion[1];

            int letraNumero = LetraNumero(letra);
            int numeroNumero = Int32.Parse(numero.ToString());

            List<string> PosicionesValidas = posicoines(letraNumero, numeroNumero);

            PosicionesValidas = bucle(posicionesOcupadas, PosicionesValidas);

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
                PosicionesValidas = bucle(posicionesOcupadasBlancas, PosicionesValidas);

                // Crear una copia de PosicionesValidas
                List<string> PosicionesAEliminar = new List<string>(PosicionesValidas);

                foreach (string posicionValidas in PosicionesValidas)
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
                    PosicionesAEliminar[indice] = borrar ? null : PosicionesAEliminar[indice];
                    indice++;
                }

                PosicionesValidas = PosicionesAEliminar;
            }
            else
            {
                PosicionesValidas = bucle(posicionesOcupadasNegras, PosicionesValidas);

                // Crear una copia de PosicionesValidas
                List<string> PosicionesAEliminar = new List<string>(PosicionesValidas);

                foreach (string posicionValidas in PosicionesValidas)
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
                    PosicionesAEliminar[indice] = borrar ? null : PosicionesAEliminar[indice];
                    indice++;
                }

                PosicionesValidas = PosicionesAEliminar;
            }

            return PosicionesValidas;
        }
    }
}
