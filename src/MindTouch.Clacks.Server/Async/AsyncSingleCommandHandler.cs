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
using System.Collections.Generic;
using System.Net;

namespace MindTouch.Clacks.Server.Async {
    public class AsyncSingleCommandHandler : IAsyncCommandHandler {

        //--- Class Methods ---
        public static IAsyncCommandHandler DisconnectHandler(string command, Action<IRequest, Action<IResponse>> handler) {
            return new AsyncSingleCommandHandler(command, handler);
        }

        //--- Fields ---
        private readonly IPEndPoint _client;
        private readonly string _command;
        private readonly string[] _arguments;
        private readonly Action<IRequest, Action<IResponse>> _handler;
        private readonly Action<IRequest, Exception, Action<IResponse>> _errorHandler;
        private readonly int _dataLength;
        private readonly bool _disconnect;
        private int _received;
        private List<byte[]> _dataChunks;

        //--- Constructors ---
        public AsyncSingleCommandHandler(IPEndPoint client, string command, string[] arguments, int dataLength, Action<IRequest, Action<IResponse>> handler, Action<IRequest, Exception, Action<IResponse>> errorHandler) {
            _client = client;
            _command = command;
            _arguments = arguments;
            _dataLength = dataLength;
            _handler = handler;
            _errorHandler = errorHandler;
        }

        private AsyncSingleCommandHandler(string command, Action<IRequest, Action<IResponse>> handler) {
            _command = command;
            _handler = handler;
            _disconnect = true;
        }

        //--- Properties ---
        public bool ExpectsData { get { return _dataLength > 0; } }
        public bool DisconnectOnCompletion { get { return _disconnect; } }
        public int OutstandingBytes { get { return _dataLength - _received; } }
        public string Command { get { return _command; } }

        //--- Methods ---
        public void Dispose() { }

        public void AcceptData(byte[] chunk) {
            if(_dataChunks == null) {
                _dataChunks = new List<byte[]>();
            }
            _dataChunks.Add(chunk);
            _received += chunk.Length;
            if(_received > _dataLength) {
                throw new DataExpectationException(_dataLength, _received);
            }
        }

        public void GetResponse(Action<IResponse, Action> responseCallback) {
            Action<IResponse> responseAction = response => responseCallback(response, null);
            if(_received < _dataLength) {
                var badRequest = new Request(_client, _command, _arguments, 0, new List<byte[]>());
                _errorHandler(badRequest, new DataExpectationException(_dataLength, _received), responseAction);
                return;
            }
            var request = new Request(_client, _command, _arguments, _dataLength, _dataChunks);
            try {
                _handler(request, responseAction);
            } catch(Exception e) {
                _errorHandler(request, e, responseAction);
            }
        }
    }
}