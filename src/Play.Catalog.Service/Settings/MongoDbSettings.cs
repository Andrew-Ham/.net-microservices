namespace Play.Catalog.Service.Settings 
{
    public class MongoDbSettings 
    {
        // Since we are not planning to change these settings after the microservice loads, we switch the 'set' to 'init'.
        // This prevents anybody on any piece of the code to modify the values after they have been initalised.
        public string Host { get; init; }   
        public int Port { get; init; }   

        // Also define a property for our connection string. We use string interpolation for this.
        // Below is whats called an expression body definition so it's a property defined directly by the value on the right side.
        public string ConnectionString => $"mongdb://{Host}:{Port}";
        
    }
}