﻿using System;
using System.Collections.Generic;

namespace ForEvolve.Azure.ApplicationInsights
{
    public interface ITelemetryClient
    {
        void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);
    }
}
