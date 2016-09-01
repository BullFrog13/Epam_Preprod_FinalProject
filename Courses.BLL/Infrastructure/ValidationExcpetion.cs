using System;

namespace Courses.BLL.Infrastructure
{
    public class ValidationExcpetion : Exception
    {
        public string Property { get; protected set; }
        public ValidationExcpetion(string message, string prop) : base(message)
        {
            Property = prop;
        }
    }
}
