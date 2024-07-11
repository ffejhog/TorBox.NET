namespace TorboxNET.Exceptions;

public class TorboxException : Exception
{
    public String Error { get; }
    
    
    public TorboxException(String error)
        : base(error) 
    {
        Error = error;
    }
    
}