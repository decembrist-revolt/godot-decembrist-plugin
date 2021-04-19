using Godot;

namespace Decembrist.Utils
{
    public static class Vectors
    {
        public static Vector2 AsVector(this float source) => new Vector2(source, source);
        
        public static Vector2 AsVector(this int source) => new Vector2(source, source);
        
        public static Vector2 Plus(this Vector2 @base, int delta) => @base + delta.AsVector();
        
        public static Vector2 WithX(this Vector2 @base, float x) => new Vector2(x, @base.y);
        
        public static Vector2 WithY(this Vector2 @base, float y) => new Vector2(@base.x, y);
        
        public static Vector2 PlusX(this Vector2 @base, int x) => @base.WithX(@base.x + x);
        
        public static Vector2 PlusY(this Vector2 @base, int y) => @base.WithY(@base.y + y);

        public static Vector2Builder Builder2D(Vector2 vector = new Vector2()) => new Vector2Builder(vector);
        
        public static Vector2Builder Builder2D(float x, float y) => new Vector2Builder(x, y);
    }

    public class Vector2Builder
    {
        private float _x = 0;
        private float _y = 0;

        public Vector2Builder()
        {
        }

        public Vector2Builder(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public Vector2Builder(Vector2 vector): this(vector.x, vector.y)
        {
        }

        public Vector2Builder NewX(float x)
        {
            _x = x;
            return this;
        }
        
        public Vector2Builder NewY(float y)
        {
            _y = y;
            return this;
        }
        
        public Vector2Builder Plus(float value)
        {
            _x += value;
            _y += value;
            return this;
        }
        
        public Vector2Builder PlusX(float x)
        {
            _x += x;
            return this;
        }
        
        public Vector2Builder PlusY(float y)
        {
            _y += y;
            return this;
        }
        
        public Vector2Builder Div(float value)
        {
            _x /= value;
            _y /= value;
            return this;
        }

        public Vector2Builder DivX(float x)
        {
            _x /= x;
            return this;
        }

        public Vector2Builder DivY(float y)
        {
            _y /= y;
            return this;
        }

        public Vector2 Build() => new Vector2(_x, _y);
        
        public static Vector2Builder operator +(Vector2Builder builder, float value) => builder.Plus(value);
        
        public static Vector2Builder operator /(Vector2Builder builder, float value) => builder.Div(value);

        public static implicit operator Vector2(Vector2Builder builder) => builder.Build();
    }
}