using Stadhawk.Laundry.Utility.GoogleResponseViewModel;

namespace Stadhawk.Laundry.Utility.IHandler
{
    public interface IDirectionHandler
    {
        System.Threading.Tasks.Task<GoogleDirectionResponse> GetGoogleDirectionResponseAsync(string sLatitude, string sLongitude, string dLatitude, string dLongitude);
    }
}
