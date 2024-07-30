namespace MetodoHill
{
    internal class Encriptar
    {
        public static void encriptar()
        {
            // Declcarar recuadro de 28
            Dictionary<string, int> recuadro = new Dictionary<string, int>
            {
                {"A", 0 }, { "B", 1 }, {"C", 2}, {"D", 3}, { "E", 4 }, { "F", 5 }, {"G", 6 }, {"H", 7 }, { "I", 8 }, {"J", 9}, {"K", 10}, {"L", 11}, {"M", 12}, {"N", 13}, {"Ñ", 14}, {"O", 15 }, {"P", 16}, {"Q", 17}, {"R", 18}, {"S", 19 }, {"T", 20}, {"U", 21 }, {"V", 22}, {"W", 23 }, {"X", 24}, {"Y", 25}, {"Z", 26}, {"_", 27 }
            };

            string frase = "LIFE_GOES_ON";
            int[,] matrizClave = { { 4, 3, 1 }, { 2, 2, 1 }, { 1, 1, 1 } }; // Matriz de 3x3

            int[] textoNumerico = convertirNumero(frase, recuadro);
            int[,] matrizCifrada = cifrarTexto(textoNumerico, matrizClave);

            string textoCifrado = convertirNumeroATexto(matrizCifrada, recuadro);
            Console.WriteLine("Texto Cifrado: "+textoCifrado);
        }

        static int[] convertirNumero(string frase, Dictionary<string, int> recuadro)
        {
            List<int> numeros = new List<int>();
            foreach (char c in frase)
            {
                if (recuadro.ContainsKey(c.ToString()))
                {
                    numeros.Add(recuadro[c.ToString()]);
                }
            }

            return numeros.ToArray();
        }

        static int[,] cifrarTexto(int[] textoNumerico, int[,] matrizClave)
        {
            int tamMatriz = matrizClave.GetLength(0);
            int bloques = (int)Math.Ceiling((double)textoNumerico.Length / tamMatriz);
            int[,] resultado = new int[bloques, tamMatriz];

            for (int i = 0; i < bloques; i++)
            {
                int[] bloque = new int[tamMatriz];
                for (int j = 0; j < tamMatriz; j++)
                {
                    if (i * tamMatriz + j < textoNumerico.Length)
                    {
                        bloque[j] = textoNumerico[i * tamMatriz + j];
                    }
                    else
                    {
                        bloque[j] = 0;
                    }
                }

                int[] bloqueCifrado = multiplicarMatrices(matrizClave, bloque);
                for (int j = 0; j < tamMatriz; j++)
                {
                    resultado[i, j] = bloqueCifrado[j] % 28;
                }
            }

            return resultado;
        }

        static int[] multiplicarMatrices(int[,] matriz, int[] vector)
        {
            int filas = matriz.GetLength(0);
            int cols = matriz.GetLength(1);

            int[] result = new int[filas];

            for (int i = 0; i < filas; i++)
            {
                int sum = 0;
                for (int j = 0; j < cols; j++)
                {
                    sum += matriz[i, j] * vector[j];
                }

                result[i] = sum;
            }

            return result;
        }

        static string convertirNumeroATexto(int[,] numeros, Dictionary<string, int> recuadro)
    {
        Dictionary<int, string> recuadroInverso = new Dictionary<int, string>();
        foreach (var match in recuadro)
        {
            recuadroInverso[match.Value] = match.Key;
        }

        string resultado = "";
        int filas = numeros.GetLength(0);
        int cols = numeros.GetLength(1);

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                resultado += recuadroInverso[numeros[i, j]];
            }
        }

        return resultado;
    }
    }
}
