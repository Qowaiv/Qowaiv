namespace Qowaiv.Web;

/// <summary>All HTTP response status codes are separated into five classes or
/// categories. The first digit of the status code defines the class of response,
/// while the last two digits do not have any classifying or categorization role.
/// </summary>
public enum HttpResponseStatusCategory
{
    /// <summary>None/unknown.</summary>
    None = 0,
    
    /// <summary>1xx informational response – the request was received, continuing process</summary>
    InformationalResponse = 1,
    
    /// <summary>2xx successful – the request was successfully received, understood, and accepted</summary>
    Successful = 2,
    
    /// <summary>3xx redirection – further action needs to be taken in order to complete the request</summary>
    Redirection = 3,
    
    /// <summary>4xx client error – the request contains bad syntax or cannot be fulfilled</summary>
    ClientError = 4,
    
    /// <summary>5xx server error – the server failed to fulfil an apparently valid request</summary>
    ServerError = 5,
}
