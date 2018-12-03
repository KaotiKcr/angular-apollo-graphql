using Links.Models;
using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Links.Services
{
    public class VoteEventService : IVoteEventService
    {
        private readonly ISubject<VoteEvent> _eventStream = new ReplaySubject<VoteEvent>(1);

        public VoteEventService()
        {
            AllEvents = new ConcurrentStack<VoteEvent>();
        }

        public ConcurrentStack<VoteEvent> AllEvents { get; }

        public void AddError(Exception exception)
        {
            _eventStream.OnError(exception);
        }

        public VoteEvent AddEvent(VoteEvent voteEvent)
        {
            AllEvents.Push(voteEvent);
            _eventStream.OnNext(voteEvent);
            return voteEvent;
        }

        public IObservable<VoteEvent> EventStream()
        {
            return _eventStream.AsObservable();
        }
    }

    public interface IVoteEventService
    {
        ConcurrentStack<VoteEvent> AllEvents { get; }
        void AddError(Exception exception);
        VoteEvent AddEvent(VoteEvent voteEvent);
        IObservable<VoteEvent> EventStream();


    }
}
