#nullable enable
using System;
using Godot;

namespace Decembrist.Example
{
    public static class Assertions
    {
        public static void AssertTrue(bool expression, string test = "true assertion failed")
        {
            if (expression)
            {
                GD.Print($"PASSED:{test}");
            }
            else
            {
                var message = $"FAILED:{test}";
                GD.PrintErr(message);
                throw new Exception(message);
            }
        }
        
        public static void AssertNotNull(object? expression, string test = "null assertion failed")
        {
            if (expression != null)
            {
                GD.Print($"PASSED:{test}");
            }
            else
            {
                var message = $"FAILED:{test}";
                GD.PrintErr(message);
                throw new Exception(message);
            }
        }

        public static void AssertEquals(object? expected, object? actual, string test = "equals assertion failed")
        {
            AssertTrue(expected == actual, test);
        }
    }
}