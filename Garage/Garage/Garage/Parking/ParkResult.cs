using System;
using System.Collections.Generic;
using System.Text;

namespace Garage.Parking
{
    public record ParkResult(
    bool Success,
    int? SpotIndex,
    string Message
    
    );
}
