using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Chees_console.Program;

namespace Chees_console
{
    internal class Program
    {
        public enum Mov { moverPeon, moverPieza, comerConPeon, comerConPieza, enroque, moverEspecificoLetra, moverEspecificoNumero, comerEspecificoLetra, comerEspecificoNumero }
        public enum Piezas { peon, torre, caballo, alfil, dama, rey }

        public static string[,] Tablero =
        {
            {"A8", "B8", "C8", "D8", "E8", "F8", "G8", "H8"},
            {"A7", "B7", "C7", "D7", "E7", "F7", "G7", "H7"},
            {"A6", "B6", "C6", "D6", "E6", "F6", "G6", "H6"},
            {"A5", "B5", "C5", "D5", "E5", "F5", "G5", "H5"},
            {"A4", "B4", "C4", "D4", "E4", "F4", "G4", "H4"},
            {"A3", "B3", "C3", "D3", "E3", "F3", "G3", "H3"},
            {"A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2"},
            {"A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1"}
        };

        public static string[,] TableroAlReves =
        {
            {"H1", "G1", "F1", "E1", "D1", "C1", "B1", "A1"},
            {"H2", "G2", "F2", "E2", "D2", "C2", "B2", "A2"},
            {"H3", "G3", "F3", "E3", "D3", "C3", "B3", "A3"},
            {"H4", "G4", "F4", "E4", "D4", "C4", "B4", "A4"},
            {"H5", "G5", "F5", "E5", "D5", "C5", "B5", "A5"},
            {"H6", "G6", "F6", "E6", "D6", "C6", "B6", "A6"},
            {"H7", "G7", "F7", "E7", "D7", "C7", "B7", "A7"},
            {"H8", "G8", "F8", "E8", "D8", "C8", "B8", "A8"}
        };

        public static Peon[] peonesBlancos = new Peon[10];
        public static Peon[] peonesNegros = new Peon[10];
        public static Torre[] torresBlancas = new Torre[10];
        public static Torre[] torresNegras = new Torre[10];
        public static Caballo[] caballosBlancos = new Caballo[10];
        public static Caballo[] caballosNegros = new Caballo[10];
        public static Alfil[] alfilesBlancos = new Alfil[10];
        public static Alfil[] alfilesNegros = new Alfil[10];
        public static Rey[] reyBlanco = new Rey[1];
        public static Rey[] reyNegro = new Rey[1];
        public static Dama[] damaBlanca = new Dama[10];
        public static Dama[] damaNegra = new Dama[10];

        public static Pieza[,] piezas = new Pieza[12, 10];
        public static Pieza[,] piezasBlancas = new Pieza[6, 10];
        public static Pieza[,] piezasNegras = new Pieza[6, 10];

        public static bool turnoBlanco = true;

        public static bool jugar = true;

        public static List<string> movimientos = new List<string>();

        static void Main(string[] args)
        {
            inicializacionDeTodo();

            while (jugar)
            {
                juego();
            }
        }

