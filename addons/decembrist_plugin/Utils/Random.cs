using Godot;

namespace Decembrist.Utils
{
    public static class Random
    {
        private static readonly RandomNumberGenerator _random = new RandomNumberGenerator();
        public static int RandomInt(int from, int to)
        {
            _random.Randomize();
            return _random.RandiRange(from, to);
        }

        public static bool RandomBool()
        {
            return RandomInt(0, 1) == 1;
        }

        public static int RandomIntRange(int from, int to)
        {
            return _random.RandiRange(from, to);
        }
        
        public static float RandomFloatRange(float from, float to)
        {
            _random.Randomize();
            return _random.RandfRange(from, to);
        }
        
        public static int RandomIntRange(float from, float to)
        {
            _random.Randomize();
            return _random.RandiRange((int) from, (int) to);
        }

        public static Vector2 RandomVector(Vector2 from, Vector2 to)
        {
            var randomX = RandomIntRange(from.x, to.x);
            var randomY = RandomIntRange(from.y, to.y);
            return new Vector2(randomX, randomY);
        }
    }
}