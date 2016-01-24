using System;

namespace ETL
{
    public interface IEntityAdapter
    {
        IComWrapper GetCursor();
    }
}