        public static void juego()
        {
            reiniciarComerAlPaso();

            ArrayList output = null;
            string input;
            resetTable();
            ArrayList correcto = null;
            bool Enroque = false;

            bool alguienGano = true;

            bool repetir = false;
            do
            {
                alguienGano = comprovarJaqueMate(LugaresOcupados(), LugaresOcupadosNegras(), LugaresOcupadosBlancas());

                if (!alguienGano)
                {
                    int counter = 1;
                    if (!turnoBlanco) Console.WriteLine("\nGanaron blancas\n");
                    else Console.WriteLine("\nGanaron negras\n");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.WriteLine();
                    Console.WriteLine("Movimientos de la partida: ");
                    foreach (string movimiento in movimientos)
                    {
                        if (counter % 2 == 0)
                        {
                            Console.WriteLine(movimiento);
                        }
                        else
                        {
                            Console.Write("{0} -> {1} -", (counter / 2) + 1, movimiento);
                        }
                        counter++;
                    }

                    Console.WriteLine("\n");
                    Environment.Exit(0);
                }
                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.Cyan;
                if (turnoBlanco) Console.WriteLine("Torno blancas: ");
                else Console.WriteLine("Turno negras:");
                Console.Write("Ingrese su movimiento: ");
                input = Console.ReadLine();
                Console.WriteLine();
                output = conversion(input);

                if (output != null && output.Count == 3)
                {
                    switch (output[1])
                    {
                        case Piezas pieza when (pieza == Piezas.peon && (bool)output[2] == false):
                            correcto = moverPeon(output[0].ToString());
                            break;
                        case Piezas pieza when (pieza != Piezas.rey && (bool)output[2] == false):
                            correcto = moverPieza(output[0].ToString(), (Piezas)output[1]);
                            break;
                        case Piezas pieza when (pieza == Piezas.rey && (bool)output[2] == false):
                            correcto = moverPieza(output[0].ToString(), (Piezas)output[1]);
                            break;

                        case Piezas pieza when (pieza == Piezas.peon && (bool)output[2] == true):
                            correcto = ComerPeon(output[0].ToString(), input);
                            break;
                        case Piezas pieza when (pieza != Piezas.rey && (bool)output[2] == true):
                            correcto = comerPieza(output[0].ToString(), (Piezas)output[1]);
                            break;
                        case Piezas pieza when (pieza == Piezas.rey && (bool)output[2] == true):
                            correcto = comerPieza(output[0].ToString(), (Piezas)output[1]);
                            break;
                    }
                }
                else if (output != null && output.Count == 6)
                {
                    switch (output[1])
                    {
                        case Piezas pieza when (pieza == Piezas.torre && (bool)output[2] == false && (bool)output[3] && !(bool)output[4]):
                            correcto = moverPiezaEspecificoLetra(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;
                        case Piezas pieza when (pieza == Piezas.torre && (bool)output[2] == false && !(bool)output[3] && (bool)output[4]):
                            correcto = moverPiezaEspecificoNumero(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;

                        case Piezas pieza when (pieza == Piezas.caballo && (bool)output[2] == false && (bool)output[3] && !(bool)output[4]):
                            correcto = moverPiezaEspecificoLetra(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;
                        case Piezas pieza when (pieza == Piezas.caballo && (bool)output[2] == false && !(bool)output[3] && (bool)output[4]):
                            correcto = moverPiezaEspecificoNumero(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;

                        case Piezas pieza when (pieza == Piezas.dama && (bool)output[2] == false && (bool)output[3] && !(bool)output[4]):
                            correcto = moverPiezaEspecificoLetra(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;
                        case Piezas pieza when (pieza == Piezas.dama && (bool)output[2] == false && !(bool)output[3] && (bool)output[4]):
                            correcto = moverPiezaEspecificoNumero(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;

                        case Piezas pieza when (pieza == Piezas.alfil && (bool)output[2] == false && (bool)output[3] && !(bool)output[4]):
                            correcto = moverPiezaEspecificoLetra(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;
                        case Piezas pieza when (pieza == Piezas.alfil && (bool)output[2] == false && !(bool)output[3] && (bool)output[4]):
                            correcto = moverPiezaEspecificoNumero(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;



                        case Piezas pieza when (pieza == Piezas.torre && (bool)output[2] == true && (bool)output[3] && !(bool)output[4]):
                            correcto = comerPiezaEspecificoLetra(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;
                        case Piezas pieza when (pieza == Piezas.torre && (bool)output[2] == true && !(bool)output[3] && (bool)output[4]):
                            correcto = comerPiezaEspecificoNumero(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;

                        case Piezas pieza when (pieza == Piezas.caballo && (bool)output[2] == true && (bool)output[3] && !(bool)output[4]):
                            correcto = comerPiezaEspecificoLetra(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;
                        case Piezas pieza when (pieza == Piezas.caballo && (bool)output[2] == true && !(bool)output[3] && (bool)output[4]):
                            correcto = comerPiezaEspecificoNumero(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;

                        case Piezas pieza when (pieza == Piezas.dama && (bool)output[2] == true && (bool)output[3] && !(bool)output[4]):
                            correcto = comerPiezaEspecificoLetra(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;
                        case Piezas pieza when (pieza == Piezas.dama && (bool)output[2] == true && !(bool)output[3] && (bool)output[4]):
                            correcto = comerPiezaEspecificoNumero(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;

                        case Piezas pieza when (pieza == Piezas.dama && (bool)output[2] == true && (bool)output[3] && !(bool)output[4]):
                            correcto = comerPiezaEspecificoLetra(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;
                        case Piezas pieza when (pieza == Piezas.dama && (bool)output[2] == true && !(bool)output[3] && (bool)output[4]):
                            correcto = comerPiezaEspecificoNumero(output[0].ToString(), (Piezas)output[1], (List<string>)output[5], input);
                            break;
                    }
                }
                else if (output != null && output[1].GetType().ToString() == "Ajedrez.Program+Mov")
                {
                    if (output != null && output.Count == 2 && (Mov)output[1] == Mov.enroque && correcto == null)
                    {

                        if (turnoBlanco)
                        {
                            Enroque = enroque(output[0].ToString(), reyBlanco);
                        }
                        else
                        {
                            Enroque = enroque(output[0].ToString(), reyNegro);
                        }

                        if (Enroque)
                        {
                            repetir = true;
                        }
                    }
                }

                if (!Enroque && output != null && correcto != null)
                {
                    repetir = comprovarSiFunciona((Pieza)correcto[0], correcto[1].ToString());
                }

                if (!repetir)
                {
                    resetTable();
                    Console.WriteLine("Error");
                }
                else
                {
                    movimientos.Add(input);
                }

            } while (!repetir);

            if ((output.Count == 3 || output.Count == 6) && turnoBlanco && (bool)output[2] == true)
            {
                eliminar((Piezas)output[1], true);
                if ((Piezas)output[1] == Piezas.peon)
                {
                    cambioDePieza();
                }
            }
            else if ((output.Count == 3 || output.Count == 6) && !turnoBlanco && (bool)output[2] == true)
            {
                eliminar((Piezas)output[1], false);
                if ((Piezas)output[1] == Piezas.peon)
                {
                    cambioDePieza();
                }
            }

            turnoBlanco = !turnoBlanco;
        }

        //Cambiar tipo de pieza si peon llega al final
        public static void cambioDePieza()
        {
            bool cambio = false;
            Peon peonCambiar = null;
            if (turnoBlanco)
            {
                foreach (Peon item in peonesBlancos)
                {
                    if (item != null)
                    {
                        cambio = item.llegarAlFilnal();

                        if (cambio)
                        {
                            peonCambiar = item;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (Peon item in peonesNegros)
                {
                    if (item != null)
                    {
                        cambio = item.llegarAlFilnal();

                        if (cambio)
                        {
                            peonCambiar = item;
                            break;
                        }
                    }
                }
            }

            if (cambio)
            {
                int indice = 0;
                foreach (Pieza piez in piezas)
                {
                    if (piez == peonCambiar)
                    {
                        break;
                    }
                    indice++;
                }
                string posicionPeon = peonCambiar.Posicion;
                bool repetir = true;

                while (repetir)
                {
                    Console.WriteLine("¿A que pieza quiere combertir el peon?:\n\tCaballo = c\n\tAlfil = a\n\tTorre = t\n\tDama = d\n-----------------------------");
                    Console.Write("Pieza : ");

                    string inputPieza = Console.ReadLine().ToLower().Trim();

                    if (inputPieza != "c" && inputPieza != "a" && inputPieza != "t" && inputPieza != "d")
                    {
                        resetTable();
                        Console.WriteLine("Error");
                    }
                    else
                    {
                        repetir = false;
                        int counter = 0;
                        int counter2 = 0;
                        if (turnoBlanco)
                        {

                            foreach (Pieza pieza in piezasBlancas)
                            {
                                if (pieza == peonCambiar) break;

                                counter2++;
                            }

                            switch (inputPieza)
                            {
                                case "c":
                                    foreach (Pieza pieza in caballosBlancos)
                                    {
                                        if (pieza == null) break;

                                        counter++;
                                    }

                                    Caballo caballoDePeon = new Caballo('c', posicionPeon, true);
                                    piezas[indice / 10, indice % 10] = caballoDePeon;
                                    caballosBlancos[counter] = caballoDePeon;
                                    piezasBlancas[counter2 / 10, counter2 % 10] = caballoDePeon;
                                    break;
                                case "a":
                                    foreach (Pieza pieza in alfilesBlancos)
                                    {
                                        if (pieza == null) break;

                                        counter++;
                                    }

                                    Alfil alfilDePeon = new Alfil('a', posicionPeon, true);
                                    piezas[indice / 10, indice % 10] = alfilDePeon;
                                    alfilesBlancos[counter] = alfilDePeon;
                                    piezasBlancas[counter2 / 10, counter2 % 10] = alfilDePeon;
                                    break;
                                case "t":
                                    foreach (Pieza pieza in torresBlancas)
                                    {
                                        if (pieza == null) break;

                                        counter++;
                                    }
                                    Torre torreDePeon = new Torre('t', posicionPeon, true);
                                    piezas[indice / 10, indice % 10] = torreDePeon;
                                    torresBlancas[counter] = torreDePeon;
                                    piezasBlancas[counter2 / 10, counter2 % 10] = torreDePeon;
                                    break;
                                case "d":
                                    foreach (Pieza pieza in damaBlanca)
                                    {
                                        if (pieza == null) break;

                                        counter++;
                                    }
                                    Dama damaDePeon = new Dama('d', posicionPeon, true);
                                    piezas[indice / 10, indice % 10] = damaDePeon;
                                    damaBlanca[counter] = damaDePeon;
                                    piezasBlancas[counter2 / 10, counter2 % 10] = damaDePeon;
                                    break;
                            }
                        }
                        else
                        {
                            foreach (Pieza pieza in piezasNegras)
                            {
                                if (pieza == peonCambiar) break;

                                counter2++;
                            }

                            switch (inputPieza)
                            {
                                case "c":
                                    foreach (Pieza pieza in caballosNegros)
                                    {
                                        if (pieza == null) break;

                                        counter++;
                                    }
                                    Caballo caballoDePeon = new Caballo('c', posicionPeon, false);
                                    piezas[indice / 10, indice % 10] = caballoDePeon;
                                    caballosNegros[counter] = caballoDePeon;
                                    piezasNegras[counter2 / 10, counter2 % 10] = caballoDePeon;
                                    break;
                                case "a":
                                    foreach (Pieza pieza in alfilesNegros)
                                    {
                                        if (pieza == null) break;

                                        counter++;
                                    }

                                    Alfil alfilDePeon = new Alfil('a', posicionPeon, false);
                                    piezas[indice / 10, indice % 10] = alfilDePeon;
                                    alfilesNegros[counter] = alfilDePeon;
                                    piezasNegras[counter2 / 10, counter2 % 10] = alfilDePeon;
                                    break;
                                case "t":
                                    foreach (Pieza pieza in torresNegras)
                                    {
                                        if (pieza == null) break;

                                        counter++;
                                    }

                                    Torre torreDePeon = new Torre('t', posicionPeon, false);
                                    piezas[indice / 10, indice % 10] = torreDePeon;
                                    torresNegras[counter] = torreDePeon;
                                    piezasNegras[counter2 / 10, counter2 % 10] = torreDePeon;
                                    break;
                                case "d":

                                    foreach (Pieza pieza in damaNegra)
                                    {
                                        if (pieza == null) break;

                                        counter++;
                                    }

                                    Dama damaDePeon = new Dama('d', posicionPeon, false);
                                    piezas[indice / 10, indice % 10] = damaDePeon;
                                    damaNegra[counter] = damaDePeon;
                                    piezasNegras[counter2 / 10, counter2 % 10] = damaDePeon;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        //Comprueba si un movimiento no genera jaques
        public static bool comprovarSiFunciona(Pieza pieza, string posicion)
        {
            bool permitirMovimiento = false;

            Pieza piezaAnalizar = pieza;
            string posicionAntelacion = piezaAnalizar.Posicion;
            piezaAnalizar.Posicion = posicion;

            if (turnoBlanco) permitirMovimiento = reyBlanco[0].comprovarJaque(LugaresAtacadosNegros(LugaresOcupados(), LugaresOcupadosNegras(), LugaresOcupadosBlancas()));
            else permitirMovimiento = reyNegro[0].comprovarJaque(LugaresAtacadosBlancos(LugaresOcupados(), LugaresOcupadosNegras(), LugaresOcupadosBlancas()));

            if (!permitirMovimiento)
            {
                piezaAnalizar.Posicion = posicionAntelacion;
                return false;
            }
            return true;
        }

        public static bool comprovarSiFunciona2(Pieza pieza, string posicion)
        {
            bool permitirMovimiento = false;

            Pieza piezaAnalizar = pieza;
            string posicionAntelacion = piezaAnalizar.Posicion;
            piezaAnalizar.Posicion = posicion;

            if (turnoBlanco) permitirMovimiento = reyBlanco[0].comprovarJaque(LugaresAtacadosNegros(LugaresOcupados(), LugaresOcupadosNegras(), LugaresOcupadosBlancas()));
            else permitirMovimiento = reyNegro[0].comprovarJaque(LugaresAtacadosBlancos(LugaresOcupados(), LugaresOcupadosNegras(), LugaresOcupadosBlancas()));

            piezaAnalizar.Posicion = posicionAntelacion;
            if (!permitirMovimiento) return false;
            return true;
        }

        //comprueva jaque mates
        public static bool comprovarJaqueMate(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas)
        {
            List<string> lugaresAtacadosOtroColor = new List<string>();
            bool seguir = true;
            Rey rey = null;
            if (turnoBlanco)
            {
                lugaresAtacadosOtroColor = LugaresAtacadosNegros(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                lugaresAtacadosOtroColor.RemoveAll(n => n == "" || n == null || int.TryParse(n, out int a));
                rey = reyBlanco[0];
            }
            else
            {
                lugaresAtacadosOtroColor = LugaresAtacadosBlancos(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                lugaresAtacadosOtroColor.RemoveAll(n => n == "" || n == null || int.TryParse(n, out int a));
                rey = reyNegro[0];
            }

            seguir = rey.comprovarJaque(lugaresAtacadosOtroColor);
            List<string> checking = new List<string>();
            if (!seguir)
            {
                bool noMate = false;
                if (turnoBlanco)
                {
                    noMate = plantillaJaqueMate(reyBlanco, posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                    if (noMate) return true;
                    noMate = plantillaJaqueMate(peonesBlancos, posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                    if (noMate) return true;
                    noMate = plantillaJaqueMate(torresBlancas, posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                    if (noMate) return true;
                    noMate = plantillaJaqueMate(caballosBlancos, posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                    if (noMate) return true;
                    noMate = plantillaJaqueMate(alfilesBlancos, posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                    if (noMate) return true;
                    noMate = plantillaJaqueMate(damaBlanca, posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                    if (noMate) return true;
                }
                else
                {
                    noMate = plantillaJaqueMate(reyNegro, posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                    if (noMate) return true;
                    noMate = plantillaJaqueMate(peonesNegros, posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                    if (noMate) return true;
                    noMate = plantillaJaqueMate(torresNegras, posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                    if (noMate) return true;
                    noMate = plantillaJaqueMate(caballosNegros, posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                    if (noMate) return true;
                    noMate = plantillaJaqueMate(alfilesNegros, posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                    if (noMate) return true;
                    noMate = plantillaJaqueMate(damaNegra, posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                    if (noMate) return true;
                }
            }
            else return true;
            return false;
        }

        public static bool plantillaJaqueMate<T>(T[] piezasAnalizar, string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas) where T : Pieza
        {
            List<string> checking = new List<string>();
            bool noMate = false;

            List<string> lugaresAtacadosColorContrario = null;

            string c = piezasAnalizar.GetType().ToString();

            if (piezasAnalizar.GetType().ToString() == "Ajedrez.Rey[]")
            {
                lugaresAtacadosColorContrario = piezasAnalizar[0].Blanca ? LugaresAtacadosNegros(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas) : LugaresAtacadosBlancos(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas);
                lugaresAtacadosColorContrario.RemoveAll(n => n == "" || n == null || int.TryParse(n, out int a));
            }
            foreach (T pieza in piezasAnalizar)
            {
                if (pieza != null)
                {
                    checking = pieza.PosicionesValidas(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas, lugaresAtacadosColorContrario);
                    checking.AddRange(pieza.comer(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas, lugaresAtacadosColorContrario));
                    if (checking != null)
                    {
                        foreach (string posicion in checking)
                        {
                            if (posicion != null)
                            {
                                noMate = comprovarSiFunciona2(pieza, posicion);
                                if (noMate) return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        //Elimina piezas si se comen
        public static void eliminar(Piezas pieza, bool blanca)
        {
            switch (pieza)
            {
                case Piezas.torre:

                    if (blanca) plantillaEliminarBlancas(torresBlancas);
                    else plantillaEliminarNegras(torresNegras);

                    break;
                case Piezas.caballo:
                    if (blanca) plantillaEliminarBlancas(caballosBlancos);
                    else plantillaEliminarNegras(caballosNegros);
                    break;
                case Piezas.alfil:
                    if (blanca) plantillaEliminarBlancas(alfilesBlancos);
                    else plantillaEliminarNegras(alfilesNegros);
                    break;
                case Piezas.dama:

                    if (blanca) plantillaEliminarBlancas(damaBlanca);
                    else plantillaEliminarNegras(damaNegra);
                    break;
                case Piezas.rey:

                    if (blanca) plantillaEliminarBlancas(reyBlanco);
                    else plantillaEliminarNegras(reyNegro);
                    break;
                case Piezas.peon:

                    if (blanca) plantillaEliminarBlancas(peonesBlancos);
                    else plantillaEliminarNegras(peonesNegros);
                    break;
            }
        }

        public static void plantillaEliminarBlancas(Pieza[] piezaQueCome)
        {
            int counter = 0;

            foreach (Pieza pieza in piezaQueCome)
            {
                counter = 0;
                if (pieza != null)
                {
                    foreach (Pieza piezaNegra in piezasNegras)
                    {
                        if (piezaNegra != null && pieza != null && piezaNegra.Posicion == pieza.Posicion)
                        {
                            char inicial = ' ';
                            if (piezasNegras[counter / 10, counter % 10] != null)
                            {
                                inicial = piezasNegras[counter / 10, counter % 10].InicilPieza;
                                piezasNegras[counter / 10, counter % 10] = null;

                                if (counter / 10 == 0) piezas[1, counter % 10] = null;
                                else if (counter / 10 == 1) piezas[3, counter % 10] = null;
                                else if (counter / 10 == 2) piezas[5, counter % 10] = null;
                                else if (counter / 10 == 3) piezas[7, counter % 10] = null;
                                else if (counter / 10 == 4) piezas[9, counter % 10] = null;
                                else if (counter / 10 == 5) piezas[11, counter % 10] = null;

                                switch (inicial)
                                {
                                    case 't':
                                        torresNegras[counter % 10] = null;
                                        break;
                                    case 'a':
                                        alfilesNegros[counter % 10] = null;
                                        break;
                                    case 'c':
                                        caballosNegros[counter % 10] = null;
                                        break;
                                    case 'd':
                                        damaNegra[counter % 10] = null;
                                        break;
                                    case 'r':
                                        reyNegro[0] = null;
                                        break;
                                    case 'p':
                                        peonesNegros[counter % 10] = null;
                                        break;
                                }
                                break;
                            }
                        }

                        counter++;
                    }
                }
            }
        }

        public static void plantillaEliminarNegras(Pieza[] piezaQueCome)
        {
            int counter = 0;

            foreach (Pieza pieza in piezaQueCome)
            {
                counter = 0;
                foreach (Pieza piezaOtroColor in piezasBlancas)
                {
                    if (pieza != null && piezaOtroColor != null && piezaOtroColor.Posicion == pieza.Posicion)
                    {
                        char inicial = ' ';
                        if (piezasBlancas[counter / 10, counter % 10] != null)
                        {
                            inicial = piezasBlancas[counter / 10, counter % 10].InicilPieza;
                            piezasBlancas[counter / 10, counter % 10] = null;

                            if (counter / 10 == 0) piezas[0, counter % 10] = null;
                            else if (counter / 10 == 1) piezas[2, counter % 10] = null;
                            else if (counter / 10 == 2) piezas[4, counter % 10] = null;
                            else if (counter / 10 == 3) piezas[6, counter % 10] = null;
                            else if (counter / 10 == 4) piezas[8, counter % 10] = null;
                            else if (counter / 10 == 5) piezas[10, counter % 10] = null;

                            switch (inicial)
                            {
                                case 't':
                                    torresBlancas[counter % 10] = null;
                                    break;
                                case 'a':
                                    alfilesBlancos[counter % 10] = null;
                                    break;
                                case 'c':
                                    caballosBlancos[counter % 10] = null;
                                    break;
                                case 'd':
                                    damaBlanca[counter % 10] = null;
                                    break;
                                case 'r':
                                    reyBlanco[0] = null;
                                    break;
                                case 'p':
                                    peonesBlancos[counter % 10] = null;
                                    break;
                            }
                            break;
                        }
                    }

                    counter++;
                }
            }
        }

        //Hace la conversion del input y devuelve varios valores como la pieza que se va a mover, a donde etc
        public static ArrayList conversion(string input)
        {
            //variable retorno
            ArrayList retorno = new ArrayList();
            string posicion = "";
            Piezas? pieza = null;
            bool? comer = null;

            bool especificoLetra = false;
            bool especificoNumero = false;

            string[] pattern = { "^[TCAD][a-h][a-h].*[1-8]$", "^[TCAD][1-8][a-h].*[1-8]$", "^[TCAD][a-h]x[a-h].*[1-8]$", "^[TCAD][1-8]x[a-h].*[1-8]$", "^[a-h][1-8]", "0-0", "0-0-0", "^[TCADR][a-h].*[1-8]$", "^[a-h]x[a-h][1-8]$", "^[TCADR]x[a-h][1-8]$" };
            Match match = null;

            Mov? mov = null;
            int counter = 0;
            foreach (string Pattern in pattern)
            {
                match = Regex.Match(input, Pattern);

                if (match.Success)
                {
                    break;
                }
                counter++;
            }

            //Dice lo que esta pasando
            Func<Mov>[] actions = new Func<Mov>[]
            {
                () => Mov.moverEspecificoLetra,
                () => Mov.moverEspecificoNumero,
                () => Mov.comerEspecificoLetra,
                () => Mov.comerEspecificoNumero,
                () => Mov.moverPeon,
                () => Mov.enroque,
                () => Mov.enroque,
                () => Mov.moverPieza,
                () => Mov.comerConPeon,
                () => Mov.comerConPieza
            };
            //Dice la pieza por la letra
            Func<char, Piezas> indexPieza = delegate (char inicial)
            {
                switch (inicial)
                {
                    case 'T':
                        return Piezas.torre;
                    case 'C':
                        return Piezas.caballo;
                    case 'A':
                        return Piezas.alfil;
                    case 'D':
                        return Piezas.dama;
                    default:
                        return Piezas.rey;
                }
            };


            if (match.Success)
            {
                bool? letrasONumeros = null;
                Piezas inicialPieza = indexPieza(input[0]);

                List<string> nomenclaturaSimilar = null;
                if (inicialPieza == Piezas.torre)
                {
                    if (turnoBlanco) nomenclaturaSimilar = nomenclaturaIgual(torresBlancas);
                    else nomenclaturaSimilar = nomenclaturaIgual(torresNegras);

                    if (nomenclaturaSimilar != null)
                    {
                        foreach (string pos in nomenclaturaSimilar)
                        {
                            if (pos == input[input.Length - 2].ToString().ToUpper() + input[input.Length - 1])
                            {
                                if (turnoBlanco) letrasONumeros = LetrasONumeros(torresBlancas);
                                else letrasONumeros = LetrasONumeros(torresNegras);
                            }
                        }
                    }
                }
                else if (inicialPieza == Piezas.caballo)
                {
                    if (turnoBlanco) nomenclaturaSimilar = nomenclaturaIgual(caballosBlancos);
                    else nomenclaturaSimilar = nomenclaturaIgual(caballosNegros);

                    if (nomenclaturaSimilar != null)
                    {
                        foreach (string pos in nomenclaturaSimilar)
                        {
                            if (pos == input[input.Length - 2].ToString().ToUpper() + input[input.Length - 1])
                            {
                                if (turnoBlanco) letrasONumeros = LetrasONumeros(caballosBlancos);
                                else letrasONumeros = LetrasONumeros(caballosNegros);
                            }
                        }
                    }
                }
                else if (inicialPieza == Piezas.dama)
                {
                    if (turnoBlanco) nomenclaturaSimilar = nomenclaturaIgual(damaBlanca);
                    else nomenclaturaSimilar = nomenclaturaIgual(damaNegra);

                    if (nomenclaturaSimilar != null)
                    {
                        foreach (string pos in nomenclaturaSimilar)
                        {
                            if (pos == input[input.Length - 2].ToString().ToUpper() + input[input.Length - 1])
                            {
                                if (turnoBlanco) letrasONumeros = LetrasONumeros(damaBlanca);
                                else letrasONumeros = LetrasONumeros(damaNegra);
                            }
                        }
                    }
                }
                else if (inicialPieza == Piezas.alfil)
                {
                    if (turnoBlanco) nomenclaturaSimilar = nomenclaturaIgual(alfilesBlancos);
                    else nomenclaturaSimilar = nomenclaturaIgual(alfilesNegros);

                    if (nomenclaturaSimilar != null)
                    {
                        foreach (string pos in nomenclaturaSimilar)
                        {
                            if (pos == input[input.Length - 2].ToString().ToUpper() + input[input.Length - 1])
                            {
                                if (turnoBlanco) letrasONumeros = LetrasONumeros(alfilesBlancos);
                                else letrasONumeros = LetrasONumeros(alfilesNegros);
                            }
                        }
                    }
                }

                mov = actions[counter]();

                if (mov != null)
                {
                    switch (mov)
                    {
                        case Mov.moverPeon:
                            posicion = input.ToUpper();
                            pieza = Piezas.peon;
                            comer = false;
                            break;
                        case Mov p when (p == Mov.moverPieza && letrasONumeros == null):
                            posicion = input[1].ToString().ToUpper() + input[2];
                            pieza = inicialPieza;
                            comer = false;
                            break;
                        case Mov.comerConPeon:
                            posicion = input[2].ToString().ToUpper() + input[3];
                            pieza = Piezas.peon;
                            comer = true;
                            break;
                        case Mov p when (p == Mov.comerConPieza && letrasONumeros == null):
                            posicion = input[2].ToString().ToUpper() + input[3];
                            pieza = inicialPieza;
                            comer = true;
                            break;
                        case Mov.enroque:
                            retorno.Add(input);
                            retorno.Add(Mov.enroque);
                            return retorno;
                        case Mov p when (p == Mov.moverEspecificoLetra && letrasONumeros == true):
                            posicion = input[2].ToString().ToUpper() + input[3];
                            pieza = inicialPieza;
                            comer = false;
                            especificoLetra = true;
                            especificoNumero = false;

                            retorno.Add(posicion);
                            retorno.Add(pieza);
                            retorno.Add(comer);
                            retorno.Add(especificoLetra);
                            retorno.Add(especificoNumero);
                            retorno.Add(nomenclaturaSimilar);
                            return retorno;
                        case Mov p when (p == Mov.moverEspecificoNumero && letrasONumeros == false):
                            posicion = input[2].ToString().ToUpper() + input[3];
                            pieza = inicialPieza;
                            comer = false;
                            especificoLetra = false;
                            especificoNumero = true;

                            retorno.Add(posicion);
                            retorno.Add(pieza);
                            retorno.Add(comer);
                            retorno.Add(especificoLetra);
                            retorno.Add(especificoNumero);
                            retorno.Add(nomenclaturaSimilar);
                            return retorno;
                        case Mov p when (p == Mov.comerEspecificoLetra && letrasONumeros == true):
                            posicion = input[3].ToString().ToUpper() + input[4];
                            pieza = inicialPieza;
                            comer = true;
                            especificoLetra = true;
                            especificoNumero = false;

                            retorno.Add(posicion);
                            retorno.Add(pieza);
                            retorno.Add(comer);
                            retorno.Add(especificoLetra);
                            retorno.Add(especificoNumero);
                            retorno.Add(nomenclaturaSimilar);
                            return retorno;
                        case Mov p when (p == Mov.comerEspecificoNumero && letrasONumeros == false):
                            posicion = input[3].ToString().ToUpper() + input[4];
                            pieza = inicialPieza;
                            comer = true;
                            especificoLetra = false;
                            especificoNumero = true;

                            retorno.Add(posicion);
                            retorno.Add(pieza);
                            retorno.Add(comer);
                            retorno.Add(especificoLetra);
                            retorno.Add(especificoNumero);
                            retorno.Add(nomenclaturaSimilar);
                            return retorno;

                    }

                    retorno.Add(posicion);
                    retorno.Add(pieza);
                    retorno.Add(comer);
                    return retorno;
                }
            }

            return null;
        }

        //Crea el tablero
        public static void CrearTablero()
        {
            if (turnoBlanco) PlantillaCrearTablero(Tablero);
            else PlantillaCrearTablero(TableroAlReves);
        }

        public static void PlantillaCrearTablero(string[,] Tablero_)
        {
            bool color = false;
            int counter = 1;
            string InicialDeLaCasilla;
            bool añadirEspacio = true;
            foreach (string a in Tablero_)
            {
                color = !color;
                switch (color)
                {
                    case true:
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        break;
                    case false:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;
                }

                InicialDeLaCasilla = inicialDevolver(a);


                if (añadirEspacio)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (i == 7)
                        {
                            Console.WriteLine("       ");
                        }
                        else
                        {
                            Console.Write("       ");
                        }
                        color = !color;

                        switch (color)
                        {
                            case true:
                                Console.BackgroundColor = ConsoleColor.White;
                                break;
                            case false:
                                Console.BackgroundColor = ConsoleColor.Black;
                                break;
                        }
                    }
                }

                if (InicialDeLaCasilla == " ")
                {
                    if (counter % 8 == 0)
                    {
                        Console.WriteLine("       ");
                        color = !color;
                        añadirEspacio = true;
                    }
                    else
                    {
                        Console.Write("       ");
                        añadirEspacio = false;
                    }
                }
                else
                {
                    if (counter % 8 == 0)
                    {
                        Console.WriteLine("  " + InicialDeLaCasilla.ToString().ToUpper() + "   ");
                        color = !color;
                        añadirEspacio = true;
                    }
                    else
                    {
                        Console.Write("  " + InicialDeLaCasilla.ToString().ToUpper() + "   ");
                        añadirEspacio = false;
                    }
                }

                if (añadirEspacio)
                {
                    for (int i = 0; i < 8; i++)
                    {

                        switch (color)
                        {
                            case true:
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.BackgroundColor = ConsoleColor.White;
                                break;
                            case false:
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.BackgroundColor = ConsoleColor.Black;
                                break;
                        }
                        color = !color;

                        if (i == 7)
                        {
                            Console.WriteLine("       ");
                        }
                        else
                        {
                            Console.Write("       ");
                        }


                    }
                }

                counter++;
            }
        }

        public static string inicialDevolver(string b)
        {
            string InicialDeLaCasilla = " ";


            foreach (Pieza c in piezas)
            {
                if (c != null && c.Posicion == b)
                {
                    DevolverColor(c);
                    InicialDeLaCasilla += c.InicilPieza.ToString();
                }
            }
            return InicialDeLaCasilla;
        }

        //Reinicia el tablero
        public static void resetTable()
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Clear();
            CrearTablero();
        }

        //Devuelve el color de la pieza a traves de su propiedad "Blanco" que es booleana
        public static void DevolverColor(Pieza c)
        {
            switch (c.Blanca)
            {
                case true:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case false:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
            }
        }

        //Devuelve los lugares atacados de cada color
        public static List<string> LugaresAtacadosBlancos(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas)
        {
            List<string> lugaresAtacados = new List<string>();
            List<string> checking = new List<string>();

            foreach (Pieza pieza in piezasBlancas)
            {
                if (pieza != null)
                {
                    checking = pieza.PosicionesValidas(posicionesOcupadas, null, null, null);
                    if (checking != null) lugaresAtacados.AddRange(checking);
                    checking = pieza.comer(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas, null);
                    if (checking != null) lugaresAtacados.AddRange(checking);
                }
            }

            checking = reyBlanco[0].lugaresAtacados();
            if (checking != null) lugaresAtacados.AddRange(checking);

            lugaresAtacados.RemoveAll(item => item == null);
            return lugaresAtacados;
        }

        public static List<string> LugaresAtacadosNegros(string[] posicionesOcupadas, string[] posicionesOcupadasNegras, string[] posicionesOcupadasBlancas)
        {
            List<string> lugaresAtacados = new List<string>();
            List<string> checking = new List<string>();

            foreach (Pieza pieza in piezasNegras)
            {
                if (pieza != null)
                {
                    checking = pieza.PosicionesValidas(posicionesOcupadas, null, null, null);
                    if (checking != null) lugaresAtacados.AddRange(checking);
                    checking = pieza.comer(posicionesOcupadas, posicionesOcupadasNegras, posicionesOcupadasBlancas, null);
                    if (checking != null) lugaresAtacados.AddRange(checking);
                }
            }

            checking = reyNegro[0].lugaresAtacados();
            if (checking != null) lugaresAtacados.AddRange(checking);

            lugaresAtacados.RemoveAll(item => item == null);
            return lugaresAtacados;
        }

        //Analiza lugares ocupados de manera distinta cada uno
        public static string[] LugaresOcupados()
        {
            string[] posicionesOcupadas = new string[32];
            int indice = 0;

            foreach (Pieza p in piezas)
            {
                if (p != null)
                {
                    posicionesOcupadas[indice] = p.Posicion;
                    indice++;
                }
            }
            return posicionesOcupadas;
        }

        public static string[] LugaresOcupadosBlancas()
        {
            string[] posicionesOcupadas = new string[16];
            int indice = 0;

            foreach (Pieza p in piezasBlancas)
            {
                if (p != null)
                {
                    posicionesOcupadas[indice] = p.Posicion;
                    indice++;
                }
            }
            return posicionesOcupadas;
        }

        public static string[] LugaresOcupadosNegras()
        {
            string[] posicionesOcupadas = new string[16];
            int indice = 0;

            foreach (Pieza p in piezasNegras)
            {
                if (p != null)
                {
                    posicionesOcupadas[indice] = p.Posicion;
                    indice++;
                }
            }
            return posicionesOcupadas;
        }

        //Inicializa los objetos
        public static void inicializacionDeTodo()
        {
            Peon peonA2 = new Peon('p', "A2", true);
            Peon peonB2 = new Peon('p', "B2", true);
            Peon peonC2 = new Peon('p', "C2", true);
            Peon peonD2 = new Peon('p', "D2", true);
            Peon peonE2 = new Peon('p', "E2", true);
            Peon peonF2 = new Peon('p', "F2", true);
            Peon peonG2 = new Peon('p', "G2", true);
            Peon peonH2 = new Peon('p', "H2", true);

            Peon[] IBpeones = { peonA2, peonB2, peonC2, peonD2, peonE2, peonF2, peonG2, peonH2, null, null };
            peonesBlancos = IBpeones;


            Peon peonA8 = new Peon('p', "A7", false);
            Peon peonB8 = new Peon('p', "B7", false);
            Peon peonC8 = new Peon('p', "C7", false);
            Peon peonD8 = new Peon('p', "D7", false);
            Peon peonE8 = new Peon('p', "E7", false);
            Peon peonF8 = new Peon('p', "F7", false);
            Peon peonG8 = new Peon('p', "G7", false);
            Peon peonH8 = new Peon('p', "H7", false);

            Peon[] INpeones = { peonA8, peonB8, peonC8, peonD8, peonE8, peonF8, peonG8, peonH8, null, null };
            peonesNegros = INpeones;

            Torre torreA1 = new Torre('t', "A1", true);
            Torre torreH1 = new Torre('t', "H1", true);

            Torre[] IBtorresBlancas = { torreA1, torreH1, null, null, null, null, null, null, null, null };
            torresBlancas = IBtorresBlancas;

            Torre torreA8 = new Torre('t', "A8", false);
            Torre torreH8 = new Torre('t', "H8", false);

            Torre[] INtorresNegras = { torreA8, torreH8, null, null, null, null, null, null, null, null };
            torresNegras = INtorresNegras;

            Caballo caballoB1 = new Caballo('c', "B1", true);
            Caballo caballoF1 = new Caballo('c', "G1", true);

            Caballo[] IBcaballosBlancos = { caballoB1, caballoF1, null, null, null, null, null, null, null, null };
            caballosBlancos = IBcaballosBlancos;

            Caballo caballoB8 = new Caballo('c', "B8", false);
            Caballo caballoG8 = new Caballo('c', "G8", false);

            Caballo[] INcaballosNegros = { caballoB8, caballoG8, null, null, null, null, null, null, null, null };
            caballosNegros = INcaballosNegros;

            Alfil alfilC1 = new Alfil('a', "C1", true);
            Alfil alfilF1 = new Alfil('a', "F1", true);

            Alfil[] IBalfilesBlancos = { alfilC1, alfilF1, null, null, null, null, null, null, null, null };
            alfilesBlancos = IBalfilesBlancos;

            Alfil alfilC8 = new Alfil('a', "C8", false);
            Alfil alfilF8 = new Alfil('a', "F8", false);

            Alfil[] INalfilesNegros = { alfilC8, alfilF8, null, null, null, null, null, null, null, null };
            alfilesNegros = INalfilesNegros;

            Rey ReyBlanco = new Rey('r', "E1", true);
            Rey[] IBReyBlanco = { ReyBlanco };
            reyBlanco = IBReyBlanco;
            Rey ReyNegro = new Rey('r', "E8", false);
            Rey[] INReyNegro = { ReyNegro };
            reyNegro = INReyNegro;

            Dama DamaBlanca = new Dama('d', "D1", true);
            Dama[] IBDamaBlanca = { DamaBlanca, null, null, null, null, null, null, null, null };
            damaBlanca = IBDamaBlanca;
            Dama DamaNegra = new Dama('d', "D8", false);
            Dama[] INDamaNegra = { DamaNegra, null, null, null, null, null, null, null, null };
            damaNegra = INDamaNegra;


            int fila = 0;
            int filaBlamco = 0;
            int filaNegro = 0;
            for (int col = 0; col < peonesBlancos.Length; col++)
            {
                piezas[fila, col] = peonesBlancos[col];
                piezasBlancas[filaBlamco, col] = peonesBlancos[col];
            }
            fila++;
            filaBlamco++;
            for (int col = 0; col < peonesNegros.Length; col++)
            {
                piezas[fila, col] = peonesNegros[col];
                piezasNegras[filaNegro, col] = peonesNegros[col];
            }
            fila++;
            filaNegro++;
            for (int col = 0; col < torresBlancas.Length; col++)
            {
                piezas[fila, col] = torresBlancas[col];
                piezasBlancas[filaBlamco, col] = torresBlancas[col];
            }
            fila++;
            filaBlamco++;
            for (int col = 0; col < torresNegras.Length; col++)
            {
                piezas[fila, col] = torresNegras[col];
                piezasNegras[filaNegro, col] = torresNegras[col];
            }
            fila++;
            filaNegro++;
            for (int col = 0; col < caballosBlancos.Length; col++)
            {
                piezas[fila, col] = caballosBlancos[col];
                piezasBlancas[filaBlamco, col] = caballosBlancos[col];
            }
            fila++;
            filaBlamco++;
            for (int col = 0; col < caballosNegros.Length; col++)
            {
                piezas[fila, col] = caballosNegros[col];
                piezasNegras[filaNegro, col] = caballosNegros[col];
            }
            fila++;
            filaNegro++;
            for (int col = 0; col < alfilesBlancos.Length; col++)
            {
                piezas[fila, col] = alfilesBlancos[col];
                piezasBlancas[filaBlamco, col] = alfilesBlancos[col];
            }
            fila++;
            filaBlamco++;
            for (int col = 0; col < alfilesNegros.Length; col++)
            {
                piezas[fila, col] = alfilesNegros[col];
                piezasNegras[filaNegro, col] = alfilesNegros[col];
            }
            fila++;
            filaNegro++;
            piezas[fila++, 0] = reyBlanco[0];
            piezasBlancas[filaBlamco++, 0] = reyBlanco[0];
            piezas[fila++, 0] = reyNegro[0];
            piezasNegras[filaNegro++, 0] = reyNegro[0];
            piezas[fila++, 0] = damaBlanca[0];
            piezasBlancas[filaBlamco++, 0] = damaBlanca[0];
            piezas[fila++, 0] = damaNegra[0];
            piezasNegras[filaNegro++, 0] = damaNegra[0];
        }

        //Se llama a los movimientos de las piezas pasando argumentos distintos segun el contexto

        //Peones
        public static ArrayList moverPeon(string input)
        {
            ArrayList retorno = new ArrayList();
            switch (turnoBlanco)
            {
                case true:
                    retorno = PlantillaMovimientosPeon(input, peonesBlancos);
                    break;
                default:
                    retorno = PlantillaMovimientosPeon(input, peonesNegros);
                    break;
            }

            return retorno;
        }
        //Comer con peones
        public static ArrayList ComerPeon(string input, string ingreso)
        {
            ArrayList retorno = new ArrayList();

            switch (turnoBlanco)
            {
                case true:
                    retorno = plantillaComer(input, peonesBlancos, ingreso);
                    break;
                default:
                    retorno = plantillaComer(input, peonesNegros, ingreso);
                    break;
            }

            return retorno;
        }

        //Resto de piezas

        public static ArrayList moverPieza(string input, Piezas pieza)
        {
            ArrayList retorno = new ArrayList();
            switch (pieza)
            {
                case Piezas.torre:
                    retorno = turnoBlanco ? PlantillaMovimientos(input, torresBlancas) : PlantillaMovimientos(input, torresNegras);
                    break;
                case Piezas.caballo:
                    retorno = turnoBlanco ? PlantillaMovimientos(input, caballosBlancos) : PlantillaMovimientos(input, caballosNegros);
                    break;
                case Piezas.alfil:
                    retorno = turnoBlanco ? PlantillaMovimientos(input, alfilesBlancos) : PlantillaMovimientos(input, alfilesNegros);
                    break;
                case Piezas.dama:
                    retorno = turnoBlanco ? PlantillaMovimientos(input, damaBlanca) : PlantillaMovimientos(input, damaNegra);
                    break;
                default:
                    retorno = turnoBlanco ? PlantillaMovimientos(input, reyBlanco) : PlantillaMovimientos(input, reyNegro);
                    break;
            }
            return retorno;
        }

        public static ArrayList moverPiezaEspecificoLetra(string input, Piezas pieza, List<string> listaSitiosEspecificos, string entrada)
        {
            ArrayList retorno = new ArrayList();
            switch (pieza)
            {
                case Piezas.torre:
                    retorno = turnoBlanco ? PlantillaMovimientosEspecificoLetra(input, torresBlancas, listaSitiosEspecificos, entrada) : PlantillaMovimientosEspecificoLetra(input, torresNegras, listaSitiosEspecificos, entrada);
                    break;
                case Piezas.caballo:
                    retorno = turnoBlanco ? PlantillaMovimientosEspecificoLetra(input, caballosBlancos, listaSitiosEspecificos, entrada) : PlantillaMovimientosEspecificoLetra(input, caballosNegros, listaSitiosEspecificos, entrada);
                    break;
                case Piezas.dama:
                    retorno = turnoBlanco ? PlantillaMovimientosEspecificoLetra(input, damaBlanca, listaSitiosEspecificos, entrada) : PlantillaMovimientosEspecificoLetra(input, damaNegra, listaSitiosEspecificos, entrada);
                    break;
                case Piezas.alfil:
                    retorno = turnoBlanco ? PlantillaMovimientosEspecificoLetra(input, alfilesBlancos, listaSitiosEspecificos, entrada) : PlantillaMovimientosEspecificoLetra(input, alfilesNegros, listaSitiosEspecificos, entrada);
                    break;
            }
            return retorno;
        }

        public static ArrayList moverPiezaEspecificoNumero(string input, Piezas pieza, List<string> listaSitiosEspecificos, string entrada)
        {
            ArrayList retorno = new ArrayList();
            switch (pieza)
            {
                case Piezas.torre:
                    retorno = turnoBlanco ? PlantillaMovimientosEspecificoNumero(input, torresBlancas, listaSitiosEspecificos, entrada) : PlantillaMovimientosEspecificoNumero(input, torresNegras, listaSitiosEspecificos, entrada);
                    break;
                case Piezas.caballo:
                    retorno = turnoBlanco ? PlantillaMovimientosEspecificoNumero(input, caballosBlancos, listaSitiosEspecificos, entrada) : PlantillaMovimientosEspecificoNumero(input, caballosNegros, listaSitiosEspecificos, entrada);
                    break;
                case Piezas.dama:
                    retorno = turnoBlanco ? PlantillaMovimientosEspecificoNumero(input, damaBlanca, listaSitiosEspecificos, entrada) : PlantillaMovimientosEspecificoNumero(input, damaNegra, listaSitiosEspecificos, entrada);
                    break;
                case Piezas.alfil:
                    retorno = turnoBlanco ? PlantillaMovimientosEspecificoNumero(input, alfilesBlancos, listaSitiosEspecificos, entrada) : PlantillaMovimientosEspecificoNumero(input, alfilesNegros, listaSitiosEspecificos, entrada);
                    break;
            }
            return retorno;
        }

        //Se llama a los movimientos que comen de las piezas pasando argumentos distintos segun el contexto
        public static ArrayList comerPieza(string input, Piezas pieza)
        {
            ArrayList retorno = new ArrayList();
            switch (pieza)
            {
                case Piezas.torre:
                    retorno = turnoBlanco ? plantillaComer(input, torresBlancas, "") : plantillaComer(input, torresNegras, "");
                    break;
                case Piezas.caballo:
                    retorno = turnoBlanco ? plantillaComer(input, caballosBlancos, "") : plantillaComer(input, caballosNegros, "");
                    break;
                case Piezas.alfil:
                    retorno = turnoBlanco ? plantillaComer(input, alfilesBlancos, "") : plantillaComer(input, alfilesNegros, "");
                    break;
                case Piezas.dama:
                    retorno = turnoBlanco ? plantillaComer(input, damaBlanca, "") : plantillaComer(input, damaNegra, "");
                    break;
                default:
                    retorno = turnoBlanco ? plantillaComer(input, reyBlanco, "") : plantillaComer(input, reyNegro, "");
                    break;
            }

            return retorno;
        }

        public static ArrayList comerPiezaEspecificoLetra(string input, Piezas pieza, List<string> listaSitiosEspecificos, string entrada)
        {
            ArrayList retorno = new ArrayList();
            switch (pieza)
            {
                case Piezas.torre:
                    retorno = turnoBlanco ? plantillaComerEspecificoLetra(input, torresBlancas, listaSitiosEspecificos, entrada) : plantillaComerEspecificoLetra(input, torresNegras, listaSitiosEspecificos, entrada);
                    break;
                case Piezas.caballo:
                    retorno = turnoBlanco ? plantillaComerEspecificoLetra(input, caballosBlancos, listaSitiosEspecificos, entrada) : plantillaComerEspecificoLetra(input, caballosNegros, listaSitiosEspecificos, entrada);
                    break;
                case Piezas.dama:
                    retorno = turnoBlanco ? plantillaComerEspecificoLetra(input, damaBlanca, listaSitiosEspecificos, entrada) : plantillaComerEspecificoLetra(input, damaNegra, listaSitiosEspecificos, entrada);
                    break;
                case Piezas.alfil:
                    retorno = turnoBlanco ? plantillaComerEspecificoLetra(input, alfilesBlancos, listaSitiosEspecificos, entrada) : plantillaComerEspecificoLetra(input, alfilesNegros, listaSitiosEspecificos, entrada);
                    break;
            }

            return retorno;
        }

        public static ArrayList comerPiezaEspecificoNumero(string input, Piezas pieza, List<string> listaSitiosEspecificos, string entrada)
        {
            ArrayList retorno = new ArrayList();
            switch (pieza)
            {
                case Piezas.torre:
                    retorno = turnoBlanco ? plantillaComerEspecificoNumero(input, torresBlancas, listaSitiosEspecificos, entrada) : plantillaComerEspecificoNumero(input, torresNegras, listaSitiosEspecificos, entrada);
                    break;
                case Piezas.caballo:
                    retorno = turnoBlanco ? plantillaComerEspecificoNumero(input, caballosBlancos, listaSitiosEspecificos, entrada) : plantillaComerEspecificoNumero(input, caballosNegros, listaSitiosEspecificos, entrada);
                    break;
            }

            return retorno;
        }

        //Enroque
        public static bool enroque(string input, Rey[] ReyAnalizar)
        {
            bool retorno = false;

            string[] todo = LugaresOcupados();
            string[] blancas = LugaresOcupadosBlancas();
            string[] negras = LugaresOcupadosNegras();

            List<string> lugaresAtacadosColorContrario = ReyAnalizar[0].Blanca ? LugaresAtacadosNegros(todo, negras, blancas) : LugaresAtacadosBlancos(todo, negras, blancas);

            List<string> conjunto = new List<string>();
            conjunto.AddRange(todo); conjunto.AddRange(blancas); conjunto.AddRange(negras); conjunto.AddRange(lugaresAtacadosColorContrario);


            Func<string, string, string, Torre, bool> condicion = delegate (string thisInput, string posicion1, string posicion2, Torre torresAnalizar)
            {
                bool posible = true;
                if (input == thisInput && torresAnalizar.permiteEnroque)
                {
                    foreach (string posicion in conjunto)
                    {
                        if (posicion == posicion1 || posicion == posicion2)
                        {
                            posible = false;
                            break;
                        }
                    }
                }
                if (posible)
                {
                    torresAnalizar.Posicion = posicion1;
                    ReyAnalizar[0].Posicion = posicion2;
                }

                return posible;
            };

            if (ReyAnalizar[0].Blanca && ReyAnalizar[0].permiteEnroque)
            {
                if (input == "0-0")
                {
                    retorno = condicion("0-0", "F1", "G1", torresBlancas[1]);
                    if (retorno)
                    {
                        torresBlancas[1].permiteEnroque = false;
                        reyBlanco[0].permiteEnroque = false;
                    }
                }

                if (input == "0-0-0")
                {
                    retorno = condicion("0-0-0", "D1", "C1", torresBlancas[0]);
                    if (retorno)
                    {
                        torresBlancas[0].permiteEnroque = false;
                        reyBlanco[0].permiteEnroque = false;
                    }
                }
            }
            else if (!ReyAnalizar[0].Blanca && ReyAnalizar[0].permiteEnroque)
            {
                if (input == "0-0")
                {
                    retorno = condicion("0-0", "F8", "G8", torresNegras[1]);
                    if (retorno)
                    {
                        torresNegras[1].permiteEnroque = false;
                        reyNegro[0].permiteEnroque = false;
                    }
                }

                if (input == "0-0-0")
                {
                    retorno = condicion("0-0-0", "D8", "C8", torresNegras[0]);
                    if (retorno)
                    {
                        torresNegras[0].permiteEnroque = false;
                        reyNegro[0].permiteEnroque = false;
                    }
                }
            }

            return retorno;
        }

        // Lugares ocupados para peones
        public static string[] LugaresOcupadosBlancasParaPeon()
        {
            string[] posicionesOcupadas = new string[17];
            int indice = 0;

            foreach (Pieza p in piezasBlancas)
            {
                if (p != null)
                {
                    posicionesOcupadas[indice] = p.Posicion;
                    indice++;
                }
            }

            foreach (Peon peon in peonesBlancos)
            {
                if (peon != null && peon.comerAlPaso != null && peon.comerAlPaso != "")
                {
                    posicionesOcupadas[16] = peon.comerAlPaso;
                    break;
                }
            }
            return posicionesOcupadas;
        }

        public static string[] LugaresOcupadosNegrasParaPeon()
        {
            string[] posicionesOcupadas = new string[17];
            int indice = 0;

            foreach (Pieza p in piezasNegras)
            {
                if (p != null)
                {
                    posicionesOcupadas[indice] = p.Posicion;
                    indice++;
                }
            }

            foreach (Peon peon in peonesNegros)
            {
                if (peon != null && peon.comerAlPaso != null && peon.comerAlPaso != "")
                {
                    posicionesOcupadas[16] = peon.comerAlPaso;
                    break;
                }
            }
            return posicionesOcupadas;
        }


        public static ArrayList PlantillaMovimientosPeon(string input, Peon[] piezasAnalizar)
        {
            string[] todo = LugaresOcupados();
            string[] blancas = LugaresOcupadosBlancas();
            string[] negras = LugaresOcupadosNegras();

            ArrayList retorno = new ArrayList();

            List<string> posicionesValidas = null;

            foreach (Peon pieza in piezasAnalizar)
            {
                if (pieza != null) posicionesValidas = pieza.PosicionesValidas(todo, negras, blancas, null);

                foreach (string posicion in posicionesValidas)
                {
                    if (posicion == input)
                    {
                        retorno.Add(pieza);
                        retorno.Add(input);

                        if (posicionesValidas[1] != null && turnoBlanco && posicionesValidas[1] == posicion) pieza.comerAlPaso = posicion[0] + "3";
                        else if (posicionesValidas[1] != null && !turnoBlanco && posicionesValidas[1] == posicion) pieza.comerAlPaso = posicion[0] + "6";
                        return retorno;
                    }
                }
            }
            retorno = null;
            return retorno;
        }

        //Plantilla de movimientos
        public static ArrayList PlantillaMovimientos<T>(string input, T[] piezasAnalizar) where T : Pieza
        {
            string[] todo = LugaresOcupados();
            string[] blancas = LugaresOcupadosBlancas();
            string[] negras = LugaresOcupadosNegras();

            ArrayList retorno = new ArrayList();

            List<string> posicionesValidas = null;

            string tipoDato = piezasAnalizar.GetType().ToString();

            List<string> lugaresAtacadosColorContrario = null;
            if (tipoDato == "Ajedrez.Rey[]")
            {
                lugaresAtacadosColorContrario = piezasAnalizar[0].Blanca ? LugaresAtacadosNegros(todo, negras, blancas) : LugaresAtacadosBlancos(todo, negras, blancas);
            }

            foreach (T pieza in piezasAnalizar)
            {
                if (pieza != null) posicionesValidas = pieza.PosicionesValidas(todo, negras, blancas, lugaresAtacadosColorContrario);

                if (posicionesValidas != null)
                {
                    foreach (string posicion in posicionesValidas)
                    {
                        if (posicion == input)
                        {
                            retorno.Add(pieza);
                            retorno.Add(input);
                            return retorno;
                        }
                    }
                }
            }
            retorno = null;
            return retorno;
        }

        public static ArrayList PlantillaMovimientosEspecificoLetra<T>(string input, T[] piezasAnalizar, List<string> listaSitiosEspecificos, string entrada) where T : Pieza
        {
            string[] todo = LugaresOcupados();
            string[] blancas = LugaresOcupadosBlancas();
            string[] negras = LugaresOcupadosNegras();

            ArrayList retorno = new ArrayList();

            foreach (T pieza in piezasAnalizar)
            {
                foreach (string posicion in listaSitiosEspecificos)
                {
                    if (posicion == input && entrada[1].ToString().ToUpper() == pieza.Posicion[0].ToString().ToUpper())
                    {
                        retorno.Add(pieza);
                        retorno.Add(input);
                        return retorno;
                    }
                }
            }
            retorno = null;
            return retorno;
        }

        public static ArrayList PlantillaMovimientosEspecificoNumero<T>(string input, T[] piezasAnalizar, List<string> listaSitiosEspecificos, string entrada) where T : Pieza
        {
            string[] todo = LugaresOcupados();
            string[] blancas = LugaresOcupadosBlancas();
            string[] negras = LugaresOcupadosNegras();

            ArrayList retorno = new ArrayList();

            foreach (T pieza in piezasAnalizar)
            {
                foreach (string posicion in listaSitiosEspecificos)
                {
                    if (posicion == input && entrada[1].ToString() == pieza.Posicion[1].ToString())
                    {
                        retorno.Add(pieza);
                        retorno.Add(input);
                        return retorno;
                    }
                }
            }
            retorno = null;
            return retorno;
        }

        //Plantilla de comer
        public static ArrayList plantillaComer<T>(string input, T[] piezasAnalizar, string ingreso) where T : Pieza
        {
            ArrayList retorno = new ArrayList();

            List<string> posicionesValidas = null;
            string[] todo = LugaresOcupados();

            string tipo = piezasAnalizar.GetType().ToString();

            if (tipo == "Ajedrez.Peon[]")
            {
                string[] blancas = LugaresOcupadosBlancasParaPeon();
                string[] negras = LugaresOcupadosNegrasParaPeon();
                foreach (T pieza in piezasAnalizar)
                {
                    if (pieza != null && ingreso[0].ToString().ToUpper() == pieza.Posicion[0].ToString())
                    {
                        if (pieza != null) posicionesValidas = pieza.comer(todo, negras, blancas, null);

                        if (posicionesValidas != null)
                        {
                            foreach (string posicion in posicionesValidas)
                            {
                                if (posicion == input)
                                {
                                    retorno.Add(pieza);
                                    retorno.Add(input);

                                    if (negras[16] == input || blancas[16] == input)
                                    {
                                        ComerAlPasoeliminarAlComerAlPaso(input);
                                    }
                                    return retorno;
                                }

                            }
                        }
                    }
                }
            }
            else
            {
                string[] blancas = LugaresOcupadosBlancas();
                string[] negras = LugaresOcupadosNegras();

                List<string> lugaresAtacadosColorContrario = null;

                if (tipo == "Ajedrez.Rey[]")
                {
                    lugaresAtacadosColorContrario = piezasAnalizar[0].Blanca ? LugaresAtacadosNegros(todo, negras, blancas) : LugaresAtacadosBlancos(todo, negras, blancas);
                }


                foreach (T pieza in piezasAnalizar)
                {
                    if (pieza != null) posicionesValidas = pieza.comer(todo, negras, blancas, lugaresAtacadosColorContrario);

                    if (posicionesValidas != null)
                    {
                        foreach (string posicion in posicionesValidas)
                        {
                            if (posicion == input)
                            {
                                retorno.Add(pieza);
                                retorno.Add(input);
                                return retorno;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static ArrayList plantillaComerEspecificoLetra<T>(string input, T[] piezasAnalizar, List<string> listaSitiosEspecificos, string entrada) where T : Pieza
        {
            ArrayList retorno = new ArrayList();

            string[] todo = LugaresOcupados();
            string[] blancas = LugaresOcupadosBlancas();
            string[] negras = LugaresOcupadosNegras();


            foreach (T pieza in piezasAnalizar)
            {
                foreach (string posicion in listaSitiosEspecificos)
                {
                    if (posicion == input && pieza.Posicion[0].ToString() == entrada[1].ToString().ToUpper())
                    {
                        retorno.Add(pieza);
                        retorno.Add(input);
                        return retorno;
                    }
                }
            }

            return null;
        }
        public static ArrayList plantillaComerEspecificoNumero<T>(string input, T[] piezasAnalizar, List<string> listaSitiosEspecificos, string entrada) where T : Pieza
        {
            ArrayList retorno = new ArrayList();

            string[] todo = LugaresOcupados();
            string[] blancas = LugaresOcupadosBlancas();
            string[] negras = LugaresOcupadosNegras();


            foreach (T pieza in piezasAnalizar)
            {
                foreach (string posicion in listaSitiosEspecificos)
                {
                    if (posicion == input && pieza.Posicion[1].ToString() == entrada[1].ToString().ToUpper())
                    {
                        retorno.Add(pieza);
                        retorno.Add(input);
                        return retorno;
                    }
                }

            }

            return null;
        }

        //Por si hay dos piezas que usan la misma nomenclatura para mover en una partida
        public static List<string> nomenclaturaIgual<T>(T[] piezas) where T : Pieza
        {
            List<string> retorno = new List<string>();

            string[] todo = LugaresOcupados();
            string[] blancas = LugaresOcupadosBlancas();
            string[] negras = LugaresOcupadosNegras();

            List<string> movs1 = new List<string>();
            List<string> movs2 = new List<string>();

            if (piezas[0] != null && piezas[1] != null)
            {
                movs1.AddRange(piezas[0].PosicionesValidas(todo, negras, blancas, null));
                movs1.AddRange(piezas[0].comer(todo, negras, blancas, null));

                movs2.AddRange(piezas[1].PosicionesValidas(todo, negras, blancas, null));
                movs2.AddRange(piezas[1].comer(todo, negras, blancas, null));

                movs1.RemoveAll(item => int.TryParse(item, out int result));
                movs2.RemoveAll(item => int.TryParse(item, out int result));

                movs1.ForEach(item =>
                {
                    if (item != null && item != "")
                    {
                        movs2.ForEach(item2 =>
                        {
                            if (item2 != null && item != "" && item == item2) retorno.Add(item);
                        });
                    }
                });
            }

            if (retorno.Count == 0)
            {
                return null;
            }

            return retorno;
        }

        public static bool LetrasONumeros<T>(T[] piezas) where T : Pieza
        {
            if (piezas[0].Posicion[0] != piezas[1].Posicion[0]) return true;
            else return false;
        }

        public static void reiniciarComerAlPaso()
        {
            if (turnoBlanco)
            {
                foreach (Peon peon in peonesBlancos)
                {
                    if (peon != null) peon.comerAlPaso = "";
                }
            }
            else
            {
                foreach (Peon peon in peonesNegros)
                {
                    if (peon != null) peon.comerAlPaso = "";
                }
            }
        }

        public static void ComerAlPasoeliminarAlComerAlPaso(string input)
        {
            int counter = 0;
            if (turnoBlanco)
            {
                foreach (Peon peon in peonesNegros)
                {
                    if (peon != null && peon.Posicion == input[0] + "5")
                    {
                        peonesNegros[counter] = null;
                        piezas[1, counter] = null;
                        break;
                    }
                    counter++;
                }
            }
            else
            {
                foreach (Peon peon in peonesBlancos)
                {
                    if (peon != null && peon.Posicion == input[0] + "4")
                    {
                        peonesNegros[counter] = null;
                        piezas[0, counter] = null;
                        break;
                    }
                    counter++;
                }
            }
        }
    }
}
