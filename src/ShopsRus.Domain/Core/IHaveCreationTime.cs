using System;

namespace ShopsRus.Domain.Core
{
    public interface IHaveCreationTime
    {
        DateTime CreationTime { get; set; }
    }
}