import { Component, OnInit } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { Router } from '@angular/router';

import { CREATE_LINK_MUTATION, CreateLinkMutationResponse } from '../graphql';
import { ALL_LINKS_QUERY, AllLinksQueryResponse } from '../graphql';
import { GS_USER_ID } from '../constants';

@Component({
  selector: 'app-create-link',
  templateUrl: './create-link.component.html',
  styleUrls: ['./create-link.component.css']
})
export class CreateLinkComponent implements OnInit {
  description = '';
  url = '';

  constructor(public apollo: Apollo, public router: Router) {}

  ngOnInit() {}

  createLink() {
    const postedById = localStorage.getItem(GS_USER_ID);
    if (!postedById) {
      console.error('No user logged in');
      return;
    }

    const newDescription = this.description;
    const newUrl = this.url;
    this.description = '';
    this.url = '';

    this.apollo
      .mutate<CreateLinkMutationResponse>({
        mutation: CREATE_LINK_MUTATION,
        variables: {
          link: {
            description: newDescription,
            url: newUrl,
            userId: postedById
          }
        },
        update: (store, { data: { createLink } }) => {
          const data: any = store.readQuery<AllLinksQueryResponse>({
            query: ALL_LINKS_QUERY
          });

          data.links.items.push(createLink);
          store.writeQuery({ query: ALL_LINKS_QUERY, data });
        }
      })
      .subscribe(
        response => {
          // We injected the Router service
          this.router.navigate(['/']);
        },
        error => {
          console.error(error);
          this.description = newDescription;
          this.url = newUrl;
        }
      );
  }
}
