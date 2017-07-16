using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace ChaosCore.RepositoryLib
{
    public class ChaosCoreOption
    {
        public Type ExtensionType { get; set; }
        public Type DbContextOptionsBuilderType { get; set; }
    }
}
