#nullable enable
using System;

namespace Decembrist.Di
{
    public class UnsatisfiedDependencyException : Exception
    {
        public UnsatisfiedDependencyException(
            string? emptyDependency) : base($"Unsatisfied dependencies for {emptyDependency}")
        {
        }
    }
}