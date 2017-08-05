using System;
using System.Collections.Generic;
using System.Text;

namespace ChaosCore.BusinessLib
{
    public interface IBLLTransaction
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
