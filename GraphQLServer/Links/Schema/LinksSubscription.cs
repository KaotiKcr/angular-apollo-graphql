using System;
using GraphQL.Types;
using GraphQL.Subscription;
using GraphQL.Resolvers;
using Links.Models;
using Links.Services;
using Links.Schema.EventTypes;

namespace Links.Schema
{
    public class LinksSubscription : ObjectGraphType<object>
    {
        private readonly IVoteEventService _events;

        public LinksSubscription(IVoteEventService events)
        {
            _events = events;

            Name = "Subscription";
            AddField(new EventStreamFieldType
            {
                Name = "voteEvent",
                Type = typeof(VoteEventType),
                Resolver = new FuncFieldResolver<VoteEvent>(ResolveEvent),
                Subscriber = new EventStreamResolver<VoteEvent>(Subscribe),
            });
        }

        private VoteEvent ResolveEvent(ResolveFieldContext context)
        {
            VoteEvent voteEvent = context.Source as VoteEvent;
            return voteEvent;
        }

        private IObservable<VoteEvent> Subscribe(ResolveEventStreamContext context)
        {
            return _events.EventStream();
        }
    }
}
