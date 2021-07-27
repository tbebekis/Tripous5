/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;

namespace Tripous
{
    /// <summary>
    /// A delegate to be used in situations where a "generic" event handler is needed 
    /// with an indeterminate number of parameters.
    /// </summary>
    /// <param name="Args">Any arguments passed to the callback.</param>
    /// <returns>An object, depending on the situation, it might be a null object</returns>
    public delegate object GenericEventHandler(params object[] Args);
}