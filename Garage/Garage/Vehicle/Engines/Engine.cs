using System;
using System.Collections.Generic;
using System.Text;
using Garage.Engines.Specifications;

namespace Garage.Engines
{

    public abstract class Engine
    {
        public required string Model { get; init; }
    }

    public abstract class Engine<TSpecification> : Engine
        where TSpecification : EngineSpecification
    {
        public required TSpecification Specification { get; init; }
    }
}
