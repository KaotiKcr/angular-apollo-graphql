using GraphQL.Types;
using Links.Schema.Types;
using Links.Services;

namespace Links.Schema
{
    public class LinksQuery : ObjectGraphType
    {
        public LinksQuery(ILinkService links)
        {
            Name = "Query";
            Field<ListGraphType<LinkType>>(
                "links",
                resolve: context => links.GetLinksAsync() 
                );
        }
    }
}
