using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chees_console
{
    public class FormarDeMoverAvanzadas : Pieza
    {
        protected List<string> PosicionesValidasTorre(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas)
        {
            char letra = (char)Posicion[0];
            char numero = (char)Posicion[1];

            int letraNumero = LetraNumero(letra);
            int numeroNumero = Int32.Parse(numero.ToString());

            List<string> PosicionesValidas = new List<string>();

            int[] letrasPosibles = new int[7];
            int[] numerosPosibles = new int[7];

            List<int> letrasNumerosPosiblesParaComer = new List<int>();

            int comprobarLetra = letraNumero;
            int comprobarNumero = numeroNumero;


            //Esto mira las posiciones posibles
            bool seguir = true;
            //Letras
            int counter1 = 0;
            comprobarLetra = LetraNumero(Posicion[0]) - 1;
            while (comprobarLetra != 0 && seguir)
            {
                char l = NumeroLetra(comprobarLetra);
                foreach (string c in posicionesOcupadas)
                {
                    if (l.ToString() + Posicion[1] == c)
                    {
                        seguir = false;
                    }
                }

                if (seguir)
                {
                    letrasPosibles[counter1] = comprobarLetra;
                    counter1++;
                    comprobarLetra--;
                }
                else
                {
                    letrasNumerosPosiblesParaComer.Add(comprobarLetra--);
                    letrasNumerosPosiblesParaComer.Add(Int32.Parse(Posicion[1].ToString()));
                }
            }

            seguir = true;
            comprobarLetra = LetraNumero(Posicion[0]) + 1;
            while (comprobarLetra != 9 && seguir)
            {
                char l = NumeroLetra(comprobarLetra);

                foreach (string c in posicionesOcupadas)
                {
                    if (l.ToString() + Posicion[1] == c)
                    {
                        seguir = false;
                    }

                    if (!seguir)
                    {
                        break;
                    }
                }

                if (seguir)
                {
                    letrasPosibles[counter1] = comprobarLetra;
                    counter1++;
                    comprobarLetra++;
                }
                else
                {
                    letrasNumerosPosiblesParaComer.Add(comprobarLetra++);
                    letrasNumerosPosiblesParaComer.Add(Int32.Parse(Posicion[1].ToString()));
                }
            }

            //Numeros
            counter1 = 0;

            seguir = true;
            comprobarNumero = Int32.Parse(Posicion[1].ToString()) - 1;
            while (comprobarNumero != 0 && seguir)
            {

                foreach (string c in posicionesOcupadas)
                {
                    if (Posicion[0] + comprobarNumero.ToString() == c)
                    {
                        seguir = false;
                    }
                }

                if (seguir)
                {
                    numerosPosibles[counter1] = comprobarNumero;
                    counter1++;
                    comprobarNumero--;
                }
                else
                {
                    letrasNumerosPosiblesParaComer.Add(LetraNumero(Posicion[0]));
                    letrasNumerosPosiblesParaComer.Add(comprobarNumero--);
                }
            }

            seguir = true;
            comprobarNumero = Int32.Parse(Posicion[1].ToString()) + 1;
            while (comprobarNumero != 9 && seguir)
            {
                foreach (string c in posicionesOcupadas)
                {
                    if (Posicion[0] + comprobarNumero.ToString() == c)
                    {
                        seguir = false;
                    }
                }

                if (seguir)
                {
                    numerosPosibles[counter1] = comprobarNumero;
                    counter1++;

                    comprobarNumero++;
                }
                else
                {
                    letrasNumerosPosiblesParaComer.Add(LetraNumero(Posicion[0]));
                    letrasNumerosPosiblesParaComer.Add(comprobarNumero++);
                }
            }

            //Añade las posiciones al array final
            foreach (int i in letrasPosibles)
            {
                char l = NumeroLetra(i);
                if (i != 0)
                {
                    PosicionesValidas.Add(l.ToString() + Posicion[1]);
                }
            }

            foreach (int i in numerosPosibles)
            {
                if (i != 0)
                {
                    PosicionesValidas.Add(Posicion[0] + i.ToString());
                }
            }

            letrasNumerosPosiblesParaComer.ForEach(n =>
            {
                PosicionesValidas.Add(n.ToString());
            });

            return PosicionesValidas;
        }

        protected List<string> PosicionesValidasAlfil(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas)
        {

            char letra = (char)Posicion[0];
            char numero = (char)Posicion[1];

            int letraNumero = LetraNumero(letra);
            int numeroNumero = Int32.Parse(numero.ToString());

            List<string> PosicionesValidas = new List<string>();

            int comprobarLetra = letraNumero;
            int comprobarNumero = numeroNumero;

            List<int> comprobarLetraNumeroParaComer = new List<int>();

            bool seguir = true;
            //Letras

            comprobarLetra = LetraNumero(Posicion[0]) + 1;
            comprobarNumero = Int32.Parse(Posicion[1].ToString()) + 1;


            while (comprobarLetra < 9 && comprobarNumero < 9 && seguir)
            {
                char l = NumeroLetra(comprobarLetra);
                int n = comprobarNumero;
                foreach (string c in posicionesOcupadas)
                {
                    if (l.ToString() + n == c)
                    {
                        seguir = false;
                        break;
                    }
                }

                if (seguir)
                {
                    PosicionesValidas.Add(NumeroLetra(comprobarLetra) + comprobarNumero.ToString());
                    comprobarLetra++;
                    comprobarNumero++;
                }
                else
                {
                    comprobarLetraNumeroParaComer.Add(comprobarLetra++);
                    comprobarLetraNumeroParaComer.Add(comprobarNumero++);
                }
            }

            seguir = true;
            comprobarLetra = LetraNumero(Posicion[0]) + 1;
            comprobarNumero = Int32.Parse(Posicion[1].ToString()) - 1;
            while (comprobarLetra < 9 && comprobarNumero > 0 && seguir)
            {
                char l = NumeroLetra(comprobarLetra);
                int n = comprobarNumero;
                foreach (string c in posicionesOcupadas)
                {
                    if (l.ToString() + n == c)
                    {
                        seguir = false;
                        break;
                    }
                }

                if (seguir)
                {
                    PosicionesValidas.Add(NumeroLetra(comprobarLetra) + comprobarNumero.ToString());
                    comprobarLetra++;
                    comprobarNumero--;
                }
                else
                {
                    comprobarLetraNumeroParaComer.Add(comprobarLetra++);
                    comprobarLetraNumeroParaComer.Add(comprobarNumero--);
                }
            }


            seguir = true;
            comprobarLetra = LetraNumero(Posicion[0]) - 1;
            comprobarNumero = Int32.Parse(Posicion[1].ToString()) + 1;

            while (comprobarLetra > 0 && comprobarNumero < 9 && seguir)
            {
                char l = NumeroLetra(comprobarLetra);
                int n = comprobarNumero;
                foreach (string c in posicionesOcupadas)
                {
                    if (l.ToString() + n == c)
                    {
                        seguir = false;
                        break;
                    }
                }

                if (seguir)
                {
                    PosicionesValidas.Add(NumeroLetra(comprobarLetra) + comprobarNumero.ToString());
                    comprobarLetra--;
                    comprobarNumero++;
                }
                else
                {
                    comprobarLetraNumeroParaComer.Add(comprobarLetra--);
                    comprobarLetraNumeroParaComer.Add(comprobarNumero++);
                }
            }

            seguir = true;
            comprobarLetra = LetraNumero(Posicion[0]) - 1;
            comprobarNumero = Int32.Parse(Posicion[1].ToString()) - 1;
            while (comprobarLetra > 0 && comprobarNumero > 0 && seguir)
            {
                char l = NumeroLetra(comprobarLetra);
                int n = comprobarNumero;
                foreach (string c in posicionesOcupadas)
                {
                    if (l.ToString() + n == c)
                    {
                        seguir = false;
                        break;
                    }
                }

                if (seguir)
                {
                    PosicionesValidas.Add(NumeroLetra(comprobarLetra) + comprobarNumero.ToString());
                    comprobarLetra--;
                    comprobarNumero--;
                }
                else
                {
                    comprobarLetraNumeroParaComer.Add(comprobarLetra--);
                    comprobarLetraNumeroParaComer.Add(comprobarNumero--);
                }
            }


            foreach (int a in comprobarLetraNumeroParaComer)
            {
                PosicionesValidas.Add(a.ToString());
            }


            return PosicionesValidas;
        }

        protected List<string> ComerAlfil(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas)
        {
            int Counter = 0;

            List<string> posicionesValidas = PosicionesValidasAlfil(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);

            posicionesValidas.RemoveAll(n => int.TryParse(n, out int x) == false);

            List<string> posicoinesComibles = new List<string>();

            for (int i = Counter; i < posicionesValidas.Count; i++)
            {
                if (Blanca) posicoinesComibles.Add(PlantillaComerAlfil(posicionesOcupadasNegras, Int32.Parse(posicionesValidas[i]), Int32.Parse(posicionesValidas[i + 1])));
                else posicoinesComibles.Add(PlantillaComerAlfil(posicionesOcupadasBlancas, Int32.Parse(posicionesValidas[i]), Int32.Parse(posicionesValidas[i + 1])));
                i++;
            }

            return posicoinesComibles;
        }

        private string PlantillaComerAlfil(string[] posicionesOcupadasOtroColor, int comprobarLetra, int comprobarNumero)
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

        protected List<string> ComerTorre(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas)
        {
            int Counter = 0;

            List<string> posicionesValidas = PosicionesValidasTorre(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);

            posicionesValidas.ForEach(n => {
                bool counter = true;
                if (int.TryParse(n, out int number)) counter = false;

                if (counter) Counter++;
            });

            List<string> posicoinesComibles = new List<string>();

            for (int i = Counter; i < posicionesValidas.Count; i++)
            {
                if (Blanca) posicoinesComibles.Add(PlantillaComerTorre(posicionesOcupadasNegras, Int32.Parse(posicionesValidas[i]), Int32.Parse(posicionesValidas[i + 1])));
                else posicoinesComibles.Add(PlantillaComerTorre(posicionesOcupadasBlancas, Int32.Parse(posicionesValidas[i]), Int32.Parse(posicionesValidas[i + 1])));
                i++;
            }

            return posicoinesComibles;
        }

        private string PlantillaComerTorre(string[] posicionesOcupadasOtroColor, int comprobarLetra, int comprobarNumero)
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
