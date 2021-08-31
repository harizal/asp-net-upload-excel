using System;
using System.Collections.Generic;

namespace BTPNS.Core.GenericModel
{
    public class CustomException : Exception
    {
        public CustomException() { }
        public CustomException(List<string> errors)
        {
            Errors = errors;
        }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
