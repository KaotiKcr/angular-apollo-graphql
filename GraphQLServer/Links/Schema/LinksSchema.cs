using GraphQL;

namespace Links.Schema
{
    public class LinksSchema : GraphQL.Types.Schema
    {
        public LinksSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<LinksQuery>();
            Mutation = resolver.Resolve<LinksMutation>();
        }
    }
}