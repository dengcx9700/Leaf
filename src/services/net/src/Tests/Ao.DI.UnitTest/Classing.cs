using Ao.DI.InjectWay;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.DI.UnitTest
{
    public class A
    {

    }
    public class B
    {

    }
    public class C
    {

    }
    [AoService(ServiceLifetime.Singleton)]
    public class SA
    {

    }
    [AoService(ServiceLifetime.Scoped)]
    public class SB
    {

    }
    [AoService(ServiceLifetime.Transient)]
    public class SC
    {

    }
    public class ArefSB
    {
        public ArefSB(SB s)
        {
            
        }
    }
}
