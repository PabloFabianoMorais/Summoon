using System;

namespace sunmoon.utils
{
    /// <summary>
    /// Fornece uma coleção de funções matemáticas de utilidade geral.
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// Calcula o valor de piso, que arredonda o resultado para baixo.
        /// Útil para lidar com coordenadas de grid negativas.
        /// </summary>
        /// <param name="a">O dividendo</param>
        /// <param name="n">O divisor</param>
        /// <returns>O resultado da divisão arredondado para inteiro inferior mais próximo</returns>
        public static int FloorDiv(int a, int n)
        {
            return (int)Math.Floor((double)a / n);
        }

        /// <summary>
        /// Calcula o método matemático, que sempre retorna um resultado positivo.
        /// Diferente do operador '%' do C#, que pode retornar negativos.
        /// </summary>
        /// <param name="a">O dividendo.</param>
        /// <param name="n">O divisior</param>
        /// <returns>o resultado da divisão, garantindo como não-negativo</returns>
        public static int Mod(int a, int n)
        {
            return ((a % n) + n) % n;
        }
    }
}