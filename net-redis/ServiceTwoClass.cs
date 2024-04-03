namespace net_redis
{
    public class ServiceTwoClass
    {

        public Task<string> GetNameAsync(string id) => Task.FromResult("Bob");

    }
}
