using System;
using Microsoft.AspNetCore.Mvc;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class BasicAuthAttribute : TypeFilterAttribute
{
    public BasicAuthAttribute() : base(typeof(BasicAuthFilter))
    {
        Arguments = new object[] { };
    }
}