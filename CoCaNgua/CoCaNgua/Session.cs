using System;

namespace CoCaNgua
{
    public static class Session
    {
        public static int UserId;
        public static string Username;

        // SINGLETON network connection for whole app
        public static NetworkHelper Network = new NetworkHelper();
    }
}
