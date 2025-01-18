using System;
using System.Collections.Generic;
using System.Text;

namespace PaintingLibrary.DapperTypeHandlers
{
    /// <summary>
    /// Our other class taken from MS Docs. This makes sure whenever SQLite
    /// and Dapper comes across a GUID type, it can convert from that string
    /// in the database to an actual GUID data type.
    /// </summary>
    class GuidTypeHandler : SqliteTypeHandler<Guid>
    {
        public override Guid Parse(object value)
            => Guid.Parse((string)value);
    }
}
