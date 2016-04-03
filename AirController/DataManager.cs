using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirController
{
    public static class DataManager
    {
       public static async Task<string> GetCO2LastValue()
        {
            var value = await WebRequest.GetDataFromServer("http://air-controller.azurewebsites.net/api/GetCO2LastValue");
            return value; 
        }

        public static async void SetStatus(byte mode)
        {
            await WebRequest.PostDataToServer("http://air-controller.azurewebsites.net/api/SetStatus?mode=", mode);
        }

        public static async Task<string> GetCurrentMode()
        {
            var mode = await WebRequest.GetDataFromServer("http://air-controller.azurewebsites.net/api/GetStatus");
            return mode;
        }
    }
}
