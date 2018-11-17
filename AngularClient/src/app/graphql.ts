import { Link } from '../models/link.model';
import gql from 'graphql-tag';

export const ALL_LINKS_QUERY = gql`
  query AllLinksQuery {
    links {
      id
      createdAt
      updatedAt
      url
      description
    }
  }
`;

export interface AllLinkQueryResponse {
  links: Link[];
}

export const CREATE_LINK_MUTATION = gql`
  mutation CreateLinkMutation($description: String!, $url: String!) {
    createLink(description: $description, url: $url) {
      id
      createdAt
      updatedAt
      url
      description
    }
  }
`;

export interface CreateLinkMutationResponse {
  createLink: Link;
}

export const DELETE_LINK_MUTATION = gql`
  mutation DeleteLinkMutation($id: number!) {
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
