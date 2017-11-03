﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.API
{
    public delegate void RocketImplementationInitialized();

    public interface IRocketImplementation
    {
        event RocketImplementationInitialized OnRocketImplementationInitialized;

        IRocketImplementationEvents ImplementationEvents { get; }
        void Shutdown();
        string InstanceId { get; }
        void Reload();
    }
}
