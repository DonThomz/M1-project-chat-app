namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var client = new Client();
            client.ConnectionToServer();
        }
    }
}