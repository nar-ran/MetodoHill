namespace MetodoHill
{
    internal class Desencriptar
    {
        public static void desencriptar()
        {
            Dictionary<string, int> recuadro = new Dictionary<string, int>
            {
                {"A", 0 }, { "B", 1 }, {"C", 2}, {"D", 3}, { "E", 4 }, { "F", 5 }, {"G", 6 }, {"H", 7 }, { "I", 8 }, {"J", 9}, {"K", 10}, {"L", 11}, {"M", 12}, {"N", 13}, {"Ñ", 14}, {"O", 15 }, {"P", 16}, {"Q", 17}, {"R", 18}, {"S", 19 }, {"T", 20}, {"U", 21 }, {"V", 22}, {"W", 23 }, {"X", 24}, {"Y", 25}, {"Z", 26}, {"_", 27 }
            };

                string textoCifrado = "QOXSMJHBKZN_";

                // Matriz de clave 3x3
                int[,] matrizClave =
                {
                { 4, 3, 1 },
                { 2, 2, 1 },
                { 1, 1, 1 }
            };

            int[,] matrizClaveInversa = InvertirMatriz(matrizClave);

            int[] textoNumerico = ConvertTextToNumbers(textoCifrado, recuadro);
            int[,] matrizDesencriptada = DesencriptarTexto(textoNumerico, matrizClaveInversa);

            string textoDesencriptado = ConvertNumbersToText(matrizDesencriptada, recuadro);
            Console.WriteLine("Texto desencriptado: " + textoDesencriptado);
        }

        static int[,] InvertirMatriz(int[,] matriz)
        {
            int size = matriz.GetLength(0);
            int[,] inversa = new int[size, size];
            int determinante = Determinante(matriz);
            int inversoDeterminante = InversoModulo(determinante, 28);

            int[,] adjunta = Adjunto(matriz);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    inversa[i, j] = (adjunta[i, j] * inversoDeterminante) % 28;
                    if (inversa[i, j] < 0) inversa[i, j] += 28; 
                }
            }
            return inversa;
        }

        static int Determinante(int[,] matriz)
        {
            // Asegurarse de que la matriz es 3x3
            if (matriz.GetLength(0) != 3 || matriz.GetLength(1) != 3)
            {
                throw new ArgumentException("La matriz debe ser 3x3");
            }

            // Extraer los valores de la matriz
            int a = matriz[0, 0], b = matriz[0, 1], c = matriz[0, 2];
            int d = matriz[1, 0], e = matriz[1, 1], f = matriz[1, 2];
            int g = matriz[2, 0], h = matriz[2, 1], i = matriz[2, 2];

            // Calcular el determinante
            int determinante = a * (e * i - f * h) - b * (d * i - f * g) + c * (d * h - e * g);

            // Ajustar el resultado para estar en el rango del módulo 28
            determinante = determinante % 28;
            if (determinante < 0)
            {
                determinante += 28;
            }

            return determinante;
        }

        static int InversoModulo(int a, int mod)
        {
            a = a % mod;
            for (int x = 1; x < mod; x++)
            {
                if ((a * x) % mod == 1)
                    return x;
            }
            throw new Exception("No se encontró inverso módulo");
        }

        static int[,] Adjunto(int[,] matriz)
        {
            int size = matriz.GetLength(0);
            if (size != 3 || matriz.GetLength(1) != 3)
            {
                throw new ArgumentException("La matriz debe ser 3x3");
            }

            int[,] adjunta = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    adjunta[j, i] = Cofactor(matriz, i, j); // Transponer aquí
                }
            }

            return adjunta;
        }

        static int Cofactor(int[,] matriz, int row, int col)
        {
            int[,] submatriz = new int[2, 2];
            int subi = 0;
            for (int i = 0; i < 3; i++)
            {
                if (i == row)
                    continue;
                int subj = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (j == col)
                        continue;
                    submatriz[subi, subj] = matriz[i, j];
                    subj++;
                }
                subi++;
            }

            int determinanteSubmatriz = Determinante2x2(submatriz);
            return (int)Math.Pow(-1, row + col) * determinanteSubmatriz;
        }

        static int Determinante2x2(int[,] matriz)
        {
            return matriz[0, 0] * matriz[1, 1] - matriz[0, 1] * matriz[1, 0];
        }

        static int[] ConvertTextToNumbers(string text, Dictionary<string, int> recuadro)
        {
            List<int> numbers = new List<int>();
            foreach (char c in text)
            {
                if (recuadro.ContainsKey(c.ToString()))
                {
                    numbers.Add(recuadro[c.ToString()]);
                }
            }
            return numbers.ToArray();
        }

        static int[,] DesencriptarTexto(int[] textoNumerico, int[,] matrizClaveInversa)
        {
            int matrixSize = matrizClaveInversa.GetLength(0);
            int bloques = (int)Math.Ceiling((double)textoNumerico.Length / matrixSize);
            int[,] resultado = new int[bloques, matrixSize];

            for (int i = 0; i < bloques; i++)
            {
                int[] bloque = new int[matrixSize];
                for (int j = 0; j < matrixSize; j++)
                {
                    if (i * matrixSize + j < textoNumerico.Length)
                    {
                        bloque[j] = textoNumerico[i * matrixSize + j];
                    }
                    else
                    {
                        bloque[j] = 0;
                    }
                }

                int[] bloqueDesencriptado = MatrixMultiply(matrizClaveInversa, bloque);
                for (int j = 0; j < matrixSize; j++)
                {
                    resultado[i, j] = bloqueDesencriptado[j] % 28;
                }
            }
            return resultado;
        }

        static int[] MatrixMultiply(int[,] matrix, int[] vector)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[] result = new int[rows];

            for (int i = 0; i < rows; i++)
            {
                int sum = 0;
                for (int j = 0; j < cols; j++)
                {
                    sum += matrix[i, j] * vector[j];
                }
                result[i] = sum;
            }

            return result;
        }

        static string ConvertNumbersToText(int[,] numbers, Dictionary<string, int> recuadro)
        {
            Dictionary<int, string> reverseRecuadro = new Dictionary<int, string>();
            foreach (var pair in recuadro)
            {
                reverseRecuadro[pair.Value] = pair.Key;
            }

            string result = "";
            int rows = numbers.GetLength(0);
            int cols = numbers.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result += reverseRecuadro[numbers[i, j]];
                }
            }
            return result;
        }
    }
}
