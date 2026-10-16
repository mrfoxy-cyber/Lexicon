using System;
using System.Collections.Generic;
using System.Text;

namespace Garage
{
    public record ParkResult(
    bool Success,
    int? SpotIndex,
    string Message
    
    );
}
