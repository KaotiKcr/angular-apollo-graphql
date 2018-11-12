// schema.js

const schema = `
# declare custom scalars for date as GQDate
scalar GQDate

# registration type
type Link {
    id: ID!
    createdAt: GQDate!
    updatedAt: GQDate!

    description: String!
    url: String!
}


type Query {
    # Return a link by id
    Link(id: ID!): Link
    # Return all Links
    Links(limit: Int): [Link]
}

type Mutation {
    # Create a Link
    createLink (description: String, url: String): Link
    # Update a Link
    updateLink (id: ID!, description: String, url: String): Link
    # Delete a Link
    deleteLink(id: ID!): Link
}
`;

module.exports.Schema = schema;