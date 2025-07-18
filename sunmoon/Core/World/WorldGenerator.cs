

namespace sunmoon.Core.World
{
    public class WorldGenerator
    {
        private readonly FastNoiseLite _noise;

        public WorldGenerator(int seed)
        {
            _noise = new FastNoiseLite(seed);
            _noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

            _noise.SetFrequency(0.02f);
            _noise.SetFractalType(FastNoiseLite.FractalType.FBm);
            _noise.SetFractalOctaves(5);
            _noise.SetFractalLacunarity(2.0f);
            _noise.SetFractalGain(0.5f);

        }

        /// <summary>
        /// Obtém o tipo de tile para uma coordenada específica.
        /// </summary>
        /// <param name="x">Posição X no mapa de ruído.</param>
        /// <param name="y">Posição Y no mapa de ruído.</param>
        /// <returns>O tipo de tile dentro da lista de constantes que depende de regras específicas.</returns>
        public string GetTilePrefabName(int x, int y)
        {
            float noiseValue = _noise.GetNoise(x, y);

            if (noiseValue < -0.2)
                return "Water";
            else
                return "Grass";
        }
    }
}