using System;
using System.Runtime.Serialization;

namespace Worker.Demo
{
    [Serializable]
    public class MessageHandlerException : Exception
    {
      public MessageHandlerException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public MessageHandlerException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public MessageHandlerException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
      protected MessageHandlerException(SerializationInfo info, 
         StreamingContext context) : base(info, context)
      {
         // Implement type-specific serialization constructor logic.
      }

    }
}