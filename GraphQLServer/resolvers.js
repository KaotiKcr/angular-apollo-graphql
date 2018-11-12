
const { GraphQLScalarType } = require("graphql");

function convertDate(inputFormat) {
  function pad(s) {
    return s < 10 ? "0" + s : s;
  }
  var d = new Date(inputFormat);
  return [pad(d.getDate()), pad(d.getMonth()), d.getFullYear()].join("/");
}

// Define Date scalar type.

const GQDate = new GraphQLScalarType({
  name: "GQDate",
  description: "Date type",
  parseValue(value) {
    // value comes from the client
    return value; // sent to resolvers
  },
  serialize(value) {
    // value comes from resolvers
    return value; // sent to the client
  },
  parseLiteral(ast) {
    // value comes from the client
    return new Date(ast.value); // sent to resolvers
  }
});

// data store with default data
const links = [
  {
    id: 1,
    createdAt: new Date("2014-08-31"),
    updatedAt: new Date("2014-08-31"),
    description: "Google",
    url: "https://www.google.com/"
  },
  {
    id: 2,
    createdAt: new Date("2015-09-15"),
    updatedAt: new Date("2015-09-15"),
    description: "Facebook",
    url: "https://www.facebook.com/"
  },
  {
    id: 3,
    createdAt: new Date("2018-10-15"),
    updatedAt: new Date("2018-10-15"),
    description: "Triquimas",
    url: "http://www.triquimas.cr/"
  }
];

const resolvers = {
  Query: {
    Links: () => links, // return all links
    Link: (_, { id }) =>
      links.find(link => link.id == id) // return link by id
  },
  Mutation: {
    // create a new Link
    createLink: (root, args) => {
      // get next Link id
      const nextId =
        links.reduce((id, link) => {
          return Math.max(id, link.id);
        }, -1) + 1;
      const newLink = {
        id: nextId,        
        createdAt: new Date(),
        updatedAt: new Date(),
        description: args.description,
        url: args.url
      };
      // add Link to collection
      links.push(newLink);
      return newLink;
    }, // delete Link by id
    deleteLink: (root, args) => {
      // find index by id
      const index = links.findIndex(
        link => link.id == args.id
      );
      // remove link by index
      links.splice(index, 1);
    }, // update link
    updateLink: (root, args) => {
      // find index by id
      const index = links.findIndex(
        link => link.id == args.id
      );
      links[index].description = args.description;
      links[index].url = args.url;
      links[index].updatedAt = new Date();      
      return links[index];
    }
  },
  GQDate
};

module.exports.Resolvers = resolvers;