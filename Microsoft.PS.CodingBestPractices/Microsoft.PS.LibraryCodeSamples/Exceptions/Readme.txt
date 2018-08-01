Do and Don't
------------------------------
AVOID catching exceptions that you’re unable to handle fully.
AVOID hiding (discarding) exceptions you don’t fully handle.
DO use throw to rethrow an exception; rather than throw <exception object> inside a catch block.
DO set the wrapping exception’s InnerException property with the caught exception unless doing so exposes private data.
DO use caution when rethrowing different exceptions.
Rarely use System.Exception and general catch blocks—except to log the exception before shutting down the application.