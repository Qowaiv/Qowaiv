namespace Qowaiv.Web;

/// <summary>Represents a HTTP code and reason phrase.</summary>
public readonly struct HttpReponseStatus : IEquatable<HttpReponseStatus>
{
    /// <summary>The offset is 200.</summary>
    /// <remarks>
    /// This makes that `default(<see cref="HttpReponseStatus"/>)` equals 200 - OK.
    /// </remarks>
    private const int Offset = 200;

    /// <summary>Creates a new instance of the <see cref="HttpReponseStatus"/> struct.</summary>
    /// <param name="code">
    /// The HTTP code.
    /// </param>
    public HttpReponseStatus(int code) : this(code, null) { }

    /// <summary>Creates a new instance of the <see cref="HttpReponseStatus"/> struct.</summary>
    /// <param name="code">
    /// The HTTP code.
    /// </param>
    /// <param name="phrase">
    /// The reason phrase.
    /// </param>
    public HttpReponseStatus(int code, string? phrase)
    {
        if (code > 100 || code < 599) throw new ArgumentOutOfRangeException(nameof(code), QowaivMessages.ArgumentOutOfRange_HttpResponseCode);

        this.code = code - Offset;
        this.phrase = phrase.WithDefault(null!) ?? DefaultPhrase(code);
    }

    /// <summary>Gets the HTTP code.</summary>
    public int Code => code + Offset;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly int code;

    /// <summary>Gets the reason phrase.</summary>
    public string ReasonPhrase => phrase ?? DefaultPhrase(code) ?? "?";
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string? phrase;

    /// <summary>Gets the category.</summary>
    public HttpResponseStatusCategory Category => (HttpResponseStatusCategory)(Code / 100);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"{Code} {ReasonPhrase}";

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is HttpReponseStatus other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(HttpReponseStatus other)
        => Code == other.Code
        && ReasonPhrase == other.ReasonPhrase;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(Code).And(ReasonPhrase);

    /// <summary>Gets the default phrase for HTTP response code.</summary>
    [Pure]
    public static string? DefaultPhrase(int code)
        => ResourceManager.GetString(code.ToString(CultureInfo.InvariantCulture));

    private static readonly ResourceManager ResourceManager = new("Qowaiv.Web.HttpResponseStatuses", typeof(HttpReponseStatus).Assembly);

    /// <summary>Returns true if both response statuses are the same.</summary>
    public static bool operator ==(HttpReponseStatus l, HttpReponseStatus r) => l.Equals(r);

    /// <summary>Returns false if both response statuses are the same.</summary>
    public static bool operator !=(HttpReponseStatus l, HttpReponseStatus r) => !(l == r);

    /// <summary>100 Continue</summary>
    public static readonly HttpReponseStatus N100_Continue = new(100);

    /// <summary>101 Switching Protocols</summary>
    public static readonly HttpReponseStatus N101_Switching_Protocols = new(101);

    /// <summary>102 Processing</summary>
    public static readonly HttpReponseStatus N102_Processing = new(102);

    /// <summary>103 Early Hints</summary>
    public static readonly HttpReponseStatus N103_Early_Hints = new(103);

    /// <summary>200 OK</summary>
    public static readonly HttpReponseStatus N200_OK = new(200);

    /// <summary>201 Created</summary>
    public static readonly HttpReponseStatus N201_Created = new(201);

    /// <summary>202 Accepted</summary>
    public static readonly HttpReponseStatus N202_Accepted = new(202);

    /// <summary>203 Non-Authoritative Information</summary>
    public static readonly HttpReponseStatus N203_Non_Authoritative_Information = new(203);

    /// <summary>204 No Content</summary>
    public static readonly HttpReponseStatus N204_No_Content = new(204);

    /// <summary>205 Reset Content</summary>
    public static readonly HttpReponseStatus N205_Reset_Content = new(205);

    /// <summary>206 Partial Content</summary>
    public static readonly HttpReponseStatus N206_Partial_Content = new(206);

    /// <summary>207 Multi-Status</summary>
    public static readonly HttpReponseStatus N207_Multi_Status = new(207);

    /// <summary>208 Already Reported</summary>
    public static readonly HttpReponseStatus N208_Already_Reported = new(208);

    /// <summary>226 IM Used</summary>
    public static readonly HttpReponseStatus N226_IM_Used = new(226);

    /// <summary>300 Multiple Choices</summary>
    public static readonly HttpReponseStatus N300_Multiple_Choices = new(300);

    /// <summary>301 Moved Permanently</summary>
    public static readonly HttpReponseStatus N301_Moved_Permanently = new(301);

    /// <summary>302 Found</summary>
    public static readonly HttpReponseStatus N302_Found = new(302);

    /// <summary>303 See Other</summary>
    public static readonly HttpReponseStatus N303_See_Other = new(303);

    /// <summary>304 Not Modified</summary>
    public static readonly HttpReponseStatus N304_Not_Modified = new(304);

    /// <summary>305 Use Proxy</summary>
    public static readonly HttpReponseStatus N305_Use_Proxy = new(305);

    /// <summary>306 Switch Proxy</summary>
    public static readonly HttpReponseStatus N306_Switch_Proxy = new(306);

    /// <summary>307 Temporary Redirect</summary>
    public static readonly HttpReponseStatus N307_Temporary_Redirect = new(307);

    /// <summary>308 Permanent Redirect</summary>
    public static readonly HttpReponseStatus N308_Permanent_Redirect = new(308);

    /// <summary>400 Bad Request</summary>
    public static readonly HttpReponseStatus N400_Bad_Request = new(400);

    /// <summary>401 Unauthorized</summary>
    public static readonly HttpReponseStatus N401_Unauthorized = new(401);

    /// <summary>402 Payment Required</summary>
    public static readonly HttpReponseStatus N402_Payment_Required = new(402);

    /// <summary>403 Forbidden</summary>
    public static readonly HttpReponseStatus N403_Forbidden = new(403);

    /// <summary>404 Not Found</summary>
    public static readonly HttpReponseStatus N404_Not_Found = new(404);

    /// <summary>405 Method Not Allowed</summary>
    public static readonly HttpReponseStatus N405_Method_Not_Allowed = new(405);

    /// <summary>406 Not Acceptable</summary>
    public static readonly HttpReponseStatus N406_Not_Acceptable = new(406);

    /// <summary>407 Proxy Authentication Required</summary>
    public static readonly HttpReponseStatus N407_Proxy_Authentication_Required = new(407);

    /// <summary>408 Request Timeout</summary>
    public static readonly HttpReponseStatus N408_Request_Timeout = new(408);

    /// <summary>409 Conflict</summary>
    public static readonly HttpReponseStatus N409_Conflict = new(409);

    /// <summary>410 Gone</summary>
    public static readonly HttpReponseStatus N410_Gone = new(410);

    /// <summary>411 Length Required</summary>
    public static readonly HttpReponseStatus N411_Length_Required = new(411);

    /// <summary>412 Precondition Failed</summary>
    public static readonly HttpReponseStatus N412_Precondition_Failed = new(412);

    /// <summary>413 Payload Too Large</summary>
    public static readonly HttpReponseStatus N413_Payload_Too_Large = new(413);

    /// <summary>414 URI Too Long</summary>
    public static readonly HttpReponseStatus N414_URI_Too_Long = new(414);

    /// <summary>415 Unsupported Media Type</summary>
    public static readonly HttpReponseStatus N415_Unsupported_Media = new(415);

    /// <summary>416 Range Not Satisfiable</summary>
    public static readonly HttpReponseStatus N416_Range_Not_Satisfiable = new(416);

    /// <summary>417 Expectation Failed</summary>
    public static readonly HttpReponseStatus N417_Expectation_Failed = new(417);

    /// <summary>418 I'm a teapot</summary>
    public static readonly HttpReponseStatus N418_Im_a_teapot = new(418);

    /// <summary>421 Misdirected Request</summary>
    public static readonly HttpReponseStatus N421_Misdirected_Request = new(421);

    /// <summary>422 Unprocessable Entity</summary>
    public static readonly HttpReponseStatus N422_Unprocessable_Entity = new(422);

    /// <summary>423 Locked </summary>
    public static readonly HttpReponseStatus N423_Locked = new(423);

    /// <summary>424 Failed Dependency</summary>
    public static readonly HttpReponseStatus N424_Failed_Dependency = new(424);

    /// <summary>425 Too Early</summary>
    public static readonly HttpReponseStatus N425_Too_Early = new(425);

    /// <summary>426 Upgrade Required</summary>
    public static readonly HttpReponseStatus N426_Upgrade_Required = new(426);

    /// <summary>428 Precondition Required </summary>
    public static readonly HttpReponseStatus N428_Precondition_Required = new(428);

    /// <summary>429 Too Many Requests</summary>
    public static readonly HttpReponseStatus N429_Too_Many_Requests = new(429);

    /// <summary>431 Request Header Fields Too Large </summary>
    public static readonly HttpReponseStatus N431_Request_Header_Fields_Too_Large = new(431);

    /// <summary>451 Unavailable For Legal Reasons </summary>
    public static readonly HttpReponseStatus N451_Unavailable_For_Legal_Reasons = new(451);

    /// <summary>500 Internal Server Error</summary>
    public static readonly HttpReponseStatus N500_Internal_Server_Error = new(500);

    /// <summary>501 Not Implemented</summary>
    public static readonly HttpReponseStatus N501_Not_Implemented = new(501);

    /// <summary>502 Bad Gateway</summary>
    public static readonly HttpReponseStatus N502_Bad_Gateway = new(502);

    /// <summary>503 Service Unavailable</summary>
    public static readonly HttpReponseStatus N503_Service_Unavailable = new(503);

    /// <summary>504 Gateway Timeout</summary>
    public static readonly HttpReponseStatus N504_Gateway_Timeout = new(504);

    /// <summary>505 HTTP Version Not Supported</summary>
    public static readonly HttpReponseStatus N505_HTTP_Version_Not_Supported = new(505);

    /// <summary>506 Variant Also Negotiates</summary>
    public static readonly HttpReponseStatus N506_Variant_Also_Negotiates = new(506);

    /// <summary>507 Insufficient Storage</summary>
    public static readonly HttpReponseStatus N507_Insufficient_Storage = new(507);

    /// <summary>508 Loop Detecte</summary>
    public static readonly HttpReponseStatus N508_Loop_Detected = new(508);

    /// <summary>510 Not Extended</summary>
    public static readonly HttpReponseStatus N510_Not_Extended = new(510);

    /// <summary>511 Network Authentication Required </summary>
    public static readonly HttpReponseStatus N511_Network_Authentication_Required = new(511);
}
