using System;
using System.Linq;
using System.Linq.Expressions;

namespace BE.Repositories.RepositoryServices
{
    public static class DynamicLinqStatementService
    {
        public static Func<TIn,TOut> CreateNewStatement<TIn, TOut>( string fields )
        {
            // input parameter "o"
            var xParameter = Expression.Parameter( typeof( TIn ), "o" );

            // new statement "new Data()"
            var xNew = Expression.New( typeof( TOut ) );

            // create initializers
            var bindings = fields.Split( ',' ).Select( o => o.Trim() )
                .Select( o => {

                        // property "Field1"
                        var mi = typeof( TIn ).GetProperty( o );

                        // original value "o.Field1"
                        var xOriginal = Expression.Property( xParameter, mi );

                        // set value "Field1 = o.Field1"
                        return Expression.Bind( mi, xOriginal );
                    }
                );

            // initialization "new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var xInit = Expression.MemberInit( xNew, bindings );

            // expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<TIn,TOut>>( xInit, xParameter );

            // compile to Func<Data, Data>
            return lambda.Compile();
        }
    }
}