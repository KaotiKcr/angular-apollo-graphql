import { Link, User } from '../models/link.model';
import gql from 'graphql-tag';

export const ALL_LINKS_QUERY = gql`
  query AllLinksQuery {
    links {
      id
      createdAt
      updatedAt
      url
      description
      user {
        id
        createdAt
        updatedAt
        name
        email
      }
    }
  }
`;

export interface AllLinkQueryResponse {
  links: Link[];
}

export const CREATE_LINK_MUTATION = gql`
  mutation CreateLinkMutation($link: LinkInput!) {
    createLink(link: $link) {
      id
      createdAt
      updatedAt
      url
      description
      user {
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

