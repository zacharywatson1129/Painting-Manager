using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PaintingLibrary.DapperTypeHandlers
{
    /// <summary>
    /// This is code straight from MS Docs. This class provides a generic
    /// type handler for when Dapper will need to handle not typical types
    /// in conversion.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SqliteTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
        // Parameters are converted by Microsoft.Data.Sqlite
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = value;
        }
    }
}
