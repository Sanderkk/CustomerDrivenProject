using System;
using System.Collections.Generic;
using HotChocolate;

namespace src.Database
{
    public class GraphQLErrorFilter: IErrorFilter
    {
            public IError OnError(IError error)
            {
                return error.WithMessage(error.Exception.Message);
            }
    }
}