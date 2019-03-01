import { Link, User, Vote } from '../models/link.model';
import gql from 'graphql-tag';

export const ALL_LINKS_QUERY = gql`
  query AllLinksQuery($after: String, $first: Int, $searchText: String) {
    links(after: $after, first: $first, searchText: $searchText) {
      items {
        id
        createdAt
        updatedAt
        url
        description
        postedBy {
          id
          createdAt
          updatedAt
          name
          email
        }
        votes {
          id
          user {
            id
          }
        }
      }
      totalCount
    }
  }
`;

export interface LinkConnection {
  items: Link[];
  totalCount: number;
  pageInfo: PageInfo;
}

export interface PageInfo {
  startCursor: String;
  endCursor: String;
  hasNextPage?: boolean;
  hasPreviousPage?: boolean;
}

export interface AllLinksQueryResponse {
  links: LinkConnection;
}

export const ALL_USERS_QUERY = gql`
  query AllUsersQuery {
    users {
      items {
        id
        email
        name
        password
      }
    }
  }
`;

export interface UserConnection {
  items: User[];
}

export interface AllUsersQueryResponse {
  users: UserConnection;
}

export const CREATE_LINK_MUTATION = gql`
  mutation CreateLinkMutation($link: LinkInput!) {
    createLink(link: $link) {
      id
      createdAt
      updatedAt
      url
      description
      postedBy {
        id
        createdAt
        updatedAt
        name
        email
      }
    }
  }
`;

export interface CreateLinkMutationResponse {
  createLink: Link;
}

export const DELETE_LINK_MUTATION = gql`
  mutation DeleteLinkMutation($id: Int!) {
    deleteLink(id: $id) {
      id
      createdAt
      updatedAt
      url
      description
    }
  }
`;

export interface DeleteLinkMutationResponse {
  deleteLink: Link;
}

export const CREATE_USER_MUTATION = gql`
  mutation CreateUserMutation($user: UserInput!) {
    createUser(user: $user) {
      id
    }

    signinUser(user: $user) {
      token
      user {
        id
      }
    }
  }
`;

export interface CreateUserMutationResponse {
  loading: boolean;
  createUser: User;
  signinUser: {
    token: string;
    user?: User;
  };
}

export const SIGNIN_USER_MUTATION = gql`
  mutation SigninUserMutation($signinUser: SigninUserInput!) {
    signinUser(signinUser: $signinUser) {
      token
      user {
        id
      }
    }
  }
`;

export const CREATE_VOTE_MUTATION = gql`
  mutation CreateVoteMutation($userId: Int!, $linkId: Int!) {
    createVote(userId: $userId, linkId: $linkId) {
      id
      link {
        id
        votes {
          id
          user {
            id
          }
        }
      }
      user {
        id
      }
    }
  }
`;

export interface CreateVoteMutationResponse {
  loading: boolean;
  createVote: {
    id: string;
    link: Link;
    user: User;
  };
}

export const NEW_LINKS_SUBSCRIPTION = gql`
  subscription newLink {
    linkCreated {
      id
      createdAt
      updatedAt
      url
      description
      postedBy {
        id
        createdAt
        updatedAt
        name
        email
      }
      votes {
        id
        user {
          id
        }
      }
    }
  }
`;

export interface NewLinkSubscriptionResponse {
  linkCreated: Link;
}

export const DELETE_LINKS_SUBSCRIPTION = gql`
  subscription deleteLink {
    linkDeleted {
      id
      createdAt
      updatedAt
      url
      description
      postedBy {
        id
        createdAt
        updatedAt
        name
        email
      }
      votes {
        id
        user {
          id
        }
      }
    }
  }
`;

export interface DeleteLinkSubscriptionResponse {
  linkDeleted: Link;
}
export const NEW_VOTES_SUBSCRIPTION = gql`
  subscription newVote {
    voteCreated {
      id
      link {
        id
        createdAt
        updatedAt
        url
        description
        postedBy {
          id
          createdAt
          updatedAt
          name
          email
        }
        votes {
          id
          user {
            id
          }
        }
      }
      user {
        id
        email
      }
    }
  }
`;

export interface NewVoteSubscriptionResponse {
  voteCreated: Vote;
}
