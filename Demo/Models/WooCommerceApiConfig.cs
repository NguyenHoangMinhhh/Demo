namespace Demo.Models;


public class WooCommerceApiConfig
{
    public string Endpoint { get; set; }
    public string ConsumerKey { get; set; }
    public string ConsumerSecret { get; set; }
    
    // Constructor
    public WooCommerceApiConfig()
    {
        // Kiểm tra null nếu cần thiết
        //if (Endpoint == null) throw new ArgumentNullException(nameof(Endpoint));
     //   if (ConsumerKey == null) throw new ArgumentNullException(nameof(ConsumerKey));
       // if (ConsumerSecret == null) throw new ArgumentNullException(nameof(ConsumerSecret));
    }
}