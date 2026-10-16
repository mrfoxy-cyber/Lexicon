using System;
using System.Collections.Generic;
using System.Text;

namespace Garage.Engines
{

    public abstract class Engine
    {
        public required string Model { get; init; }

    }
}
