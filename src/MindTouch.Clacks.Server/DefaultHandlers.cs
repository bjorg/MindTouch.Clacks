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
using System.Text;
using MindTouch.Clacks.Server.Async;
using MindTouch.Clacks.Server.Sync;

namespace MindTouch.Clacks.Server {
    public static class DefaultHandlers {

        //--- Class Fields ---
        private static readonly Logger.ILog _log = Logger.CreateLog();

        //--- Class Properties ---
        public static ISyncCommandRegistration SyncCommandRegistration {
            get {
                return new SyncCommandRegistration(
                    DataExpectation.Auto,
                    (client, cmd, dataLength, arguments, errorHandler) =>
                        new SyncSingleCommandHandler(client, cmd, arguments, dataLength, DefaultResponse, errorHandler)
                );
            }
        }

        public static IAsyncCommandRegistration AsyncCommandRegistration {
            get {
                return new AsyncCommandRegistration(
                    DataExpectation.Auto,
                    (client, command, dataLength, arguments, errorHandler) =>
                        new AsyncSingleCommandHandler(client, command, arguments, dataLength, (r, c) => c(DefaultResponse(r)), errorHandler)
                );
            }
        }

        //--- Class Methods ---
        public static IResponse ErrorHandler(IRequest request, Exception e) {
            _log.Warn(string.Format("Request [{0}] threw an exception of type {1}: {2}", request.Command, e.GetType(), e.Message), e);
            return Response.Create("ERROR").WithArgument(e.GetType()).WithData(Encoding.UTF8.GetBytes(e.Message));
        }

        public static IResponse DisconnectHandler(IRequest request) {
            return Response.Create("BYE");
        }

        public static void ErrorHandler(IRequest request, Exception e, Action<IResponse> responseCallback) {
            var response = ErrorHandler(request, e);
            responseCallback(response);
        }

        private static IResponse DefaultResponse(IRequest request) {
            return Response.Create("UNKNOWN");
        }

        public static void DisconnectHandler(IRequest request, Action<IResponse> responseCallback) {
            responseCallback(DisconnectHandler(request));
        }
    }
}
