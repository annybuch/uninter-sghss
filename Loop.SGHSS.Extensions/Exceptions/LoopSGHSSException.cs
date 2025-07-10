namespace Loop.SGHSS.Extensions.Exceptions
{
    [Serializable]
    public class SGHSSBadRequestException : Exception
    {
        public SGHSSBadRequestException(string message) : base(message) { }
    }

    [Serializable]
    public class SGHSSUnauthorizedException : Exception
    {
        public SGHSSUnauthorizedException(string message) : base(message) { }
    }

    [Serializable]
    public class SGHSSForbiddenException : Exception
    {
        public SGHSSForbiddenException(string message) : base(message) { }
    }

    [Serializable]
    public class SGHSSNotFoundException : Exception
    {
        public SGHSSNotFoundException(string message) : base(message) { }
    }

    [Serializable]
    public class SGHSSRequestTimeoutException : Exception
    {
        public SGHSSRequestTimeoutException(string message) : base(message) { }
    }

    [Serializable]
    public class SGHSSInternalServerErrorException : Exception
    {
        public SGHSSInternalServerErrorException(string message) : base(message) { }
    }

    [Serializable]
    public class SGHSSNotImplementedException : Exception
    {
        public SGHSSNotImplementedException(string message) : base(message) { }
    }

    [Serializable]
    public class SGHSSGatewayTimeoutException : Exception
    {
        public SGHSSGatewayTimeoutException(string message) : base(message) { }
    }

    [Serializable]
    public class SGHSSRefuseMessageException : Exception
    {
        public SGHSSRefuseMessageException() : base() { }
    }
}
