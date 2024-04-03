namespace net_redis
{
    public class ServiceOneClass
    {
        public async Task<Car> GetCarAsync(int id) 
        { 
            await Task.Delay(1000);
            return new(id, $"Toyota {id}", "Me"); 
        }

        public record Car(int Id, string Mark, string Owner);
    }
}
