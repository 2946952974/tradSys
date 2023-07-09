using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSysServer {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("开始监听");
            //Server server = new Server("10.0.4.9", 6688);//10.0.4.9
            Server server = new Server("127.0.0.1", 6688);//10.0.4.9
            server.Start();
            Console.ReadKey();
            
        }
    }
}
