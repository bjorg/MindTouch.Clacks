/*
 * MindTouch.Clacks
 * 
 * Copyright (C) 2011-2013 Arne F. Claassen
 * geekblog [at] claassen [dot] net
 * http://github.com/sdether/MindTouch.Clacks
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
using System.Net.Sockets;

namespace MindTouch.Clacks.Server.Async {
    public class AsyncClientHandlerFactory : IClientHandlerFactory {
        private readonly IAsyncCommandDispatcher _dispatcher;

        public AsyncClientHandlerFactory(IAsyncCommandDispatcher dispatcher) {
            _dispatcher = dispatcher;
        }

        public IClientHandler Create(Guid clientId, Socket socket, IClacksInstrumentation instrumentation, Action<IClientHandler> removeCallback) {
            return new AsyncClientHandler(clientId, socket, _dispatcher, instrumentation, removeCallback);
        }
    }
}