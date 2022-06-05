using System;

namespace IgnoredAssertions;

public class Service
{
    private readonly IDependency dependency;

    public Service(IDependency dependency)
    {
        this.dependency = dependency;
    }

    public bool MethodCall(int input)
    {
        try
        {
            this.dependency.DependencyCall(input);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}
