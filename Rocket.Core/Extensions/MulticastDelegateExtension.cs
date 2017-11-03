﻿using System;
using System.Threading;
using PointBlank.API;

namespace Rocket.Core.Extensions
{
    public static class MulticastDelegateExtension
    {
        public static void TryInvoke(this System.MulticastDelegate theDelegate, params object[] args)
        {
            if (theDelegate == null) return;
            foreach (Delegate handler in theDelegate.GetInvocationList())
            {
                try
                {
                    handler.DynamicInvoke(args);
                }
                catch (Exception ex)
                {
                    PointBlankLogging.LogError("Error in MulticastDelegate " + handler.GetType().Name + "!", ex);
                }
            }
        }
    }
}
