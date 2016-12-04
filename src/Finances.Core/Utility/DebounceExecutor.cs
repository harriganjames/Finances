//using System;
//using System.Collections.Generic;
//using System.Collections.Concurrent;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Finances.Core.Interfaces;

//namespace Finances.Core.Utility
//{
//    public class DebounceExecutor<T> : IDebounceExecutor<T>
//    {
//        TimeSpan ts;
//        Action<T> action;
//        ConcurrentQueue<T> queue = new ConcurrentQueue<T>();

//        public void Initialize(TimeSpan ts, Action<T> action)
//        {
//            this.ts = ts;
//            this.action = action;
//        }


//        public async void Execute(T item)
//        {
//            bool runAction = false;
//            queue.Enqueue(item);

//            await Task.Delay(ts).ConfigureAwait(continueOnCapturedContext:true);

//            T nextItem;
//            lock(queue)
//            {
//                if (queue.TryDequeue(out nextItem))
//                {
//                    if (queue.Count() == 0)
//                    {
//                        runAction = true;
//                    }
//                }
//            }

//            if(runAction)
//                action(nextItem);

//        }



//    }
//}
