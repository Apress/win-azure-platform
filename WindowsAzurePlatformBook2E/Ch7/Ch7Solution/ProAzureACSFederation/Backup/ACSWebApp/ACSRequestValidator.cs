using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Util;

namespace ACSWebApp
{
    public class ACSRequestValidator : RequestValidator
    {
        protected override bool IsValidRequestString(HttpContext context, string value, RequestValidationSource requestValidationSource, string collectionKey, out int validationFailureIndex)
        {
            return base.IsValidRequestString(context, value, requestValidationSource, collectionKey, out validationFailureIndex);
        }
    }
}