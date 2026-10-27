using System;

namespace RDH.Core;

public abstract record EntityId(Guid Value)
{
    public override string ToString()
    {
        return Value.ToString();
    }
};
