/*
 * MindTouch.Clacks
 * 
 * Copyright (c) 2006-2015 MindTouch Inc
 * http://github.com/mindtouch/MindTouch.Clacks
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Net;

namespace MindTouch.Clacks.Memcache {
    public class MemcacheProgram {

        //--- Class Methods ---
        public static void Main(string[] args) {
            var ip = "127.0.0.1";
            var port = 11211;
            if(args.Length > 0 ) {
                ip = args[0];
            }
            if(args.Length > 1) {
                port = int.Parse(args[1]);
            }
            var endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            using(var server = new Memcached(endPoint)) {
                Console.WriteLine("press any key to exit");
                Console.ReadKey();
            }
        }
    }
}
