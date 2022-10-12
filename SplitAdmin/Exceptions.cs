using System;

namespace SplitAdmin
{
    public class SplitAdminException : Exception {
    
        public SplitAdminException(string message) : base(message) { }
    }

    public class SplitAdminResponseException : SplitAdminException {
        public SplitAdminResponseException(string message) : base(message) { }
    }
}
