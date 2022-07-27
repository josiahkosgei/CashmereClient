namespace Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount
{
    public class ResponseHeaderTypeDto
    {
        
        public string CorrelationID {get;set;}
        
        public long ElapsedTime {get;set;}
        
        public bool ElapsedTimeSpecified {get;set;}
        
        public string MessageID {get;set;}
        
        public string StatusCode {get;set;}
        
        public string StatusDescription {get;set;}
        
        public string StatusDescriptionKey {get;set;}
        public StatusMessagesTypeDto StatusMessages {get;set;}
    }
}