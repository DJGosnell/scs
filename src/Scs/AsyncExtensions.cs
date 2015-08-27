using Hik.Communication.Scs.Client;
using Hik.Communication.Scs.Communication.Messages;
using Hik.Communication.Scs.Communication.Messengers;
using System;
using System.Threading.Tasks;

namespace Hik
{
    /// <summary>
    /// Asynchronus Extensions for SCS
    /// </summary>
    public static class AsyncExtensions
    {
        /// <summary>
        /// Send a Message and Wati for Reply
        /// </summary>
        /// <typeparam name="T">The expected Value</typeparam>
        /// <param name="msg">the message to send</param>
        /// <returns></returns>
        public static Task<IScsMessage> SendMessageWithReplyAsync(this IScsClient client, IScsMessage msg)
        {
            TaskCompletionSource<IScsMessage> tcs = new TaskCompletionSource<IScsMessage>();

            using (var requestReplyMessenger = new RequestReplyMessenger<IScsClient>(client))
            {
                requestReplyMessenger.Start(); //Start request/reply messenger

                //Send user message to the server and get response
                try {
                    var response = requestReplyMessenger.SendMessageAndWaitForResponse(msg);
                    tcs.SetResult(response);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }

            return tcs.Task;
        }
    }
}