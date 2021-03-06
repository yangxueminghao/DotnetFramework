﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Dotnet.Utility
{
    public class BufferQueue<TMessage>
    {
        private readonly int _requestsWriteThreshold;
        private ConcurrentQueue<TMessage> _inputQueue;
        private ConcurrentQueue<TMessage> _processQueue;
        private readonly Action<TMessage> _handleMessageAction;
        private readonly string _name;
        private int _isProcesingMessage;

        public BufferQueue(string name, int requestsWriteThreshold, Action<TMessage> handleMessageAction)
        {
            _name = name;
            _requestsWriteThreshold = requestsWriteThreshold;
            _handleMessageAction = handleMessageAction;
            _inputQueue = new ConcurrentQueue<TMessage>();
            _processQueue = new ConcurrentQueue<TMessage>();
            
        }

        public void EnqueueMessage(TMessage message)
        {
            _inputQueue.Enqueue(message);
            TryProcessMessages();

            if (_inputQueue.Count >= _requestsWriteThreshold)
            {
                Thread.Sleep(20);
            }
        }

        private void TryProcessMessages()
        {
            if (Interlocked.CompareExchange(ref _isProcesingMessage, 1, 0) == 0)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        if (_processQueue.Count == 0 && _inputQueue.Count > 0)
                        {
                            SwapInputQueue();
                        }

                        if (_processQueue.Count > 0)
                        {
                            var count = 0;
                            TMessage message;
                            while (_processQueue.TryDequeue(out message))
                            {
                                try
                                {
                                    _handleMessageAction(message);
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception($"{_name} process message has exception. error detail:{ex.Message}");
                                }
                                finally
                                {
                                    count++;
                                }
                            }
                        }
                    }
                    finally
                    {
                        Interlocked.Exchange(ref _isProcesingMessage, 0);
                        if (_inputQueue.Count > 0)
                        {
                            TryProcessMessages();
                        }
                    }
                });
            }
        }
        private void SwapInputQueue()
        {
            var tmp = _inputQueue;
            _inputQueue = _processQueue;
            _processQueue = tmp;
        }
    }
}
