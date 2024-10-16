namespace APIAutomation
{
    public class TestFixture : IDisposable
    {
        public string POSTMAN_API_URL { get; set; }
        public string NHTS_API_URL { get; set; }
        public string API_URL { get; set; }
        public int TIME_OUT { get; set; }
        public TestFixture()
        {
        
            POSTMAN_API_URL = "https://postman-echo.com/post";
            NHTS_API_URL = "https://vpic.nhtsa.dot.gov/api/vehicles/GetVehicleTypesForMakeId/440?format=xml";

            TIME_OUT = 15000;
        }
        public void Dispose()
        {
            // Cleanup
        }


    }
}
