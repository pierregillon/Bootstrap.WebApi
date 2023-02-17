using Bootstrap.Domain;

namespace Bootstrap.Infrastructure;

public class SystemClock : IClock
{
    public DateTime Now() => DateTime.UtcNow;
}
