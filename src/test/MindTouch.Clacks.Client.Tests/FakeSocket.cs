﻿/*
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
using MindTouch.Clacks.Client.Net;

namespace MindTouch.Clacks.Client.Tests {
    public class FakeSocket : ISocket {

        //--- Fields ---
        public int DisposeCalled;
        public int ConnectedCalled;
        public Action SendCallback = () => { };
        public int SendCalled;
        public Func<byte[], int, int, int> ReceiveCallback = (buffer, offset, size) => 0;
        public int ReceiveCalled;
        private bool _connnected;

        //--- Constructors ---
        public FakeSocket() {
            Connected = true;
        }

        //--- Properties ---
        public bool Connected {
            get { ConnectedCalled++; return _connnected; }
            set { _connnected = value; }
        }

        public bool IsDisposed { get { return DisposeCalled > 0; } }

        //--- Methods ---
        public void Dispose() {
            DisposeCalled++;
            Connected = false;
        }

        public int Send(byte[] buffer, int offset, int size) {
            SendCalled++;
            SendCallback();
            return size;
        }

        public int Receive(byte[] buffer, int offset, int size) {
            ReceiveCalled++;
            return ReceiveCallback(buffer, offset, size);
        }
    }
}