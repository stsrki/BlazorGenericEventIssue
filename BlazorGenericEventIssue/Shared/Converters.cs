using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGenericEventIssue.Shared
{
    public static class Converters
    {
        public static bool TryChangeType<TValue>( object o, out TValue value )
        {
            try
            {
                Type conversionType = Nullable.GetUnderlyingType( typeof( TValue ) ) ?? typeof( TValue );

                if ( conversionType.IsEnum && EnumTryParse( o?.ToString(), conversionType, out TValue theEnum ) )
                    value = theEnum;
                else if ( conversionType == typeof( Guid ) )
                    value = (TValue)Convert.ChangeType( Guid.Parse( o.ToString() ), conversionType );
                else
                    value = (TValue)Convert.ChangeType( o, conversionType );

                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }

        public static bool EnumTryParse<TValue>( string input, Type conversionType, out TValue theEnum )
        {
            if ( input != null )
            {
                foreach ( string en in Enum.GetNames( conversionType ) )
                {
                    if ( en.Equals( input, StringComparison.InvariantCultureIgnoreCase ) )
                    {
                        theEnum = (TValue)Enum.Parse( conversionType, input, true );
                        return true;
                    }
                }
            }

            theEnum = default;
            return false;
        }
    }
}
